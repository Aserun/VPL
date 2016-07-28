using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace CaptiveAire.VPL.Extensions
{
    public static class ResourceDictionaryExtensions
    {
        public static IEnumerable<TResource> GetResourcesOfType<TResource>(this ResourceDictionary resourceDictionary)
        {
            var dictionary = resourceDictionary as IDictionary;

            if (dictionary == null)
            {
                throw new InvalidOperationException("Unable to find the bloody resources.");
            }

            return dictionary.Values
                .OfType<TResource>();
        }
    }
}