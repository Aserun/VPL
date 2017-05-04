using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CaptiveAire.VPL.Extensions;
using CaptiveAire.VPL.Interfaces;
using CaptiveAire.VPL.Metadata;

namespace CaptiveAire.VPL
{
    /// <summary>
    /// This class will be instantiated for the lifetime of a function execution. The initial function may
    /// call other functions.
    /// </summary>
    internal class ExecutionContext : IExecutionContext
    {
        private readonly IVplServiceContext _serviceContext;
        private readonly IDictionary<Guid, FunctionMetadata> _cachedFunctions = new ConcurrentDictionary<Guid, FunctionMetadata>();
        private readonly Lazy<IFunctionService> _functionService;

        private readonly CallStack _callStack = new CallStack();

        private readonly object[] _runtimeServices;

        private bool _isDisposed;

        internal ExecutionContext(IVplServiceContext serviceContext)
        {
            if (serviceContext == null) throw new ArgumentNullException(nameof(serviceContext));
            _serviceContext = serviceContext;

            _functionService =
                new Lazy<IFunctionService>(() => serviceContext.Services.OfType<IFunctionService>().FirstOrDefault());

            List<object> runtimeServices = new List<object>();

            //Create the runtime services
            foreach (IVplPlugin plugin in serviceContext.Plugins)
            {
                //Consider each factory
                foreach (IRuntimeServiceFactory factory in plugin.RuntimeServiceFactories)
                {
                    //Create the services
                    object[] services = factory.CreateServices(serviceContext);

                    //Check to see if any were created
                    if (services.Any())
                    {
                        runtimeServices.AddRange(services);
                    }
                }
            }

            _runtimeServices = runtimeServices.ToArray();
        }
        
        private FunctionMetadata GetFunctionMetadata(Guid functionId)
        {
            FunctionMetadata functionMetadata;

            //Check to see if it's in the cache yet
            if (_cachedFunctions.TryGetValue(functionId, out functionMetadata))
            {
                return functionMetadata;
            }

            //Check to see if we have a function service.
            if (_functionService.Value == null)
            {
                //Well snap - we can't do this.
                throw new InvalidOperationException($"No {nameof(IFunctionService)} implementation was provided.");
            }

            //Grab the function
            functionMetadata = _functionService.Value.GetFunction(functionId);

            //Cache the function
            _cachedFunctions[functionId] = functionMetadata;
            
            //Return the function
            return functionMetadata;
        }

        public async Task<object> ExecuteAsync(IFunction function, object[] parameters, CancellationToken cancellationToken)
        {
            //Push this function onto the stack
            _callStack.Push(new CallStackFrame(function, parameters, _callStack.Count));

            object result;

            try
            {
                //Number the statements
                function.NumberStatements();

                //Execute the function
                result = await function.ExecuteAsync(parameters, this, cancellationToken);
            }
            catch (VplRuntimeException)
            {
                //Just throw this - we've already handled it at a higher level.
                throw;
            }
            catch (Exception ex)
            {
                //Wrap up the exception
                throw new VplRuntimeException(ex.Message, CallStack?.ToString(), ex);
            }

            //Pop this function off of the stack.
            _callStack.Pop();

            return result;
        }

        public async Task<object> ExecuteFunctionAsync(Guid functionId, object[] parameters, CancellationToken cancellationToken)
        {
            //Get the function metadata
            var functionMetadata = GetFunctionMetadata(functionId);

            //Create a new instance of the function
            var function = _serviceContext.ElementBuilder.LoadFunction(functionMetadata);

            //Execute the function
            return await ExecuteAsync(function, parameters,  cancellationToken);
        }

        public async Task ExecuteStatementsAsync(IElements elements, CancellationToken cancellationToken)
        {
            foreach (var statement in elements.OfType<IStatement>())
            {
               
                //Get the current frame on the call stack
                ICallStackFrame currentFrame = _callStack.CurrentFrame;

                //Assert
                Debug.Assert(currentFrame != null, "ExecuteStatementsAsync should never be called when the call stack is empty.");

                //We should always get a value, but be paranoid.
                if (currentFrame != null)
                {
                    //Set the current statement
                    currentFrame.CurrentStatement = statement;
                }

                //Execute the statement
                await statement.ExecuteAsync(this, cancellationToken);
               
            }
        }

        public ICallStack CallStack
        {
            get { return _callStack; }
        }

        public IEnumerable<object> RuntimeServices
        {
            get { return _runtimeServices; }
        }

        public void Dispose()
        {
            if (_isDisposed)
                return;

            _isDisposed = true;

            //Get the runtime services the implement IDisposible.
            IEnumerable<IDisposable> disposables = _runtimeServices
                .OfType<IDisposable>();

            foreach (IDisposable disposable in disposables)
            {
                disposable?.Dispose();
            }

            Debug.WriteLine("ExecutionContext disposed.");
        }
    }
}