using System.Threading;
using System.Threading.Tasks;
using CaptiveAire.VPL.Extensions;
using CaptiveAire.VPL.Interfaces;
using CaptiveAire.VPL.ViewModel;

namespace CaptiveAire.VPL.Statements
{
    public class RepeatStatement : CompoundStatementViewModel
    {
        private readonly ParameterViewModel Condition;
        private readonly BlockViewModel Block;

        public RepeatStatement(IElementOwner owner) : base(owner, Model.SystemElementIds.Repeat)
        {
            Condition = new ParameterViewModel(owner, "condition", owner.GetFloatType());

            Block = new BlockViewModel(owner, "block")
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