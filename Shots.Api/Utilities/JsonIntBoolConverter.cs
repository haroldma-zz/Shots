using System;
using Newtonsoft.Json;

namespace Shots.Api.Utilities
{
    /// <summary>
    ///     Use to convert an integer to a boolean
    /// </summary>
    public class JsonIntBoolConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof (bool);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
                                        JsonSerializer serializer)
        {
            if (reader.Value is bool) return (bool) reader.Value;
            return reader.Value as int? == 1;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(((bool) value) ? 1 : 0);
        }
    }
}