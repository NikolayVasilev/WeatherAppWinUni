using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Data;

namespace WeatherWizz.Common.Converters
{
    public class DoubleToTemperatureConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return string.Format("{0:0.0}°{1}", Math.Round((double)value, 2), 
                App.ApplicationViewModel.MeasurementUnits == MeasurementUnits.Metric ? "C" : "F");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value;
        }
    }
}
