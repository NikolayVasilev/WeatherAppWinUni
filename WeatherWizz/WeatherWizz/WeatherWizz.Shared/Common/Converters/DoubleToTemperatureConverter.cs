using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Data;

namespace WeatherWizz.Common
{
    public class DoubleToTemperatureConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return string.Format("{0:0.0}°C", Math.Round((double)value, 2));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value;
        }
    }
}
