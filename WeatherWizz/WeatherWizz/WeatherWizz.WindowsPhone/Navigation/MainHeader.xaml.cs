using System;
using System.Linq;
using Telerik.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace WeatherWizz.Navigation
{
    public sealed partial class MainHeader : UserControl
    {        
        public MainHeader()
        {
            this.InitializeComponent();
        }

        public void SettingsClick(object sender, RoutedEventArgs e)
        {
        }

        public void AddButtonClick(object sender, RoutedEventArgs e)
        {
            var ownerPage = ElementTreeHelper.FindVisualAncestor<Page>(this);

            if (ownerPage != null)
            {
                ownerPage.Frame.Navigate(typeof(FindCityPage));
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            App.ApplicationViewModel.SelectedLocation = (string)e.AddedItems.FirstOrDefault();
        }
    }
}