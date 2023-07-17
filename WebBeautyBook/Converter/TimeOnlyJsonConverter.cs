using Newtonsoft.Json;
using System.Globalization;

namespace WebBeautyBook.Converter
{
    public sealed class TimeOnlyJsonConverter : JsonConverter<TimeOnly>
    {
        private const string Format = "HH:mm";

        public override TimeOnly ReadJson(JsonReader reader, Type objectType, TimeOnly existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            return TimeOnly.ParseExact((reader.Value as string), Format, CultureInfo.InvariantCulture);
        }

        public override void WriteJson(JsonWriter writer, TimeOnly value, JsonSerializer serializer)
        {
            var isoTime = value.ToString("O");
            writer.WriteValue(value.ToString(Format, CultureInfo.InvariantCulture));
        }
    }
}