namespace WizBulbApi.Commands;

public class OmniCommand : BulbCommand
{
    private readonly bool? _isOn;
    private readonly Colour? _colour;
    private readonly Temperature? _temperature;
    private readonly Brightness? _brightness;
    private readonly Speed? _speed;
    private readonly Scene? _scene;

    public OmniCommand(bool? isOn, Colour? colour, Temperature? temperature, Brightness? brightness, Speed? speed, Scene? scene)
    {
        _isOn = isOn;
        _colour = colour;
        _temperature = temperature;
        _brightness = brightness;
        _speed = speed;
        _scene = scene;
    }

    public override StateMethod Method => StateMethod.SetPilot;
    public bool? State => _isOn;
    public int? R => _colour?.Red?.Value;
    public int? G => _colour?.Green?.Value;
    public int? B => _colour?.Blue?.Value;
    public int? C => _colour?.CoolWhite?.Value;
    public int? W => _colour?.WarmWhite?.Value;
    public int? Temp => _temperature?.Value;
    public int? Dimming => _brightness?.Value;
    public int? Speed => _speed?.Value;
    public int? SceneId => (int?)_scene;
}
