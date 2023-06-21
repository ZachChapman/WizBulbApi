using Windows.UI;

namespace WizBulbApi.WinUI;

public record SavedScene
{
    public Color Color { get; set; } = new();
    public int Temperature { get; set; }
    public int Brightness { get; set; }
    public int Speed { get; set; }
    public int SceneId { get; set; } = -1;

    public bool ColorIsEnabled { get; set; }
    public bool TemperatureIsEnabled { get; set; }
    public bool BrightnessIsEnabled { get; set; }
    public bool SpeedIsEnabled { get; set; }
}