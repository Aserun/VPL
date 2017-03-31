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
        private readonly Action<Guid> _editFunction;

        public FunctionService(Func<IEnumerable<FunctionMetadata>> functionFactory, Func<Guid, FunctionMetadata> findFunction, Action<Guid> editFunction)
        {
            if (functionFactory == null) throw new ArgumentNullException(nameof(functionFactory));
            if (findFunction == null) throw new ArgumentNullException(nameof(findFunction));
            if (editFunction == null) throw new ArgumentNullException(nameof(editFunction));

            _functionFactory = functionFactory;
            _findFunction = findFunction;
            _editFunction = editFunction;
        }

        public IEnumerable<IFunctionReference> GetFunctions()
        {
            return _functionFactory();
        }

        public FunctionMetadata GetFunction(Guid id)
        {
            return _findFunction(id);
        }

        public void EditFunction(Guid id)
        {
            _editFunction(id);
        }
    }
}