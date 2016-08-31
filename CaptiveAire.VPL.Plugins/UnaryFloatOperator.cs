using System;
using System.Threading;
using System.Threading.Tasks;
using CaptiveAire.VPL.Extensions;
using CaptiveAire.VPL.Interfaces;

namespace CaptiveAire.VPL.Plugins
{
    internal class UnaryFloatOperator : Operator
    {
        private readonly Func<double, double> _func;
        private readonly IParameter Parameter;
        private readonly string _label;

        internal UnaryFloatOperator(IElementCreationContext context, Func<double, double> func, string label) 
            : base(context, context.Owner.GetFloatType())
        {
            if (func == null) throw new ArgumentNullException(nameof(func));

            _func = func;
            _label = label;

            Parameter = context.Owner.CreateParameter("a", context.Owner.GetFloatType());
            Parameters.Add(Parameter);
        }

        public override object Label
        {
            get { return _label; }
            set {  }
        }

        protected override async Task<object> EvaluateCoreAsync(IExecutionContext executionContext, CancellationToken cancellationToken)
        {
            //Evaluate the parameter
            var rawValue = await Parameter.EvaluateAsync(executionContext, cancellationToken);

            //Convert to a double (hopefully)
            var value = rawValue.GetConvertedValue<double>();

            //Call the implementation for this function
            return _func(value);
        }        
    }
}