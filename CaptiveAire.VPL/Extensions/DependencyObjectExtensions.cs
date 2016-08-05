using System.Windows;
using System.Windows.Media;

namespace CaptiveAire.VPL.Extensions
{
    internal static class DependencyObjectExtensions
    {
        public static TParent FindVisualParent<TParent>(this DependencyObject reference) 
            where TParent : DependencyObject
        {
            DependencyObject current = reference;

            current = VisualTreeHelper.GetParent(current);

            while (current != null)
            {
                var typed = current as TParent;

                if (typed != null)
                {
                    return typed;
                }

                current = VisualTreeHelper.GetParent(current);
            }

            return null;
        }

        /// <summary>
        /// Finds a visual parent by its name and type
        /// </summary>
        /// <typeparam name="TParent"></typeparam>
        /// <param name="reference"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static TParent FindVisualParent<TParent>(this DependencyObject reference, string name)
            where TParent : FrameworkElement
        {
            DependencyObject current = reference;

            current = VisualTreeHelper.GetParent(current);

            while (current != null)
            {
                var typed = current as TParent;

                if (typed != null && string.CompareOrdinal(typed.Name, name) == 0)
                {
                    return typed;
                }

                current = VisualTreeHelper.GetParent(current);
            }

            return null;
        }
    }
}