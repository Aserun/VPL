using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CaptiveAire.VPL.Extensions;
using CaptiveAire.VPL.Interfaces;
using CaptiveAire.VPL.Model;
using Newtonsoft.Json;

namespace CaptiveAire.VPL.Plugins.Statements
{
    internal class IfElseStatement : CompoundStatement
    {
        private readonly IfElseData _data;

        private const string ConditionId = "condition";

        public IfElseStatement(IElementCreationContext context) 
            : base(context.Owner, SystemElementIds.IfElse)
        {
            _data = new IfElseData();

            try
            {
                if (!string.IsNullOrWhiteSpace(context.Data))
                {
                    _data = JsonConvert.DeserializeObject<IfElseData>(context.Data);
                }

                if (_data.NumberOfIfs == 0)
                {
                    _data.NumberOfIfs = 1;
                }
            }
            catch (Exception ex)
            {
                //TODO: Add this to the IElementCreationContext error/warning interface that we have yet to add.
                Console.WriteLine(ex);
            }

            for (var index = 0; index < _data.NumberOfIfs; index++)
            {
                var text = index == 0 ? "If" : "If Else";
                var id = index == 0 ? "0" : $"If_{index}";

                //Create the block
                var block = new Block(context.Owner, id)
                {
                    Label = text
                };

                //Add the parameter
                block.Parameters.Add(new Parameter(context.Owner, ConditionId, context.Owner.GetBooleanType()));

                //Add the block to the list
                Blocks.Add(block);
            }

            if (_data.IncludeElse)
            {
                var elseBlock = new Block(context.Owner, "1")
                {
                    Label = "Else"
                };

                Blocks.Add(elseBlock);
            }

            //TODO: Add actions for adding / removing else clauses.
            //AddAction(new ElementAction("Add If Clause", () => , () => true));
        }

        protected override async Task ExecuteCoreAsync(CancellationToken cancellationToken)
        {
            foreach (var block in Blocks)
            {
                //Get the parameter (if it's there)
                var parameter = block.Parameters.FirstOrDefault();

                if (parameter == null || (bool) await parameter.EvaluateAsync(cancellationToken))
                {
                    await block.ExecuteAsync(cancellationToken);

                    return;
                }
            }
        }

        public override string GetData()
        {
            var json = JsonConvert.SerializeObject(_data);

            return json;
        }

        private class IfElseData
        {
            public IfElseData()
            {
                NumberOfIfs = 1;
                IncludeElse = true;
            }

            public int NumberOfIfs { get; set; }

            public bool IncludeElse { get; set; }
        }
    }
}