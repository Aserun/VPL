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

        public override async Task<object> EvaluateAsync(CancellationToken token)
        {
            bool a = (bool)await ParameterA.EvaluateAsync(token);

            return !a;
        }
    }
}