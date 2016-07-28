using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using CaptiveAire.VPL.Interfaces;
using CaptiveAire.VPL.Model;
using CaptiveAire.VPL.View;
using CaptiveAire.VPL.ViewModel;

namespace CaptiveAire.VPL.Statements
{
    public class CommentStatement : StatementViewModel
    {
        private string _comment;
        private readonly CommentView _commentView;

        public CommentStatement(IElementCreationContext context) 
            : base(context.Owner, SystemElementIds.Comment)
        {
            Comment = context.Data;

            ForegroundColor = Colors.Black;
            BackgroundColor = Colors.Yellow;

            _commentView = new CommentView()
            {
                DataContext = this
            };
        }

        public override object Label
        {
            get { return _commentView; }
            set {  }
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

        protected override Task ExecuteCoreAsync(CancellationToken token)
        {
            return Task.FromResult(0);
        }
    }
}