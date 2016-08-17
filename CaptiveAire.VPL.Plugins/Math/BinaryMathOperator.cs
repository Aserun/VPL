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
    public class BinaryMathOperator : Operator
    {
        private readonly IParameter ParameterA;
        private readonly IParameter ParameterB;
        private BinaryMathOperatorType _operatorType;
        private BinaryMathOperatorService _service;
        private readonly IDictionary<BinaryMathOperatorType, BinaryMathOperatorService> _services;

        public BinaryMathOperator(IElementCreationContext context, BinaryMathOperatorType defaultOperatorType) 
            : base(context, context.Owner.GetFloatType())
        {
            var services = new []
           {
                new BinaryMathOperatorService(BinaryMathOperatorType.Addition, "+", (a, b) => a + b),
                new BinaryMathOperatorService(BinaryMathOperatorType.Subtraction, "-", (a, b) => a - b),
                new BinaryMathOperatorService(BinaryMathOperatorType.Multiplication, "*", (a, b) => a * b),
                new BinaryMathOperatorService(BinaryMathOperatorType.Division, "/", (a, b) => a / b),
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

            Service = _services[BinaryMathOperatorType.Addition];

            OperatorType = defaultOperatorType;

            if (!string.IsNullOrWhiteSpace(context.Data))
            {
                var data = JsonConvert.DeserializeObject<BinaryMathOperatorData>(context.Data);

                OperatorType = data.OperatorType;
            }
        }

        public override string GetData()
        {
            var data = new BinaryMathOperatorData()
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

            var a = (double)await ParameterA.EvaluateAsync(executionContext, cancellationToken);
            var b = (double)await ParameterB.EvaluateAsync(executionContext, cancellationToken);

            return service.Evaluate(a, b);
        }

        private BinaryMathOperatorService Service
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

        private BinaryMathOperatorType OperatorType
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

        public enum BinaryMathOperatorType
        {
            Addition = 0,

            Subtraction = 1,

            Multiplication = 2,

            Division = 3,
        }

        public class BinaryMathOperatorData
        {
            public BinaryMathOperatorType OperatorType { get; set; }
        }

        private class BinaryMathOperatorService
        {
            private readonly BinaryMathOperatorType _operatorType;
            private readonly string _text;
            private readonly Func<double, double, double> _func;

            internal BinaryMathOperatorService(BinaryMathOperatorType operatorType, string text, Func<double, double, double> func)
            {
                if (func == null) throw new ArgumentNullException(nameof(func));
                if (String.IsNullOrWhiteSpace(text))
                    throw new ArgumentException("Argument is null or whitespace", nameof(text));

                _operatorType = operatorType;
                _text = text;
                _func = func;
            }

            public BinaryMathOperatorType Type
            {
                get { return _operatorType; }
            }

            public string Text
            {
                get { return _text; }
            }

            public double Evaluate(double a, double b)
            {
                return _func(a, b);
            }
        }
    }
}