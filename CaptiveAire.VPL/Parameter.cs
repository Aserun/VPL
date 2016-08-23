using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using CaptiveAire.VPL.Extensions;
using CaptiveAire.VPL.Interfaces;

namespace CaptiveAire.VPL
{
    internal class Parameter : Element, IParameter, IElementParent
    {
        private string _prefix;
        private string _postfix;
        private bool _isDraggingOver;
        private readonly string _id;
        private readonly IVplType _type;
        private object _value;
        private readonly Lazy<Visual> _editor;
        private IOperator _operator;

        public Parameter(IElementCreationContext context, string id, IVplType type) 
            : base(context)
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
            return Operator == null;
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

        public bool CanDrop(IElementClipboardData data)
        {
            if (Operator != null)
                return false;

            if (data.Items.Length != 1)
                return false;

            var factory = Owner.Context.ElementFactoryManager.GetFactory(data.Items[0].ElementMetadata.ElementTypeId);

            if (factory == null)
                return false;

            return factory.ElementType.IsOperator();
        }

        public void Drop(IElementClipboardData data)
        {
            if (CanDrop(data))
            {
                var elements = Owner.CreateElements(data);

                Operator = elements[0] as IOperator;
            }
        }

        void IElementParent.RemoveElement(IElement element)
        {
            if (Operator == element)
            {
                Operator = null;
            }
        }

        //public bool CanDrop(Type elementType, Guid? returnType)
        //{
        //    if (Operator != null)
        //        return false;

        //    if (elementType == null)
        //        return false;

        //    if (typeof (IOperator).IsAssignableFrom(elementType))
        //        return true;

        //    if (returnType == null)
        //        return false;

        //    if (returnType.Value == _type.Id)
        //        return true;

        //    var vplType = Owner.GetVplTypeOrThrow(returnType.Value);

        //    if (vplType == null)
        //        return false;

        //    if (_type.NetType.IsAssignableFrom(vplType.NetType))
        //        return true;

        //    // This will have to be evaluated at runtime.
        //    if (returnType.Value == VplTypeId.Any)
        //        return true;

        //    return false;
        //}

        bool IElementParent.CanDrop(IElementClipboardData data)
        {
            return false;

            
        }

        void IElementParent.Drop(IElement element, IElementClipboardData data)
        {

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

        public async Task<object> EvaluateAsync(IExecutionContext executionContext, CancellationToken cancellationToken)
        {
            var op = Operator;

            if (op == null) 
                return Value;

            var raw = await op.EvaluateAsync(executionContext, cancellationToken);

            //Convert to the type that we're supposed to be supporting.
            return raw.GetConvertedValue(Type.NetType);
        }

        public Visual Editor
        {
            get { return _editor.Value; }
        }

        public IOperator Operator
        {
            get { return _operator; }
            set
            {
                var old = _operator;

                if (old != null)
                {
                    old.Parent = null;
                }

                _operator = value;
                RaisePropertyChanged();
                Owner.MarkDirty();

                var newOperator = value;

                if (newOperator != null)
                {
                    newOperator.Parent = this;
                }
            }
        }
    }
}