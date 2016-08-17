using System;
using System.Collections.Generic;
using CaptiveAire.VPL.Interfaces;
using CaptiveAire.VPL.View;
using CaptiveAire.VPL.ViewModel;
using Cas.Common.WPF;

namespace CaptiveAire.VPL.Extensions
{
    public static class VplTypeExtensions
    {
        public static Guid? SelectVplType(this IEnumerable<IVplType> types, Guid? selectedVplType = null)
        {
            if (types == null) throw new ArgumentNullException(nameof(types));

            //Create the view model
            var viewModel = new SelectTypeDialogViewModel(types)
            {
                SelectedTypeId = selectedVplType
            };

            //Create the view
            var view = new SelectTypeDialogView()
            {
                Owner = WindowUtil.GetActiveWindow(),
                DataContext = viewModel
            };

            //Show the dialog
            if (view.ShowDialog() == true)
            {
                return viewModel.SelectedTypeId;
            }

            return null;
        }
    }
}