using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using CaptiveAire.VPL.Extensions;
using CaptiveAire.VPL.Interfaces;
using CaptiveAire.VPL.Model;

namespace CaptiveAire.VPL
{
    public class Parameter : Element, IParameter,  IElementDropTarget
    {
        private string _prefix;
        private string _postfix;
        private bool _isDraggingOver;
        private readonly string _id;
        private readonly IVplType _type;
        private object _value;
        private readonly Lazy<Visual> _editor;

        public Parameter(IElementOwner owner, string id, IVplType type) 
            : base(owner, Model.SystemElementIds.Parameter)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            _id = id;
            _type = type;

            Value = type.DefaultValue;

            _editor = new Lazy<Visual>(type.CreateVisual);
        }

        public string Id
        {
            get { return _id; }
        }

        public string Prefix
        {
            get { return _prefix; }
            set
            {
                _prefix = value;
                RaisePropertyChanged();
            }
        }

        public string Postfix
        {
            get { return _postfix; }
            set
            {
                _postfix = value;
                RaisePropertyChanged();
            }
        }

        public object GetValue()
        {
            return Value;
        }

        public void SetValue(object value)
        {
            Value = value;
            Owner.MarkDirty();
        }

        public bool CanDrop()
        {
            return Next == null;
        }

        public bool IsDraggingOver
        {
            get { return _isDraggingOver; }
            set
            {
                _isDraggingOver = value;
                RaisePropertyChanged();
            }
        }

        public void Drop(IElement element)
        {
            this.CommonDrop(element);
            Owner.MarkDirty();
        }

        public bool CanDrop(Type elementType, Guid? returnType)
        {
            if (Next != null)
                return false;

            if (elementType == null)
                return false;

            if (typeof (IOperator).IsAssignableFrom(elementType))
                return true;

            if (returnType == null)
                return false;

            if (returnType.Value == _type.Id)
                return true;

            var vplType = Owner.GetVplType(returnType.Value);

            if (vplType == null)
                return false;

            if (_type.NetType.IsAssignableFrom(vplType.NetType))
                return true;

            // This will have to be evaluated at runtime.
            if (returnType.Value == VplTypeId.Any)
                return true;

            return false;
        }

        public object Value
        {
            get { return _value; }
            set
            {
                _value = value.GetConvertedValue(Type.NetType); 
                RaisePropertyChanged();
                Owner.MarkDirty();
            }
        }

        public IVplType Type
        {
            get { return _type; }
        }

        public async Task<object> EvaluateAsync(CancellationToken cancellationToken)
        {
            var op = Next as IOperator;

            if (op == null) 
                return Value;

            var raw = await op.EvaluateAsync(cancellationToken);

            //Convert to the type that we're supposed to be supporting.
            return raw.GetConvertedValue(Type.NetType);
        }

        public Visual Editor
        {
            get { return _editor.Value; }
        }
    }
}