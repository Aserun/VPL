using System;
using System.Threading;
using System.Threading.Tasks;
using CaptiveAire.VPL.Extensions;
using CaptiveAire.VPL.Interfaces;
using CaptiveAire.VPL.Model;

namespace CaptiveAire.VPL
{
    internal class Block : Element, IElementDropTarget, IBlock
    {
        private readonly string _id;
        private bool _isDraggingOver;

        public Block(IElementCreationContext context, string id) 
            : base(context)
        {
            _id = id;
        }

        public void Drop(IStatement dropped)
        {
            this.CommonDrop(dropped);
        }

        public bool CanDrop()
        {
            return true;
        }

        public bool IsDraggingOver
        {
            get { return _isDraggingOver; }
            set
            {
                _isDraggingOver = value; 
                RaisePropertyChanged();
            }
        }

        public void Drop(IElement element)
        {
            var statement = element as IStatement;

            if (statement != null)
            {
                this.CommonDrop(statement);
            }
        }

        public bool CanDrop(Type elementType, Guid? returnType)
        {
            return elementType != null && typeof(IStatement).IsAssignableFrom(elementType);
        }

        public async Task ExecuteAsync(CancellationToken token)
        {
            var executor = new StatementExecutor();

            await executor.ExecuteAsync(Next as IStatement, token);
        }

        public string Name
        {
            get { return "Block"; }
        }

        public string Id
        {
            get { return _id; }
        }
    }
}