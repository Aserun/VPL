using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CaptiveAire.VPL.Model
{
    public class HierarchicalNumber : IEnumerable<int>
    {
        private readonly List<int> _numbers;

        public const char Separator = '.';

        public HierarchicalNumber()
        {
            _numbers = new List<int>(1);
        }

        public HierarchicalNumber(string value)
        {
            var result = Parse(value);

            _numbers = new List<int>(result);
        }

        public HierarchicalNumber(IEnumerable<int> hierarchy)
        {
            if (hierarchy == null) throw new ArgumentNullException(nameof(hierarchy));

            _numbers = new List<int>(hierarchy);
        }

        public int NumberOfLevels
        {
            get { return _numbers.Count; }
        }

        public override string ToString()
        {
            return string.Join(Separator.ToString(), _numbers.Select(n => n.ToString()));
        }

        /// <summary>
        /// Gets the next hierarchical number at this level.
        /// For example, '1.4' begets '1.5'
        /// </summary>
        /// <returns></returns>
        public HierarchicalNumber GetNextSibling()
        {
            //Make a copy
            var components = _numbers.ToArray();

            components[components.Length - 1]++;

            return new HierarchicalNumber(components);
        }

        /// <summary>
        /// Gets the first child of this number.
        /// Example '1.4' begets '1.4.1'
        /// </summary>
        /// <returns></returns>
        public HierarchicalNumber GetFirstChild()
        {
            //Make a copy
            var components = _numbers.ToList();

            components.Add(1);

            return new HierarchicalNumber(components);
        }

        public IEnumerator<int> GetEnumerator()
        {
            return _numbers.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public static bool TryParse(string s, out HierarchicalNumber result)
        {
            result = null;

            if (string.IsNullOrWhiteSpace(s))
                return false;

            var split = s.Split(Separator);

            var components = new List<int>();

            foreach (var rawComponent in split)
            {
                int parsedInt;

                if (!int.TryParse(rawComponent, out parsedInt))
                {
                    return false;
                }

                components.Add(parsedInt);
            }

            if (components.Count == 0)
            {
                return false;
            }

            result = new HierarchicalNumber(components);

            return true;
        }

        public static HierarchicalNumber Parse(string s)
        {
            HierarchicalNumber result;

            if (!TryParse(s, out result))
            {
                throw new FormatException($"Unable to parse '{s}' into a hierarchical number.");
            }

            return result;
        }

    }
}