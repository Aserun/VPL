using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CaptiveAire.VPL.Interfaces;

namespace CaptiveAire.VPL
{
    public class Parameters : ObservableCollection<IElement>, IParameters
    {
        IEnumerator<IParameter> IEnumerable<IParameter>.GetEnumerator()
        {
            return this.ToArray<IElement>().Cast<IParameter>().GetEnumerator();
        }
    }
}