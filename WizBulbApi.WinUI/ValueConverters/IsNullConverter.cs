using Microsoft.UI.Xaml.Data;

namespace WizBulbApi.WinUI;

public class IsNullConverter : IValueConverter
{
    public object IsTrue { get; set; } = true;
    public object IsFalse { get; set; } = false;

    public object Convert(object value, Type targetType, object parameter, string language)
    {
        return value is null ? IsTrue : IsFalse;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}