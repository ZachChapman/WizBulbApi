using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using MvvmFramework;
using Toolbox.Reflection;
using MvvmFramework.WinUI;
using Toolbox;

namespace WizBulbApi.WinUI;

public abstract class ApplicationWindow : Window
{
	private readonly WindowProcessHook _windowProcessHook;

	public ApplicationWindow()
	{
		// Causes EngineException for unknown reasons
		//_windowProcessHook = WindowProcessHook.GetOrCreate(this.GetWindowHandle());
	}

	public virtual Frame Frame
	{
		get => Content as Frame;
		set => Content = value;
	}

	public void AddWindowMessageCallback(Action<WindowMessageEventArgs> callback)
	{
		_windowProcessHook.AddWindowMessageCallback(callback);
	}

	public void Minimise()
	{
		Frame.Content?.TryInvokeMethod(nameof(View<ViewModel>.Suspend));
		this.GetAppWindow().TryGetPresenter<OverlappedPresenter>().Minimize(true);
	}

	public void Restore()
	{
		Frame.Content?.TryInvokeMethod(nameof(View<ViewModel>.Resume));
		this.GetAppWindow().TryGetPresenter<OverlappedPresenter>().Restore(true);
	}

	public void ToggleVisibility()
	{
		if(Visible)
		{
			Minimise();
		}
		else
		{
			Restore();
		}
	}
}
