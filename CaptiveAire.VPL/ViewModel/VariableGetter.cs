using System;
using System.Threading;
using System.Threading.Tasks;
using CaptiveAire.VPL.Extensions;
using CaptiveAire.VPL.Interfaces;
using Newtonsoft.Json;

namespace CaptiveAire.VPL.ViewModel
{
    internal class VariableGetter : OperatorBase, IVariableReference
    {
        private readonly IVariable _variable;

        public VariableGetter(IElementOwner owner, Guid variableId) 
            : base(owner, Model.SystemElementIds.VariableGetter)
        {
            if (variableId == Guid.Empty)
                throw new ArgumentException("variableId was default value.", nameof(variableId));

            _variable = owner.GetVariable(variableId);
            _variable.NameChanged += VariableNameChanged;
        }

        private void VariableNameChanged(object sender, EventArgs e)
        {
            RaisePropertyChanged(() => Label);
        }

        public VariableGetter(IElementCreationContext context) 
            : base(context.Owner, Model.SystemElementIds.VariableGetter)
        {
            if (string.IsNullOrWhiteSpace(context.Data))
                throw new ArgumentNullException($"Custom serialization data missing for {GetType().Name}");

            var data = JsonConvert.DeserializeObject<VariableGetterData>(context.Data);

            _variable = Owner.GetVariable(data.VariableId);
            _variable.NameChanged += VariableNameChanged;
        }

        public override object Label
        {
            get { return $"Get [{_variable.Name}]" ; }
            set {  }
        }

        public override string GetData()
        {
            var data = new VariableGetterData()
            {
                VariableId = _variable.Id
            };

            return JsonConvert.SerializeObject(data);
        }

        public override IVplType Type
        {
            get { return _variable.Type; }
        }

        public override Task<object> EvaluateAsync(CancellationToken token)
        {
            return Task.FromResult(_variable.Value);
        }

        private class VariableGetterData
        {
            public Guid VariableId { get; set; }
        }

        public Guid VariableId
        {
            get { return _variable.Id; }
        }
    }
}