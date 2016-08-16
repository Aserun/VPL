using System;
using System.Threading;
using System.Threading.Tasks;
using CaptiveAire.VPL.Extensions;
using CaptiveAire.VPL.Interfaces;

namespace CaptiveAire.VPL.Plugins.Date
{
    public class UtcNow : Operator
    {
        public UtcNow(IElementCreationContext context) 
            : base(context, context.Owner.GetVplTypeOrThrow(VplTypeId.DateTime))
        {
        }

        public override object Label
        {
            get { return "UTC Now"; }
            set { }
        }

        public override Task<object> EvaluateAsync(IExecutionContext executionContext, CancellationToken token)
        {
            return Task.FromResult((object)DateTime.UtcNow);
        }
    }
}