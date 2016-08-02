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
        private readonly Parameter _parameter;

        public VariableSetter(IElementOwner owner, Guid variableId) 
            : base(owner, Model.SystemElementIds.VariableSetter)
        {
            if (variableId == Guid.Empty)
                throw new ArgumentException("variableId was default value.", nameof(variableId));

            _variable = owner.GetVariableOrThrow(variableId);
            _variable.NameChanged += VariableNameChanged;
            _parameter = AddParameter(owner, _variable);
        }

        public VariableSetter(IElementCreationContext context) 
            : base(context.Owner, Model.SystemElementIds.VariableSetter)
        {
            if (string.IsNullOrWhiteSpace(context.Data))
                throw new ArgumentNullException($"Custom serialization data missing for {GetType().Name}");

            var data = JsonConvert.DeserializeObject<VariableSetterData>(context.Data);

            _variable = context.Owner.GetVariableOrThrow(data.VariableId);
            _variable.NameChanged += VariableNameChanged;
            _parameter = AddParameter(context.Owner, _variable);
        }

        private void VariableNameChanged(object sender, EventArgs e)
        {
            RaisePropertyChanged(() => Label);
        }

        private Parameter AddParameter(IElementOwner owner, IVariable variable)
        {
            var parameter = new Parameter(owner, "value", variable.Type);

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

        protected override async Task ExecuteCoreAsync(CancellationToken cancellationToken)
        {
            var value = await _parameter.EvaluateAsync(cancellationToken);

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