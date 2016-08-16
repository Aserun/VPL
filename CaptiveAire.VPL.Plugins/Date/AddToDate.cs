using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CaptiveAire.VPL.Extensions;
using CaptiveAire.VPL.Interfaces;
using Newtonsoft.Json;

namespace CaptiveAire.VPL.Plugins.Date
{
    public class AddToDate : Operator
    {
        private readonly IParameter DateParameter;
        private readonly IParameter AmountParameter;

        private readonly OperationService[] _services;
        private OperationService _service;
        private OperationType _operation;

        public AddToDate(IElementCreationContext context) 
            : base(context, context.Owner.GetVplTypeOrThrow(VplTypeId.DateTime))
        {
            DateParameter = context.Owner.CreateParameter("date", context.Owner.GetVplTypeOrThrow(VplTypeId.DateTime),
                "Date");

            DateParameter.Postfix = " + ";

            AmountParameter = context.Owner.CreateParameter("amount", context.Owner.GetFloatType());

            Parameters.Add(DateParameter);
            Parameters.Add(AmountParameter);

            _services = new OperationService[]
            {
                new OperationService(OperationType.Seconds, "Seconds", TimeSpan.FromSeconds),
                new OperationService(OperationType.Minutes, "Minutes", TimeSpan.FromMinutes),
                new OperationService(OperationType.Hours, "Hours", TimeSpan.FromHours),
                new OperationService(OperationType.Days, "Days", TimeSpan.FromDays),
            };

            Operation = OperationType.Seconds;

            if (!string.IsNullOrWhiteSpace(context.Data))
            {
                var data = JsonConvert.DeserializeObject<OperatorData>(context.Data);

                Operation = data.Operation;
            }

            foreach (var service in _services)
            {
                AddAction(new ElementAction(service.Text, () => Operation = service.Operation, () => Operation != service.Operation));
            }
        }

        public override async Task<object> EvaluateAsync(IExecutionContext executionContext, CancellationToken token)
        {
            var date = (DateTime) await DateParameter.EvaluateAsync(executionContext, token);
            var amount = (double) await AmountParameter.EvaluateAsync(executionContext, token);

            return date + _service.GetTimeSpan(amount);           
        }

        public override string GetData()
        {
            var data = new OperatorData()
            {
                Operation = Operation
            };

            return JsonConvert.SerializeObject(data);
        }

        private OperationType Operation
        {
            get { return _operation; }
            set
            {
                _operation = value;
                _service = _services.FirstOrDefault(s => s.Operation == value) ?? _services.First();

                AmountParameter.Postfix = _service.Text;
                Owner.MarkDirty();
            }
        }

        private class OperationService
        {
            private readonly OperationType _operation;
            private readonly string _text;
            private readonly Func<double, TimeSpan> _func;

            public OperationService(OperationType operation, string text, Func<double, TimeSpan> func)
            {
                if (func == null) throw new ArgumentNullException(nameof(func));
                _operation = operation;
                _text = text;
                _func = func;
            }

            public OperationType Operation
            {
                get { return _operation; }
            }

            public string Text
            {
                get { return _text; }
            }

            public TimeSpan GetTimeSpan(double amount)
            {
                return _func(amount);
            }
        }

        public enum OperationType
        {
            Seconds = 0,

            Minutes = 1,

            Hours = 2,

            Days = 3,
        }

        private class OperatorData
        {
            public OperationType Operation { get; set; }
        }
    }
}