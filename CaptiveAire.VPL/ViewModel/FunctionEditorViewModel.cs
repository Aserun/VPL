using System;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using CaptiveAire.VPL.Extensions;
using CaptiveAire.VPL.Interfaces;
using CaptiveAire.VPL.Metadata;
using CaptiveAire.VPL.Model;
using Cas.Common.WPF;
using Cas.Common.WPF.Behaviors;
using Cas.Common.WPF.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Newtonsoft.Json;

namespace CaptiveAire.VPL.ViewModel
{
    internal class FunctionEditorViewModel : ViewModelBase, ICloseableViewModel
    {
        private readonly IVplServiceContext _context;
        private readonly Function _function;
        private readonly Action<FunctionMetadata> _saveAction;
        private readonly ITextEditService _textEditService;
        private CancellationTokenSource _cts;
        private readonly ToolsViewModel<IElementFactory> _tools;
        private IVplType _selectedType;
        private readonly IMessageBoxService _messageBoxService = new MessageBoxService();

        public FunctionEditorViewModel(IVplServiceContext context, Function function, Action<FunctionMetadata> saveAction, ITextEditService textEditService)
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
            CancelCommand = new RelayCommand(Cancel, CanCancel);
            ApplyCommand = new RelayCommand(Apply, CanOk);
            AddVariableCommand = new RelayCommand(AddVariable, CanAddVariable);
            PasteCommand = new RelayCommand(Paste, CanPaste);

            //Create the toolbox
            _tools = new ToolsViewModel<IElementFactory>(context.ElementFactoryManager.Factories.Where(f => f.ShowInToolbox));

            //Select a default type
            SelectedType = context.Types.FirstOrDefault(t => t.Id == VplTypeId.Float);
        }

        public ICommand RunCommand { get; private set; }
        public ICommand StopCommand { get; private set; }
        public ICommand OkCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }
        public ICommand ApplyCommand { get; private set; }
        public ICommand AddVariableCommand { get; private set; }
        public ICommand PasteCommand { get; private set; }

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
                _cts = new CancellationTokenSource();

                await function.ExecuteAsync(new object[] {}, _cts.Token);

                _cts = null;

                MessageBox.Show("Done");
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