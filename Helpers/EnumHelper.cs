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
        public static string GetDescription(Enum value)
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
                               GetDescription(e)      // Value: description text
                           )
                       )
                       .ToList();
        }
    }
}
