using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Telerik.Core;
using WeatherWizz.DataModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace WeatherWizz.Navigation
{
    public sealed partial class SearchHeader : UserControl
    {
        public SearchHeader()
        {
            this.InitializeComponent();

            this.SearchTextBox.KeyDown += SearchTextBox_KeyDown;
        }

        void SearchTextBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if(e.Key == Windows.System.VirtualKey.Enter)
            {
                string searchText = this.SearchTextBox.Text;
                this.TryAddCity(searchText);
            }
        }

        public void BackButtonClick(object sender, RoutedEventArgs e)
        {
            Frame frame = Window.Current.Content as Frame;
            if (frame == null)
            {
                return;
            }

            if (frame.CanGoBack)
            {
                frame.GoBack();
            }
        }

        public void AddButtonClick(object sender, RoutedEventArgs e)
        {
            string searchText = this.SearchTextBox.Text;
            this.TryAddCity(searchText);
        }

        private async void TryAddCity(string cityName)
        {
            var cityInfo = await WeatherDataServiceConsumer.GetWeatherInformation(cityName);
            if (cityInfo != null)
            {
                if (!App.ApplicationViewModel.SavedCities.Contains(cityInfo.CityName))
                {
                    App.ApplicationViewModel.SavedCities.Insert(1, cityName);
                }
            }
            else
            {
                MessageDialog dialog = new MessageDialog("Cannot locate city" , "Error");
                await dialog.ShowAsync();
            }

            this.SearchTextBox.Text = string.Empty;
        }
    }
}