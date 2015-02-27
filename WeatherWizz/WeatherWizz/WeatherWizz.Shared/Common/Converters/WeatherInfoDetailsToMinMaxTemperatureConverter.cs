using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Data;

namespace WeatherWizz.Common.Converters
{
    public class WeatherInfoDetailsToMinMaxTemperatureConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            WeatherDetailsInfoViewModel viewModel = value as WeatherDetailsInfoViewModel;

            if (viewModel != null)
            {
                var metric = App.ApplicationViewModel.MeasurementUnits == MeasurementUnits.Metric ? "C" : "F";

                var resultString = string.Format("{0:0.0}°{1}/{2:0.0}°{1}", 
                    Math.Round(viewModel.MinimalTemperature, 2), metric, Math.Round(viewModel.MaximalTemperature, 2));

                return resultString;
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
