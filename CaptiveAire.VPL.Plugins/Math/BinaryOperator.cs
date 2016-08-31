using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CaptiveAire.VPL.Extensions;
using CaptiveAire.VPL.Interfaces;
using Newtonsoft.Json;

namespace CaptiveAire.VPL.Plugins.Math
{
    internal class BinaryOperator : Operator
    {
        private readonly IParameter ParameterA;
        private readonly IParameter ParameterB;
        private BinaryOperatorType _operatorType;
        private BinaryOperatorService _service;
        private readonly IDictionary<BinaryOperatorType, BinaryOperatorService> _services;

        public BinaryOperator(IElementCreationContext context, BinaryOperatorType defaultOperatorType) 
            : base(context, context.Owner.GetFloatType())
        {
            var services = new []
            {
                new BinaryOperatorService(BinaryOperatorType.Addition, "+", (a, b) => a + b),
                new BinaryOperatorService(BinaryOperatorType.Subtraction, "-", (a, b) => a - b),
                new BinaryOperatorService(BinaryOperatorType.Multiplication, "*", (a, b) => a * b),
                new BinaryOperatorService(BinaryOperatorType.Division, "/", (a, b) => a / b),
                new BinaryOperatorService(BinaryOperatorType.ShiftLeft, "<<", (a, b) => a << b), 
                new BinaryOperatorService(BinaryOperatorType.ShiftRight, ">>", (a, b) => a >> b), 
                new BinaryOperatorService(BinaryOperatorType.BitwiseAnd, "&", (a , b) => a & b),
                new BinaryOperatorService(BinaryOperatorType.BitwiseOr, "|", (a , b) => a | b),
                new BinaryOperatorService(BinaryOperatorType.Modulus, "%", (a , b) => a % b),
            };

            foreach (var service in services)
            {
                AddAction(new ElementAction(service.Text, () => OperatorType = service.Type));
            }

            _services = services.ToDictionary(s => s.Type, s => s);

            ParameterA = context.Owner.CreateParameter("a", context.Owner.GetFloatType());
            ParameterB = context.Owner.CreateParameter("b", context.Owner.GetFloatType());

            Parameters.Add(ParameterA);
            Parameters.Add(ParameterB);

            Service = _services[BinaryOperatorType.Addition];

            OperatorType = defaultOperatorType;

            if (!string.IsNullOrWhiteSpace(context.Data))
            {
                var data = JsonConvert.DeserializeObject<BinaryOperatorData>(context.Data);

                OperatorType = data.OperatorType;
            }
        }

        public override string GetData()
        {
            var data = new BinaryOperatorData()
            {
                OperatorType = OperatorType
            };

            return JsonConvert.SerializeObject(data);
        }

        protected override async Task<object> EvaluateCoreAsync(IExecutionContext executionContext, CancellationToken cancellationToken)
        {
            var service = Service;

            if (service == null)
                return 0;

            dynamic a = await ParameterA.EvaluateAsync(executionContext, cancellationToken);
            dynamic b = await ParameterB.EvaluateAsync(executionContext, cancellationToken);

            return service.Evaluate(a, b);
        }

        private BinaryOperatorService Service
        {
            get { return _service; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();

                _service = value;

                ParameterA.Postfix = " " + value.Text;
            }
        }

        private BinaryOperatorType OperatorType
        {
            get { return _operatorType; }
            set
            {
                _operatorType = value; 
                RaisePropertyChanged();
                Owner.MarkDirty();
                Service = _services[value];
            }
        }

        public enum BinaryOperatorType
        {
            Addition = 0,

            Subtraction = 1,

            Multiplication = 2,

            Division = 3,
        
            ShiftLeft = 4,

            ShiftRight = 5,

            BitwiseAnd = 6,

            BitwiseOr = 7,

            Modulus = 8
        
        }

        public class BinaryOperatorData
        {
            public BinaryOperatorType OperatorType { get; set; }
        }

        private class BinaryOperatorService
        {
            private readonly BinaryOperatorType _operatorType;
            private readonly string _text;
            private readonly Func<dynamic, dynamic, dynamic> _func;

            internal BinaryOperatorService(BinaryOperatorType operatorType, string text, Func<dynamic, dynamic, dynamic> func)
            {
                if (func == null) throw new ArgumentNullException(nameof(func));
                if (String.IsNullOrWhiteSpace(text))
                    throw new ArgumentException("Argument is null or whitespace", nameof(text));

                _operatorType = operatorType;
                _text = text;
                _func = func;
            }

            public BinaryOperatorType Type
            {
                get { return _operatorType; }
            }

            public string Text
            {
                get { return _text; }
            }

            public dynamic Evaluate(dynamic a, dynamic b)
            {
                return _func(a, b);
            }
        }
    }
}