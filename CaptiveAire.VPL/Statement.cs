using System;
using System.Windows.Media;
using CaptiveAire.VPL.Interfaces;

namespace CaptiveAire.VPL
{
    public abstract class Statement : StatementBase
    {
        protected Statement(IElementOwner owner, Guid elementTypeId) 
            : base(owner, elementTypeId)
        {
            BackgroundColor = Colors.CornflowerBlue;
            ForegroundColor = Colors.White;
        }
    }
}