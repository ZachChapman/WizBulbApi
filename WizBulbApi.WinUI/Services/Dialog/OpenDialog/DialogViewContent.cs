using Toolbox;

namespace WizBulbApi.WinUI;

public class DialogViewContent : IDialogContent
{
    public Type ViewType { get; init; }
    public Dictionary<string, object> Parameters { get; init; } = new();

    public object GetContent()
    {
        var view = Activator.CreateInstance(ViewType);

        Parameters
            .ToList()
            .ForEach(p => ViewType.GetProperty(p.Key).SetValue(view, p.Value));

        return view;
    }
}
