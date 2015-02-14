using System;
using Newtonsoft.Json;

namespace Shots.Api.Utilities
{
    /// <summary>
    ///     Use to convert epoch timestapm properties to .NET DateTime objects
    /// </summary>
    public class JsonEpochDateTimeConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof (DateTime);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
                                        JsonSerializer serializer)
        {
            // Try to parse as a number, else as DateTime object
            long valueLong;

            if (reader.Value is long) valueLong = (long) reader.Value;
            else
            {
                if (!long.TryParse((string) reader.Value, out valueLong))
                {
                    DateTime dateTime;
                    DateTime.TryParse((string) reader.Value, out dateTime);
                    return dateTime;
                }
            }

            var t = valueLong;
            return t.FromUnixTimestamp();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}