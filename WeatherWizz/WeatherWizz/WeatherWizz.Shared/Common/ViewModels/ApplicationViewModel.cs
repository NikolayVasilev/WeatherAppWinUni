using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using WeatherWizz.DataModel;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Linq;
using System.Net;
using Windows.UI.Popups;

namespace WeatherWizz.Common
{
    public class ApplicationViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<string> savedCities;
        public ObservableCollection<string> SavedCities
        {
            get
            {
                return new ObservableCollection<string>(this.savedCities.Distinct());
            }
            set
            {
                if (this.savedCities != value)
                {
                    this.savedCities = value;
                    OnPropertyChanged("SavedCities");
                }
            }
        }

        private bool isBusy;

        public bool IsBusy
        {
            get
            {
                return isBusy;
            }
            set
            {
                if (isBusy != value)
                {
                    isBusy = value;
                    OnPropertyChanged("IsBusy");
                }
            }
        }


        private MeasurementUnits measurementUnits;
        public MeasurementUnits MeasurementUnits
        {
            get
            {
                return this.measurementUnits;
            }
            set
            {
                if (this.measurementUnits != value)
                {
                    this.measurementUnits = value;
                    OnPropertyChanged("MeasurementUnits");
                    RefreshWeatherInfo(true);
                }
            }
        }

        private string selectedLocation;
        public string SelectedLocation
        {
            get
            {
                return this.CurrentWeatherInfo != null ? this.CurrentWeatherInfo.CityName : this.selectedLocation;
            }
            set
            {
                if (this.selectedLocation != value && value != null)
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
                    OnPropertyChanged("SelectedLocation");
                    IsBusy = false;
                }
            }
        }

        private async void RefreshWeatherInfo(bool forceRefresh = false)
        {
            if (Window.Current.Dispatcher != null)
            {
                await Window.Current.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High, async () =>
                {
                    IsBusy = true;
                    try
                    {
                        var weatherinfo = await WeatherDataServiceConsumer.GetWeatherInformation(this.selectedLocation, forceRefresh);
                        this.CurrentWeatherInfo = weatherinfo;
                    }
                    catch
                    {
                        MessageDialog dialog = new MessageDialog("Service unavailable", "Error");
                        dialog.ShowAsync();
                    }

                });
            }
        }

        public List<MeasurementUnits> MeasurementUnitsCollection
        {
            get
            {
                return new List<MeasurementUnits>(Enum.GetValues(typeof(MeasurementUnits)).Cast<MeasurementUnits>());
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
