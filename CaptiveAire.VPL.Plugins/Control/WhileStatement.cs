using System.Threading;
using System.Threading.Tasks;
using CaptiveAire.VPL.Extensions;
using CaptiveAire.VPL.Interfaces;

namespace CaptiveAire.VPL.Plugins.Control
{
    public class WhileStatement : CompoundStatement
    {
        private readonly Parameter Condition;
        private readonly Block Block;

        public WhileStatement(IElementOwner owner) 
            : base(owner, PluginElementIds.While)
        {
            Condition = new Parameter(owner, "condition", owner.GetBooleanType());

            Block = new Block(owner, "block")
            {
                Label = "While"
            };

            Block.Parameters.Add(Condition);
            Blocks.Add(Block);
        }

        protected override async Task ExecuteCoreAsync(CancellationToken cancellationToken)
        {
            while ((bool)await Condition.EvaluateAsync(cancellationToken))
            {
                cancellationToken.ThrowIfCancellationRequested();

                await Block.ExecuteAsync(cancellationToken);
            }
        }
    }
}