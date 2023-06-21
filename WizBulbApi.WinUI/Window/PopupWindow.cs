using Microsoft.UI.Xaml.Controls;

namespace WizBulbApi.WinUI;

public class PopupWindow : ApplicationWindow
{
	public PopupWindow()
	{
		Frame ??= new Frame();
	}

	public bool LightDismissalEnabled { get; set; } = true;
}
