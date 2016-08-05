using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using CaptiveAire.VPL.Extensions;
using CaptiveAire.VPL.Interfaces;

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

        protected override async Task ExecuteCoreAsync(CancellationToken cancellationToken)
        {
            var message = await _messageParameter.EvaluateAsync(cancellationToken) as string;

            MessageBox.Show(message);
        }
    }
}