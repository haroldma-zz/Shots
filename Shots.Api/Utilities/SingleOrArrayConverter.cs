using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Shots.Api.Utilities
{
    /// <summary>
    /// Certain apis instead of return a json array alway, they send the single object.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class SingleOrArrayConverter<T> : JsonConverter
    {
        public override bool CanWrite
        {
            get { return false; }
        }

        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof (List<T>));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            var token = JToken.Load(reader);
            // If it is an array, then cast it as a list, 
            // other wise we are deailing with a single object, so make a new list with it
            return token.Type == JTokenType.Array 
                ? token.ToObject<List<T>>(serializer) 
                : new List<T> {token.ToObject<T>(serializer)};
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}