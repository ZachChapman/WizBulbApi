using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.UI.Xaml.Controls;

namespace WizBulbApi.WinUI;

public sealed partial class MainWindow : ApplicationWindow
{
    public MainWindow()
    {
        this.InitializeComponent();

        Title = "WiZ Lights";
    }

    public override Frame Frame => _Frame;
    public InAppNotification InAppNotification => _InAppNotification;
}
