using System;

namespace BoardingHouseSystem.ViewModels
{
    public class PhoneNumberValidationRule<T> : IValidationRule<T>
    {
        public string ValidationMessage { get; set ; }

        public bool Check(T value)
        {
            try
            {
                if (value.Equals(null))
                    return false;
                Convert.ToInt32(value);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
