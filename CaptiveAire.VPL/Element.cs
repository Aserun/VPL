using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using CaptiveAire.VPL.Extensions;
using CaptiveAire.VPL.Interfaces;
using CaptiveAire.VPL.Metadata;
using CaptiveAire.VPL.Model;
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
        private readonly IBlocks _blocks = new Blocks();
        private readonly IElementFactory _factory;
        private readonly ObservableCollection<IElementAction> _actions = new ObservableCollection<IElementAction>();

        private IElement _previous;
        private IElement _next;
        private Point _location;
        private Point _startLocation;
        private bool _isDragging;
        private bool _hasError;
        private string _error;
        private object _label;
        private Color _backgroundColor;
        private Color _foregroundColor;

        protected Element(IElementCreationContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            _owner = context.Owner;
            _factory = context.Factory;

            DeleteCommand = new RelayCommand(Delete, CanDelete);
            CopyCommand = new RelayCommand(Copy);
            CutCommand = new RelayCommand(Cut, CanDelete);

            BackgroundColor = Colors.Plum;
            ForegroundColor = Colors.Black;
        }

        public ICommand DeleteCommand { get; private set; }

        public ICommand CopyCommand { get; private set; }

        public ICommand CutCommand { get; private set; }

        public bool HasError
        {
            get { return _hasError; }
            private set
            {
                _hasError = value;
                RaisePropertyChanged();
            }
        }

        public string Error
        {
            get { return _error; }
            private set
            {
                _error = value;
                RaisePropertyChanged();
            }
        }

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

        public IElementFactory Factory
        {
            get { return _factory; }
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

        public Guid ElementTypeId
        {
            get { return _factory.ElementTypeId; }
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

        public virtual void ClearErrors()
        {
            HasError = false;
            Error = null;
        }

        public void SetError(string message)
        {
            Error = message;
            HasError = true;
        }

        public IError[] CheckForErrors()
        {
            //Check for errors.s
            var errors = CheckForErrorsCore();

            if (errors.Any())
            {
                //Format the errors
                var formattedErrors = errors
                    .Select(e => $"[{e.Level}] - {e.Message}");

                //Put all of these errors in one string.
                var message = string.Join(Environment.NewLine, formattedErrors);

                //Set the error
                SetError(message);
            }

            return errors;
        }

        protected virtual IError[] CheckForErrorsCore()
        {
            return new IError[] { };
        }
    }
}