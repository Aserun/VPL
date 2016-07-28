using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using CaptiveAire.VPL.Interfaces;

namespace CaptiveAire.VPL.Extensions
{
    public static class FrameworkElementExtensions
    {
        /// <summary>
        /// Returns the DataContext value of the hits below <paramref name="point"/>.
        /// </summary>
        /// <typeparam name="TDataContext">The type of DataContext to look for.</typeparam>
        /// <param name="rootElement"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static TDataContext[] HitTestForTopDataContext<TDataContext>(this FrameworkElement rootElement,
            Point point)
            where TDataContext : class
        {
            if (rootElement == null) throw new ArgumentNullException(nameof(rootElement));

            var foundList = new List<TDataContext>();

            VisualTreeHelper.HitTest(
                rootElement,
                null,
                hitTestResult =>
                {
                    var frameworkElement = hitTestResult.VisualHit as FrameworkElement;

                    var preFound = frameworkElement?.DataContext as TDataContext;

                    if (preFound != null)
                    {
                        if (!foundList.Contains(preFound))
                        {
                            foundList.Add(preFound);
                        }
                    }

                    return HitTestResultBehavior.Continue;        
                },
                new PointHitTestParameters(point));

            return foundList.ToArray();
        }

        public static IElementDropTarget GetDropTarget(this FrameworkElement designRoot, Point position, Type elementType, Guid? returnType)
        {
            var dropTargets = designRoot?.HitTestForTopDataContext<IElementDropTarget>(position);

            return dropTargets?.FirstOrDefault(d => d.CanDrop(elementType, returnType));
        }
    }
}


