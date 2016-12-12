using System;
using CaptiveAire.VPL.Extensions;
using CaptiveAire.VPL.Interfaces;
using CaptiveAire.VPL.Metadata;
using CaptiveAire.VPL.Plugins.ViewModel;
using Cas.Common.WPF;

namespace CaptiveAire.VPL.Plugins.Functions
{
    internal class DefaultFunctionSelector : IFunctionSelector
    {
        private readonly IElementOwner _owner;

        public DefaultFunctionSelector(IElementOwner owner)
        {
            if (owner == null) throw new ArgumentNullException(nameof(owner));
            _owner = owner;
        }

        public IFunctionReference SelectFunction(Guid? selectedFunctionId)
        {
            var functionService = _owner.GetService<IFunctionService>();

            if (functionService == null)
            {
                var messageBoxService = new MessageBoxService();

                messageBoxService.Show("No function service was provided.");

                return null;
            }

            var functions = functionService.GetFunctions();

            var viewModel = new FunctionSelectionDialogViewModel(functions, selectedFunctionId);

            var view = new FunctionSelectionDialogView()
            {
                DataContext = viewModel,
                Owner = WindowUtil.GetActiveWindow()
            };

            if (view.ShowDialog() == true)
            {
                return viewModel.SelectedFunction;
            }

            return null;
        }
    }
}