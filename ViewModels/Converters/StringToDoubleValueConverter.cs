using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace CurrencyConverter.ViewModels.Converters
{
    public class StringToDoubleValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            double num;
            string strvalue = value as string;
            if (double.TryParse(strvalue, out num))
            {
                return num;
            }
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            return value.ToString();
        }
    }
}
