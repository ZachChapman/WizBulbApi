using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml;

namespace WizBulbApi.WinUI;

public class ApplicationThemeBoolConverter : IValueConverter
{
    public bool IsTrue { get; set; } = true;
    public bool IsFalse { get; set; } = false;

    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if(value is not ApplicationTheme theme)
        {
            return value;
        }

        return theme is ApplicationTheme.Light;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        if(value is not bool @bool)
        {
            return value;
        }

        return @bool is true ? ApplicationTheme.Light : ApplicationTheme.Dark;
    }
}