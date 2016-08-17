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
        private readonly IVplServiceContext _serviceContext;

        public VplService(IEnumerable<IVplPlugin> plugins = null)
        {
            _serviceContext = new VplServiceContext(plugins);
        }

        public void EditFunction(FunctionMetadata metadata, Action<FunctionMetadata> saveAction, bool modal)
        {
            //Create the function view model
            var function = CreateRuntimeFunctionInner(metadata);
          
            //Create the editor view model
            var editorViewModel = new FunctionEditorDialogViewModel(_serviceContext, function, saveAction, new TextEditService());

            //Create the view
            var view = new FunctionEditorDialog(_serviceContext.CustomResources)
            {
                DataContext = editorViewModel
            };

            //Show the dialog
            if (modal)
            {
                view.Owner = WindowUtil.GetActiveWindow();
                view.ShowDialog();
            }
            else
            {
                view.Show();
            }
        }

        public IEnumerable<IVplType> Types
        {
            get { return _serviceContext.Types; }
        }

        public IExecutionContext CreateExecutionContext()
        {
            return new ExecutionContext(_serviceContext);
        }

        private Function CreateRuntimeFunctionInner(FunctionMetadata metadata)
        {
            //Create the view model
            var functionViewModel = new Function(_serviceContext, metadata.Id)
            {
                Name = metadata.Name
            };

            //Create the builder
            var builder = new ElementBuilder(_serviceContext.ElementFactoryManager, _serviceContext);

            //Add the elements to the owner (function)
            builder.LoadFunction(functionViewModel, metadata);

            return functionViewModel;
        }
    }
}