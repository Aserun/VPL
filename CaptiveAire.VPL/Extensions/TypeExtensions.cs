using System;
using CaptiveAire.VPL.Interfaces;

namespace CaptiveAire.VPL.Extensions
{
    internal static class TypeExtensions
    {
        public static bool IsStatement(this Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            return typeof (IStatement).IsAssignableFrom(type);
        }

        public static bool IsOperator(this Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            return typeof(IOperator).IsAssignableFrom(type);
        }
    }
}