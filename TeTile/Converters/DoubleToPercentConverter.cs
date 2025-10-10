using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace TeTile.Converters;

public class DoubleToPercentConverter: IValueConverter {
    public object? Convert(object? value,Type targetType,object? parameter,CultureInfo culture) {
        double val = (double)value!;
        return $"{(val * 100):0}%";
    }
    public object? ConvertBack(object? value,Type targetType,object? parameter,CultureInfo culture) {
        throw new NotSupportedException();
    }
}