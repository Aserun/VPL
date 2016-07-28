using System;
using System.Linq;
using System.Windows.Input;
using CaptiveAire.VPL.Extensions;
using CaptiveAire.VPL.Metadata;
using CaptiveAire.VPL.TestHost.Metadata;
using CaptiveAire.VPL.TestHost.Model;
using CaptiveAire.VPL.View;
using Cas.Common.WPF;
using Cas.Common.WPF.Behaviors;
using Cas.Common.WPF.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace CaptiveAire.VPL.TestHost.ViewModel
{
    public class MainViewModel : ViewModelBase, ICloseableViewModel
    {
        private readonly IFileDialogService _fileDialogService;

        private ProgramViewModel _program = new ProgramViewModel(null);
        private FunctionHeaderViewModel _selectedFunction;
        private string _filename;
        private const string FileFilter = "Visual Programmer Files (*.vpl)|*.vpl|All Files (*.*)|*.*";

        public MainViewModel(IFileDialogService fileDialogService)
        {
            if (fileDialogService == null) throw new ArgumentNullException(nameof(fileDialogService));

            _fileDialogService = fileDialogService;

            OpenCommand = new RelayCommand(Open, CanOpen);
            SaveCommand = new RelayCommand(() => Save(), CanSave);
            SaveAsCommand = new RelayCommand(() => SaveAs(), CanSaveAs);
            AddFunctionCommand = new RelayCommand(AddFunction, CanAddFunction);
            DeleteCommand = new RelayCommand(Delete, CanDelete);
        }

        public ICommand OpenCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand SaveAsCommand { get; private set; }
        public ICommand AddFunctionCommand { get; private set; }

        private void AddFunction()
        {
            var name = Program.Functions.Select(f => f.Name).CreateUniqueName("Function {0}");

            var dialog = new RenameDialog(name, "New Function")
            {
                Owner = WindowUtil.GetActiveWindow()
            };

            if (dialog.ShowDialog() == true)
            {
                var functionHeaderMetadata = new FunctionMetadata()
                {
                    Name = dialog.EditedName,
                    Id = Guid.NewGuid(),
                    Elements = new ElementMetadata[] { },
                    Width = 1000,
                    Height = 1000
                };

                var functionHeaderViewModel = new FunctionHeaderViewModel(functionHeaderMetadata);

                Program.Functions.Add(functionHeaderViewModel);
            }           
        }

        public ICommand DeleteCommand { get; private set; }

        private void Delete()
        {
            Program.Functions.Remove(SelectedFunction);
        }

        private bool CanDelete()
        {
            return SelectedFunction != null;
        }


        private bool CanAddFunction()
        {
            return true;
        }

        private bool Save()
        {
            if (string.IsNullOrWhiteSpace(_filename))
            {
                return SaveAs();
            }

            SaveInner();

            return true;
        }

        private bool SaveAs()
        {
            var options = new FileDialogOptions()
            {
                Filter = FileFilter
            };

            var result = _fileDialogService.ShowSaveFileDialog(options);

            if (result != null)
            {
                Filename = result.FileName;

                SaveInner();

                return true;
            }

            return false;
        }

        private bool CanSaveAs()
        {
            return true;
        }

        private bool CanSave()
        {
            return true;
        }

        private void Open()
        {
            var options = new FileDialogOptions()
            {
                Filter = FileFilter
            };

            var result = _fileDialogService.ShowOpenFileDialog(options);

            if (result != null)
            {
                var metadata = HostPersistor.LoadFromFile(result.FileName);

                Filename = result.FileName;

                Program = new ProgramViewModel(metadata.Functions);
            }
        }

        private bool CanOpen()
        {
            return true;
        }

        private void SaveInner()
        {
            var metadata = GetMetadata();

            HostPersistor.SaveToFile(Filename, metadata);
        }

        private RootMetadata GetMetadata()
        {
            return new RootMetadata()
            {
                Functions = Program.Functions.Select(f => f.GetMetadata()).ToArray()
            };
        }

        public ProgramViewModel Program
        {
            get { return _program; }
            private set
            {
                _program = value;
                RaisePropertyChanged();
            }
        }

        public FunctionHeaderViewModel SelectedFunction
        {
            get { return _selectedFunction; }
            set
            {
                _selectedFunction = value; 
                RaisePropertyChanged();
            }
        }

        public string Filename
        {
            get { return _filename; }
            private set
            {
                _filename = value; 
                RaisePropertyChanged();
                RaisePropertyChanged(() => Title);
            }
        }

        public string Title
        {
            get { return $"VPL Editor - {Filename}"; }
        }

        public bool CanClose()
        {
            return true;
        }

        public void Closed()
        {
        }

        public event EventHandler<CloseEventArgs> Close;
    }
}