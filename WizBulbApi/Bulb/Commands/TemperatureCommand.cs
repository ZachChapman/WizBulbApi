namespace WizBulbApi.Commands;

public class TemperatureCommand : BulbCommand
{
    private readonly Temperature _temperature;

    public TemperatureCommand(Temperature temperature)
    {
        _temperature = temperature;
    }

    public override StateMethod Method => StateMethod.SetPilot;
    public int Temp => _temperature;
}
