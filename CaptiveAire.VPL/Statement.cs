using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using CaptiveAire.VPL.Extensions;
using CaptiveAire.VPL.Interfaces;

namespace CaptiveAire.VPL
{
    /// <summary>
    /// Base class for statements.
    /// </summary>
    public abstract class Statement : Element, IElementDropTarget, IStatement
    {
        private bool _isDraggingOver;
        private bool _isExecuting;
        private bool _isEnabled = true;

        protected Statement(IElementCreationContext context) 
            : base(context)
        {
            BackgroundColor = Colors.CornflowerBlue;
            ForegroundColor = Colors.White;
        }

        public virtual bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                _isEnabled = value; 
                RaisePropertyChanged();
            }
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

        /// <summary>
        /// To be implemented by inheritors.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected abstract Task ExecuteCoreAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Executes the statement.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task ExecuteAsync(CancellationToken token)
        {
            try
            {
                //Check to see if we should still be running.
                token.ThrowIfCancellationRequested();

                //Check to see if we're enabled.
                if (IsEnabled)
                {
                    //We're executing
                    IsExecuting = true;

                    //Clear the errors (we shouldn't have to do this, but let's be paranoid)
                    ClearErrors();

                    //Execute
                    await ExecuteCoreAsync(token);
                }
            }
            catch(Exception ex)
            {
                //Display the error
                SetError(ex.ToString());
               
                //blech. I just threw up.
                throw;
            }
            finally
            {
                //We are no longer executing.
                IsExecuting = false;
            }
        }

       
    }
}