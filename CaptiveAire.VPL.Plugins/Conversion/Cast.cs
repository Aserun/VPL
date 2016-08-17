using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CaptiveAire.VPL.Extensions;
using CaptiveAire.VPL.Interfaces;
using Newtonsoft.Json;

namespace CaptiveAire.VPL.Plugins.Conversion
{
    internal class Cast : OperatorBase
    {
        private IVplType _vplType;
        private IParameter SourceParameter;

        public Cast(IElementCreationContext context) 
            : base(context)
        {
            TypeSelected(VplTypeId.Any);

            //Add the action.
            AddAction(new ElementAction("Select Type...", SelectType));

            try
            {
                if (!string.IsNullOrWhiteSpace(context.Data))
                {
                    var data = JsonConvert.DeserializeObject<OperatorData>(context.Data);

                    TypeSelected(data.VplTypeId);
                }
            }
            catch (Exception ex)
            {
                //TODO: Add something to IElementCreationContext to log this to
                Console.WriteLine(ex);
            }
        }

        private void SelectType()
        {
            //Ask the user for a VPL type
            var selectedTypeId = Owner.Context.Types.SelectVplType(Type?.Id);

            //Select this type
            TypeSelected(selectedTypeId);

            //Mark the owner as dirty.
            Owner.MarkDirty();
        }

        public override object Label
        {
            get { return "Cast"; }
            set {  }
        }

        public override string GetData()
        {
            var vplType = Type;

            if (vplType == null)
                return null;

            var data = new OperatorData()
            {
                VplTypeId = vplType.Id
            };

            return JsonConvert.SerializeObject(data);
        }

        private void TypeSelected(Guid? typeId)
        {
            if (typeId == null)
            {
                _vplType = Owner.GetAnyType();
            }
            else
            {
                _vplType = Owner.GetVplTypeOrAny(typeId.Value);
            }

            Parameters.Clear();

            SourceParameter = Owner.CreateParameter("Source", _vplType);
            SourceParameter.Prefix = $"[{_vplType.Name}]";

            Parameters.Add(SourceParameter);
        }

        public override IVplType Type
        {
            get { return _vplType; }
        }

        protected override IError[] CheckForErrorsCore()
        {
            var errors = new List<IError>(1);

            if (Type == null)
            {
                errors.Add(new Error(this, "No type was selected.", ErrorLevel.Error));
            }

            return errors
                .ToArray();
        }

        protected override Task<object> EvaluateCoreAsync(IExecutionContext executionContext, CancellationToken cancellationToken)
        {
            return SourceParameter.EvaluateAsync(executionContext, cancellationToken);
        }

        private class OperatorData
        {
            public Guid VplTypeId { get; set; }
        }
    }
}