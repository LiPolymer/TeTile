using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace TeTile.Converters;

public class SolidColorOpacityAdjuster: IMultiValueConverter {
    public object? Convert(IList<object?> values,Type targetType,object? parameter,CultureInfo culture) {
        SolidColorBrush brush = (SolidColorBrush)values[0]!;
        double opacity = 0.5;
        try {
            opacity = (double)values[1]!;
        } catch {
            //ignored
        }
        brush.Opacity = opacity;
        return brush;
    }
}