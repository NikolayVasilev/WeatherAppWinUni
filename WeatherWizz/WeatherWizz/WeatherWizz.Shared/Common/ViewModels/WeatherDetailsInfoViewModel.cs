using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeatherWizz.Common
{
    public class WeatherDetailsInfoViewModel
    {
        public double CurrentTemperature { get; set; }
        public double MinimalTemperature { get; set; }
        public double MaximalTemperature { get; set; }
        public double Pressure { get; set; }
        public double Humidity { get; set; }
        public DateTime Time { get; set; }
        public string WeatherState { get; set; }
        public double WindSpeed { get; set; }
        public string WindDirection { get; set; }

        public string Winds
        {
            get
            {
                double windSpeedKph = this.WindSpeed * 1.609344;

                return string.Format("{0}, {1:0.00} kph", this.WindDirection, windSpeedKph);
            }
        }

        public int WeatherStateCode { get; set; }
    }
}
