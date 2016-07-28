using System;
using System.Threading;
using System.Threading.Tasks;
using CaptiveAire.VPL.Extensions;
using CaptiveAire.VPL.Interfaces;
using CaptiveAire.VPL.View;
using CaptiveAire.VPL.ViewModel;

namespace CaptiveAire.VPL.Statements
{
    public class WhileStatement : CompoundStatementViewModel
    {
        private readonly ParameterViewModel Condition;
        private readonly BlockViewModel Block;

        public WhileStatement(IElementOwner owner) 
            : base(owner, Model.SystemElementIds.While)
        {
            Condition = new ParameterViewModel(owner, "condition", owner.GetBooleanType());

            Block = new BlockViewModel(owner, "block")
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