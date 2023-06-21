using MvvmFramework.WinUI;

namespace WizBulbApi.WinUI;

public abstract class _RootContextMenuView : View<RootContextMenuViewModel> { }
public sealed partial class RootContextMenuView : _RootContextMenuView
{
    public RootContextMenuView()
    {
        this.InitializeComponent();
    }
}
