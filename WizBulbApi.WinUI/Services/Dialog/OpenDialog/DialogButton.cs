namespace WizBulbApi.WinUI;

public record DialogButton(int Index, string Content, Func<Task>? Command = default);
