using System;
using System.Runtime.Serialization.Formatters;
using System.Text;
using Windows.Storage;
using Newtonsoft.Json;
using Shots.Core.Helpers;
using Shots.Core.Interfaces.Utilities;

namespace Shots.Core.Utilities
{
    public class SettingsUtility : ISettingsUtility
    {
        private readonly IStorageUtility _storageUtility;

        public SettingsUtility(IStorageUtility storageUtility)
        {
            _storageUtility = storageUtility;
        }

        public string Read(string key)
        {
            return Read<string>(key);
        }

        public T Read<T>(string key)
        {
            return Read(key, default(T));
        }

        public T Read<T>(string key, T defaulValue)
        {
            return Read(key, defaulValue, SettingsStrategy.Local);
        }

        public T Read<T>(string key, SettingsStrategy strategy)
        {
            return Read(key, default(T), strategy);
        }

        public T Read<T>(string key, T defaultValue, SettingsStrategy strategy)
        {
            object obj;

            //Try to get the settings value
            if (GetContainerFromStrategy(strategy).Values.TryGetValue(key, out obj))
            {
                try
                {
                    //Try casting it
                    return (T) obj;
                }
                catch
                {
                    // ignored
                }
            }
            return defaultValue;
        }

        public T ReadJsonAs<T>(string key)
        {
            var value = Read(key);
            var obj = default(T);

            //No string found, return the default
            if (string.IsNullOrEmpty(value)) return obj;

            if (value.StartsWith("settings-fallback-") && value.EndsWith(".json"))
            {
                var copy = value;
                value = AsyncHelper.RunSync(() => _storageUtility.ReadStringAsync(copy));
            }

            try
            {
                obj = JsonConvert.DeserializeObject<T>(value, new JsonSerializerSettings {
                    TypeNameHandling = TypeNameHandling.Objects,
                    TypeNameAssemblyFormat = FormatterAssemblyStyle.Full
                });
            }
            catch
            {
                // ignored
            }

            return obj;
        }

        public void Write(string key, object value)
        {
            Write(key, value, SettingsStrategy.Local);
        }

        public void Write(string key, object value, SettingsStrategy strategy)
        {
            if (GetContainerFromStrategy(strategy).Values.ContainsKey(key))
                GetContainerFromStrategy(strategy).Values[key] = value;

            else
                GetContainerFromStrategy(strategy).Values.Add(key, value);
        }

        public void WriteAsJson(string key, object value)
        {
            var json = JsonConvert.SerializeObject(value, new JsonSerializerSettings {
                TypeNameHandling = TypeNameHandling.Objects,
                TypeNameAssemblyFormat = FormatterAssemblyStyle.Full
            });
            try
            {
                Write(key, json);
            }
            catch
            {
                // since we are working with complex type, the size might have been to big to store
                // resort to file
                var filename = $"settings-fallback-{key}.json";
                Write(key, filename);
                AsyncHelper.RunSync(() => _storageUtility.WriteStringAsync(filename, json));
            }
        }

        private static ApplicationDataContainer GetContainerFromStrategy(SettingsStrategy location)
        {
            switch (location)
            {
                case SettingsStrategy.Roaming:
                    return ApplicationData.Current.RoamingSettings;
                default:
                    return ApplicationData.Current.LocalSettings;
            }
        }
    }
}