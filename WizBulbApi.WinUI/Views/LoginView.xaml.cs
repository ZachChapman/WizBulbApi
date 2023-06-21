using MvvmFramework.WinUI;

namespace WizBulbApi.WinUI;

public abstract class _LoginView : View<LoginViewModel> { }
public sealed partial class LoginView : _LoginView
{
    public LoginView()
    {
        this.InitializeComponent();
    }
}
