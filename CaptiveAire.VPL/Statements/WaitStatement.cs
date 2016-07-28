using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CaptiveAire.VPL.Extensions;
using CaptiveAire.VPL.Interfaces;
using CaptiveAire.VPL.Model;
using CaptiveAire.VPL.ViewModel;

namespace CaptiveAire.VPL.Statements
{
    public class WaitStatement : StatementViewModel
    {
        private readonly IParameter SecondsParameter;

        public WaitStatement(IElementOwner owner) 
            : base(owner, Model.SystemElementIds.Wait)
        {
            var secondsParameter = new ParameterViewModel(owner, "seconds", owner.GetFloatType())
            {
                Value = 5.0,
                Prefix = "Wait",
                Postfix = "Seconds"
            };

            Parameters.Add(secondsParameter);

            SecondsParameter = secondsParameter;
        }

        protected override async Task ExecuteCoreAsync(CancellationToken token)
        {
            var seconds = (double)await SecondsParameter.EvaluateAsync(token);

            await Task.Delay(TimeSpan.FromSeconds(seconds), token);
        }
    }
}