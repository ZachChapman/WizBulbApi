namespace WizBulbApi.Commands;

public class PowerCommand : BulbCommand
{
    private readonly bool _on;

    public PowerCommand(bool on)
    {
        _on = on;
    }

    public override StateMethod Method => StateMethod.SetPilot;
    public bool State => _on;
}
