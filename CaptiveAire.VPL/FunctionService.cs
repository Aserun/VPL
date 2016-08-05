using System;
using System.Collections.Generic;
using CaptiveAire.VPL.Interfaces;
using CaptiveAire.VPL.Metadata;

namespace CaptiveAire.VPL
{
    public class FunctionService : IFunctionService
    {
        private readonly Func<IEnumerable<FunctionMetadata>> _functionFactory;
        private readonly Func<Guid, FunctionMetadata> _findFunction;

        public FunctionService(Func<IEnumerable<FunctionMetadata>> functionFactory, Func<Guid, FunctionMetadata> findFunction)
        {
            if (functionFactory == null) throw new ArgumentNullException(nameof(functionFactory));
            if (findFunction == null) throw new ArgumentNullException(nameof(findFunction));

            _functionFactory = functionFactory;
            _findFunction = findFunction;
        }

        public IEnumerable<FunctionMetadata> GetFunctions()
        {
            return _functionFactory();
        }

        public FunctionMetadata GetFunction(Guid id)
        {
            return _findFunction(id);
        }
    }
}