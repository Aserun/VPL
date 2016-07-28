using System;
using System.Threading;
using System.Threading.Tasks;
using CaptiveAire.VPL.Extensions;
using CaptiveAire.VPL.Interfaces;

namespace CaptiveAire.VPL.ViewModel
{
    public abstract class StatementViewModelBase : ElementViewModel, IElementDropTarget, IStatement
    {
        private bool _isDraggingOver;
        private bool _isExecuting;
        private bool _hasError;
        private string _error;

        protected StatementViewModelBase(IElementOwner owner, Guid elementTypeId) 
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

        public bool HasError
        {
            get { return _hasError; }
            set
            {
                _hasError = value; 
                RaisePropertyChanged();
            }
        }

        public string Error
        {
            get { return _error; }
            set
            {
                _error = value; 
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

        protected abstract Task ExecuteCoreAsync(CancellationToken token);

        public async Task ExecuteAsync(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            try
            {
                IsExecuting = true;
                HasError = false;
                Error = null;

                await ExecuteCoreAsync(token);
            }
            catch(Exception ex)
            {
                HasError = true;
                Error = ex.ToString();
                
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