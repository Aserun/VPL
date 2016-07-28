using System;
using System.Windows;
using System.Windows.Input;
using CaptiveAire.VPL.Metadata;
using CaptiveAire.VPL.View;
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
            var dialog = new RenameDialog(Name, "Function")
            {
                Owner = WindowUtil.GetActiveWindow()
            };

            if (dialog.ShowDialog() == true)
            {
                Name = dialog.EditedName;
            }
        }

        private bool CanRename()
        {
            return true;
        }

        private void Edit()
        {
            try
            {
                HostViewModelLocator.VplService.EditFunction(_metadata, m =>
                {
                    _metadata = m;
                    RaisePropertyChanged(() => Name);
                });
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