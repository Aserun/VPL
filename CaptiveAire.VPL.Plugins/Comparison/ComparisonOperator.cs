using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CaptiveAire.VPL.Extensions;
using CaptiveAire.VPL.Interfaces;
using Newtonsoft.Json;

namespace CaptiveAire.VPL.Plugins.Comparison
{
    public class ComparisonOperator : Operator
    {
        private readonly IParameter ParameterA;
        private readonly IParameter ParameterB;

        private readonly ComparisonOperatorService[] _services;
        private ComparisonOperatorService _service;

        public ComparisonOperator(IElementCreationContext context, ComparisonOperatorType comparisonType) 
            : base(context, context.Owner.GetBooleanType())
        {
            ParameterA = Owner.CreateParameter("a", context.Owner.GetFloatType());
            ParameterB = Owner.CreateParameter("b", context.Owner.GetFloatType());

            Parameters.Add(ParameterA);
            Parameters.Add(ParameterB);

            _services = new[]
            {
                new ComparisonOperatorService(ComparisonOperatorType.LessThan, "<", (a, b) => a < b),
                new ComparisonOperatorService(ComparisonOperatorType.LessThanOrEqual, "<=", (a, b) => a <= b),
                new ComparisonOperatorService(ComparisonOperatorType.Equal, "=", (a, b) => a == b),
                new ComparisonOperatorService(ComparisonOperatorType.NotEqual, "<>", (a, b) => a != b),
                new ComparisonOperatorService(ComparisonOperatorType.GreaterThan, ">", (a, b) => a > b),
                new ComparisonOperatorService(ComparisonOperatorType.GreaterThanOrEqual, ">=", (a, b) => a >= b),
            };

            //Add actions so we can just change the type instead of having to recreate it.
            foreach (var service in _services)
            {
                AddAction(new ElementAction(service.Text, () => ComparisonType = service.ComparisonType, () => ComparisonType != service.ComparisonType));
            }

            ComparisonType = ComparisonOperatorType.Equal;

            if (!string.IsNullOrWhiteSpace(context.Data))
            {
                var data = JsonConvert.DeserializeObject<ComparisonOperatorData>(context.Data);

                ComparisonType = data.ComparisonType;
            }
            else
            {
                ComparisonType = comparisonType;
            }

        }

        public override string GetData()
        {
            var data = new ComparisonOperatorData()
            {
                ComparisonType = ComparisonType
            };

            return JsonConvert.SerializeObject(data);
        }

        private ComparisonOperatorType ComparisonType
        {
            get { return _service.ComparisonType; }
            set
            {
                //If we can't find the specified service (which would be weird), just pick the first one.
                _service = _services.FirstOrDefault(s => s.ComparisonType == value) ?? _services[0];
                ParameterA.Postfix = " " + _service?.Text;
                Owner.MarkDirty();
            }
        }

        protected override async Task<object> EvaluateCoreAsync(IExecutionContext executionContext, CancellationToken cancellationToken)
        {
            var a = (double)await ParameterA.EvaluateAsync(executionContext, cancellationToken);
            var b = (double)await ParameterB.EvaluateAsync(executionContext, cancellationToken);

            return _service.Evaluate(a, b);
        }

        private class ComparisonOperatorService
        {
            private readonly ComparisonOperatorType _comparisonType;
            private readonly string _text;
            private readonly Func<double, double, bool> _func;

            internal ComparisonOperatorService(ComparisonOperatorType comparisonType, string text,
                Func<double, double, bool> func)
            {
                if (func == null) throw new ArgumentNullException(nameof(func));

                _comparisonType = comparisonType;
                _text = text;
                _func = func;
            }

            public ComparisonOperatorType ComparisonType
            {
                get { return _comparisonType; }
            }

            public string Text
            {
                get { return _text; }
            }

            public bool Evaluate(double a, double b)
            {
                return _func(a, b);
            }
        }

        public enum ComparisonOperatorType
        {
            LessThan = 0,

            LessThanOrEqual = 1,

            Equal = 2,

            NotEqual = 3,

            GreaterThan = 4,

            GreaterThanOrEqual = 5
        }

        private class ComparisonOperatorData
        {
            public ComparisonOperatorType ComparisonType { get; set; }
        }
    }
}