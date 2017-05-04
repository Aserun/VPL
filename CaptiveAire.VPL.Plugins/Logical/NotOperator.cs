using System.Threading;
using System.Threading.Tasks;
using CaptiveAire.VPL.Extensions;
using CaptiveAire.VPL.Interfaces;

namespace CaptiveAire.VPL.Plugins.Logical
{
    internal class NotOperator : Operator
    {
        private readonly IParameter ParameterA;

        public NotOperator(IElementCreationContext context) 
            : base(context, context.Owner.GetBooleanType())
        {
            ParameterA = context.Owner.CreateParameter("a", context.Owner.GetVplTypeOrThrow(VplTypeId.Boolean), "Not");

            Parameters.Add(ParameterA);
        }

        protected override async Task<object> EvaluateCoreAsync(IExecutionContext executionContext, CancellationToken cancellationToken)
        {
            bool a = (await ParameterA.EvaluateAsync(executionContext, cancellationToken)).GetConvertedValue<bool>();

            return !a;
        }
    }
}