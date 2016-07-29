using System.Threading;
using System.Threading.Tasks;
using CaptiveAire.VPL.Extensions;
using CaptiveAire.VPL.Interfaces;

namespace CaptiveAire.VPL.Plugins.Statements
{
    public class WhileStatement : CompoundStatement
    {
        private readonly Parameter Condition;
        private readonly Block Block;

        public WhileStatement(IElementOwner owner) 
            : base(owner, Model.SystemElementIds.While)
        {
            Condition = new Parameter(owner, "condition", owner.GetBooleanType());

            Block = new Block(owner, "block")
            {
                Label = "While"
            };

            Block.Parameters.Add(Condition);
            Blocks.Add(Block);
        }

        protected override async Task ExecuteCoreAsync(CancellationToken token)
        {
            while ((bool)await Condition.EvaluateAsync(token))
            {
                token.ThrowIfCancellationRequested();

                await Block.ExecuteAsync(token);
            }
        }
    }
}