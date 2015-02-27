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
using Windows.Devices.Geolocation;

namespace WeatherWizz.DataModel
{
    public sealed class WeatherDataServiceConsumer
    {
        private static DateTime lastUpdate;
        private static WeatherInfoViewModel currentWeatherInfo;
        private static string currentLocationName = string.Empty;

        public static async Task<WeatherInfoViewModel> GetWeatherInformation(string locationName, bool forceRefresh = false)
        {
            if ((DateTime.Now - lastUpdate).TotalMinutes < 60 && 
                locationName == WeatherDataServiceConsumer.currentLocationName && 
                currentWeatherInfo != null &&
                !forceRefresh)
            {
                return currentWeatherInfo;
            }

            //lat=35&lon=139

            string dailyForecastString = string.Format("http://api.openweathermap.org/data/2.5/forecast?q={0}&mode=xml", locationName);
            string weeklyForecastString = string.Format("http://api.openweathermap.org/data/2.5/forecast/daily?q={0}&mode=xml&units=metric&cnt=7", locationName);

#if WINDOWS_PHONE_APP
            if (locationName != "CurrentLocation")
            {
                dailyForecastString = string.Format("http://api.openweathermap.org/data/2.5/forecast?q={0}&mode=xml", locationName);
                weeklyForecastString = string.Format("http://api.openweathermap.org/data/2.5/forecast/daily?q={0}&mode=xml&units=metric&cnt=7", locationName);
            }
            else
            {
                Geolocator geolocator = new Geolocator();
                geolocator.DesiredAccuracyInMeters = 50;

                Geoposition geoposition = await geolocator.GetGeopositionAsync();

                dailyForecastString = string.Format("http://api.openweathermap.org/data/2.5/forecast?lat={0}&lon={1}&mode=xml", geoposition.Coordinate.Latitude, geoposition.Coordinate.Longitude);
                weeklyForecastString = string.Format("http://api.openweathermap.org/data/2.5/forecast/daily?lat={0}&lon={1}&mode=xml&units=metric&cnt=7", geoposition.Coordinate.Latitude, geoposition.Coordinate.Longitude);
            }
#endif

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(dailyForecastString);

            var response = await request.GetResponseAsync().ConfigureAwait(false);
            var stream = response.GetResponseStream();

            var streamReader = new StreamReader(stream);
            string responseText = streamReader.ReadToEnd();

            XDocument xDocDailyForecast = null;
            try
            {
                xDocDailyForecast = XDocument.Parse(responseText);
            }
            catch
            {
                return null;
            }

            var weatherInfo = new WeatherInfoViewModel();

            weatherInfo.CityName = xDocDailyForecast.Root.Element("location").Element("name").Value;
            weatherInfo.CountryName = xDocDailyForecast.Root.Element("location").Element("country").Value;

            DateTime cleanDate;
            string dateString = xDocDailyForecast.Root.Element("sun").Attribute("rise").Value;
            DateTime.TryParseExact(dateString, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out cleanDate);

            weatherInfo.Sunrise = cleanDate.ToLocalTime();

            dateString = xDocDailyForecast.Root.Element("sun").Attribute("set").Value;
            DateTime.TryParseExact(dateString, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out cleanDate);

            weatherInfo.Sunset = cleanDate.ToLocalTime();

            ParseDaylyForecast(xDocDailyForecast, weatherInfo);

            HttpWebRequest weeklyRequest = (HttpWebRequest)WebRequest.Create(weeklyForecastString);

            response = await weeklyRequest.GetResponseAsync().ConfigureAwait(false);
            stream = response.GetResponseStream();

            streamReader = new StreamReader(stream);
            responseText = streamReader.ReadToEnd();

            XDocument xDocWeeklyForecast = null;

            try
            {
                xDocWeeklyForecast = XDocument.Parse(responseText);
            }
            catch
            {
                return null;
            }

            ParseWeeklyForecast(xDocWeeklyForecast, weatherInfo);

            lastUpdate = DateTime.Now;
            currentLocationName = locationName;
            currentWeatherInfo = weatherInfo;

            return weatherInfo;
        }

        private static void ParseWeeklyForecast(XDocument xDoc, WeatherInfoViewModel weatherInfo)
        {
            var forecast = xDoc.Root.Element("forecast").Elements();

            foreach (var node in forecast)
            {
                var weatherDetails = new WeatherDetailsInfoViewModel();
                DateTime cleanDate;

                string dateString = node.Attribute("day").Value;
                DateTime.TryParseExact(dateString, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out cleanDate);

                weatherDetails.Time = cleanDate.ToLocalTime();
                weatherDetails.WeatherState = node.Element("symbol").Attribute("name").Value.ToUpper();
                weatherDetails.WeatherStateCode = int.Parse(node.Element("symbol").Attribute("number").Value);

                weatherDetails.WindDirection = node.Element("windDirection").Attribute("code").Value;
                weatherDetails.WindSpeed = double.Parse(node.Element("windSpeed").Attribute("mps").Value);

                var temperatureNode = node.Element("temperature");

                weatherDetails.CurrentTemperature = double.Parse(temperatureNode.Attribute("day").Value);
                weatherDetails.MinimalTemperature = double.Parse(temperatureNode.Attribute("min").Value);
                weatherDetails.MaximalTemperature = double.Parse(temperatureNode.Attribute("max").Value);
                weatherDetails.Pressure = double.Parse(node.Element("pressure").Attribute("value").Value);
                weatherDetails.Humidity = double.Parse(node.Element("humidity").Attribute("value").Value);

                weatherInfo.WeeklyTermWeatherDetails.Add(weatherDetails);
            }
        }

        private static void ParseDaylyForecast(XDocument xDoc, WeatherInfoViewModel weatherInfo)
        {
            var forecast = xDoc.Root.Element("forecast").Elements();

            foreach (var node in forecast)
            {
                var weatherDetails = new WeatherDetailsInfoViewModel();
                DateTime cleanDate;

                string dateString = node.Attribute("from").Value;
                DateTime.TryParseExact(dateString, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out cleanDate);

                weatherDetails.Time = cleanDate.ToLocalTime();
                weatherDetails.WeatherState = node.Element("symbol").Attribute("name").Value.ToUpper();
                weatherDetails.WeatherStateCode = int.Parse(node.Element("symbol").Attribute("number").Value);

                weatherDetails.WindDirection = node.Element("windDirection").Attribute("code").Value;
                weatherDetails.WindSpeed = double.Parse(node.Element("windSpeed").Attribute("mps").Value);

                var temperatureNode = node.Element("temperature");

                weatherDetails.CurrentTemperature = double.Parse(temperatureNode.Attribute("value").Value);
                weatherDetails.MinimalTemperature = double.Parse(temperatureNode.Attribute("min").Value);
                weatherDetails.MaximalTemperature = double.Parse(temperatureNode.Attribute("max").Value);
                weatherDetails.Pressure = double.Parse(node.Element("pressure").Attribute("value").Value);
                weatherDetails.Humidity = double.Parse(node.Element("humidity").Attribute("value").Value);

                weatherInfo.ShortTermWeatherDetails.Add(weatherDetails);
            }
        }

        public static List<string> GetDefaultCities()
        {
            return new List<string>()
            {
                "Sofia",
                "London",
                "Paris",
                "Barcelona",
                "Rome",
                "New York",
                "Warszawa",
                "Las Vegas"
            };
        }
    }
}
