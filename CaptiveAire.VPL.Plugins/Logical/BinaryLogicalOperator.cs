using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CaptiveAire.VPL.Extensions;
using CaptiveAire.VPL.Interfaces;
using Newtonsoft.Json;

namespace CaptiveAire.VPL.Plugins.Logical
{
    internal class BinaryLogicalOperator : Operator
    {
        private readonly IParameter ParameterA;
        private readonly IParameter ParameterB;
        private BinaryLogicalOperatorType _operatorType;

        private readonly IDictionary<BinaryLogicalOperatorType, BinaryLogicalOperatorService> _services;
        private BinaryLogicalOperatorService _service;

        public BinaryLogicalOperator(IElementCreationContext context, BinaryLogicalOperatorType defaultType) 
            : base(context, context.Owner.GetBooleanType())
        {
            ParameterA = context.Owner.CreateParameter("a", context.Owner.GetBooleanType());
            ParameterB = context.Owner.CreateParameter("b", context.Owner.GetBooleanType());

            Parameters.Add(ParameterA);
            Parameters.Add(ParameterB);

            var services = new[]
            {
                new BinaryLogicalOperatorService(BinaryLogicalOperatorType.And, "And", (a, b) => a && b),
                new BinaryLogicalOperatorService(BinaryLogicalOperatorType.Or, "Or", (a, b) => a || b),
            };

            foreach (var service in services)
            {
                AddAction(new ElementAction(service.Text, () => OperatorType = service.Type));
            }

            _services = services.ToDictionary(s => s.Type, s => s);

            OperatorType = defaultType;

            if (!string.IsNullOrWhiteSpace(context.Data))
            {
                var data = JsonConvert.DeserializeObject<BinaryLogicalOperatorData>(context.Data);

                OperatorType = data.OperatorType;
            }
        }

        public override string GetData()
        {
            var data = new BinaryLogicalOperatorData()
            {
                OperatorType = OperatorType
            };

            return JsonConvert.SerializeObject(data);
        }

        protected override async Task<object> EvaluateCoreAsync(IExecutionContext executionContext, CancellationToken cancellationToken)
        {
            var a = (bool)await ParameterA.EvaluateAsync(executionContext, cancellationToken);
            var b = (bool)await ParameterB.EvaluateAsync(executionContext, cancellationToken);

            return Service.Evaluate(a, b);
        }

        private BinaryLogicalOperatorType OperatorType
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

        private BinaryLogicalOperatorService Service
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

        public enum BinaryLogicalOperatorType
        {
            And = 0,

            Or = 1,
        }

        public class BinaryLogicalOperatorData
        {
            public BinaryLogicalOperatorType OperatorType { get; set; }
        }

        private class BinaryLogicalOperatorService
        {
            private readonly BinaryLogicalOperatorType _operatorType;
            private readonly string _text;
            private readonly Func<bool, bool, bool> _func;

            internal BinaryLogicalOperatorService(BinaryLogicalOperatorType operatorType, string text,
                Func<bool, bool, bool> func)
            {
                if (func == null) throw new ArgumentNullException(nameof(func));

                _operatorType = operatorType;
                _text = text;
                _func = func;
            }

            public BinaryLogicalOperatorType Type
            {
                get { return _operatorType; }
            }

            public string Text
            {
                get { return _text; }
            }

            public bool Evaluate(bool a, bool b)
            {
                return _func(a, b);
            }
        }
    }
}