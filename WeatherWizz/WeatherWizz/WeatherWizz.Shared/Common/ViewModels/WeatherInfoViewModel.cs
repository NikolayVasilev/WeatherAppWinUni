using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Linq;

namespace WeatherWizz.Common
{
    public class WeatherInfoViewModel
    {
        private string cityName;

        public string CityName
        {
            get
            {
                return cityName;
            }
            set
            {
                if (this.cityName != value)
                {
                    cityName = value;
                }
            }
        }

        private string countryName;

        public string CountryName
        {
            get 
            { 
                return countryName;
            }
            set 
            {
                if (this.countryName != value)
                {
                    countryName = value;
                }
            }
        }

        private ObservableCollection<WeatherDetailsInfoViewModel> dailyWeatherDetails;
        public ObservableCollection<WeatherDetailsInfoViewModel> ShortTermWeatherDetails
        {
            get
            {
                if (this.dailyWeatherDetails == null)
                {
                    this.dailyWeatherDetails = new ObservableCollection<WeatherDetailsInfoViewModel>();
                }

                return dailyWeatherDetails;
            }
            set
            {
                dailyWeatherDetails = value;
            }
        }

        public IEnumerable<WeatherDetailsInfoViewModel> DailyWeatherDetails
        {
            get
            {
                return this.ShortTermWeatherDetails.Where(wd => wd.Time < DateTime.Now.AddDays(1));
            }
        }

        private ObservableCollection<WeatherDetailsInfoViewModel> weeklyWeatherDetails;
        public ObservableCollection<WeatherDetailsInfoViewModel> WeeklyTermWeatherDetails
        {
            get
            {
                if (this.weeklyWeatherDetails == null)
                {
                    this.weeklyWeatherDetails = new ObservableCollection<WeatherDetailsInfoViewModel>();
                }

                return weeklyWeatherDetails;
            }
            set
            {
                weeklyWeatherDetails = value;
            }
        }

        public WeatherDetailsInfoViewModel CurrentWeatherState
        {
            get
            {
                return this.ShortTermWeatherDetails.FirstOrDefault(wd => wd.Time < DateTime.Now && (DateTime.Now - wd.Time).TotalHours < 3);
            }
        }


        public DateTime Sunrise 
        { 
            get; 
            set; 
        }

        public DateTime Sunset 
        {
            get; 
            set; 
        }
    }
}
