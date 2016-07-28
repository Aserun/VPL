using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CaptiveAire.VPL.Extensions;
using CaptiveAire.VPL.Interfaces;
using CaptiveAire.VPL.Model;
using CaptiveAire.VPL.ViewModel;
using Newtonsoft.Json;

namespace CaptiveAire.VPL.Operators
{
    public class ComparisonOperator : OperatorViewModel
    {
        private readonly ParameterViewModel ParameterA;
        private readonly ParameterViewModel ParameterB;

        private readonly ComparisonOperatorService[] _services;
        private ComparisonOperatorService _service;

        public ComparisonOperator(IElementCreationContext context, ComparisonOperatorType comparisonType) 
            : base(context.Owner, SystemElementIds.ComparisonOperator, context.Owner.GetBooleanType())
        {
            ParameterA = new ParameterViewModel(context.Owner, "a", context.Owner.GetFloatType());
            ParameterB = new ParameterViewModel(context.Owner, "b", context.Owner.GetFloatType());

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
            }
        }

        public override async Task<object> EvaluateAsync(CancellationToken token)
        {
            var a = (double)await ParameterA.EvaluateAsync(token);
            var b = (double)await ParameterB.EvaluateAsync(token);

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