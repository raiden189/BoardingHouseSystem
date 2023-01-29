using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;
using BoardingHouseSystem.Models;

namespace BoardingHouseSystem.Services
{
    public class LoginConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] != null && values[1] != null && values.Length == 2)
            {
                string user = values[0].ToString();
                string pass = values[1].ToString();
                return new Login { Username = user, Password = pass };
            }
            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
