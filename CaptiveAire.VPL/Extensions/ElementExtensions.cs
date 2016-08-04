using System;
using System.Windows;
using CaptiveAire.VPL.Interfaces;

namespace CaptiveAire.VPL.Extensions
{
    public static class ElementExtensions
    {
        public static Guid? GetReturnType(this IElement element)
        {
            var op = element as IOperator;

            return op?.Type.Id;
        }

        public static IElement GetLast(this IElement start)
        {
            if (start == null) throw new ArgumentNullException(nameof(start));

            var current = start;

            while (current.GetNext() != null)
            {
                current = current.GetNext();
            }

            return current;
        }

        public static void ForEach(this IElement start, Action<IElement> action)
        {
            if (start == null) throw new ArgumentNullException(nameof(start));

            var current = start;

            while (current != null)
            {
                action(current);

                current = current.GetNext();
            }
        }

        /// <summary>
        /// Performs an action on all of the elements in a hierarchy. This includes compound components.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="action"></param>
        public static void ForAll(this IElement start, Action<IElement> action)
        {
            if (start == null) throw new ArgumentNullException(nameof(start));

            var current = start;

            while (current != null)
            {
                action(current);

                //Do the same for each block
                foreach (var block in current.Blocks)
                {
                    ForAll(block, action);
                }

                current = current.GetNext();
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <param name="delete">Optionally deletes the element completely.</param>
        public static void DisconnectFromPrevious(this IElement element, bool delete = false)
        {
            if (element == null) throw new ArgumentNullException(nameof(element));

            var previous = element.GetPrevious();

            if (previous != null)
            {
                previous.SetNext(null);
                element.SetPrevious(null);

                if (!delete)
                {
                    element.Owner.Add(element);
                }
            }
            else if (delete)
            {
                element.Owner.Remove(element);
            }
        }

        public static void CommonDrop(this IElement target, IElement dropped)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));
            if (dropped == null) throw new ArgumentNullException(nameof(dropped));

            //Remove the dropped statement from wherever it came from
            dropped.DisconnectFromPrevious();

            dropped.ForEach(s => s.Location = new Point(0, 0));

            //Now hook it up to where it belongs now
            var afterDrop = target.GetNext();

            //The dropped statement is now the next statement
            target.SetNext(dropped);

            //The new statement now points back to this
            dropped.SetPrevious(target);

            //Get the last statement in this chain
            var lastDropped = dropped.GetLast();

            //Hook this up just in case the new statement was dropped in the middle of a chain
            lastDropped.SetNext(afterDrop);

            //If we had a next, make sure that it points back at its new predecessor
            afterDrop?.SetPrevious(lastDropped);

            //Remove the dropped item from the design surface.
            target.Owner.Remove(dropped);
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