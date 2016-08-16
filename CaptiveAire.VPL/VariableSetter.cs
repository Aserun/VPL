using System;
using System.Threading;
using System.Threading.Tasks;
using CaptiveAire.VPL.Extensions;
using CaptiveAire.VPL.Interfaces;
using Newtonsoft.Json;

namespace CaptiveAire.VPL
{
    internal class VariableSetter : Statement, IVariableReference
    {
        private readonly IVariable _variable;
        private readonly IParameter _parameter;

        public VariableSetter(IElementCreationContext context, Guid variableId) 
            : base(context)
        {
            if (variableId == Guid.Empty)
                throw new ArgumentException("variableId was default value.", nameof(variableId));

            _variable = context.Owner.GetVariableOrThrow(variableId);
            _variable.NameChanged += VariableNameChanged;
            _parameter = AddParameter(_variable);
        }

        public VariableSetter(IElementCreationContext context) 
            : base(context)
        {
            if (string.IsNullOrWhiteSpace(context.Data))
                throw new ArgumentNullException($"Custom serialization data missing for {GetType().Name}");

            var data = JsonConvert.DeserializeObject<VariableSetterData>(context.Data);

            _variable = context.Owner.GetVariableOrThrow(data.VariableId);
            _variable.NameChanged += VariableNameChanged;
            _parameter = AddParameter(_variable);
        }

        private void VariableNameChanged(object sender, EventArgs e)
        {
            RaisePropertyChanged(() => Label);
        }

        private IParameter AddParameter(IVariable variable)
        {
            var parameter = Owner.CreateParameter("value", variable.Type);

            Parameters.Add(parameter);

            return parameter;
        }

        public override object Label
        {
            get { return $"Set [{_variable.Name}]"; }
            set { }
        }

        public override string GetData()
        {
            var data = new VariableSetterData()
            {
                VariableId = _variable.Id
            };

            return JsonConvert.SerializeObject(data);
        }

        protected override async Task ExecuteCoreAsync(IExecutionContext executionContext, CancellationToken cancellationToken)
        {
            var value = await _parameter.EvaluateAsync(executionContext, cancellationToken);

            _variable.Value = value;
        }

        private class VariableSetterData
        {
            public Guid VariableId { get; set; }
        }

        public Guid VariableId
        {
            get { return _variable.Id; }
        }
    }
}