using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace WeatherWizz.Views
{
    public sealed partial class EditCitiesView : UserControl
    {
        private List<string> pendingCities = new List<string>();
        private string selectedItem = string.Empty;

        public EditCitiesView()
        {
            this.InitializeComponent();
        }

        public void DeleteButtonClicked(object sender, RoutedEventArgs e)
        {
            foreach (var city in pendingCities)
            {
                App.ApplicationViewModel.SavedCities.Remove(city);
            }

            this.DoneButton.Visibility = Windows.UI.Xaml.Visibility.Visible;
            this.DeleteButton.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }

        public void CancelButtonClicked(object sender, RoutedEventArgs e)
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

        public void DoneButtonClick(object sender, RoutedEventArgs e)
        {
            Frame frame = Window.Current.Content as Frame;

            if (frame != null)
            {
                frame.Navigate(typeof(HubPage));
            }
        }

        public void SelectButtonClicked(object sender, RoutedEventArgs e)
        {
            var item = (string)(sender as Button).DataContext;
            if (item != null)
            {
                App.ApplicationViewModel.SelectedLocation = item;

                Frame frame = Window.Current.Content as Frame;

                if (frame != null)
                {
                    frame.Navigate(typeof(HubPage));
                }
            }
        }

        public void ItemCheckBoxUnchecked(object sender, RoutedEventArgs e)
        {
            var item = (string)(sender as CheckBox).DataContext;
            this.pendingCities.Remove(item);

            if (this.pendingCities.Count == 0)
            {
                this.DoneButton.Visibility = Windows.UI.Xaml.Visibility.Visible;
                this.DeleteButton.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }
        }

        public void ItemCheckBoxChecked(object sender, RoutedEventArgs e)
        {
            var item = (string)(sender as CheckBox).DataContext;
            this.pendingCities.Add(item);

            if (this.pendingCities.Count > 0)
            {
                this.DoneButton.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                this.DeleteButton.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }
        }

        private void CheckBox_Loaded(object sender, RoutedEventArgs e)
        {
            var checkBox = (sender as CheckBox);
            var item = (string)checkBox.DataContext;

            if (item != null)
            {
                if (item.Equals("CurrentLocation"))
                {
                    checkBox.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                }
            }
        }
    }
}