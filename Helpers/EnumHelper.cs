using QWellApp.DBConnection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QWellApp.Helpers
{
    public class EnumHelper
    {
        public static string GetDescriptionFromEnum(Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = field.GetCustomAttributes(typeof(DescriptionAttribute), false)
                                 .FirstOrDefault() as DescriptionAttribute;
            return attribute?.Description ?? value.ToString();
        }

        public static List<KeyValuePair<int, string>> GetIntEnumDescriptionList<T>() where T : Enum
        {
            return Enum.GetValues(typeof(T))
                       .Cast<Enum>()
                       .Select(e =>
                           new KeyValuePair<int, string>(
                               Convert.ToInt32(e),    // Key: int value of enum (1,2,3...)
                               GetDescriptionFromEnum(e)      // Value: description text
                           )
                       )
                       .ToList();
        }

        public static string GetDescriptionFromString<TEnum>(string enumValue) where TEnum : struct, Enum
        {
            if (string.IsNullOrWhiteSpace(enumValue))
                return string.Empty;

            // Try to parse the string to the enum
            if (!Enum.TryParse<TEnum>(enumValue, true, out var parsedEnum))
                return string.Empty;

            // Get the Description attribute
            var field = typeof(TEnum).GetField(parsedEnum.ToString());
            var attribute = field.GetCustomAttributes(typeof(DescriptionAttribute), false)
                                 .FirstOrDefault() as DescriptionAttribute;

            return attribute?.Description ?? parsedEnum.ToString();
        }
    }
}
