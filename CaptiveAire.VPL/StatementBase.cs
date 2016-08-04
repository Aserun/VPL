using System;
using System.Threading;
using System.Threading.Tasks;
using CaptiveAire.VPL.Extensions;
using CaptiveAire.VPL.Interfaces;

namespace CaptiveAire.VPL
{
    public abstract class StatementBase : Element, IElementDropTarget, IStatement
    {
        private bool _isDraggingOver;
        private bool _isExecuting;
        

        protected StatementBase(IElementOwner owner, Guid elementTypeId) 
            : base(owner, elementTypeId)
        {
        }

        public void Drop(IStatement dropped)
        {
            this.CommonDrop(dropped);
        }

        public virtual bool CanDrop()
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

        public bool IsExecuting
        {
            get { return _isExecuting; }
            set
            {
                _isExecuting = value;
                RaisePropertyChanged();
            } 
        }

        protected abstract Task ExecuteCoreAsync(CancellationToken cancellationToken);

        public async Task ExecuteAsync(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            try
            {
                IsExecuting = true;
                ClearErrors();

                await ExecuteCoreAsync(token);
            }
            catch(Exception ex)
            {
                SetError(ex.ToString());
               
                //blech. I just threw up.
                throw;
            }
            finally
            {
                IsExecuting = false;
            }
        }

       
    }
}