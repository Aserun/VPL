using System.Threading;
using System.Threading.Tasks;
using CaptiveAire.VPL.Extensions;
using CaptiveAire.VPL.Interfaces;

namespace CaptiveAire.VPL.Plugins.Control
{
    public class WhileStatement : CompoundStatement
    {
        private readonly IParameter Condition;
        private readonly IBlock Block;

        public WhileStatement(IElementCreationContext context) 
            : base(context)
        {
            Condition = Owner.CreateParameter("condition", Owner.GetBooleanType());

            Block = Owner.CreateBlock("block", "While");

            Block.Parameters.Add(Condition);
            Blocks.Add(Block);
        }

        protected override async Task ExecuteCoreAsync(IExecutionContext executionContext, CancellationToken cancellationToken)
        {
            while ((bool)await Condition.EvaluateAsync(executionContext, cancellationToken))
            {
                cancellationToken.ThrowIfCancellationRequested();

                await Block.ExecuteAsync(executionContext, cancellationToken);
            }
        }
    }
}