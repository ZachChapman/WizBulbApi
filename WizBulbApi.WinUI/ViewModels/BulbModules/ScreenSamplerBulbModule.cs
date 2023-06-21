using MvvmFramework.WinUI;
using Toolbox;

namespace WizBulbApi.WinUI;

public class ScreenSamplerBulbModule : IBulbModule
{
    private readonly ISamplePointDataAccess _samplePointDataAccess;
    private readonly ScreenCaptureService _screenCaptureService = new();
    private readonly ScreenSampler _screenSampler = new();
    private readonly List<SamplePoint> _samplePoints = new();
    private Repeater _sampleScreenRepeater;
    private bool _isInitialised;

    public ScreenSamplerBulbModule(ISamplePointDataAccess samplePointDataAccess)
    {
        _samplePointDataAccess = samplePointDataAccess;
    }

    public int SceneId => 9998;

    public async void Execute(BulbViewModel bulbViewModel)
    {
        if(!_isInitialised)
        {
            _isInitialised = true;
            await Initialise(bulbViewModel);
        }

        if(_sampleScreenRepeater.IsRunning)
        {
            _sampleScreenRepeater.Stop();
            return;
        }

        _sampleScreenRepeater.Start();
    }

    private async Task Initialise(BulbViewModel bulbViewModel)
    {
        _sampleScreenRepeater = new(async () =>
        {
            await SampleScreenColour(bulbViewModel);
        }, TimeSpan.FromSeconds(1));

        _samplePoints.AddRange(await _samplePointDataAccess.GetSamplePointsAsync());
    }

    private async Task SampleScreenColour(BulbViewModel bulbViewModel)
    {
        using var screen = _screenCaptureService.CaptureDesktop();
        if(screen is null) return;

        var samplePoints =
            _samplePoints
            .Select(samplePoint => new System.Drawing.Point((int)(samplePoint.X * screen.Width), (int)(samplePoint.Y * screen.Height)))
            .ToArray();

        var colour = _screenSampler.GetDesktopAverageColour(samplePoints);

        var color = bulbViewModel.Color;
        color.R = colour.R;
        color.G = colour.G;
        color.B = colour.B;
        bulbViewModel.Color = color;
    }
}
