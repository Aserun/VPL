using System;
using System.Windows.Media;
using CaptiveAire.VPL.Interfaces;

namespace CaptiveAire.VPL.ViewModel
{
    public abstract class StatementViewModel : StatementViewModelBase
    {
        protected StatementViewModel(IElementOwner owner, Guid elementTypeId) 
            : base(owner, elementTypeId)
        {
            BackgroundColor = Colors.CornflowerBlue;
            ForegroundColor = Colors.White;
        }
    }
}