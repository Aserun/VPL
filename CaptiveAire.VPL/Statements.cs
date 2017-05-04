//using System;
//using System.Collections.ObjectModel;
//using System.Linq;
//using CaptiveAire.VPL.Extensions;
//using CaptiveAire.VPL.Interfaces;

//namespace CaptiveAire.VPL
//{
//    public class Statements : ObservableCollection<IStatement>, IStatements
//    {
//        private readonly IElementOwner _owner;

//        public Statements(IElementOwner owner)
//        {
//            if (owner == null) throw new ArgumentNullException(nameof(owner));

//            _owner = owner;
//        }

//        protected override void SetItem(int index, IStatement item)
//        {
//            Items[index].Parent = null;

//            item.Parent = this;

//            base.SetItem(index, item);
//        }

//        protected override void InsertItem(int index, IStatement item)
//        {
//            item.Parent = this;

//            base.InsertItem(index, item);
//        }

//        protected override void RemoveItem(int index)
//        {
//            Items[index].Parent = null;

//            base.RemoveItem(index);
//        }

//        public void RemoveStatement(IStatement element)
//        {
//            Remove(element);
//        }

//        public bool CanDrop(IElementClipboardData data)
//        {
//            return _owner.AreAllItemsStatements(data);
//        }

//        public void Drop(IStatement element, IElementClipboardData data)
//        {
//            if (CanDrop(data))
//            {
//                var index = IndexOf(element);

//                if (index >= 0)
//                {
//                    //Create the elements
//                    var statements = _owner.CreateElements(data).Cast<IStatement>();

//                    foreach (var newElement in statements)
//                    {
//                        Insert(++index, newElement);
//                    }

//                    _owner.SaveUndoState();
//                }
//            }
//        }

//        public IStatement[] GetChildren()
//        {
//            return this.ToArray();
//        }
//    }
//}