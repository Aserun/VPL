using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
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

        private readonly IFunctionEditorManager _functionEditorManager = new FunctionEditorManager();

        public VplService(IEnumerable<IVplPlugin> plugins = null)
        {
            _serviceContext = new VplServiceContext(this, plugins);
        }

        [Obsolete]
        public void EditFunction(FunctionMetadata metadata, Action<FunctionMetadata> saveAction, bool modal, string displayName)
        {
            var existing = _functionEditorManager.Get(metadata.Id);

            if (existing == null)
            {

                //Create the function view model
                var function = CreateRuntimeFunctionInner(metadata);

                //Create the editor view model
                var editorViewModel = new FunctionEditorDialogViewModel(_serviceContext, function, saveAction,
                    new TextEditService(), displayName, _functionEditorManager);

                //Create the view
                var view = new FunctionEditorDialog(_serviceContext.CustomResources)
                {
                    DataContext = editorViewModel,
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
            else
            {
                //Activate this one
                existing.Activate();
            }
        }

        public void EditFunction(Guid id, Action<FunctionMetadata> saveAction, bool modal = true, string displayName = null, Window owner = null)
        {
            //Try to get the function service.
            var functionService = _serviceContext.Services
                .OfType<IFunctionService>()
                .FirstOrDefault();

            if (functionService == null)
                throw new InvalidOperationException("No function service has been registered.");

            var existing = _functionEditorManager.Get(id);

            if (existing == null)
            {
                //Get the metadata.
                var metadata = functionService.GetFunction(id);

                //Create the function view model
                var function = CreateRuntimeFunctionInner(metadata);

                //Create the editor view model
                var editorViewModel = new FunctionEditorDialogViewModel(_serviceContext, function, saveAction,
                    new TextEditService(), displayName, _functionEditorManager);

                //Create the view
                var view = new FunctionEditorDialog(_serviceContext.CustomResources)
                {
                    DataContext = editorViewModel,
                    Owner = WindowUtil.GetActiveWindow()
                };

                //Show the dialog
                if (modal)
                {
                    view.ShowDialog();
                }
                else
                {
                    view.Owner = owner;
                    view.Show();
                }
            }
            else
            {
                //Activate this one
                existing.Activate();
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

    internal class FunctionEditorManager : IFunctionEditorManager
    {
        private readonly IDictionary<Guid, IFunctionEditorDialogViewModel> _editors = new Dictionary<Guid, IFunctionEditorDialogViewModel>();

        private static Guid GetId(IFunctionEditorDialogViewModel editor)
        {
            return editor.Function.Id;
        }

        public void Register(IFunctionEditorDialogViewModel editor)
        {
            var id = GetId(editor);

            if (_editors.ContainsKey(id))
                throw new Exception($"Function {id} has already been registered.");

            _editors.Add(id, editor);
        }

        public void Unregister(IFunctionEditorDialogViewModel editor)
        {
            var id = GetId(editor);

            if (_editors.ContainsKey(id))
            {
                _editors.Remove(id);
            }
        }

        public IFunctionEditorDialogViewModel Get(Guid id)
        {
            IFunctionEditorDialogViewModel editor;

            _editors.TryGetValue(id, out editor);

            return editor;
        }
    }
}