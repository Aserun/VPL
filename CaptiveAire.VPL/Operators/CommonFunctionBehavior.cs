using System;
using System.Windows;
using CaptiveAire.VPL.Extensions;
using CaptiveAire.VPL.Interfaces;
using CaptiveAire.VPL.Metadata;
using CaptiveAire.VPL.Model;
using CaptiveAire.VPL.View;
using CaptiveAire.VPL.ViewModel;
using Cas.Common.WPF;
using Cas.Common.WPF.Interfaces;
using GalaSoft.MvvmLight;
using Newtonsoft.Json;

namespace CaptiveAire.VPL.Operators
{
    internal class CommonFunctionBehavior : ViewModelBase
    {
        private readonly ParametersViewModel _parameters;
        private readonly string _text;
        private readonly IViewService _viewService;
        private readonly IElementOwner _owner;
        private readonly CallFunctionView _labelView;
        private readonly CallFunctionData _model;
        private FunctionMetadata _function;
        private readonly IElementAction[] _actions;

        internal CommonFunctionBehavior(IElementCreationContext context, ParametersViewModel parameters, string text)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));

            _owner = context.Owner;
            _parameters = parameters;
            _text = text;
            _viewService = context.Owner.Context.ViewService;

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

            if (_viewService.ShowDialog(viewModel) == true)
            {
                SelectFunction(viewModel.SelectedFunction);
            }
        }

        private void SelectFunction(FunctionMetadata function)
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
                        _parameters.Add(new ParameterViewModel(_owner, parameterIndex.ToString(), _owner.GetVplType(argument.TypeId))
                        {
                            Prefix = argument.Name
                        });

                        parameterIndex++;
                    }
                }
            }
        }

        private FunctionMetadata Function
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

        public bool HasFunction
        {
            get { return Function != null; }
        }

        public FunctionViewModel GetFunctionOrThrow()
        {
            if (Function == null)
                throw new InvalidOperationException("Unable to find function to call.");

            //Create an instance of the function to be called
            var function = new FunctionViewModel(_owner.Context, Function.Id);

            //Create the element builder
            var elementBuilder = new ElementBuilder(_owner.Context.ElementFactoryManager, function);

            //Load up this function
            elementBuilder.LoadFunction(function, Function);

            //We've got the function now
            return function;
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
    }
}