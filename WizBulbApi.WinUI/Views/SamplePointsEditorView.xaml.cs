using Microsoft.UI;
using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Shapes;
using MvvmFramework.WinUI;
using System.Drawing;
using Toolbox;

namespace WizBulbApi.WinUI;

public abstract class _SamplePointsEditorView : View<SamplePointsEditorViewModel> { }
public sealed partial class SamplePointsEditorView : _SamplePointsEditorView
{
    private readonly ScreenCaptureService _screenCaptureService = new();
    private Border _previewCrosshair;
    private Repeater _screenCaptureRepeater;
    private Size _scale;

    public SamplePointsEditorView()
    {
        this.InitializeComponent();

        _screenCaptureRepeater = new(ScreenCaptureHandler, TimeSpan.FromMilliseconds(ScreenCaptureInterval));

        Loaded += SamplePointsEditorView_Loaded;

        RegisterPropertyChangedCallback(ScreenCaptureIntervalProperty, OnScreenCaptureIntervalChanged);
        RegisterPropertyChangedCallback(GridLinesStepSizeProperty, OnGridLinesStepSizeChanged);



    }

    public double ScreenCaptureInterval
    {
        get => (double)GetValue(ScreenCaptureIntervalProperty);
        set => SetValue(ScreenCaptureIntervalProperty, value);
    }
    public double GridLinesStepSize
    {
        get => (double)GetValue(GridLinesStepSizeProperty);
        set => SetValue(GridLinesStepSizeProperty, value);
    }
    public double HorizontalStepSize { get; private set; }
    public double VerticalStepSize { get; private set; }

    public static readonly DependencyProperty ScreenCaptureIntervalProperty = DependencyPropertyHelper.AutoRegister(nameof(ScreenCaptureInterval), new(500d));
    public static readonly DependencyProperty GridLinesStepSizeProperty = DependencyPropertyHelper.AutoRegister(nameof(GridLinesStepSize), new(0.125d));

    private async void SamplePointsEditorView_Loaded(object sender, RoutedEventArgs e)
    {
        await DrawScreenCaptureWidgetsAsync();
    }

    private async Task DrawScreenCaptureWidgetsAsync()
    {
        SamplePointsCanvas.Children.Clear();
        GridLinesCanvas.Children.Clear();

        var monitor = await DisplayHelper.GetPrimaryMonitorInfoAsync();
        var scale = ScaleSize(
            new Size((int)ScreenCaptureRoot.ActualWidth, (int)ScreenCaptureRoot.ActualHeight),
            new Size(monitor.Bounds.Width, monitor.Bounds.Height));

        var widthPadding = (int)(ScreenCaptureRoot.Padding.Left + ScreenCaptureRoot.Padding.Right);
        var heightPadding = (int)(ScreenCaptureRoot.Padding.Top + ScreenCaptureRoot.Padding.Bottom);
        _scale = new(scale.Width - widthPadding, scale.Height - heightPadding - (widthPadding / 2));

        HorizontalStepSize = GetCaptureSize().Width * GridLinesStepSize;
        VerticalStepSize = GetCaptureSize().Height * GridLinesStepSize;

        _previewCrosshair = CreateCrosshair();
        _previewCrosshair.Tag = "PreviewCrosshair";
        _previewCrosshair.IsHitTestVisible = false;
        _previewCrosshair.Opacity = 0.25;
        _previewCrosshair.Visibility = Visibility.Collapsed;
        SamplePointsCanvas.Children.Add(_previewCrosshair);

        foreach(var samplePoint in ViewModel.SamplePoints)
        {
            var crosshair = CreateCrosshair();
            crosshair.Tag = samplePoint.Id;

            Canvas.SetLeft(crosshair, samplePoint.X * GetCaptureSize().Width - (_previewCrosshair.Width / 2));
            Canvas.SetTop(crosshair, samplePoint.Y * GetCaptureSize().Height - (_previewCrosshair.Height / 2));

            SamplePointsCanvas.Children.Add(crosshair);
        }

        DrawGridLines();

        _screenCaptureRepeater.Start();
    }

