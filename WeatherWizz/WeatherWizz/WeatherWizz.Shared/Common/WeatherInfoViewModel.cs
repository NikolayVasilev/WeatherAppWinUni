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
                    //OnPropertyChanged("CityName");
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
                    //OnPropertyChanged("CountryName");
                }
            }
        }

        ObservableCollection<WeatherDetailsInfoViewModel> weatherInfos;

        public ObservableCollection<WeatherDetailsInfoViewModel> WeatherDetails
        {
            get
            {
                if (this.weatherInfos == null)
                {
                    this.weatherInfos = new ObservableCollection<WeatherDetailsInfoViewModel>();
                }

                return weatherInfos;
            }
            set
            {
                weatherInfos = value;
            }
        }

        public WeatherDetailsInfoViewModel CurrentWeatherState
        {
            get
            {
                return this.WeatherDetails.FirstOrDefault(wd => wd.Time < DateTime.Now && (DateTime.Now - wd.Time).TotalHours < 3);
            }
        }

    }
}
