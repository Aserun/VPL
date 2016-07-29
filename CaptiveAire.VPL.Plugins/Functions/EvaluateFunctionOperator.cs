using System;
using System.Threading;
using System.Threading.Tasks;
using CaptiveAire.VPL.Extensions;
using CaptiveAire.VPL.Interfaces;
using CaptiveAire.VPL.Model;

namespace CaptiveAire.VPL.Plugins.Functions
{
    public class EvaluateFunctionOperator : Operator
    {
        private readonly CommonFunctionBehavior _behavior;

        public EvaluateFunctionOperator(IElementCreationContext context) 
            : base(context.Owner, SystemElementIds.EvaluateFunction, context.Owner.GetAnyType())
        {
            _behavior = new CommonFunctionBehavior(context, Parameters, "Evaluate");

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

        public override Task<object> EvaluateAsync(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            //Create an instance of the function to be called
            var function = _behavior.GetFunctionOrThrow();

            var result = function.GetEntrancePoint();

            if (result.Statement == null)
            {
                throw new InvalidOperationException(result.Error);
            }

            //TODO: Pass in params
            //var executor = new Executor();

            throw new NotImplementedException("function evaluation is not yet complete.");

            //await executor.ExecuteAsync(result.Statement, token);
        }
    }
}