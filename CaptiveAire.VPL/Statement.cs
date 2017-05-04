using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
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
        private string _number;

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

        public bool CanDrop(IElementClipboardData data)
        {
            return Parent?.CanDrop(data) == true;
        }

        public void Drop(IElementClipboardData data)
        {
            Parent?.Drop(this, data);
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

        public string Number
        {
            get { return _number; }
            set
            {
                _number = value;
                RaisePropertyChanged();
            }
        }


        /// <summary>
        /// To be implemented by inheritors.
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected abstract Task ExecuteCoreAsync(IExecutionContext executionContext, CancellationToken cancellationToken);

        /// <summary>
        /// Executes the statement.
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task ExecuteAsync(IExecutionContext executionContext, CancellationToken token)
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
                    await ExecuteCoreAsync(executionContext, token);
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