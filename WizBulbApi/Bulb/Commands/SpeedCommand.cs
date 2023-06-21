namespace WizBulbApi.Commands;

public class SpeedCommand : BulbCommand
{
    private readonly Speed _speed;

    public SpeedCommand(Speed speed)
    {
        _speed = speed;
    }

    public override StateMethod Method => StateMethod.SetPilot;
    public int Speed => _speed;
}
