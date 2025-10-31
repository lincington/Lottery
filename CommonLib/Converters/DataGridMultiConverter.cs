using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Common.Converters
{
    public class DataGridMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (values == null || values.Length < 2)
                    return DependencyProperty.UnsetValue;

                if (values[0] == DependencyProperty.UnsetValue || values[1] == DependencyProperty.UnsetValue)
                    return DependencyProperty.UnsetValue;

                if (!TryToInt(values[0], out int H) || !TryToInt(values[1], out int S))
                    return DependencyProperty.UnsetValue;

                if (H > S )
                {
                    return new SolidColorBrush(Colors.Red);
                }
                else
                {
                    if (H == S)
                    {
                        return new SolidColorBrush(Colors.Yellow);
                    }
                    else
                    {
                        return new SolidColorBrush(Colors.Green);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
           
        }
        private bool TryToInt(object v, out int result)
        {
            if (v is int i) { result = i; return true; }
            var s = v?.ToString();
            return int.TryParse(s, out result);
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
