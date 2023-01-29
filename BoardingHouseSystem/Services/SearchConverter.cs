using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;
using BoardingHouseSystem.Models;

namespace BoardingHouseSystem.Services
{
    public class SearchConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            string searchItem = string.Empty, priceType = string.Empty;
            int distanceType = 0;
            if (values[0] != null)
                searchItem = values[0].ToString();
            if (values[1] != null)
                priceType = values[1].ToString();
            if (values[2] != null)
                distanceType = System.Convert.ToInt32(values[2]);

            return new Search
            {
                SearchItem = searchItem,
                SelectedPriceType = priceType,
                SelectedDistanceType = distanceType
            };
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
