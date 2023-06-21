using Windows.UI;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;

namespace WizBulbApi.WinUI;

public class ColorBrushConverter : IValueConverter
{
    public bool IgnoreAlpha { get; set; }

    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if(value is Color color)
        {
            if(IgnoreAlpha)
            {
                color.A = 255;
            }

            return new SolidColorBrush(color);
        }

        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