    private void OnScreenCaptureIntervalChanged(DependencyObject sender, DependencyProperty property)
    {
        _screenCaptureRepeater.Stop();
        _screenCaptureRepeater.Dispose();
        _screenCaptureRepeater = new(ScreenCaptureHandler, TimeSpan.FromMilliseconds(ScreenCaptureInterval));
        _screenCaptureRepeater.Start();
    }

    private void OnGridLinesStepSizeChanged(DependencyObject sender, DependencyProperty property)
    {
        HorizontalStepSize = GetCaptureSize().Width * GridLinesStepSize;
        VerticalStepSize = GetCaptureSize().Height * GridLinesStepSize;

        GridLinesCanvas.Children.Clear();

        DrawGridLines();
    }

    private void SamplePointsCanvas_PointerMoved(object sender, PointerRoutedEventArgs e)
    {
        var pointer = e.GetCurrentPoint(SamplePointsCanvas);

        // Round coords to closest multiple of StepSize
        var x = Math.Round(pointer.Position.X / HorizontalStepSize) * HorizontalStepSize;
        var y = Math.Round(pointer.Position.Y / VerticalStepSize) * VerticalStepSize;

        Canvas.SetLeft(_previewCrosshair, x - (_previewCrosshair.Width / 2));
        Canvas.SetTop(_previewCrosshair, y - (_previewCrosshair.Height / 2));
    }

    private void SamplePointsCanvas_PointerReleased(object sender, PointerRoutedEventArgs e)
    {
        var pointer = e.GetCurrentPoint(SamplePointsCanvas);
        var isLeftClick = pointer.Properties.PointerUpdateKind is PointerUpdateKind.LeftButtonReleased;
        var isRightClick = pointer.Properties.PointerUpdateKind is PointerUpdateKind.RightButtonReleased;

        if(isLeftClick)
        {
            // Round coords to closest multiple of StepSize
            var x = Math.Round(pointer.Position.X / HorizontalStepSize) * HorizontalStepSize;
            var y = Math.Round(pointer.Position.Y / VerticalStepSize) * VerticalStepSize;

            var exists =
                SamplePointsCanvas
                .Children
                .OfType<Border>()
                .Any(child =>
                    child.Tag != "PreviewCrosshair"
                    && Canvas.GetLeft(child) == x - (_previewCrosshair.Width / 2)
                    && Canvas.GetTop(child) == y - (_previewCrosshair.Height / 2));

            if(exists)
            {
                return;
            }

            var index = !ViewModel.SamplePoints.Any()
                ? 0
                : ViewModel.SamplePoints.Max(samplePoint => samplePoint.Id) + 1;

            var crosshair = CreateCrosshair();
            crosshair.Tag = index;

            Canvas.SetLeft(crosshair, x - (_previewCrosshair.Width / 2));
            Canvas.SetTop(crosshair, y - (_previewCrosshair.Height / 2));

            SamplePointsCanvas.Children.Add(crosshair);

            var normalisedX = x / GetCaptureSize().Width;
            var normalisedY = y / GetCaptureSize().Height;

            ViewModel.SamplePoints.Add(new(index, normalisedX, normalisedY));

            return;
        }

        if(isRightClick)
        {
            // Round coords to closest multiple of StepSize
            var x = Math.Round(pointer.Position.X / HorizontalStepSize) * HorizontalStepSize;
            var y = Math.Round(pointer.Position.Y / VerticalStepSize) * VerticalStepSize;

            var border =
                SamplePointsCanvas
                .Children
                .OfType<Border>()
                .FirstOrDefault(child =>
                    child.Tag != "PreviewCrosshair"
                    && Canvas.GetLeft(child) == x - (_previewCrosshair.Width / 2)
                    && Canvas.GetTop(child) == y - (_previewCrosshair.Height / 2));

            if(border is null)
            {
                return;
            }

            SamplePointsCanvas.Children.Remove(border);

            var samplePoint = ViewModel.SamplePoints.FirstOrDefault(samplePoint => samplePoint.Id == (int)border.Tag);
            if(samplePoint is not null)
            {
                ViewModel.SamplePoints.Remove(samplePoint);
            }

            return;
        }
    }

