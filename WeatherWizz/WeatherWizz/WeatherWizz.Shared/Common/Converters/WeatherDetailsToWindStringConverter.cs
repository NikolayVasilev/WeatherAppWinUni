using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Data;

namespace WeatherWizz.Common.Converters
{
    public class WeatherDetailsToWindStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var weatherDetails = value as WeatherDetailsInfoViewModel;

            if(weatherDetails != null)
            {
                var measurementString = App.ApplicationViewModel.MeasurementUnits == MeasurementUnits.Metric ? "kph" : "mph";
                var speedValue = App.ApplicationViewModel.MeasurementUnits == MeasurementUnits.Metric ? weatherDetails.WindSpeed * 1.61 : weatherDetails.WindSpeed;

                return string.Format("{0}, {1:0.00} {2}", weatherDetails.WindDirection, speedValue, measurementString);
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value;
        }
    }
}
