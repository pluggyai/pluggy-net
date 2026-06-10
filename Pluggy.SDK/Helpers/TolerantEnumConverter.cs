using System;
using System.Linq;
using Newtonsoft.Json;

namespace Pluggy.SDK.Utils
{
    class TolerantEnumConverter : JsonConverter
    {
        private readonly string _defaultValue;

        public TolerantEnumConverter()
        {
        }

        // Allows configuring an explicit fallback value (e.g. "BRL" for CurrencyCode)
        // via [JsonConverter(typeof(TolerantEnumConverter), "BRL")].
        public TolerantEnumConverter(string defaultValue)
        {
            _defaultValue = defaultValue;
        }

        public override bool CanConvert(Type objectType)
        {
            Type type = IsNullableType(objectType) ? Nullable.GetUnderlyingType(objectType) : objectType;
            return type.IsEnum;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            bool isNullable = IsNullableType(objectType);
            Type enumType = isNullable ? Nullable.GetUnderlyingType(objectType) : objectType;

            string[] names = Enum.GetNames(enumType);

            if (reader.TokenType == JsonToken.String)
            {
                string enumText = reader.Value.ToString();

                if (!string.IsNullOrEmpty(enumText))
                {
                    string match = names
                        .Where(n => string.Equals(n, enumText, StringComparison.OrdinalIgnoreCase))
                        .FirstOrDefault();

                    if (match != null)
                    {
                        return Enum.Parse(enumType, match);
                    }
                }
            }
            else if (reader.TokenType == JsonToken.Integer)
            {
                int enumVal = Convert.ToInt32(reader.Value);
                int[] values = (int[])Enum.GetValues(enumType);
                if (values.Contains(enumVal))
                {
                    return Enum.Parse(enumType, enumVal.ToString());
                }
            }

            if (!isNullable)
            {
                string defaultName = null;

                if (!string.IsNullOrEmpty(_defaultValue))
                {
                    defaultName = names
                        .Where(n => string.Equals(n, _defaultValue, StringComparison.OrdinalIgnoreCase))
                        .FirstOrDefault();
                }

                if (defaultName == null)
                {
                    defaultName = names
                        .Where(n => string.Equals(n, "Unknown", StringComparison.OrdinalIgnoreCase))
                        .FirstOrDefault();
                }

                if (defaultName == null)
                {
                    defaultName = names.First();
                }

                return Enum.Parse(enumType, defaultName);
            }

            return null;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }

        private bool IsNullableType(Type t)
        {
            return (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>));
        }
    }
}
