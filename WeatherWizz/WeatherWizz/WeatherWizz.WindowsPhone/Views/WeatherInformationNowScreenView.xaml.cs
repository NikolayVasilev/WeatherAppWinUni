using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
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
    public sealed partial class WeatherInformationNowScreenView : UserControl
    {
        public WeatherInformationNowScreenView()
        {
            this.InitializeComponent();

            this.SizeChanged += WeatherInformationNowScreenView_SizeChanged;
        }

        private void WeatherInformationNowScreenView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var currentOrientation = ApplicationView.GetForCurrentView().Orientation;

            if (currentOrientation == ApplicationViewOrientation.Portrait)
            {
                Grid.SetColumnSpan(this.InformationView, 2);
                Grid.SetRowSpan(this.InformationView, 1);

                Grid.SetRow(this.DetailsView, 1);
                Grid.SetColumnSpan(this.DetailsView, 2);
                Grid.SetColumn(this.DetailsView, 0);
                Grid.SetRowSpan(this.DetailsView, 1);
            }
            else
            {
                Grid.SetColumnSpan(this.InformationView, 1);
                Grid.SetRowSpan(this.InformationView, 2);
                
                Grid.SetRow(this.DetailsView, 0);
                Grid.SetColumnSpan(this.DetailsView, 1);
                Grid.SetColumn(this.DetailsView, 1);
                Grid.SetRowSpan(this.DetailsView, 2);
            }
        }
    }
}