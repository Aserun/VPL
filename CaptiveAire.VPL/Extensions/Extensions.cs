using System;
using System.Collections.Generic;
using System.Linq;

namespace CaptiveAire.VPL.Extensions
{
    public static class Extensions
    {
        public static string CreateUniqueName(this IEnumerable<string> existingNames, string format, StringComparison stringComparison = StringComparison.OrdinalIgnoreCase)
        {
            int index = 1;

            string currentName = string.Format(format, index);

            string[] materialized = existingNames.ToArray();

            while (materialized.Any(n => string.Compare(n, currentName, stringComparison) == 0))
            {
                index++;

                currentName = string.Format(format, index);
            }

            return currentName;
        }
    }
}