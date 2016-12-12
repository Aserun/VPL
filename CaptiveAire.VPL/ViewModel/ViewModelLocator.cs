using System;
using CaptiveAire.VPL.Extensions;
using CaptiveAire.VPL.Model;
using Cas.Common.WPF;

namespace CaptiveAire.VPL.ViewModel
{
    internal static class ViewModelLocator
    {
        public static FunctionEditorDialogViewModel FunctionEditorDialog
        {
            get
            {
                var context = new VplServiceContext();

                var function = new Function(context, Guid.NewGuid())
                {
                    Name = "Hello"
                };

                var variable1 = new Variable(function, function.GetFloatType(), Guid.NewGuid())
                {
                    Name = "Test1"
                };

                var variable2 = new Variable(function, function.GetFloatType(), Guid.NewGuid())
                {
                    Name = "Test2"
                };

                function.AddVariable(variable1);
                function.AddVariable(variable2);

                return new FunctionEditorDialogViewModel(context, function, m => { }, new TextEditService(), "Design time!");
            }
        }
    }
}