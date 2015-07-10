using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Shots.Web.Converters
{
    /// <summary>
    ///     Workaround for apis that return an empty array to represent null. (Like Shots -.-)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class ArrayForNullConverter<T> : JsonConverter
    {
        public override bool CanWrite => false;

        public override bool CanConvert(Type objectType)
        {
            return (objectType != typeof (T));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            var token = JToken.Load(reader);

            if (token.Type == JTokenType.Array)
                return null;
            return token.ToObject<T>(serializer);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}