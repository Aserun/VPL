using System;
using CaptiveAire.VPL.Interfaces;

namespace CaptiveAire.VPL
{
    public abstract class CompoundStatement : Statement
    {
        protected CompoundStatement(IElementCreationContext context)
            : base(context)
        {
        }

        public override bool IsEnabled
        {
            get { return base.IsEnabled; }
            set
            {
                base.IsEnabled = value;

                foreach (var block in Blocks)
                {
                    block.IsEnabled = value;
                }
            }
        }
    }
}