using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using CaptiveAire.VPL.Extensions;
using CaptiveAire.VPL.Interfaces;

namespace CaptiveAire.VPL.Plugins.Functions
{
    public class EvaluateFunctionOperator : Operator
    {
        private readonly CommonFunctionBehavior _behavior;

        public EvaluateFunctionOperator(IElementCreationContext context) 
            : base(context, context.Owner.GetAnyType())
        {
            _behavior = new CommonFunctionBehavior(context, Parameters, "Evaluate", this);

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

        protected override async Task<object> EvaluateCoreAsync(IExecutionContext executionContext, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            //Create an instance of the function to be called
            if (_behavior.FunctionId == null)
                throw new InvalidOperationException("No function was selected.");

            var functionId = _behavior.FunctionId;

            //Create an instance of the function to be called
            if (functionId == null)
                throw new InvalidOperationException("No function was selected.");

            //Get the values from the parameters...
            var parameters = await _behavior.GetParameterValuesAsync(executionContext, cancellationToken);

            //Execute the function
            return await executionContext.ExecuteFunctionAsync(functionId.Value, parameters, cancellationToken);
        }

        protected override IError[] CheckForErrorsCore()
        {
            return _behavior.CheckForErrors();
        }
    }
}