namespace WizBulbApi.WinUI;

public interface IDialogService
{
    Task<OpenDialogResponse> OpenDialog(OpenDialogRequest request);
}
