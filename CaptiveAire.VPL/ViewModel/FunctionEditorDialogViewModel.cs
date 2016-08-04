using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using CaptiveAire.VPL.Extensions;
using CaptiveAire.VPL.Interfaces;
using CaptiveAire.VPL.Metadata;
using CaptiveAire.VPL.Model;
using CaptiveAire.VPL.View;
using Cas.Common.WPF;
using Cas.Common.WPF.Behaviors;
using Cas.Common.WPF.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Newtonsoft.Json;

namespace CaptiveAire.VPL.ViewModel
{
    internal class FunctionEditorDialogViewModel : ViewModelBase, ICloseableViewModel
    {
        private readonly IVplServiceContext _context;
        private readonly Function _function;
        private readonly Action<FunctionMetadata> _saveAction;
        private readonly ITextEditService _textEditService;
        private CancellationTokenSource _cts;
        private readonly ToolsViewModel<IElementFactory> _tools;
        private IVplType _selectedType;
        private readonly IMessageBoxService _messageBoxService = new MessageBoxService();
        private ErrorViewModel[] _errors;

        public FunctionEditorDialogViewModel(IVplServiceContext context, Function function, Action<FunctionMetadata> saveAction, ITextEditService textEditService)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (function == null) throw new ArgumentNullException(nameof(function));
            if (saveAction == null) throw new ArgumentNullException(nameof(saveAction));
            if (textEditService == null) throw new ArgumentNullException(nameof(textEditService));

            _context = context;
            _function = function;
            _saveAction = saveAction;
            _textEditService = textEditService;

            //Commands
            RunCommand = new RelayCommand(Run, CanRun);
            StopCommand = new RelayCommand(Stop, CanStop);
            OkCommand = new RelayCommand(Ok, CanOk);
            CloseCommand = new RelayCommand(Cancel, CanCancel);
            SaveCommand = new RelayCommand(Apply, CanOk);
            AddVariableCommand = new RelayCommand(AddVariable, CanAddVariable);
            PasteCommand = new RelayCommand(Paste, CanPaste);
            SelectReturnTypeCommand = new RelayCommand(SelectReturnType);
            ClearReturnTypeCommand = new RelayCommand(() => ClearReturnType(), CanClearReturnType);
            CheckForErrorsCommand = new RelayCommand(CheckForErrors);

            //Create the toolbox
            _tools = new ToolsViewModel<IElementFactory>(context.ElementFactoryManager.Factories.Where(f => f.ShowInToolbox));

            //Select a default type
            SelectedType = context.Types.FirstOrDefault(t => t.Id == VplTypeId.Float);
        }

        public ICommand RunCommand { get; private set; }
        public ICommand StopCommand { get; private set; }
        public ICommand OkCommand { get; private set; }
        public ICommand CloseCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand AddVariableCommand { get; private set; }
        public ICommand PasteCommand { get; private set; }
        public ICommand SelectReturnTypeCommand { get; private set; }
        public ICommand ClearReturnTypeCommand { get; private set; }
        public ICommand CheckForErrorsCommand { get; private set; }

        private void CheckForErrors()
        {
            Errors = null;

            var errors = Function.CheckForErrors();

            if (errors.Length == 0)
            {
                _messageBoxService.Show("No errors or warnings found.");
            }
            else
            {
                Errors = errors
                    .Select(e => new ErrorViewModel(e))
                    .ToArray();
            }
        }

        private void SelectReturnType()
        {
            //Create the view model
            var viewModel = new SelectTypeDialogViewModel(_context.Types)
            {
                SelectedTypeId = Function.ReturnTypeId
            };

            var view = new SelectTypeDialogView()
            {
                Owner = WindowUtil.GetActiveWindow(),
                DataContext = viewModel
            };

            if (view.ShowDialog() == true)
            {
                if (ClearReturnType())
                {

                    Function.ReturnTypeId = viewModel.SelectedTypeId;

                    if (Function.ReturnTypeId != null)
                    {
                        //Get the type
                        var type = Function.GetVplType(Function.ReturnTypeId.Value);

                        //Add the return variable
                        Function.AddVariable(new ReturnValueVariable(Function, type));
                    }
                }
            }
        }

