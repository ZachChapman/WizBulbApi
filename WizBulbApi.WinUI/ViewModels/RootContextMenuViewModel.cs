using MvvmFramework;

namespace WizBulbApi.WinUI;

public class RootContextMenuViewModel : ViewModel
{
    public RelayCommand ExitAppCommand => new(ExitApp);

    private void ExitApp()
    {
        App.WindowManager.MainWindow.Close();
    }
}
