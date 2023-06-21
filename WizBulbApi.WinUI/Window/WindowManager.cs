using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using MvvmFramework.WinUI;
using System.Diagnostics;
using Toolbox;

namespace WizBulbApi.WinUI;

public class WindowManager
{
	private readonly List<Window> _modalWindows = new();

	public MainWindow? MainWindow { get; private set; }
	public IReadOnlyList<Window> ModalWindows => _modalWindows;

	public void SetTheme(ApplicationTheme theme)
	{
		ApplyToAllWindows(window => window.SetRootTheme(theme));
	}

	public void SetAlwaysOnTop(bool alwaysOnTop)
	{
		ApplyToAllWindows(window => window.GetAppWindow().TryGetPresenter<OverlappedPresenter>().IsAlwaysOnTop = alwaysOnTop);
	}

	public void RegisterMainWindow(MainWindow window)
	{
		MainWindow = window;
		MainWindow.Activated += MainWindow_Activated;
        MainWindow.Closed += MainWindow_Closed;

        //MainWindow.AddWindowMessageCallback(args =>
        //{
        //	Debug.WriteLine("Main: " + args.WindowMessage);
        //});
    }

    public void RegisterModalWindow(Window window)
	{
		window.Activated += ModalWindow_Activated;

        var mainAppWindow = MainWindow.GetAppWindow();
		var appWindow = window.GetAppWindow();

		window.SetRootTheme(MainWindow.GetRootTheme());
		appWindow.TryGetPresenter<OverlappedPresenter>().IsAlwaysOnTop =
			mainAppWindow.TryGetPresenter<OverlappedPresenter>().IsAlwaysOnTop;

		//(window as ApplicationWindow).AddWindowMessageCallback(args =>
		//{
		//	Debug.WriteLine($"Modal: {args.WindowHandle} {args.WindowMessage} {args.WParam} {args.LParam}");
		//});

		_modalWindows.Add(window);
	}

    public void UnregisterModalWindow(Window window)
	{
		window.Activated -= ModalWindow_Activated;

		_modalWindows.Remove(window);
	}

	private void ApplyToAllWindows(Action<Window> windowAction)
	{
		windowAction(MainWindow);
		_modalWindows.ForEach(windowAction);
	}

	private async void MainWindow_Activated(object sender, WindowActivatedEventArgs args)
	{
		Debug.WriteLine("Main: " + args.WindowActivationState);

		if(args.WindowActivationState is WindowActivationState.Deactivated)
		{
			if(Debugger.IsAttached) return;

			(sender as ApplicationWindow).Minimise();
		}
	}

	private async void ModalWindow_Activated(object sender, WindowActivatedEventArgs args)
	{
		Debug.WriteLine("Modal: " + args.WindowActivationState);

		if(args.WindowActivationState is WindowActivationState.Deactivated)
		{
			(sender as ApplicationWindow).Minimise();
		}
	}

    private void MainWindow_Closed(object sender, WindowEventArgs args)
    {
		ModalWindows
			.ToList()
			.ForEach(w =>
			{
				w.Minimise();
				w.Close();
			});
    }
}

//public static class PopupWindowCreator
//{
//	public static AppWindow CreatePopupWindow(Frame content)
//	{
//		var presenter = OverlappedPresenter.Create();
//		presenter.SetBorderAndTitleBar(false, false);
//		presenter.IsMinimizable = false;
//		presenter.IsMaximizable = false;
//		presenter.IsResizable = false;

//		var appWindow = AppWindow.Create(presenter, App.WindowManager.MainWindow.GetAppWindow().Id);
//		appWindow.Changed += AppWindow_Changed;
//		appWindow.Closing += AppWindow_Closing;
//		appWindow.Destroying += AppWindow_Destroying;

//		var mainAppWindow = App.WindowManager.MainWindow.GetAppWindow();
//		appWindow.IsShownInSwitchers = false;

//		appWindow.MoveAndResize(new()
//		{
//			X = mainAppWindow.Position.X - mainAppWindow.ClientSize.Width - 5,
//			Y = mainAppWindow.Position.Y,
//			Width = mainAppWindow.Size.Width,
//			Height = mainAppWindow.Size.Height,
//		});

//		return appWindow;
//	}



//	private static void AppWindow_Changed(AppWindow sender, AppWindowChangedEventArgs args)
//	{
//	}

//	private static void AppWindow_Destroying(AppWindow sender, object args)
//	{
//	}

//	private static void AppWindow_Closing(AppWindow sender, AppWindowClosingEventArgs args)
//	{
//	}

//	//public Frame ContentFrame
//	//{
//	//	get => Content as Frame;
//	//	set => Content = value;
//	//}

//	//public bool LightDismissalEnabled { get; set; } = true;

//	//public void RegisterModalWindow() => App.WindowManager.RegisterModalWindow(this);

//	//private void PopupWindow_Activated(object sender, WindowActivatedEventArgs args)
//	//{
//	//	if(args.WindowActivationState is not WindowActivationState.Deactivated)
//	//	{
//	//		//App.WindowManager.RegisterModalWindow(this);
//	//	}
//	//	else if(LightDismissalEnabled)
//	//	{
//	//		Close();
//	//	}
//	//}

//	//private void PopupWindow_Closed(object sender, WindowEventArgs args)
//	//{
//	//	//App.WindowManager.UnregisterModalWindow(this);
//	//}
//}