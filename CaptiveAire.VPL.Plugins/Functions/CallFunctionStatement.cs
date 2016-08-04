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
            : base(context.Owner, PluginElementIds.CallFunction)
        {
            _behavior = new CommonFunctionBehavior(context, Parameters, "Call");

            AddActions(_behavior.Actions);

            BackgroundColor = Colors.Blue;
            ForegroundColor = Colors.White;
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

        protected override IError[] CheckForErrorsCore()
        {
            var errors = new List<IError>(1);

            if (!_behavior.HasFunction)
            {
                errors.Add(new Error(this, "No function is selected.", ErrorLevel.Error));
            }

            HasError = errors.Any();

            return errors.ToArray();
        }
    }
}