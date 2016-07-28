using System;
using System.Threading;
using System.Threading.Tasks;
using CaptiveAire.VPL.Extensions;
using CaptiveAire.VPL.Interfaces;
using CaptiveAire.VPL.Model;
using CaptiveAire.VPL.ViewModel;

namespace CaptiveAire.VPL.Operators
{
    public class NotOperator : OperatorViewModel
    {
        private readonly ParameterViewModel ParameterA;

        public NotOperator(IElementOwner owner) 
            : base(owner, Model.SystemElementIds.NotOperator, owner.GetBooleanType())
        {
            ParameterA = new ParameterViewModel(owner, "a", owner.GetVplType(VplTypeId.Boolean))
            {
                Prefix = "Not"
            };

            Parameters.Add(ParameterA);
        }

        public override async Task<object> EvaluateAsync(CancellationToken token)
        {
            bool a = (bool)await ParameterA.EvaluateAsync(token);

            return !a;
        }
    }
}