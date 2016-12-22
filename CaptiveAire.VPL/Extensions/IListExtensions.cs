using System;
using System.Collections.Generic;

namespace CaptiveAire.VPL.Extensions
{
    public static class IListExtensions
    {
        public static void AddRange<T>(this IList<T> list, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                list.Add(item);
            }
        }

        internal static int? IndexOf<T>(this IEnumerable<T> items, Func<T, bool> func)
        {
            int index = 0;

            foreach (var item in items)
            {
                if (func(item))
                {
                    return index;
                }

                index++;
            }

            return null;
        }
            
    }
}