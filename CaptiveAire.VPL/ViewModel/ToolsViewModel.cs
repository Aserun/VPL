using System.Collections.Generic;
using System.Linq;
using CaptiveAire.VPL.Interfaces;
using GalaSoft.MvvmLight;

namespace CaptiveAire.VPL.ViewModel
{
    internal class ToolsViewModel<TTool> : ViewModelBase
       where TTool : ITool
    {
        private readonly IEnumerable<ToolCategoryViewModel<TTool>> _categories;

        public ToolsViewModel(IEnumerable<TTool> tools)
        {
            var categories = tools.GroupBy(t => t.Category);

            _categories = categories
                .Select(c => new ToolCategoryViewModel<TTool>(c.Key, c.OrderBy(f => f.Name)))
                .OrderBy(c => c.Name)
                .ToArray();
        }

        public IEnumerable<ToolCategoryViewModel<TTool>> Categories
        {
            get { return _categories; }
        }
    }
}