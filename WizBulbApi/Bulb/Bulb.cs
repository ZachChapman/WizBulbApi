using WizBulbApi.Commands;

namespace WizBulbApi;

public class Bulb
{
    private readonly BulbClient _bulbClient;

    public Bulb(BulbClient bulbClient)
    {
        _bulbClient = bulbClient;
    }

    public async Task<BulbState?> GetPilot() => await _bulbClient.TrySendStateAsync(new GetPilotCommand());
    public async Task<BulbState?> GetSystemConfig() => await _bulbClient.TrySendStateAsync(new GetSystemConfigCommand());
    public async Task<BulbState?> GetUserConfig() => await _bulbClient.TrySendStateAsync(new GetUserConfigCommand());

    public async Task<BulbState?> SetPower(bool on) => await _bulbClient.TrySendStateAsync(new PowerCommand(on));
    public async Task<BulbState?> SetBrightness(Brightness brightness) => await _bulbClient.TrySendStateAsync(new BrightnessCommand(brightness));
    public async Task<BulbState?> SetTemperature(Temperature temperature) => await _bulbClient.TrySendStateAsync(new TemperatureCommand(temperature));
    public async Task<BulbState?> SetColour(Colour colour) => await _bulbClient.TrySendStateAsync(new ColourCommand(colour));
    public async Task<BulbState?> SetScene(Scene scene, Speed speed) => await _bulbClient.TrySendStateAsync(new SceneCommand(scene, speed));
    public async Task<BulbState?> SetSpeed(Speed speed) => await _bulbClient.TrySendStateAsync(new SpeedCommand(speed));
    public async Task<BulbState?> Pulse(int delta, int duration) => await _bulbClient.TrySendStateAsync(new PulseCommand(delta, duration));
}