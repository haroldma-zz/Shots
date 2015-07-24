using System;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Shots.Core.Helpers;

namespace Shots.Core.Extensions
{
    public static class StringExtensions
    {
        public static bool IsValidPhoneNumber(this string text)
            => new Regex(@"(\([2-9]\d\d\)|[2-9]\d\d) ?[-.,]? ?[2-9]\d\d ?[-.,]? ?\d{4}").IsMatch(text);

        public static string ToDigitsOnly(this string text) => new Regex(@"[^\d]").Replace(text, "");
        public static bool IsAnyNullOrEmpty(params string[] values) => values.Any(string.IsNullOrEmpty);

        public static T TryDeserializeJson<T>(this string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch
            {
                return default(T);
            }
        }

        public static object TryDeserializeJsonWithTypeInfo(this string json)
        {
            try
            {
                return JsonConvert.DeserializeObject(json, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All,
                    TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple
                });
            }
            catch
            {
                return null;
            }
        }

        public static string SerializeJsonWithTypeInfo(this object obj)
        {
            try
            {
                return JsonConvert.SerializeObject(obj, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All,
                    TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple
                });
            }
            catch
            {
                return null;
            }
        }

        public static string ToUnaccentedText(this string accentedString)
        {
            return string.IsNullOrEmpty(accentedString) ? accentedString : DiacritisHelper.Remove(accentedString);
        }

        public static string ToSanitizedFileName(this string str, string invalidMessage)
        {
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }

            if (str.Length > 35)
            {
                str = str.Substring(0, 35);
            }

            str = str.ToValidFileNameEnding();

            /*
             * A filename cannot contain any of the following characters:
             * \ / : * ? " < > |
             */
            var name =
                str.Replace("\\", string.Empty)
                    .Replace("/", string.Empty)
                    .Replace(":", " ")
                    .Replace("*", string.Empty)
                    .Replace("?", string.Empty)
                    .Replace("\"", "'")
                    .Replace("<", string.Empty)
                    .Replace(">", string.Empty)
                    .Replace("|", " ");

            return string.IsNullOrEmpty(name) ? invalidMessage : name;
        }

        public static string ToValidFileNameEnding(this string str)
        {
            var isNonAccepted = true;

            while (isNonAccepted)
            {
                var lastChar = str[str.Length - 1];

                isNonAccepted = lastChar == ' ' || lastChar == '.' || lastChar == ';' || lastChar == ':';

                if (isNonAccepted) str = str.Remove(str.Length - 1);
                else break;

                if (str.Length == 0) return str;

                isNonAccepted = lastChar == ' ' || lastChar == '.' || lastChar == ';' || lastChar == ':';
            }

            return str;
        }

        public static string ToHtmlStrippedText(this string str)
        {
            var array = new char[str.Length];
            var arrayIndex = 0;
            var inside = false;

            foreach (var o in str.ToCharArray())
            {
                switch (o)
                {
                    case '<':
                        inside = true;
                        continue;
                    case '>':
                        inside = false;
                        continue;
                }
                if (inside) continue;

                array[arrayIndex] = o;
                arrayIndex++;
            }
            return new string(array, 0, arrayIndex);
        }

        public static string Append(this string left, string right) => left + " " + right;

        public static Uri ToUri(this string url, UriKind kind = UriKind.Absolute)
        {
            return new Uri(url, kind);
        }
    }
}