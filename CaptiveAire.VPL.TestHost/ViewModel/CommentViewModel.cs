using System;
using CaptiveAire.VPL.Interfaces;
using CaptiveAire.VPL.TestHost.Model;
using CaptiveAire.VPL.ViewModel;

namespace CaptiveAire.VPL.TestHost.ViewModel
{
    public class CommentViewModel : Element
    {
        private string _comment;

        public CommentViewModel(IElementCreationContext context) 
            : base(context)
        {
            Comment = context.Data;
        }

        public string Comment
        {
            get { return _comment; }
            set
            {
                _comment = value; 
                RaisePropertyChanged();
            }
        }

        public override string GetData()
        {
            return Comment;
        }
    }
}