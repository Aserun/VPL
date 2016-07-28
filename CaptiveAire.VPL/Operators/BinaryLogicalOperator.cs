using System;
using System.Collections.Generic;
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
    public class BinaryLogicalOperator : OperatorViewModel
    {
        private readonly ParameterViewModel ParameterA;
        private readonly ParameterViewModel ParameterB;
        private BinaryLogicalOperatorType _operatorType;

        private readonly IDictionary<BinaryLogicalOperatorType, BinaryLogicalOperatorService> _services;
        private BinaryLogicalOperatorService _service;

        public BinaryLogicalOperator(IElementCreationContext context, BinaryLogicalOperatorType defaultType) 
            : base(context.Owner, Model.SystemElementIds.BinaryLogicOperator, context.Owner.GetBooleanType())
        {
            ParameterA = new ParameterViewModel(context.Owner, "a", context.Owner.GetBooleanType());
            ParameterB = new ParameterViewModel(context.Owner, "b", context.Owner.GetBooleanType());

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

        public override async Task<object> EvaluateAsync(CancellationToken token)
        {
            var a = (bool)await ParameterA.EvaluateAsync(token);
            var b = (bool)await ParameterB.EvaluateAsync(token);

            return Service.Evaluate(a, b);
        }

        private BinaryLogicalOperatorType OperatorType
        {
            get { return _operatorType; }
            set
            {
                _operatorType = value; 
                RaisePropertyChanged();

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