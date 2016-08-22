using System;
using System.Collections.ObjectModel;
using CaptiveAire.VPL.Interfaces;

namespace CaptiveAire.VPL
{
    internal class Elements : ObservableCollection<IElement>, IElements
    {
        protected override void SetItem(int index, IElement item)
        {
            Items[index].Parent = null;

            item.Parent = this;

            base.SetItem(index, item);
        }

        protected override void InsertItem(int index, IElement item)
        {
            item.Parent = this;

            base.InsertItem(index, item);
        }

        protected override void RemoveItem(int index)
        {
            Items[index].Parent = null;

            base.RemoveItem(index);
        }

        public void RemoveElement(IElement element)
        {
            Remove(element);
        }

        public bool CanDrop(Type elementType, Guid? returnType)
        {
            return typeof (IStatement).IsAssignableFrom(elementType);
        }

        public void Drop(IElement element, IElement droppedElement)
        {
            var index = IndexOf(element);

            if (index >= 0)
            {
                Insert(index + 1, droppedElement);
            }
        }
    }

    
}