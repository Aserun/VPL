using System;
using System.Globalization;
using System.Windows.Data;

namespace CaptiveAire.VPL.Converters
{
    public class PercentageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var temp = System.Convert.ToDouble(value);

                return temp * 100;
            }
            catch (Exception)
            {
                return 0.0;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var temp = System.Convert.ToDouble(value);

                return temp / 100;
            }
            catch (Exception)
            {
                return 0.0;
            }
        }
    }
}