using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Lucide.Avalonia;

namespace TeTile.Converters;

public class TopmostBoolToIconConverter: IValueConverter {
    public object? Convert(object? value,Type targetType,object? parameter,CultureInfo culture) {
        bool val = (bool)value!;
        return val ? LucideIconKind.Pin : LucideIconKind.PinOff;
    }
    public object? ConvertBack(object? value,Type targetType,object? parameter,CultureInfo culture) {
        throw new NotSupportedException();
    }
}