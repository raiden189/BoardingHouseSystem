using Android.Locations;
using BoardingHouseSystem.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace BoardingHouseSystem.Services
{
    public class AccountConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            bool accountType = false;
            string firstName = string.Empty, middleName = string.Empty, lastName = string.Empty, fbLink = string.Empty, contactNum = string.Empty, adddress = string.Empty;
            if (values[0] != null)
                accountType = values[0].ToString().Equals("Student") ? true : false;
            if (values[1] != null)
                firstName = values[1].ToString();
            if (values[2] != null)
                middleName = values[2].ToString();
            if (values[3] != null)
                lastName = values[3].ToString();
            if (values[4] != null)
                contactNum = values[4].ToString();
            if (values[5] != null)
                adddress = values[5].ToString();
            //if (values[6] != null)
            //    fbLink = values[6].ToString();

            return new Account
            {
                isStudent = accountType,
                FirstName = firstName,
                MiddleName = middleName,
                LastName = lastName,
                ContactNum = contactNum,
                Address = adddress,
                FbAccount = fbLink
            };
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
