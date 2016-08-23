using System;
using Newtonsoft.Json;

namespace CaptiveAire.VPL.Extensions
{
    public static class ObjectExtensions
    {
        public static object GetConvertedValue(this object value, Type targetType)
        {
            if (value == null)
                return null;

            if (value.GetType() == targetType)
                return value;

            return Convert.ChangeType(value, targetType);
        }

        public static T Clone<T>(this T source)
        {
            var json = JsonConvert.SerializeObject(source);

            return JsonConvert.DeserializeObject<T>(json);
        }
    }

  
}