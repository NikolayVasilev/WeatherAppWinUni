using System;
using System.Linq;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace WeatherWizz.Views
{
    public sealed partial class HourlyView : UserControl
    {
        public HourlyView()
        {
            this.InitializeComponent();

            //this.SizeChanged += WeatherInformationNowScreenView_SizeChanged;
        }

        private void WeatherInformationNowScreenView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var currentOrientation = ApplicationView.GetForCurrentView().Orientation;

            if (currentOrientation == ApplicationViewOrientation.Portrait)
            {
                Grid.SetColumnSpan(this.HourlyTemperatureChart, 2);
                Grid.SetRowSpan(this.HourlyTemperatureChart, 1);

                Grid.SetRow(this.DetailsListView, 1);
                Grid.SetColumnSpan(this.DetailsListView, 2);
                Grid.SetColumn(this.DetailsListView, 0);
                Grid.SetRowSpan(this.DetailsListView, 1);
            }
            else
            {
                Grid.SetColumnSpan(this.HourlyTemperatureChart, 1);
                Grid.SetRowSpan(this.HourlyTemperatureChart, 2);

                Grid.SetRow(this.DetailsListView, 0);
                Grid.SetColumnSpan(this.DetailsListView, 1);
                Grid.SetColumn(this.DetailsListView, 1);
                Grid.SetRowSpan(this.DetailsListView, 2);
            }
        }
    }
}
