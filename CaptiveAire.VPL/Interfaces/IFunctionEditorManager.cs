using System;

namespace CaptiveAire.VPL.Interfaces
{
    internal interface IFunctionEditorManager
    {
        void Register(IFunctionEditorDialogViewModel editor);

        void Unregister(IFunctionEditorDialogViewModel editor);

        IFunctionEditorDialogViewModel Get(Guid id);
    }
}