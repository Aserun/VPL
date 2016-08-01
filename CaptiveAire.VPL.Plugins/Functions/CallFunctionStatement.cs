using System.Threading;
using System.Threading.Tasks;
using CaptiveAire.VPL.Interfaces;
using CaptiveAire.VPL.Model;

namespace CaptiveAire.VPL.Plugins.Functions
{
    public class CallFunctionStatement : Statement
    {
        private readonly CommonFunctionBehavior _behavior;

        public CallFunctionStatement(IElementCreationContext context) 
            : base(context.Owner, SystemElementIds.CallFunction)
        {
            _behavior = new CommonFunctionBehavior(context, Parameters, "Call");

            AddActions(_behavior.Actions);
        }

        public override object Label
        {
            get { return _behavior.Label; }
        }

        public override string GetData()
        {
            return _behavior.GetData();
        }

        protected override async Task ExecuteCoreAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            //Create an instance of the function to be called
            var function = _behavior.GetFunctionOrThrow();

            //Get the values from the parameters...
            var parameters = await _behavior.GetParameterValuesAsync(cancellationToken);

            //Call the function
            await function.ExecuteAsync(parameters, cancellationToken);
        }
    }
}