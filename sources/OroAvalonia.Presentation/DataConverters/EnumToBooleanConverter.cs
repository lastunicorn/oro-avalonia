using System.Globalization;
using Avalonia.Data.Converters;

namespace DustInTheWind.OroAvalonia.Presentation.DataConverters;

public class EnumToBooleanConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null || parameter == null)
            return false;

        return value.Equals(parameter);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool boolValue && boolValue && parameter != null)
            return parameter;

        return null;
    }
}
