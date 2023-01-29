
using Javax.Xml.Validation;

namespace BoardingHouseSystem.ViewModels
{
    public class IsNotNullOrEmptyRule<T> : IValidationRule<T>
    {
        public string ValidationMessage { get; set; }

        public bool Check(T value)
        {
            if (value.Equals(null))
                return false;

            var str = value as string;
            return !string.IsNullOrEmpty(str);
        }
    }
}
