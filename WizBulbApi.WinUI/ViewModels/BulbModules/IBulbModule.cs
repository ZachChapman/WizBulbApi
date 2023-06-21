namespace WizBulbApi.WinUI;

public interface IBulbModule
{
    int SceneId { get; }
    void Execute(BulbViewModel bulbViewModel);
}
