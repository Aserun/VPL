using System;
using System.Threading;
using System.Threading.Tasks;
using CaptiveAire.VPL.Extensions;
using CaptiveAire.VPL.Interfaces;

namespace CaptiveAire.VPL.Plugins.Statements
{
    public class WaitStatement : Statement
    {
        private readonly IParameter SecondsParameter;

        public WaitStatement(IElementOwner owner) 
            : base(owner, Model.SystemElementIds.Wait)
        {
            var secondsParameter = new Parameter(owner, "seconds", owner.GetFloatType())
            {
                Value = 5.0,
                Prefix = "Wait",
                Postfix = "Seconds"
            };

            Parameters.Add(secondsParameter);

            SecondsParameter = secondsParameter;
        }

        protected override async Task ExecuteCoreAsync(CancellationToken cancellationToken)
        {
            var seconds = (double)await SecondsParameter.EvaluateAsync(cancellationToken);

            await Task.Delay(TimeSpan.FromSeconds(seconds), cancellationToken);
        }
    }
}