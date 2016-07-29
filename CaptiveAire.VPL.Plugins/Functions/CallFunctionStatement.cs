using System;
using System.Threading;
using System.Threading.Tasks;
using CaptiveAire.VPL.Extensions;
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

        protected override async Task ExecuteCoreAsync(CancellationToken token)
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
            var executor = new Executor();

            await executor.ExecuteAsync(result.Statement, token);
        }
    }
}