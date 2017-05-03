using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace CaptiveAire.VPL
{
    internal class ObservableStack<T> : ICollection,  INotifyCollectionChanged, IReadOnlyCollection<T>
    {
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        private readonly Stack<T> _stack;

        public ObservableStack()
        {
            _stack = new Stack<T>();
        }

        public void Push(T item)
        {
            _stack.Push(item);

            CollectionChanged?.Invoke(this,
                new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, new T[] { item }));
        }

        public T Pop()
        {
            T item = _stack.Pop();

            CollectionChanged?.Invoke(this, 
                new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, new T[] { item }));

            return item;
        }

        public T Peek()
        {
            return _stack.Peek();
        }

        public void Clear()
        {
            _stack.Clear();

            CollectionChanged?.Invoke(this,
                new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _stack.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _stack.GetEnumerator();
        }

        public void CopyTo(Array array, int index)
        {
            ((ICollection)_stack).CopyTo(array, index);
        }

        public int Count
        {
            get { return _stack.Count; }
        }

        public object SyncRoot
        {
            get { return ((ICollection)_stack).SyncRoot; }
        }

        public bool IsSynchronized
        {
            get { return ((ICollection)_stack).IsSynchronized; }
        }
    }
}