using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using WeatherWizz.Common;
using Windows.Data.Json;

namespace WeatherWizz.DataModel
{
    public sealed class WeatherDataServiceConsumer
    {
        public static async Task<WeatherInfoViewModel> GetWeatherInformation(string cityName)
        {
            string queryString = "http://api.openweathermap.org/data/2.5/forecast?q=NewYork,us&mode=xml";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(queryString);

            var response = await request.GetResponseAsync().ConfigureAwait(false);
            var stream = response.GetResponseStream();

            var streamReader = new StreamReader(stream);
            string responseText = streamReader.ReadToEnd();

            XDocument xDoc = XDocument.Parse(responseText);

            var weatherInfo = new WeatherInfoViewModel();

            weatherInfo.CityName = xDoc.Root.Element("location").Element("name").Value;
            weatherInfo.CountryName = xDoc.Root.Element("location").Element("country").Value;

            var forecast = xDoc.Root.Element("forecast").Elements();

            foreach (var node in forecast)
            {
                var weatherDetails = new WeatherDetailsInfoViewModel();

                string dateString = node.Attribute("from").Value;
                DateTime cleanDate;

                DateTime.TryParseExact(dateString,"yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out cleanDate);

                weatherDetails.Time = cleanDate;
                weatherDetails.WeatherState = node.Element("symbol").Attribute("name").Value;
                weatherDetails.WindDirection = node.Element("windDirection").Attribute("code").Value;
                weatherDetails.WindSpeed = double.Parse(node.Element("windSpeed").Attribute("mps").Value);
                
                var temperatureNode = node.Element("temperature");

                weatherDetails.CurrentTemperature = double.Parse(temperatureNode.Attribute("value").Value);
                weatherDetails.MinimalTemperature = double.Parse(temperatureNode.Attribute("min").Value);
                weatherDetails.MaximalTemperature = double.Parse(temperatureNode.Attribute("max").Value);
                weatherDetails.Pressure = double.Parse(node.Element("pressure").Attribute("value").Value);

                weatherInfo.WeatherDetails.Add(weatherDetails);
            }

            return weatherInfo;
        }
    }
}
