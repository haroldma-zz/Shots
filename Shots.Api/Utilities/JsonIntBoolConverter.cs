using System;
using Newtonsoft.Json;

namespace Shots.Api.Utilities
{
    /// <summary>
    /// Use to convert an integer to a boolean
    /// </summary>
    public class JsonIntBoolConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(((bool)value) ? 1 : 0);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (string.IsNullOrEmpty(reader.Value as string)) return false;

            int value;
            if (int.TryParse((string)reader.Value, out value))
                return value == 1;

            return (bool) reader.Value;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(bool);
        }
    }
}