using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using CaptiveAire.VPL.Extensions;
using CaptiveAire.VPL.Interfaces;
using CaptiveAire.VPL.Metadata;
using CaptiveAire.VPL.Plugins.ViewModel;
using Cas.Common.WPF;
using GalaSoft.MvvmLight;
using Newtonsoft.Json;

namespace CaptiveAire.VPL.Plugins.Functions
{
    internal class CommonFunctionBehavior : ViewModelBase
    {
        private readonly Parameters _parameters;
        private readonly string _text;
        private readonly IElement _parent;
        private readonly IElementOwner _owner;
        private readonly CallFunctionView _labelView;
        private readonly CallFunctionData _model;
        private IFunctionReference _function;
        private readonly IElementAction[] _actions;

        internal CommonFunctionBehavior(IElementCreationContext context, Parameters parameters, string text, IElement parent)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));
            if (parent == null) throw new ArgumentNullException(nameof(parent));


            _owner = context.Owner;
            _parameters = parameters;
            _text = text;
            _parent = parent;

            _labelView = new CallFunctionView()
            {
                DataContext = this
            };

            if (string.IsNullOrWhiteSpace(context.Data))
            {
                _model = new CallFunctionData();
            }
            else
            {
                _model = JsonConvert.DeserializeObject<CallFunctionData>(context.Data);

                var functionService = context.Owner.GetService<IFunctionService>();

                if (functionService == null)
                {
                    MessageBox.Show("No function service was provided.");
                    return;
                }

                //Get the function
                SelectFunction(functionService.GetFunction(_model.FunctionId));
            }

            _actions = new IElementAction[]
            {
                new ElementAction("Select function...", SelectFunction)
            };
        }

        public async Task<object[]> GetParameterValuesAsync(IExecutionContext executionContext, CancellationToken cancellationToken)
        {
            //Create a place to store the values we get from the parameters.
            var values = new List<object>(_parameters.Count);

            //Deal with each parameter
            foreach (var parameter in (IParameters)_parameters)
            {
                //Evaluate the parameter
                var value = await parameter.EvaluateAsync(executionContext, cancellationToken);

                //Add the value to the results
                values.Add(value);
            }

            return values.ToArray();
        }

        public string GetData()
        {
            return JsonConvert.SerializeObject(_model);
        }

        public object Label
        {
            get { return _labelView; }
        }

        private void SelectFunction()
        {
            var functionService = _owner.GetService<IFunctionService>();

            if (functionService == null)
            {
                var messageBoxService = new MessageBoxService();

                messageBoxService.Show("No function service was provided.");

                return;
            }

            var functions = functionService.GetFunctions();

            var viewModel = new FunctionSelectionDialogViewModel(functions, _model.FunctionId);

            var view = new FunctionSelectionDialogView()
            {
                DataContext = viewModel
            };

            if (view.ShowDialog() == true)
            {
                SelectFunction(viewModel.SelectedFunction);
            }
        }

        private void SelectFunction(IFunctionReference function)
        {
            //Ditch the parameters
            _parameters.Clear();

            Function = function;

            if (function != null)
            {
                _model.FunctionId = function.Id;

                if (function.Arguments != null)
                {
                    int parameterIndex = 0;

                    foreach (var argument in function.Arguments)
                    {
                        //Create the parameter
                        var parameter = _owner.CreateParameter(
                            parameterIndex.ToString(),
                            _owner.GetVplTypeOrThrow(argument.TypeId), 
                            argument.Name);

                        _parameters.Add(parameter);

                        parameterIndex++;
                    }
                }
            }
        }

        private IFunctionReference Function
        {
            get { return _function; }
            set
            {
                _function = value;
                RaisePropertyChanged();
                RaisePropertyChanged(() => FunctionName);
                RaisePropertyChanged(() => HasFunction);
            }
        }

        public Guid? FunctionId
        {
            get { return _function?.Id; }
        }

        public bool HasFunction
        {
            get { return Function != null; }
        }

        public string FunctionName
        {
            get { return Function?.Name; }
        }

        public IElementAction[] Actions
        {
            get { return _actions; }
        }

        public string Text
        {
            get { return _text; }
        }

        private class CallFunctionData
        {
            public Guid FunctionId { get; set; }
        }

        internal IError[] CheckForErrors()
        {
            var errors = new List<IError>(1);

            if (!HasFunction)
            {
                errors.Add(new Error(_parent, "No function is selected.", ErrorLevel.Error));
            }

            return errors.ToArray();
        }
    }
}