using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace TeTile.Converters;

public class CourseDataStringToVisibilityConverter: IValueConverter {
    public object? Convert(object? value,Type targetType,object? parameter,CultureInfo culture) {
        string val = (string)value!;
        return val != "0.0" && val != "???" && val != "";
    }
    public object? ConvertBack(object? value,Type targetType,object? parameter,CultureInfo culture) {
        throw new NotSupportedException();
    }
}