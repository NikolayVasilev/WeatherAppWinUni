﻿using System;
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
