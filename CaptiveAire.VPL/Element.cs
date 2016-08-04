using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using CaptiveAire.VPL.Extensions;
using CaptiveAire.VPL.Interfaces;
using CaptiveAire.VPL.Metadata;
using CaptiveAire.VPL.Model;
using CaptiveAire.VPL.ViewModel;
using Cas.Common.WPF;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Newtonsoft.Json;

namespace CaptiveAire.VPL
{
    public abstract class Element : ViewModelBase, IElement
    {
        private readonly Parameters _parameters = new Parameters();
        private readonly IElementOwner _owner;
        private readonly Guid _elementTypeId;
        private Point _location;
        private Point _startLocation;
        private bool _isDragging;
        private readonly IBlocks _blocks = new Blocks();
        private readonly ObservableCollection<IElementAction> _actions = new ObservableCollection<IElementAction>();

        private IElement _previous;
        private IElement _next;

        private object _label;
        private Color _backgroundColor;
        private Color _foregroundColor;

        protected Element(IElementOwner owner, Guid elementTypeId)
        {
            if (owner == null) throw new ArgumentNullException(nameof(owner));

            _owner = owner;
            _elementTypeId = elementTypeId;

            DeleteCommand = new RelayCommand(Delete, CanDelete);
            CopyCommand = new RelayCommand(Copy);
            CutCommand = new RelayCommand(Cut, CanDelete);

            BackgroundColor = Colors.Plum;
            ForegroundColor = Colors.Black;
        }

        public ICommand DeleteCommand { get; private set; }

        public ICommand CopyCommand { get; private set; }

        public ICommand CutCommand { get; private set; }

        public Color BackgroundColor
        {
            get { return _backgroundColor; }
            set
            {
                _backgroundColor = value; 
                RaisePropertyChanged();
            }
        }

        public Color ForegroundColor
        {
            get { return _foregroundColor; }
            set
            {
                _foregroundColor = value; 
                RaisePropertyChanged();
            }
        }

        private void Cut()
        {
            Copy();
            Delete();
        }

        private void Delete()
        {
            //Delete
            this.DisconnectFromPrevious(true);
            Owner.MarkDirty();
        }

        private void Copy()
        {
            var metadata = this.ToMetadata();

            var json = JsonConvert.SerializeObject(metadata, Formatting.Indented);

            Clipboard.SetData(nameof(ElementMetadata), json);
        }

        private bool CanDelete()
        {
            return true;
        }
        public virtual object Label
        {
            get { return _label; }
            set
            {
                _label = value;
                RaisePropertyChanged();
            }
        }

        public bool IsDragging
        {
            get { return _isDragging; }
            set
            {
                _isDragging = value;
                RaisePropertyChanged();
            }
        }

        public IElement Previous
        {
            get { return _previous; }
            set
            {
                _previous = value;
                RaisePropertyChanged();
                Owner.MarkDirty();
            }
        }

        public IElement Next
        {
            get { return _next; }
            set
            {
                _next = value;
                RaisePropertyChanged();
                Owner.MarkDirty();
            }
        }

        public Parameters Parameters
        {
            get { return _parameters; }
        }

        public virtual IElements Children
        {
            get { return null; }
        }

        public Point Location
        {
            get { return _location; }
            set
            {
                _location = value;
                RaisePropertyChanged();
                Owner.MarkDirty();
            }
        }

        public IElementOwner Owner
        {
            get { return _owner; }
        }

        public virtual Guid ElementTypeId
        {
            get { return _elementTypeId; }
        }

        public void StartMove()
        {
            _startLocation = Location;
        }

        public void ContinueMove(Vector vector)
        {
            _location = _startLocation + vector;

            RaisePropertyChanged(() => Location);
        }

        public void CancelMove()
        {
            _location = _startLocation;

            RaisePropertyChanged(() => Location);
        }

        public void CompleteMove(Vector vector)
        {
            Location = _startLocation + vector;
        }

        public  IElement GetNext()
        {
            return Next;
        }

        public void SetNext(IElement element)
        {
            Next = element;
        }

        public IElement GetPrevious()
        {
            return Previous;
        }

        public void SetPrevious(IElement element)
        {
            Previous = element;
        }

        public virtual string GetData()
        {
            return null;
        }

        IParameters IElement.Parameters
        {
            get { return Parameters; }
        }

        public IBlocks Blocks
        {
            get { return _blocks; }
        }

        public void AddAction(IElementAction action)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));

            _actions.Add(action);
        }

        protected void AddActions(IEnumerable<IElementAction> actions)
        {
            if (actions == null) throw new ArgumentNullException(nameof(actions));

            _actions.AddRange(actions);
        }

        protected void RemoveAction(IElementAction action)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));

            _actions.Remove(action);
        }

        public IEnumerable<IElementAction> Actions
        {
            get { return _actions; }
        }
    }
}