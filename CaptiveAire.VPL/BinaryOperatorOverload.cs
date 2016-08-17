using System;
using CaptiveAire.VPL.Interfaces;

namespace CaptiveAire.VPL
{
    public class BinaryOperatorOverload : IBinaryOperatorOverload
    {
        private readonly IVplType _typeA;
        private readonly IVplType _typeB;
        private readonly IVplType _returnType;
        private readonly Func<object, object, object> _evaluate;

        public BinaryOperatorOverload(
            IVplType typeA, 
            IVplType typeB, 
            IVplType returnType, 
            Func<object, object, object> evaluate)
        {
            if (typeA == null) throw new ArgumentNullException(nameof(typeA));
            if (typeB == null) throw new ArgumentNullException(nameof(typeB));
            if (returnType == null) throw new ArgumentNullException(nameof(returnType));
            if (evaluate == null) throw new ArgumentNullException(nameof(evaluate));

            _typeA = typeA;
            _typeB = typeB;
            _returnType = returnType;
            _evaluate = evaluate;
        }

        public IVplType TypeA
        {
            get { return _typeA; }
        }

        public IVplType TypeB
        {
            get { return _typeB; }
        }

        public IVplType ReturnType
        {
            get { return _returnType; }
        }

        public object Evaluate(IExecutionContext executionContext, object a, object b)
        {
            return _evaluate(a, b);
        }
    }
}