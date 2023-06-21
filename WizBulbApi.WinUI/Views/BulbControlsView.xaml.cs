using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using MvvmFramework.WinUI;
using System.Collections.Specialized;

namespace WizBulbApi.WinUI;

public abstract class _BulbControlsView : View<BulbViewModel> { }
public sealed partial class BulbControlsView : _BulbControlsView
{
    public BulbControlsView()
    {
        this.InitializeComponent();

        Loaded += BulbControlsView_Loaded;
        Unloaded += BulbControlsView_Unloaded;
    }

    private void BulbControlsView_Loaded(object sender, RoutedEventArgs e)
    {
        //var collection = RecentScenesItemsControl.ItemsSource as ObservableCollection<int>;
        //if(collection is null) return;

        //collection.CollectionChanged += Collection_CollectionChanged;

        //Collection_CollectionChanged(collection, default);
    }

    private void BulbControlsView_Unloaded(object sender, RoutedEventArgs e)
    {
        //var collection = RecentScenesItemsControl.ItemsSource as ObservableCollection<int>;
        //if(collection is null) return;

        //collection.CollectionChanged -= Collection_CollectionChanged;
    }

    private void Collection_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        //var collection = sender as ObservableCollection<int>;
        //if(collection is null) return;

        //for(var i = 0; i < collection.Count; i++)
        //{
        //    var sceneId = collection[i];
        //    var itemContainer = (FrameworkElement)RecentScenesItemsControl.ContainerFromIndex(i);

        //    if(sceneId == ViewModel.SceneId)
        //    {
        //        itemContainer.Visibility = Visibility.Collapsed;
        //    }
        //    else
        //    {
        //        itemContainer.Visibility = Visibility.Visible;
        //    }
        //}
    }

    private void RestoreSavedSceneListView_ItemClick(object sender, ItemClickEventArgs e)
    {
        var bulbCommandSavedItem = e.ClickedItem as SavedScene;
        if(bulbCommandSavedItem is null) return;

        ViewModel.RestoreSavedSceneCommand.Execute(bulbCommandSavedItem);
    }

    private async void RemoveSavedSceneMenuFlyoutItem_Click(object sender, RoutedEventArgs e)
    {
        var menuFlyoutItem = sender as MenuFlyoutItem;
        if(menuFlyoutItem is null) return;

        var bulbCommandSavedItem = menuFlyoutItem.DataContext as SavedScene;
        if(bulbCommandSavedItem is null) return;

        await ViewModel.RemoveSavedSceneCommand.ExecuteAsync(bulbCommandSavedItem);
    }
}
