using Microsoft.UI.Xaml.Data;

namespace WizBulbApi.WinUI;

public class StringModifier : IValueConverter
{
    public string Prefix { get; set; }
    public string Suffix { get; set; }

    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if(!string.IsNullOrWhiteSpace(Prefix))
        {
            value = Prefix + value;
        }
        if(!string.IsNullOrWhiteSpace(Suffix))
        {
            value = value + Suffix;
        }

        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
