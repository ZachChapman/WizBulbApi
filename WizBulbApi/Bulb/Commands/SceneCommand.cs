namespace WizBulbApi.Commands;

public class SceneCommand : BulbCommand
{
    private readonly Scene _scene;
    private readonly Speed _speed;

    public SceneCommand(Scene scene, Speed speed)
    {
        _scene = scene;
        _speed = speed;
    }

    public override StateMethod Method => StateMethod.SetPilot;
    public int SceneId => (int)_scene;
    public int Speed => (int)_speed;
}
