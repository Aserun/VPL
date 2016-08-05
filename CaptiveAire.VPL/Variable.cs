using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using CaptiveAire.VPL.Extensions;
using CaptiveAire.VPL.Interfaces;
using Cas.Common.WPF;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace CaptiveAire.VPL
{
    internal class Variable : ViewModelBase, IVariable
    {
        private string _name;
        private readonly IElementOwner _owner;
        private readonly IVplType _type;
        private readonly Guid _id;
        private object _value;
        private readonly Lazy<Visual> _editor;

        public event EventHandler NameChanged;

        public Variable(IElementOwner owner, IVplType type, Guid id)
        {
            if (owner == null) throw new ArgumentNullException(nameof(owner));
            if (type == null) throw new ArgumentNullException(nameof(type));

            _owner = owner;
            _type = type;
            _id = id;

            RenameCommand = new RelayCommand(Rename, CanRename);
            DeleteCommand = new RelayCommand(() => Delete(), CanDelete);

            _editor = new Lazy<Visual>(type.CreateVisual);
            _value = type.DefaultValue;
        }

        public ICommand RenameCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }

        protected virtual void Rename()
        {
            //TODO: DI this
            var textEditService = new TextEditService();

            textEditService.EditText(Name, "Name", "Rename Variable", t => Name = t, t => !string.IsNullOrWhiteSpace(t));
        }
        protected virtual bool CanRename()
        {
            return true;
        }

        public virtual bool Delete()
        {
            //Check to see if this variable is in use
            if (_owner.GetAllElements().OfType<IVariableReference>().Any(v => v.VariableId == Id))
            {
                MessageBox.Show($"Variable '{Name}' is use.", "Unable to delete variable");
                return false;
            }
            
            //Remove the variable
            _owner.RemoveVariable(this);
            return true;
        }

        public virtual bool CanDelete()
        {
            return true;
        }

        public virtual string Name
        {
            get { return _name; }
            set
            {
                _name = value; 
                RaisePropertyChanged();
                NameChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public IVplType Type
        {
            get { return _type; }
        }

        public object Value
        {
            get { return _value; }
            set
            {
                _value = value.GetConvertedValue(Type.NetType); 
                RaisePropertyChanged();
            }
        }

        public Visual Editor
        {
            get { return _editor.Value; }
        }

        public Guid Id
        {
            get { return _id; }
        }

        public virtual bool Persist
        {
            get { return true; }
        }
    }
}