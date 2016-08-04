using System.Threading;
using System.Threading.Tasks;
using CaptiveAire.VPL.Extensions;
using CaptiveAire.VPL.Interfaces;
using CaptiveAire.VPL.Model;

namespace CaptiveAire.VPL.Plugins.Operators
{
    internal class NotOperator : Operator
    {
        private readonly Parameter ParameterA;

        public NotOperator(IElementOwner owner) 
            : base(owner, PluginElementIds.NotOperator, owner.GetBooleanType())
        {
            ParameterA = new Parameter(owner, "a", owner.GetVplType(VplTypeId.Boolean))
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