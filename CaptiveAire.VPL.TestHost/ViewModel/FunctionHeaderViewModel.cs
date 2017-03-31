using System;
using System.Windows;
using System.Windows.Input;
using CaptiveAire.VPL.Metadata;
using Cas.Common.WPF;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace CaptiveAire.VPL.TestHost.ViewModel
{
    public class FunctionHeaderViewModel : ViewModelBase
    {
        private FunctionMetadata _metadata;

        public FunctionHeaderViewModel(FunctionMetadata metadata)
        {
            if (metadata == null) throw new ArgumentNullException(nameof(metadata));

            _metadata = metadata;

            EditCommand = new RelayCommand(Edit, CanEdit);
            RenameCommand = new RelayCommand(Rename, CanRename);
        }

        public FunctionMetadata GetMetadata()
        {
            return _metadata;
        }

        public string Name
        {
            get { return _metadata.Name; }
            set
            {
                _metadata.Name = value; 
                RaisePropertyChanged();
            }
        }

        public Guid Id
        {
            get { return _metadata.Id; }
        }

        public ICommand EditCommand { get; private set; }
        public ICommand RenameCommand { get; private set; }

        private void Rename()
        {
            var textEditService = new TextEditService();

            textEditService.EditText(Name, "Function", "Rename Function", t => Name = t, t => !string.IsNullOrWhiteSpace(t));
        }

        private bool CanRename()
        {
            return true;
        }

        public void Edit()
        {
            try
            {
                HostViewModelLocator.VplService.EditFunction(Id, m =>
                {
                    _metadata = m;
                    RaisePropertyChanged(() => Name);
                    
                }, false, owner: Application.Current.MainWindow);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private bool CanEdit()
        {
            return true;
        }
    }
}