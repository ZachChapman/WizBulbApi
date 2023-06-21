namespace WizBulbApi.Commands;

public class BrightnessCommand : BulbCommand
{
    private readonly Brightness _brightness;

    public BrightnessCommand(Brightness brightness)
    {
        _brightness = brightness;
    }

    public override StateMethod Method => StateMethod.SetPilot;
    public int Dimming => _brightness;
}