        private bool ClearReturnType()
        {
            //Check to see if the return variable is in use.
            var variable = Function.GetVariable(ReturnValueVariable.ReturnVariableId);

            if (variable == null)
                return true;
            
            //Try to delete the variable
            if (variable.Delete())
            {
                Function.ReturnTypeId = null;

                return true;
            }

            return false;
        }

        public ErrorViewModel[] Errors
        {
            get { return _errors; }
            private set
            {
                _errors = value; 
                RaisePropertyChanged();
            }
        }

        private bool CanClearReturnType()
        {
            return _function.ReturnTypeId.HasValue;
        }

        private void Paste()
        {
            try
            {
                string json = Clipboard.GetData(nameof(ElementMetadata)) as string;

                if (!string.IsNullOrWhiteSpace(json))
                {
                    var element = JsonConvert.DeserializeObject<ElementMetadata>(json);

                    var elementBuilder = new ElementBuilder(Function.Context.ElementFactoryManager, _context);

                    elementBuilder.AddToOwner(Function, new ElementMetadata[] { element });
                }
            }
            catch (Exception ex)
            {
                _messageBoxService.Show(ex.Message, "Unable to paste");
            }
        }

        private bool CanPaste()
        {
            return Clipboard.ContainsData(nameof(ElementMetadata));
        }

        private void AddVariable()
        {
            var name = Function.Variables.Select(v => v.Name).CreateUniqueName($"{SelectedType.Name} {{0}}");

            bool wasAccepted = false;

            _textEditService.EditText(name, "Name", "Add Variable", t =>
            {
                name = t;
                wasAccepted = true;
            }, t => !string.IsNullOrWhiteSpace(t));

            if (wasAccepted)
            {
                var variable = new Variable(Function, SelectedType, Guid.NewGuid())
                {
                    Name = name
                };

                Function.AddVariable(variable);
            }
        }

        private bool CanAddVariable()
        {
            return SelectedType != null;
        }

        public IVplType SelectedType
        {
            get { return _selectedType; }
            set
            {
                _selectedType = value; 
                RaisePropertyChanged();
            }
        }

        public ToolsViewModel<IElementFactory> Tools
        {
            get { return _tools; }
        }

        private void Cancel()
        {
            Close?.Invoke(this, new CloseEventArgs(false));
        }

        private bool CanCancel()
        {
            return true;
        }

        private void Ok()
        {
            Apply();

            Close?.Invoke(this, new CloseEventArgs(true));
        }

        private bool CanOk()
        {
            return Function.IsDirty;
        }

        private bool Save()
        {
            try
            {
                _saveAction(GetMetadata());

                Function.MarkClean();

                return true;
            }
            catch (Exception ex)
            {
                _messageBoxService.Show(ex.Message, "Unable to save");
            }

            return false;
        }

        private void Apply()
        {
            Save();
        }

        private bool SaveIfDirty()
        {
            if (!Function.IsDirty)
                return true;

            var result = _messageBoxService.Show("Save changes?", "Function has changed", MessageBoxButton.YesNoCancel);

            switch (result)
            {
                case MessageBoxResult.Yes:
                    return Save();

                case MessageBoxResult.No:
                    return true;

               case MessageBoxResult.Cancel:
                    return false;

                default:
                    _messageBoxService.Show("That was odd.");
                    return false;
            }
        }

        private void Run()
        {
            RunInner(Function);
        }

        private bool CanRun()
        {
            return _cts == null;
        }

        private async void RunInner(IFunction function)
        {
            try
            {
                Errors = null;

                _cts = new CancellationTokenSource();

                await function.ExecuteAsync(new object[] {}, _cts.Token);

                _cts = null;

                _messageBoxService.Show("Done");
            }
            catch (Exception ex)
            {
                _cts = null;

                _messageBoxService.Show(ex.Message);
            }
        }

        private void Stop()
        {
            _cts?.Cancel();
        }

        private bool CanStop()
        {
            return _cts != null;
        }

        public Function Function
        {
            get { return _function; }
        }

        public FunctionMetadata GetMetadata()
        {
            return _function.ToMetadata();
        }

        public bool CanClose()
        {
            if (_cts != null)
            {
                _messageBoxService.Show("Cannot close while running");
                return false;
            }

            return SaveIfDirty();
        }

        public void Closed()
        {
        }

        public event EventHandler<CloseEventArgs> Close;
    }
}