using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using CaptiveAire.VPL.Interfaces;
using CaptiveAire.VPL.Metadata;

namespace CaptiveAire.VPL
{
    internal class ExecutionContext : IExecutionContext, IDisposable
    {
        private readonly IVplServiceContext _serviceContext;

        private readonly IDictionary<Guid, FunctionMetadata> _cachedFunctions = new ConcurrentDictionary<Guid, FunctionMetadata>();

        private readonly Lazy<IFunctionService> _functionService;

        internal ExecutionContext(IVplServiceContext serviceContext)
        {
            if (serviceContext == null) throw new ArgumentNullException(nameof(serviceContext));
            _serviceContext = serviceContext;

            _functionService =
                new Lazy<IFunctionService>(() => serviceContext.Services.OfType<IFunctionService>().FirstOrDefault());
        }

        public void Dispose()
        {
        }

        public FunctionMetadata GetFunctionMetadata(Guid functionId)
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
                throw new InvalidOperationException("No IFunctionService implementation was provided.");
            }

            //Grab the function
            functionMetadata = _functionService.Value.GetFunction(functionId);

            //Cache the function
            _cachedFunctions[functionId] = functionMetadata;
            
            //Return the function
            return functionMetadata;
        }
    }
}