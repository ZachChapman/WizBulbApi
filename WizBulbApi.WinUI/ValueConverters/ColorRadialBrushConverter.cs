using Windows.UI;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI;

namespace WizBulbApi.WinUI;

public class ColorRadialBrushConverter : IValueConverter
{
    public int? AlphaOverride { get; set; }

    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if(value is Color color)
        {
            if(AlphaOverride is not null)
            {
                color.A = (byte)AlphaOverride;
            }

            var brush = new RadialGradientBrush();

            brush.GradientStops.Add(new GradientStop
            {
                Color = color,
                Offset = 0
            });

            brush.GradientStops.Add(new GradientStop
            {
                Color = Colors.Transparent,
                Offset = 1
            });

            return brush;
        }

        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}