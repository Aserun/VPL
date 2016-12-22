using System;
using System.Collections.ObjectModel;
using System.Linq;
using CaptiveAire.VPL.Extensions;
using CaptiveAire.VPL.Interfaces;

namespace CaptiveAire.VPL
{
    internal class Elements : ObservableCollection<IElement>, IElements
    {
        private readonly IElementOwner _owner;

        public Elements(IElementOwner owner)
        {
            if (owner == null) throw new ArgumentNullException(nameof(owner));

            _owner = owner;
        }

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

        public bool CanDrop(IElementClipboardData data)
        {
            return _owner.AreAllItemsStatements(data);
        }

        public void Drop(IElement element, IElementClipboardData data)
        {
            if (CanDrop(data))
            {
                var index = IndexOf(element);

                if (index >= 0)
                {
                    //Create the elements
                    var elements = _owner.CreateElements(data);

                    foreach (var newElement in elements)
                    {
                        Insert(++index, newElement);
                    }

                    _owner.SaveUndoState();
                }
            }
        }

        public IElement[] GetChildren()
        {
            return this.ToArray();
        }
    }
}