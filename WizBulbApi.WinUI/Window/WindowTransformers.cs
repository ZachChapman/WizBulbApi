using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml.Media;
using MvvmFramework.WinUI;

namespace WizBulbApi.WinUI;

public static class WindowTransformers
{
	public static void MainWindowDefaultTransform(MainWindow window, int width, int height)
	{
        window.SystemBackdrop = new DesktopAcrylicBackdrop();

        var appWindow = window.GetAppWindow();
        appWindow.IsShownInSwitchers = false;

        Task.Run(async () =>
        {
            var taskbarHeight = TaskbarHelper.GetTaskbarHeight();
            var margin = 12;

            var displaySize = await DisplayHelper.GetPrimaryMonitorInfoAsync();
            appWindow.MoveAndResize(new()
            {
                X = displaySize.Bounds.Width - width - margin,
                Y = displaySize.Bounds.Height - height - taskbarHeight - margin,
                Width = width,
                Height = height,
            });
        });

        var presenter = appWindow.TryGetPresenter<OverlappedPresenter>();
        presenter.SetBorderAndTitleBar(false, false);
        presenter.IsMinimizable = false;
        presenter.IsMaximizable = false;
        presenter.IsResizable = false;
        presenter.IsAlwaysOnTop = true;
    }

	public static void PopupWindowDefaultTransform(PopupWindow window, int width, int height)
	{
        //window.SystemBackdrop = new DesktopAcrylicBackdrop();

        var appWindow = window.GetAppWindow();
        appWindow.IsShownInSwitchers = false;

        Task.Run(async () =>
        {
            var taskbarHeight = TaskbarHelper.GetTaskbarHeight();
            var margin = 12;

            var displaySize = await DisplayHelper.GetPrimaryMonitorInfoAsync();
            appWindow.MoveAndResize(new()
            {
                X = displaySize.Bounds.Width - width - margin,
                Y = displaySize.Bounds.Height - height - taskbarHeight - margin,
                Width = width,
                Height = height,
            });
        });

        var presenter = appWindow.TryGetPresenter<OverlappedPresenter>();
        presenter.SetBorderAndTitleBar(false, false);
        presenter.IsMinimizable = false;
        presenter.IsMaximizable = false;
        presenter.IsResizable = false;
    }
}
