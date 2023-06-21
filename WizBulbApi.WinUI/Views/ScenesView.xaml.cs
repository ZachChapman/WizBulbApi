using CommunityToolkit.WinUI.UI;
using Microsoft.UI.Xaml;
using MvvmFramework.WinUI;
using System.Collections;
using System.Collections.ObjectModel;
using Toolbox;

namespace WizBulbApi.WinUI;

public abstract class _ScenesView : View<BulbViewModel> { }
public sealed partial class ScenesView : _ScenesView
{
    public ScenesView()
    {
        this.InitializeComponent();

        Loaded += ScenesView_Loaded;
    }

    public List<GroupedScenes> Scenes
    {
        get { return (List<GroupedScenes>)GetValue(ScenesProperty); }
        set { SetValue(ScenesProperty, value); }
    }
    public static readonly DependencyProperty ScenesProperty =
        DependencyPropertyHelper.AutoRegister(nameof(Scenes), new PropertyMetadata(GetGroupedScenes()));
    private static List<GroupedScenes> GetGroupedScenes()
    {
        return
            Scene
            .AsEnumerable()
            .GroupBy(
                scene => scene.Category.DisplayName,
                (key, scenes) => new GroupedScenes(key, scenes))
            .Append(new GroupedScenes("Experimental", new List<Scene> { new(9998, "Sample screen", new(99, "Experimental")) }))
            .ToList();
    }

    private void ScenesView_Loaded(object sender, RoutedEventArgs e)
    {
        var listView = ScenesListView;
        var listViewItem = listView.Items.OfType<Scene>().FirstOrDefault(i => i.Index == ViewModel.SceneId);
        if(listViewItem is null)
        {
            return;
        }

        listView.SmoothScrollIntoViewWithItemAsync(listViewItem, ScrollItemPlacement.Center);
    }
}

public class GroupedScenes : IGrouping<string, Scene>
{
    private readonly List<Scene> _scenes = new();

    public GroupedScenes(string key, IEnumerable<Scene> scenes)
    {
        Key = key;
        _scenes = scenes.ToList();
    }

    public string Key { get; }
    public ObservableCollection<Scene> Scenes => _scenes.ToObservableCollection();

    public IEnumerator<Scene> GetEnumerator() => _scenes.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => _scenes.GetEnumerator();
}
