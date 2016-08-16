using System;
using System.Threading;
using System.Threading.Tasks;
using CaptiveAire.VPL.Extensions;
using CaptiveAire.VPL.Interfaces;
using Newtonsoft.Json;

namespace CaptiveAire.VPL
{
    internal class VariableGetter : OperatorBase, IVariableReference
    {
        private readonly Guid _id;
        private readonly Lazy<IVariable> _variable;

        public VariableGetter(IElementCreationContext context, Guid variableId) 
            : base(context)
        {
            if (variableId == Guid.Empty)
                throw new ArgumentException("variableId was default value.", nameof(variableId));

            _id = variableId;

            _variable = new Lazy<IVariable>(GetVariable);
        }

        public VariableGetter(IElementCreationContext context)
          : base(context)
        {
            if (string.IsNullOrWhiteSpace(context.Data))
                throw new ArgumentNullException($"Custom serialization data missing for {GetType().Name}");

            var data = JsonConvert.DeserializeObject<VariableGetterData>(context.Data);

            _id = data.VariableId;

            _variable = new Lazy<IVariable>(GetVariable);
        }

        private IVariable GetVariable()
        {
            // Get the variable
            var variable = Owner.GetVariableOrThrow(_id);

            //Listen to the name changed event.
            variable.NameChanged += VariableNameChanged;

            return variable;
        }

        private void VariableNameChanged(object sender, EventArgs e)
        {
            RaisePropertyChanged(() => Label);
        }

        public override object Label
        {
            get { return $"Get [{_variable.Value.Name}]" ; }
            set {  }
        }

        public override string GetData()
        {
            var data = new VariableGetterData()
            {
                VariableId = _id
            };

            return JsonConvert.SerializeObject(data);
        }

        public override IVplType Type
        {
            get { return _variable.Value.Type; }
        }

        public override Task<object> EvaluateAsync(IExecutionContext executionContext, CancellationToken token)
        {
            return Task.FromResult(_variable.Value.Value);
        }

        private class VariableGetterData
        {
            public Guid VariableId { get; set; }
        }

        public Guid VariableId
        {
            get { return _id; }
        }
    }
}