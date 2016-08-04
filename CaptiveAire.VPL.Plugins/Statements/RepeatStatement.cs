using System.Threading;
using System.Threading.Tasks;
using CaptiveAire.VPL.Extensions;
using CaptiveAire.VPL.Interfaces;

namespace CaptiveAire.VPL.Plugins.Statements
{
    public class RepeatStatement : CompoundStatement
    {
        private readonly Parameter Condition;
        private readonly Block Block;

        public RepeatStatement(IElementOwner owner) : base(owner, PluginElementIds.Repeat)
        {
            Condition = new Parameter(owner, "condition", owner.GetFloatType());

            Block = new Block(owner, "block")
            {
                Label = "Repeat"
            };

            Block.Parameters.Add(Condition);
            Blocks.Add(Block);
        }

        protected override async Task ExecuteCoreAsync(CancellationToken cancellationToken)
        {
            double repeatCount = (double) await Condition.EvaluateAsync(cancellationToken);

            for (double index = 0; index < repeatCount; index++)
            {
                cancellationToken.ThrowIfCancellationRequested();

                await Block.ExecuteAsync(cancellationToken);
            }
        }
    }
}