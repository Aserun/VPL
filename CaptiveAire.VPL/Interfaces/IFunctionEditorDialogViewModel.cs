using System;
using System.ComponentModel;
using System.Windows.Input;
using CaptiveAire.VPL.Metadata;
using CaptiveAire.VPL.ViewModel;
using Cas.Common.WPF.Behaviors;

namespace CaptiveAire.VPL.Interfaces
{
    internal interface IFunctionEditorDialogViewModel : ICloseableViewModel, IActivateable, INotifyPropertyChanged
    {
        ICommand AddVariableCommand { get; }
        ICommand CheckForErrorsCommand { get; }
        ICommand ClearReturnTypeCommand { get; }
        ICommand CloseCommand { get; }
        ICommand OkCommand { get; }
        ICommand PasteCommand { get; }
        ICommand ResetZoomCommand { get; }
        ICommand RunCommand { get; }
        ICommand SaveCommand { get; }
        ICommand SelectReturnTypeCommand { get; }
        ICommand StopCommand { get; }

        ErrorViewModel[] Errors { get; }

        Function Function { get; }

        bool IsErrorsExpanded { get; set; }
        
        double Scale { get; set; }

        double ScaleMax { get; }

        double ScaleMin { get; }

        IVplType SelectedType { get; set; }

        string Title { get; }

        ToolsViewModel<IElementFactory> Tools { get; }

        FunctionMetadata GetMetadata();
    }
}