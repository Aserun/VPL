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

        public RepeatStatement(IElementOwner owner) : base(owner, Model.SystemElementIds.Repeat)
        {
            Condition = new Parameter(owner, "condition", owner.GetFloatType());

            Block = new Block(owner, "block")
            {
                Label = "Repeat"
            };

            Block.Parameters.Add(Condition);
            Blocks.Add(Block);
        }

        protected override async Task ExecuteCoreAsync(CancellationToken token)
        {
            double repeatCount = (double) await Condition.EvaluateAsync(token);

            for (double index = 0; index < repeatCount; index++)
            {
                token.ThrowIfCancellationRequested();

                await Block.ExecuteAsync(token);
            }
        }
    }
}