using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CaptiveAire.VPL.Interfaces;

namespace CaptiveAire.VPL.ViewModel
{
    public class ParametersViewModel : ObservableCollection<IElement>, IParameters
    {
        IEnumerator<IParameter> IEnumerable<IParameter>.GetEnumerator()
        {
            return this.ToArray<IElement>().Cast<IParameter>().GetEnumerator();
        }
    }
}