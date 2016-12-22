using System;
using System.Collections.Generic;
using System.Windows;
using CaptiveAire.VPL.Interfaces;

namespace CaptiveAire.VPL.Extensions
{
    /// <summary>
    /// Extension methods for elements.
    /// </summary>
    public static class ElementExtensions
    {
        /// <summary>
        /// Gets the return type of an element. If the element is not an operator, Null is returned.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static Guid? GetReturnType(this IElement element)
        {
            var op = element as IOperator;

            return op?.Type.Id;
        }

        /// <summary>
        /// Performs an action on all of the elements in a hierarchy. This includes compound components.
        /// </summary>
        /// <param name="elements"></param>
        /// <param name="action"></param>
        public static void ForAll(this IElements elements, Action<IElement> action)
        {
            if (elements == null) throw new ArgumentNullException(nameof(elements));

            foreach (var element in elements)
            {
                action(element);
            }
        }

        public static IEnumerable<IElement> EnumerateAllElements(this IElements elements)
        {
            foreach (var element in elements)
            {
                foreach (var parameter in element.Parameters)
                {
                    yield return parameter;
                }

                foreach (var block in element.Blocks)
                {
                    yield return block;

                    foreach (var childElement in block.Elements.EnumerateAllElements())
                    {
                        yield return childElement;
                    }
                }

                yield return element;
            }           
        }

        /// <summary>
        /// Shortcut for adding an element action.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="name"></param>
        /// <param name="execute"></param>
        /// <param name="canExecute"></param>
        /// <returns></returns>
        public static IElementAction AddAction(this Element element, string name, Action execute, Func<bool> canExecute = null)
        {
            if (element == null) throw new ArgumentNullException(nameof(element));
            if (execute == null) throw new ArgumentNullException(nameof(execute));

            var action = new ElementAction(name, execute, canExecute);

            element.AddAction(action);

            return action;
        }
    }
}