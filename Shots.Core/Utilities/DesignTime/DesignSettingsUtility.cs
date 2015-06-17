using Shots.Core.Utilities.Interfaces;

namespace Shots.Core.Utilities.DesignTime
{
    public class DesignSettingsUtility : ISettingsUtility
    {
        public string Read(string key)
        {
            return null;
        }

        public T Read<T>(string key)
        {
            return default(T);
        }

        public T Read<T>(string key, T defaulValue)
        {
            return default(T);
        }

        public T Read<T>(string key, SettingsStrategy strategy)
        {
            return default(T);
        }

        public T Read<T>(string key, T defaultValue, SettingsStrategy strategy)
        {
            return default(T);
        }

        public T ReadJsonAs<T>(string key)
        {
            return default(T);
        }

        public void Write(string key, object value)
        {

        }

        public void Write(string key, object value, SettingsStrategy strategy)
        {

        }

        public void WriteAsJson(string key, object value)
        {

        }
    }
}