using System;
using CaptiveAire.VPL.Interfaces;
using GalaSoft.MvvmLight;

namespace CaptiveAire.VPL.ViewModel
{
    internal class ToolViewModel<TTool> : ViewModelBase
        where TTool : ITool
    {
        private readonly TTool _tool;

        public ToolViewModel(TTool tool)
        {
            if (tool == null) throw new ArgumentNullException(nameof(tool));

            _tool = tool;
        }

        public string Name
        {
            get { return _tool.Name; }
        }

        public TTool Tool
        {
            get { return _tool; }
        }
    }
}