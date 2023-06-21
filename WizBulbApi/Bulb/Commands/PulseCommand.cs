namespace WizBulbApi.Commands;

public class PulseCommand : BulbCommand
{
    private readonly int _delta;
    private readonly int _duration;

    public PulseCommand(int delta, int duration)
    {
        _delta = delta;
        _duration = duration;
    }

    public override StateMethod Method => StateMethod.Pulse;
    public int Delta => _delta;
    public int Duration => _duration;
}
