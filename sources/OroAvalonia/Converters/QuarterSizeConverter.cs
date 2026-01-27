using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace DustInTheWind.ClockAvalonia.ClearClock;

public class QuarterSizeConverter : IValueConverter
{
    public static readonly QuarterSizeConverter Instance = new();

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is double size)
            return size / 4.0;

        return 0.0;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is double size)
            return size * 4.0;

        return 0.0;
    }
}
