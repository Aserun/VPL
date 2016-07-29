using System.Threading;
using System.Threading.Tasks;
using CaptiveAire.VPL.Extensions;
using CaptiveAire.VPL.Interfaces;
using CaptiveAire.VPL.Model;

namespace CaptiveAire.VPL.Plugins.Statements
{
    internal class IfElseStatement : CompoundStatement
    {
        private readonly Parameter Condition;
        private readonly Block IfBlock;
        private readonly Block ElseBlock;

        public IfElseStatement(IElementOwner owner) 
            : base(owner, Model.SystemElementIds.IfElse)
        {
            Condition = new Parameter(owner, "condition", owner.GetVplType(VplTypeId.Boolean));

            IfBlock = new Block(owner, "0")
            {
                Label = "If"
            };

            IfBlock.Parameters.Add(Condition);

            ElseBlock = new Block(owner, "1")
            {
                Label = "Else"
            };

            Blocks.Add(IfBlock);
            Blocks.Add(ElseBlock);

            //TODO: Add actions for adding / removing else clauses.
            //AddAction(new ElementAction("Add Clause", () => MessageBox.Show("Not Implemented"), () => true));
        }

        protected override async Task ExecuteCoreAsync(CancellationToken token)
        {
            var condition = (bool)await Condition.EvaluateAsync(token);

            if (condition)
            {
                await IfBlock.ExecuteAsync(token);
            }
            else
            {
                await ElseBlock.ExecuteAsync(token);
            }
        }
    }
}