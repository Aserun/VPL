using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using CaptiveAire.VPL.Interfaces;
using CommentView = CaptiveAire.VPL.Plugins.Annotations.CommentView;

namespace CaptiveAire.VPL.Plugins.Annotations
{
    internal class CommentStatement : Statement
    {
        private string _comment;
        private readonly CommentView _commentView;

        public CommentStatement(IElementCreationContext context) 
            : base(context.Owner, PluginElementIds.Comment)
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
                Owner.MarkDirty();
            }
        }

        public override string GetData()
        {
            return Comment;
        }

        protected override Task ExecuteCoreAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(0);
        }
    }
}