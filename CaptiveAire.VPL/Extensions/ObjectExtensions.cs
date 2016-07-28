using System;
using System.ComponentModel;

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
    }
}