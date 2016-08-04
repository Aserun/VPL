using System;
using System.Collections.Generic;
using CaptiveAire.VPL.Interfaces;
using CaptiveAire.VPL.Metadata;
using CaptiveAire.VPL.Model;
using CaptiveAire.VPL.View;
using CaptiveAire.VPL.ViewModel;
using Cas.Common.WPF;

namespace CaptiveAire.VPL
{
    public class VplService : IVplService
    {
        private readonly IVplServiceContext _context;

        public VplService(IEnumerable<IVplPlugin> plugins = null)
        {
            _context = new VplServiceContext(plugins);
        }

        public void EditFunction(FunctionMetadata metadata, Action<FunctionMetadata> saveAction, bool modal)
        {
            //Create the view model
            var functionViewModel = CreateRuntimeFunctionInner(metadata);
          
            //Create the view model
            var editorViewModel = new FunctionEditorDialogViewModel(_context, functionViewModel, saveAction, new TextEditService());

            //Create the view
            var view = new FunctionEditorDialog(_context.CustomResources)
            {
                Owner = WindowUtil.GetActiveWindow(),
                DataContext = editorViewModel
            };

            //Show the dialog
            if (modal)
            {
                view.ShowDialog();
            }
            else
            {
                view.Show();
            }
        }

        public IFunction CreateRuntimeFunction(FunctionMetadata metadata)
        {
            return CreateRuntimeFunctionInner(metadata);
        }

        public Guid? SelectVplType(Guid? selectedVplType = null)
        {
            //Create the view model
            var viewModel = new SelectTypeDialogViewModel(_context.Types)
            {
                SelectedTypeId = selectedVplType
            };

            var view = new SelectTypeDialogView()
            {
                Owner = WindowUtil.GetActiveWindow(),
                DataContext = viewModel
            };

            if (view.ShowDialog() == true)
            {
                return viewModel.SelectedTypeId;
            }

            return null;
        }

        public IEnumerable<IVplType> Types
        {
            get { return _context.Types; }
        }

        private Function CreateRuntimeFunctionInner(FunctionMetadata metadata)
        {
            //Create the view model
            var functionViewModel = new Function(_context, metadata.Id)
            {
                Name = metadata.Name
            };

            //Create the builder
            var builder = new ElementBuilder(_context.ElementFactoryManager, _context);

            //Add the elements to the owner (function)
            builder.LoadFunction(functionViewModel, metadata);

            return functionViewModel;
        }
    }
}