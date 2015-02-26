using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using WeatherWizz.DataModel;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WeatherWizz.Common
{
    public class ApplicationViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<string> SavedCities { get; set; }

        public MeasurementUnits MeasurementUnits { get; set; }

        private string selectedLocation; 
        public string SelectedLocation 
        {
            get
            {
                return this.CurrentWeatherInfo != null ? this.CurrentWeatherInfo.CityName : this.selectedLocation;
            }
            set
            {
                if(this.selectedLocation != value && value != null)
                {
                    if (!this.SavedCities.Contains(value))
                    {
                        this.SavedCities.Add(value);
                    }

                    this.selectedLocation = value;
                    OnPropertyChanged("SelectedLocation");
                    RefreshWeatherInfo();
                }
            }
        }

        private WeatherInfoViewModel currentWeatherInfo;
        public WeatherInfoViewModel CurrentWeatherInfo
        {
            get
            {
                return this.currentWeatherInfo;
            }
            set
            {
                if (this.currentWeatherInfo != value && value != null)
                {
                    this.currentWeatherInfo = value;
                    this.SelectedLocation = this.CurrentWeatherInfo.CityName;
                    OnPropertyChanged("CurrentWeatherInfo");
                }
            }
        }

        private async void RefreshWeatherInfo()
        {
            if (Window.Current.Dispatcher != null)
            {
                await Window.Current.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High, async () =>
                {
                    var weatherinfo = await WeatherDataServiceConsumer.GetWeatherInformation(this.SelectedLocation, true);
                    this.CurrentWeatherInfo = weatherinfo;
                });
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
