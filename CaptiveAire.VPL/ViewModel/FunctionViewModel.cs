using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using CaptiveAire.VPL.Interfaces;
using CaptiveAire.VPL.Metadata;
using CaptiveAire.VPL.Model;
using Cas.Common.WPF;
using GalaSoft.MvvmLight;

namespace CaptiveAire.VPL.ViewModel
{
    internal class FunctionViewModel : ViewModelBase, IFunction
    {
        private readonly IVplServiceContext _context;
        private readonly Guid _functionId;
        private string _name;
        private double _width = 1000;
        private double _height = 1000;
        private readonly ObservableCollection<IElement> _elements = new ObservableCollection<IElement>();
        private readonly ObservableCollection<IVariable> _variables = new ObservableCollection<IVariable>();
        private readonly OrderedListViewModel<IArgument> _arguments;
        private Guid? _returnTypeId;
        private bool _isDirty;

        public FunctionViewModel(IVplServiceContext context, Guid functionId)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            _context = context;
            _functionId = functionId;

            _arguments = new OrderedListViewModel<IArgument>(
                () => new ArgumentViewModel(new ArgumentMetadata()));
        }

        public bool IsDirty
        {
            get { return _isDirty; }
            private set
            {
                if (_isDirty != value)
                {
                    _isDirty = value;
                    RaisePropertyChanged();
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        public void MarkDirty()
        {
            IsDirty = true;
            Console.WriteLine("MarkDirty");
        }

        public void MarkClean()
        {
            IsDirty = false;
        }

        private IElement CreateElement(IElementFactory factory)
        {
            if (factory == null) throw new ArgumentNullException(nameof(factory));

            var context = new ElementCreationContext(this, null);

            return factory.Create(context);
        }

        public void DropFromToolbox(IElementFactory factory, Point point)
        {
            var element = CreateElement(factory);

            element.Location = point;

            Elements.Add(element);

            MarkDirty();
        }

        public void DropFromToolbox(IElementFactory factory, IElementDropTarget dropTarget)
        {
            var element = CreateElement(factory);

            dropTarget.Drop(element);

            MarkDirty();
        }

        public void Add(IElement element)
        {
            if (element != null && !_elements.Contains(element))
            {
                _elements.Add(element);

                MarkDirty();
            }
        }

        public void Remove(IElement element)
        {
            if (element != null)
            {
                Elements.Remove(element);

                MarkDirty();
            }
        }

        public IEnumerable<IVariable> Variables
        {
            get { return _variables; }
        }

        public void AddVariable(IVariable variable)
        {
            _variables.Add(variable);

            MarkDirty();
        }

        public void RemoveVariable(IVariable variable)
        {
            _variables.Remove(variable);

            MarkDirty();
        }

        public ObservableCollection<IElement> Elements
        {
            get { return _elements; }
        }

        public Guid Id
        {
            get { return _functionId; }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value; 
                RaisePropertyChanged();
                MarkDirty();
            }
        }

        public double Width
        {
            get { return _width; }
            set
            {
                _width = value; 
                RaisePropertyChanged();
                MarkDirty();
            }
        }

        public double Height
        {
            get { return _height; }
            set
            {
                _height = value; 
                RaisePropertyChanged();
                MarkDirty();
            }
        }

        public Guid? ReturnTypeId
        {
            get { return _returnTypeId; }
            set
            {
                _returnTypeId = value; 
                RaisePropertyChanged();
                MarkDirty();
            }
        }

        public IVplServiceContext Context
        {
            get { return _context; }
        }

        public OrderedListViewModel<IArgument> Arguments
        {
            get { return _arguments; }
        }

        private IEnumerable<IElement> EnumerateElements(IElement element)
        {
            var current = element;

            while (current != null)
            {
                foreach (var parameter in current.Parameters)
                {
                    if (parameter.Next != null)
                    {
                        foreach (var parameterElement in EnumerateElements(parameter.Next))
                        {
                            yield return parameterElement;
                        }
                    }
                }

                foreach (var block in current.Blocks)
                {
                    if (block.Next != null)
                    {
                        foreach (var blockElement in EnumerateElements(block.Next))
                        {
                            yield return blockElement;
                        }
                    }
                }

                yield return current;

                current = current.Next;
            }
        }

        public IEnumerable<IElement> GetAllElements()
        {
            return Elements.SelectMany(EnumerateElements);
        }

        public IEnumerable<IElement> GetRootElements()
        {
            return Elements
                .ToArray();
        }

        public void AddArgument(IArgument argument)
        {
            if (argument == null) throw new ArgumentNullException(nameof(argument));
            
            _arguments.Add(argument);

            MarkDirty();
        }
    }
}