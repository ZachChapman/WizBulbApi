using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using MvvmFramework;
using System.ComponentModel;

namespace WizBulbApi.WinUI;

public class SettingsViewModel : ViewModel
{
    private readonly INavigationService _navigationService;
    private readonly ISettingsDataAccess _settingsDataAccess;
    private readonly WindowManager _windowManager;
    private ApplicationTheme _initialTheme;

    public SettingsViewModel(INavigationService navigationService, ISettingsDataAccess settingsDataAccess, WindowManager windowManager)
    {
        _navigationService = navigationService;
        _settingsDataAccess = settingsDataAccess;
        _windowManager = windowManager;
    }

    public static SettingsViewModel Create()
    {
        var viewModel = App.Host.Services.GetRequiredService<SettingsViewModel>();

        return viewModel;
    }

    public List<NetworkInfo> NetworkInterfaces { get; private set; } = new();
    public NetworkInfo NetworkInterface { get; set; }
    public int? HomeId { get; set; }
    public ApplicationTheme Theme { get; set; }
    public bool ThemeHasChanged { get; private set; }
    public bool HasChanges { get; private set; }

    public AsyncRelayCommand CancelCommand => new(Cancel);
    public AsyncRelayCommand SaveCommand => new(SaveAndLeave);
    public AsyncRelayCommand RestartCommand => new(SaveAndRestart);

    public override async Task Initialise()
    {
        NetworkInterfaces = NetworkHelper.GetNetworkInfo().ToList();

        var settings = await _settingsDataAccess.LoadAsync();
        if(settings is not null)
        {
            HomeId = settings.HomeId;
            NetworkInterface = NetworkInterfaces.FirstOrDefault(info => info.Id == settings.NetworkInterfaceId);
            Theme = settings.Theme;
        }

        _initialTheme = Theme;

        PropertyChanged += SettingsViewModel_PropertyChanged;
    }

    public override async Task Validate()
    {
        ClearErrors();

        if(NetworkInterface is null)
        {
            AddError(NetworkInterface, "Please select a network interface.");
        }

        if(HomeId is null or 0 or int.MinValue)
        {
            AddError(HomeId, "Please enter a valid WiZ Home ID.");
        }
    }

    public override async Task OnNavigatingFrom(ViewModelNavigationArgs args)
    {
        if(HasChanges && Theme != _initialTheme)
        {
            _windowManager.SetTheme(_initialTheme);
        }
    }

    private async Task Cancel()
    {
        var success = _navigationService.GoBack();
        if(!success)
        {
            if(HomeId is null)
            {
                await _navigationService.GoToLogin();
            }
            else
            {
                await _navigationService.GoToBulbList(HomeId!.Value);
            }
        }
    }

    private async Task Save()
    {
        if(!HasChanges) return;

        await Validate();

        if(HasErrors) return;

        var homeId = HomeId!.Value;

        await _settingsDataAccess.SaveAsync(new()
        {
            HomeId = homeId,
            NetworkInterfaceId = NetworkInterface.Id,
            Theme = Theme,
        });

        HasChanges = false;
    }

    private async Task SaveAndLeave()
    {
        await Save();
        _navigationService.GoBack();
    }

    private async Task SaveAndRestart()
    {
        await Save();
        await App.Current.RestartAsync(true);
    }

    private void SettingsViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if(e.PropertyName is nameof(HasChanges))
        {
            return;
        }

        HasChanges = true;

        if(e.PropertyName is nameof(Theme))
        {
            _windowManager.SetTheme(Theme);
            ThemeHasChanged = true;
        }
    }
}
