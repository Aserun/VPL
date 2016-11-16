using System;
using System.Threading;
using System.Threading.Tasks;
using CaptiveAire.VPL.Extensions;
using CaptiveAire.VPL.Interfaces;

namespace CaptiveAire.VPL.Plugins.Control
{
    public class WaitStatement : Statement
    {
        private readonly IParameter SecondsParameter;

        public WaitStatement(IElementCreationContext context) 
            : base(context)
        {
            SecondsParameter = Owner.CreateParameter("seconds", Owner.GetFloatType(), "Wait", "Seconds");

            SecondsParameter.SetValue(5.0);

            Parameters.Add(SecondsParameter);
        }

        protected override async Task ExecuteCoreAsync(IExecutionContext executionContext, CancellationToken cancellationToken)
        {
            var seconds = (await SecondsParameter.EvaluateAsync(executionContext, cancellationToken)).GetConvertedValue<double>();

            await Task.Delay(TimeSpan.FromSeconds(seconds), cancellationToken);
        }
    }
}