    private void SamplePointsCanvas_PointerEntered(object sender, PointerRoutedEventArgs e)
    {
        _previewCrosshair.Visibility = Visibility.Visible;
    }

    private void SamplePointsCanvas_PointerExited(object sender, PointerRoutedEventArgs e)
    {
        _previewCrosshair.Visibility = Visibility.Collapsed;
    }

    private void ScreenCaptureHandler()
    {
        using var screen = _screenCaptureService.CaptureDesktop();
        if(screen is null) return;

        using var stream = new MemoryStream();
        var bitmapImage = new BitmapImage();
        screen.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
        stream.Position = 0;
        bitmapImage.SetSource(stream.AsRandomAccessStream());

        ScreenCaptureImage.Source = bitmapImage;
    }

    private Border CreateCrosshair()
    {
        return new()
        {
            Width = 7,
            Height = 7,
            Background = new SolidColorBrush(Colors.Transparent),
            BorderBrush = new SolidColorBrush(Colors.Red),
            BorderThickness = new(1),

            Child = new Microsoft.UI.Xaml.Controls.Grid
            {
                IsHitTestVisible = false,
                Children =
                {
                    new Border
                    {
                        Width = 1,
                        Height = 13,
                        Margin = new(0, -3, 0, -3),
                        Background = new SolidColorBrush(Colors.Red),
                    },

                    new Border
                    {
                        Width = 13,
                        Height = 1,
                        Margin = new(-3, 0, -3, 0),
                        Background = new SolidColorBrush(Colors.Red),
                    },
                }
            }
        };
    }

    private void DrawGridLines()
    {
        var requestedHorizontalLines = GetCaptureSize().Width / HorizontalStepSize;
        var requestedVerticalLines = GetCaptureSize().Height / VerticalStepSize;
        var thicc = 2;

        var horizontalLines = GetCaptureSize().Width / requestedHorizontalLines;
        var verticalLines = GetCaptureSize().Height / requestedVerticalLines;

        // Horizontal lines
        for(var i = 1; i <= requestedHorizontalLines; i++)
        {
            var line = new Line
            {
                X1 = horizontalLines * i,
                X2 = horizontalLines * i,
                Y1 = 0,
                Y2 = GetCaptureSize().Height,
                Stroke = new SolidColorBrush(Colors.White),
                StrokeThickness = i % thicc == 0 ? 2 : 1,
                Opacity = 0.05,
            };

            GridLinesCanvas.Children.Add(line);
        }

        // Vertical lines
        for(var i = 1; i <= requestedVerticalLines; i++)
        {
            var line = new Line
            {
                X1 = 0,
                X2 = GetCaptureSize().Width,
                Y1 = verticalLines * i,
                Y2 = verticalLines * i,
                Stroke = new SolidColorBrush(Colors.White),
                StrokeThickness = i % thicc == 0 ? 2 : 1,
                Opacity = 0.05,
            };

            GridLinesCanvas.Children.Add(line);
        }
    }

    private Size GetCaptureSize() => _scale;

    private Size ScaleSize(Size size, Size maxSize)
    {
        var ratioX = (double)size.Width / maxSize.Width;
        var ratioY = (double)size.Height / maxSize.Height;
        var ratio = Math.Min(ratioX, ratioY);

        var width = (int)(maxSize.Width * ratio);
        var height = (int)(maxSize.Height * ratio);

        return new Size(width, height);
    }
}
