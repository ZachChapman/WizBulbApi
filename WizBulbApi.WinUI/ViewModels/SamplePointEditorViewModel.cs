using Microsoft.Extensions.DependencyInjection;
using MvvmFramework;
using MvvmFramework.WinUI;
using Toolbox;
using Windows.UI;

namespace WizBulbApi.WinUI;

public class SamplePointsEditorViewModel : ViewModel
{
    private readonly ISamplePointDataAccess _samplePointDataAccess;
    private readonly ScreenCaptureService _screenCaptureService = new();
    private readonly ScreenSampler _screenSampler = new();
    private readonly Repeater _sampleScreenRepeater;

    public SamplePointsEditorViewModel(ISamplePointDataAccess samplePointDataAccess)
    {
        _samplePointDataAccess = samplePointDataAccess;

        _sampleScreenRepeater = new(async () =>
        {
            await SampleScreenColour();
        }, TimeSpan.FromSeconds(1));
    }

    public TrackedObservableCollection<SamplePoint> SamplePoints { get; } = new();
    public Color PreviewColor { get; private set; }

    public AsyncRelayCommand SaveCommand => new(Save);

    public async Task Initialise()
    {
        SamplePoints.EnableTracking(enabled: false);
        var samplePoints = await _samplePointDataAccess.GetSamplePointsAsync();
        SamplePoints.AddRange(samplePoints);
        SamplePoints.EnableTracking();

        _sampleScreenRepeater.Start();
    }

    public override Task OnNavigatedFrom()
    {
        _sampleScreenRepeater.Dispose();

        return base.OnNavigatedFrom();
    }

    public async Task Save()
    {
        var added = SamplePoints.GetAddedItems();
        var removed = SamplePoints.GetRemovedItems();

        if(added.Any())
        {
            await _samplePointDataAccess.AddSamplePointsAsync(added);
        }
        if(removed.Any())
        {
            await _samplePointDataAccess.RemoveSamplePointsAsync(removed);
        }
    }

    private async Task SampleScreenColour()
    {
        using var screen = _screenCaptureService.CaptureDesktop();
        if(screen is null) return;

        var samplePoints =
            SamplePoints
            .Select(samplePoint => new System.Drawing.Point((int)(samplePoint.X * screen.Width), (int)(samplePoint.Y * screen.Height)))
            .ToArray();

        var color = _screenSampler.GetDesktopAverageColour(samplePoints);
        var previewColor = PreviewColor;
        previewColor.R = color.R;
        previewColor.G = color.G;
        previewColor.B = color.B;
        PreviewColor = previewColor;
    }
}
