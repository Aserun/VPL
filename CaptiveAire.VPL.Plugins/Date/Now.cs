using System;
using System.Threading;
using System.Threading.Tasks;
using CaptiveAire.VPL.Extensions;
using CaptiveAire.VPL.Interfaces;

namespace CaptiveAire.VPL.Plugins.Date
{
    public class Now : Operator
    {
        public Now(IElementCreationContext context) 
            : base(context, context.Owner.GetVplTypeOrThrow(VplTypeId.DateTime))
        {
        }

        public override object Label
        {
            get { return "Now"; }
            set {  }
        }

        public override Task<object> EvaluateAsync(CancellationToken token)
        {
            return Task.FromResult((object) DateTime.Now);
        }
    }
}