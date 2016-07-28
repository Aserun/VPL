using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CaptiveAire.VPL.Metadata;
using GalaSoft.MvvmLight;

namespace CaptiveAire.VPL.TestHost.ViewModel
{
    public class ProgramViewModel : ViewModelBase
    {
        private readonly ObservableCollection<FunctionHeaderViewModel> _functions = new ObservableCollection<FunctionHeaderViewModel>();
        private FunctionHeaderViewModel _selectedFunction;

        public ProgramViewModel(IEnumerable<FunctionMetadata> functions)
        {
            if (functions != null)
            {
                foreach (var function in functions.OrderBy(f => f.Name))
                {
                    _functions.Add(new FunctionHeaderViewModel(function));
                }
            }

            SelectedFunction = Functions.FirstOrDefault();
        }
        
        public ObservableCollection<FunctionHeaderViewModel> Functions
        {
            get { return _functions; }
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
    }
}