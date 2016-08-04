using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using CaptiveAire.VPL.Extensions;
using CaptiveAire.VPL.Interfaces;
using CaptiveAire.VPL.Model;

namespace CaptiveAire.VPL.Plugins.Functions
{
    public class EvaluateFunctionOperator : Operator
    {
        private readonly CommonFunctionBehavior _behavior;

        public EvaluateFunctionOperator(IElementCreationContext context) 
            : base(context.Owner, PluginElementIds.EvaluateFunction, context.Owner.GetAnyType())
        {
            _behavior = new CommonFunctionBehavior(context, Parameters, "Evaluate");

            AddActions(_behavior.Actions);

            ForegroundColor = Colors.MediumSeaGreen;
            ForegroundColor = Colors.Black;
        }

        public override object Label
        {
            get { return _behavior.Label; }
        }

        public override string GetData()
        {
            return _behavior.GetData();
        }

        public override async Task<object> EvaluateAsync(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            //Create an instance of the function to be called
            var function = _behavior.GetFunctionOrThrow();

            //Get the parameters
            var parameters = await _behavior.GetParameterValuesAsync(token);

            //Execute the function and return its value
            return await function.ExecuteAsync(parameters, token);
        }
    }
}