using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using CaptiveAire.VPL.Extensions;
using CaptiveAire.VPL.Interfaces;
using CaptiveAire.VPL.TestHost.Model;

namespace CaptiveAire.VPL.TestHost
{
    public class Alert : Statement
    {
        private readonly IParameter _messageParameter;

        public Alert(IElementCreationContext context) : base(context)
        {
            _messageParameter = context.Owner.CreateParameter("message", context.Owner.GetStringType());

            Parameters.Add(_messageParameter);
        }

        public override object Label
        {
            get { return "Alert"; }
            set {  }
        }

        protected override async Task ExecuteCoreAsync(IExecutionContext executionContext, CancellationToken cancellationToken)
        {
            var message = (await _messageParameter.EvaluateAsync(executionContext, cancellationToken))
                .GetConvertedValue<string>();

            MyRuntimeService service = executionContext.RuntimeServices.OfType<MyRuntimeService>().First();

            //throw new Exception("Something blew up!!!!!");

            MessageBox.Show(message);
        }
    }
}