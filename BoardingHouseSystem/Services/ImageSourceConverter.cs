using System;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Xamarin.Forms;

namespace BoardingHouseSystem.Services
{
    public class ImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var source = value as string;

            if (string.IsNullOrEmpty(source))
                return null;

            var decodedUrl = HttpUtility.UrlDecode(source, Encoding.UTF8);
            var byteImage = DownlodaImage(decodedUrl);
            var stream = new MemoryStream(byteImage);

            return ImageSource.FromStream(() => stream);
        }

        byte[] DownlodaImage(string url)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                byte[] data = httpClient.GetByteArrayAsync(url).GetAwaiter().GetResult();
                return data;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
