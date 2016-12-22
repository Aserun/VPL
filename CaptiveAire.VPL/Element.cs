using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using CaptiveAire.VPL.Extensions;
using CaptiveAire.VPL.Interfaces;
using CaptiveAire.VPL.Model;
using Cas.Common.WPF;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace CaptiveAire.VPL
{
    public abstract class Element : ViewModelBase, IElement
    {
        private readonly Parameters _parameters = new Parameters();
        private readonly IElementOwner _owner;
        private readonly IBlocks _blocks = new Blocks();
        private readonly IElementFactory _factory;
        private readonly ObservableCollection<IElementAction> _actions = new ObservableCollection<IElementAction>();

        private Point _location;
        private bool _isDragging;
        private bool _hasError;
        private string _error;
        private object _label;
        private Color _backgroundColor;
        private Color _foregroundColor;
        private bool _isSelected;

        protected Element(IElementCreationContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            _owner = context.Owner;
            _factory = context.Factory;

            DeleteCommand = new RelayCommand(DeleteSelected, CanDelete);
            CopyCommand = new RelayCommand(Copy);
            CutCommand = new RelayCommand(Cut, CanDelete);
            PasteCommand = new RelayCommand(Paste, CanPaste);

            BackgroundColor = Colors.Plum;
            ForegroundColor = Colors.Black;
        }

        public ICommand DeleteCommand { get; }

        public ICommand CopyCommand { get; }

        public ICommand CutCommand { get; }

        public ICommand PasteCommand { get; }


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

        private void Paste()
        {
            var data = ClipboardUtility.Paste();

            if (data != null)
            {
                var parent = Parent;

                if (parent != null)
                {
                    parent.Drop(this, data);

                    Owner.SaveUndoState();
                }
            }
        }

        private bool CanPaste()
        {
            return ClipboardUtility.CanPaste();
        }

        private void Cut()
        {
            Copy();
            DeleteSelected();
        }

        private void DeleteSelected()
        {
            Owner.SelectionService.EnsureSelected(this);

            foreach (var deletable in Owner.SelectionService.GetSelected().OfType<IDeleteable>())
            {
                deletable.Delete();
            }

            Owner.SaveUndoState();
        }

        private void Delete()
        {
            //Delete
            Parent?.RemoveElement(this);
            Owner.MarkDirty();
        }

        private void Copy()
        {
            Owner.SelectionService.EnsureSelected(this);
            Owner.SelectionService.CopySelected();
        }

        private bool CanDelete()
        {
            return Parent != null;
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

        public IElementFactory Factory
        {
            get { return _factory; }
        }

        public IElementParent Parent { get; set; }

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

        bool IDeleteable.Delete()
        {
            Delete();

            return true;
        }

        bool IDeleteable.CanDelete()
        {
            return CanDelete();
        }

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value; 
                RaisePropertyChanged();
            }
        }

        public virtual ISelectable[] GetSiblings()
        {
            if (Parent == null)
            {
                return new ISelectable[] { this };
            }

            return Parent.GetChildren()
                .Cast<ISelectable>()
                .ToArray();
        }
    }
}