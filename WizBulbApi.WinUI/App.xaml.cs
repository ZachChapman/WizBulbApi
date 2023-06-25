using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.Windows.AppLifecycle;
using MvvmFramework;
using MvvmFramework.Extensions;
using MvvmFramework.WinUI;
using PInvoke;
using System.Diagnostics;
using Toolbox;

namespace WizBulbApi.WinUI;

/// <summary> Provides application-specific behavior to supplement the default Application class. </summary>
public partial class App : Application
{
    private MainWindow _mainWindow;
    private PopupWindow _popupWindow;
    private NotifyIconManager _notifyIconManager;
    private int _notifyIconId;

    public App()
    {
        this.InitializeComponent();
    }

    public static IHost Host { get; private set; }
    public static WindowManager WindowManager { get; } = new();
    public new static App Current => (App)Application.Current;

    public static void DispatcherEnqueue(DispatcherQueueHandler handler) => WindowManager.MainWindow.DispatcherQueue.TryEnqueue(handler);

    public async Task RestartAsync(bool recover = false)
    {
        if(recover)
        {
            await RestartAppWithRecovery();
            return;
        }

        AppInstance.Restart("/Restart");
    }

    protected override async void OnLaunched(LaunchActivatedEventArgs args)
    {
        //if(!Debugger.IsAttached) Debugger.Launch();
        UnhandledException += App_UnhandledException;

        await ConfigureServices();

        CreateMainWindow();
        CreatePopupWindow();
        CreateNotifyIcon();

        var settingsDataAccess = Host.Services.GetRequiredService<ISettingsDataAccess>();
        var settings = await settingsDataAccess.LoadAsync();

        if(settings?.HomeId == default)
        {
            await NavigateToLogin();
            return;
        }

        WindowManager.SetTheme(settings!.Theme);

        if(await TryRecovery())
        {
            _mainWindow.ToggleVisibility();
            return;
        }

        await NavigateToBulbList(settings!.HomeId);

        ////var a = GraphicsCaptureSession.IsSupported();
        ////var picker = new GraphicsCapturePicker();
        ////InitializeWithWindow.Initialize(picker, _mainWindow.GetWindowHandle());
        ////var item = await picker.PickSingleItemAsync();

        //var navigationService = Host.Services.GetRequiredService<INavigationService>();

        //var viewModel = SamplePointsEditorViewModel.Create();
        //await viewModel.Initialise();

        //navigationService.Navigate(typeof(SamplePointsEditorView), viewModel);
    }

    private async Task ConfigureServices()
    {
        Host =
            Microsoft.Extensions.Hosting.Host
            .CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                services.AddLogging(builder =>
                {
                    builder.AddDebug();
                    builder.AddConsole();
                    builder.SetMinimumLevel(LogLevel.Debug);
                });

                services.RegisterAllOfType<ViewModel>(ServiceLifetime.Transient);

                if(!Directory.Exists(Storage.FromRootPath("Data"))) Directory.CreateDirectory(Storage.FromRootPath("Data"));
                services.AddSingleton<ISettingsDataAccess, SettingsDataAccess>(p => new(Storage.FromRootPath("Data", "Settings.json")));
                services.AddSingleton<IBulbDataAccess, BulbDataAccess>(p => new(Storage.FromRootPath("Data", "BulbHandles.json")));
                services.AddSingleton<ISamplePointDataAccess, SamplePointDataAccess>(p => new(Storage.FromRootPath("Data", "SamplePoints.json")));
                services.AddSingleton<IAppStateDataAccess, AppStateDataAccess>(p => new(Storage.FromRootPath("Data", "AppState.json")));

                services.AddSingleton<IViewModelProvider, ViewModelProvider>();
                services.AddSingleton<INavigationService, NavigationService>(x => new(_mainWindow.Frame));
                services.AddSingleton<IDialogService, DialogService>();
                services.AddSingleton<IStorageService, StorageService>();
                services.AddSingleton<INotificationService, NotificationService>(x => new(_mainWindow.InAppNotification));
                services.AddSingleton<AppRecovery>();
                services.AddSingleton<NavigationCommands>();

                services.AddSingleton<WindowManager>(x => WindowManager);
            })
            .Build();

        await Host.StartAsync();

        ViewTypeRegister.Register();
    }

    private void CreateMainWindow()
    {
        _mainWindow = new MainWindow();
        _mainWindow.Closed += MainWindow_Closed;
        WindowManager.RegisterMainWindow(_mainWindow);
        WindowTransformers.MainWindowDefaultTransform(_mainWindow, 360, 430);
    }

    private void CreatePopupWindow()
    {
        _popupWindow = new PopupWindow();
        _popupWindow.Frame.Navigate(typeof(RootContextMenuView), new RootContextMenuViewModel());
        WindowManager.RegisterModalWindow(_popupWindow);
        WindowTransformers.PopupWindowDefaultTransform(_popupWindow, 200, 100);
    }

    private void CreateNotifyIcon()
    {
        _notifyIconManager = new(_mainWindow.GetWindowHandle());
        _notifyIconId = _notifyIconManager.AddNotifyIcon(Paths.LightBulbIcon_Light, "WiZ Bulb API", args =>
        {
            if(args.LParam == (nint)User32.WindowMessage.WM_LBUTTONDOWN)
            {
                _mainWindow.ToggleVisibility();
            }
            else if(args.LParam == (nint)User32.WindowMessage.WM_RBUTTONDOWN)
            {
                _popupWindow.ToggleVisibility();
            }
        });
    }

    private static async Task NavigateToLogin()
    {
        var navigationService = Host.Services.GetRequiredService<INavigationService>();
        var viewModelProvider = Host.Services.GetRequiredService<IViewModelProvider>();

        var viewModel = viewModelProvider.Create<LoginViewModel>();
        await viewModel.Initialise();

        navigationService.Navigate(typeof(LoginView), viewModel);
    }

    private static async Task NavigateToBulbList(int homeId)
    {
        var navigationService = Host.Services.GetRequiredService<INavigationService>();
        var viewModelProvider = Host.Services.GetRequiredService<IViewModelProvider>();

        var viewModel = viewModelProvider.Create<BulbListViewModel>();
        await viewModel.Initialise(homeId);

        navigationService.Navigate(typeof(BulbListView), viewModel);
    }

    private void MainWindow_Closed(object sender, WindowEventArgs args)
    {
        _mainWindow.Closed -= MainWindow_Closed;

        _notifyIconManager.RemoveNotifyIcon(_notifyIconId);
    }

    private void App_UnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs args)
    {
        Debug.WriteLine(args.Exception.ToString());

        AsyncHelpers.RunSync(RestartAppWithRecovery);
    }

    private async Task RestartAppWithRecovery()
    {
        var recoveryService = Host.Services.GetRequiredService<AppRecovery>();

        if(_mainWindow.Frame.Content is not IView view)
        {
            return;
        }

        var viewModel = view.GetViewModel();
        var viewModelState = viewModel is null ? null : await viewModel.GetState();

        var viewTypeName = view.GetType().Name;
        var viewModelTypeName = viewModel?.GetType().Name;

        await recoveryService.StoreState(new AppState
        {
            ViewTypeName = viewTypeName,
            ViewModelTypeName = viewModelTypeName,
            ViewModelState = viewModelState,
        });

        AppInstance.Restart("/RestartWithRecovery");
    }

    private async Task<bool> TryRecovery()
    {
        var recoveryService = Host.Services.GetRequiredService<AppRecovery>();
        return await recoveryService.TryRestoreLastState();
    }
}
