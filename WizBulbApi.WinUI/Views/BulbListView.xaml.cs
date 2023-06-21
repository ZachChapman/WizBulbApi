using Microsoft.UI.Xaml.Controls;
using MvvmFramework.WinUI;

namespace WizBulbApi.WinUI;

public abstract class _BulbListView : View<BulbListViewModel> { }
public sealed partial class BulbListView : _BulbListView
{
    public BulbListView()
    {
        this.InitializeComponent();
    }

    private async void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var bulbItem = (BulbListItemViewModel)e.AddedItems.FirstOrDefault();

        if(bulbItem is null) return;

        ViewModel.GoToBulbCommand.Execute(bulbItem.Handle);
    }
}
