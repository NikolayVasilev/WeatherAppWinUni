using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

namespace WeatherWizz.Common
{
    public class WeatherStateToIConPathConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var weatherStatecode = (int)value;
#if WINDOWS_PHONE_APP 
            string basePath = "ms-appx:///Assets/weathericons128p/{0}.png";
#else
            string basePath = "ms-appx:///Assets/weathericons512p/{0}.png";
#endif
            string weatherStateString = string.Empty;

            if(weatherStatecode >= 200 && weatherStatecode <=232)
            {
                weatherStateString = "thunderstorm";
            }
            else if(weatherStatecode >= 300 && weatherStatecode <= 321 ||
                weatherStatecode>=521 && weatherStatecode<=531)
            {
                weatherStateString = "showerrain";
            }
            else if(weatherStatecode >= 500 && weatherStatecode <= 504)
            {
                weatherStateString = "rain";
            }
            else if (weatherStatecode == 511 || 
                weatherStatecode >=600 && weatherStatecode <= 622)
            {
                weatherStateString = "snow";
            }
            else if (weatherStatecode >= 701 && weatherStatecode <= 781)
            {
                weatherStateString = "mist";
            }
            else if (weatherStatecode == 800)
            {
                weatherStateString = "clearsky";
            }
            else if (weatherStatecode == 801)
            {
                weatherStateString = "fewclouds";
            }
            else if (weatherStatecode == 802)
            {
                weatherStateString = "scatteredclouds";
            }
            else if (weatherStatecode == 803 || weatherStatecode == 804)
            {
                weatherStateString = "brokenclouds";
            }

            BitmapImage bitmapImage = new BitmapImage();
            Uri uri = new Uri(string.Format(basePath, weatherStateString));
            bitmapImage.UriSource = uri;

            return bitmapImage;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
