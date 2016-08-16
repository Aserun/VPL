using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CaptiveAire.VPL.Extensions;
using CaptiveAire.VPL.Interfaces;
using Newtonsoft.Json;

namespace CaptiveAire.VPL.Plugins.Control
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// For backwards compatibility, the If / Else blocks use the following ids:
    /// - If            : "0"
    /// - Else If       : "If_1"
    /// - Else If       : "If_2"
    /// - Else          : "1"
    /// </remarks>
    internal class IfElseStatement : CompoundStatement
    {
        private readonly IfElseData _data;

        private const string ConditionId = "condition";

        public IfElseStatement(IElementCreationContext context) 
            : base(context)
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
                AddIfClause(index);
            }

            if (_data.IncludeElse)
            {
               AddElse();
            }

            //Add actions for adding / removing else clauses.
            this.AddAction("Add 'Else If'", AddIfClause);
            this.AddAction("Remove last 'Else If'", RemoveLastIf, CanRemoveLastIf);
            this.AddAction("Add 'Else'", AddElse, CanAddElse);
            this.AddAction("Remove 'Else'", RemoveElse, CanRemoveElse);
        }

        private void AddIfClause(int index)
        {
            var text = index == 0 ? "If" : "Else If";
            var id = index == 0 ? "0" : $"If_{index}";

            //Create the block
            var block = Owner.CreateBlock(id, text);

            block.IsEnabled = IsEnabled;

            //Add the parameter
            block.Parameters.Add(Owner.CreateParameter(ConditionId, Owner.GetBooleanType()));

            //Add the block to the list
            Blocks.Insert(index, block);
        }

        private void AddIfClause()
        {
            AddIfClause(_data.NumberOfIfs);

            _data.NumberOfIfs++;
        }

        private void RemoveLastIf()
        {
            if (!CanRemoveLastIf())
                return;

            Blocks.RemoveAt(_data.NumberOfIfs - 1);

            _data.NumberOfIfs--;
        }

        private bool CanRemoveLastIf()
        {
            return _data.NumberOfIfs > 1;
        }

        private void AddElse()
        {
            var elseBlock = Owner.CreateBlock("1", "Else");

            elseBlock.IsEnabled = IsEnabled;

            Blocks.Add(elseBlock);

            _data.IncludeElse = true;
        }

        private bool CanAddElse()
        {
            return !_data.IncludeElse;
        }

        private void RemoveElse()
        {
            Blocks.RemoveAt(Blocks.Count - 1);

            _data.IncludeElse = false;
        }

        private bool CanRemoveElse()
        {
            return _data.IncludeElse;
        }

        protected override async Task ExecuteCoreAsync(IExecutionContext executionContext, CancellationToken cancellationToken)
        {
            foreach (var block in Blocks)
            {
                //Get the parameter (if it's there)
                var parameter = block.Parameters.FirstOrDefault();

                if (parameter == null || (bool) await parameter.EvaluateAsync(executionContext, cancellationToken))
                {
                    await block.ExecuteAsync(executionContext, cancellationToken);

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