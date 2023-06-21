using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using MvvmFramework;
using Toolbox;

namespace WizBulbApi.WinUI;

public class DialogService : IDialogService
{
    public async Task<OpenDialogResponse> OpenDialog(OpenDialogRequest request)
    {
        var dialog = new ContentDialog
        {
            Title = request.Title,
            Margin = new Thickness(0, 10, 0, 0),
            MaxHeight = 410,
            DefaultButton = ContentDialogButton.Primary,
            Resources = new ResourceDictionary
            {
                new KeyValuePair<object, object>("ContentDialogPadding", new Thickness(0)),
                new KeyValuePair<object, object>("ContentDialogCommandSpaceMargin", new Thickness(3)),
                new KeyValuePair<object, object>("ContentDialogBackground", App.Current.Resources["ApplicationPageBackgroundThemeBrush"]),
            },
            XamlRoot = App.WindowManager.MainWindow.Content.XamlRoot,
        };

        if(request.Content is IDialogContent dialogContent)
        {
            dialog.Content = dialogContent.GetContent();
        }
        else
        {
            dialog.Content = request.Content;
        }

        // TODO: Move style stuff.

        var buttons = request.Buttons.OrderBy(b => b.Index).Take(3).ToList();
        foreach(var button in buttons)
        {
            if(button.Index == 0)
            {
                dialog.CloseButtonText = button.Content;
                if(button.Command is not null)
                {
                    dialog.CloseButtonCommand = new AsyncRelayCommand(button.Command);
                }
                continue;
            }
            if(button.Index == 1)
            {
                dialog.PrimaryButtonText = button.Content;
                if(button.Command is not null)
                {
                    dialog.PrimaryButtonCommand = new AsyncRelayCommand(button.Command);
                }
                continue;
            }
            if(button.Index == 2)
            {
                dialog.SecondaryButtonText = button.Content;
                if(button.Command is not null)
                {
                    dialog.SecondaryButtonCommand = new AsyncRelayCommand(button.Command);
                }
                continue;
            }
            break;
        }

        var result = await dialog.ShowAsync();

        return new()
        {
            InvokedButton = request.Buttons.First(b => b.Index == (int)result) with { },
        };
    }
}
