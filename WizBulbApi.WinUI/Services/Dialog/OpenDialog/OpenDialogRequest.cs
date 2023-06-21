namespace WizBulbApi.WinUI;

public class OpenDialogRequest
{
    public string? Title { get; init; }
    public object Content { get; init; }
    public List<DialogButton> Buttons { get; init; } = new();
}
