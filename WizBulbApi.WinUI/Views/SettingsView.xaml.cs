using MvvmFramework.WinUI;

namespace WizBulbApi.WinUI;

public abstract class _SettingsView : View<SettingsViewModel> { }
public sealed partial class SettingsView : _SettingsView
{
    public SettingsView()
    {
        this.InitializeComponent();
    }
}
