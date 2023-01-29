using Android.Locations;
using BoardingHouseSystem.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace BoardingHouseSystem.Services
{
    public class BoardinHouseConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            string name = string.Empty, contactPerson = string.Empty, contactNumber = string.Empty, address = string.Empty, details = string.Empty;
            int capacity = 0;
            double monthly = 0;
            if (values[0] != null)
                name = values[0].ToString();
            if (values[1] != null)
                contactPerson = values[1].ToString();
            if (values[2] != null)
                contactNumber = values[2].ToString();
            if (values[3] != null)
                address = values[3].ToString();
            if (values[4] != null)
                if (values[4] != string.Empty)
                    if(!values[4].ToString().StartsWith("."))
                        monthly = System.Convert.ToDouble(values[4]);
            if (values[5] != null)
                if(values[5] != string.Empty)
                    if(!values[5].ToString().StartsWith("."))
                        capacity = System.Convert.ToInt32(values[5]);
            if (values[6] != null)
                details = values[6].ToString();

            return new Boarding
            {
                Name = name,
                ContactPerson = contactPerson,
                ContactNumber = contactNumber,
                Address = address,
                MonthlyPayment = monthly,
                Capacity = capacity,
                Details = details
            };
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
