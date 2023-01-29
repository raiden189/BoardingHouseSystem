using System;
using System.IO;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace BoardingHouseSystem.Services
{
    public class ImageJsonConverter : JsonConverter
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return (string)reader.Value;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var base64 = (string)value;
            writer.WriteValue(Convert.FromBase64String(base64));
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Image);
        }
    }
}
