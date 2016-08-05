using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using CaptiveAire.VPL.Interfaces;

namespace CaptiveAire.VPL.Converters
{
    public class CollapsedIfNotStatementConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is IStatement ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}