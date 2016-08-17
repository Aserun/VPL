using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using CaptiveAire.VPL.Interfaces;

namespace CaptiveAire.VPL.Plugins.Functions
{
    public class CallFunctionStatement : Statement
    {
        private readonly CommonFunctionBehavior _behavior;

        public CallFunctionStatement(IElementCreationContext context) 
            : base(context)
        {
            _behavior = new CommonFunctionBehavior(context, Parameters, "Call", this);

            AddActions(_behavior.Actions);

            BackgroundColor = Colors.Lavender;
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

        protected override async Task ExecuteCoreAsync(IExecutionContext executionContext, CancellationToken cancellationToken)
        {
            var functionId = _behavior.FunctionId;

            //Create an instance of the function to be called
            if (functionId == null)
                throw new InvalidOperationException("No function was selected.");

            //Get the values from the parameters...
            var parameters = await _behavior.GetParameterValuesAsync(executionContext, cancellationToken);

            //Execute the function
            await executionContext.ExecuteFunctionAsync(functionId.Value, parameters, cancellationToken);
        }

        protected override IError[] CheckForErrorsCore()
        {
            return _behavior.CheckForErrors();
        }
    }
}