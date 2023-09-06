using System.Globalization;
using Newtonsoft.Json;

namespace WebBeautyBook.Converter
{
    /// <summary>
    /// JSON converter for serializing and deserializing <see cref="TimeOnly"/> values in <see cref="Format">HH:mm</see> format.
    /// </summary>
    public class TimeOnlyJsonConverter : JsonConverter<TimeOnly>
    {
        private const string Format = "HH:mm";

        /// <summary>
        /// Reads a JSON representation of a <see cref="TimeOnly"/> value and converts it to a <see cref="TimeOnly"/> object.
        /// </summary>
        /// <param name="reader">The JSON reader.</param>
        /// <param name="objectType">The type of the object to deserialize.</param>
        /// <param name="existingValue">The existing value of the object being read.</param>
        /// <param name="hasExistingValue">A flag indicating whether an existing value is present.</param>
        /// <param name="serializer">The JSON serializer.</param>
        /// <returns>A <see cref="TimeOnly"/> object representing the deserialized value.</returns>
        public override TimeOnly ReadJson(JsonReader reader, Type objectType, TimeOnly existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.Value == null) return default;
            var value = reader.Value.ToString().Substring(0, reader.Value.ToString().IndexOf(':') + 3);
            return TimeOnly.ParseExact(value, Format, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Writes a <see cref="TimeOnly"/> value to a JSON representation in HH:mm format.
        /// </summary>
        /// <param name="writer">The JSON writer.</param>
        /// <param name="value">The <see cref="TimeOnly"/> value to serialize.</param>
        /// <param name="serializer">The JSON serializer.</param>
        public override void WriteJson(JsonWriter writer, TimeOnly value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString(Format, CultureInfo.InvariantCulture));
        }
    }
}