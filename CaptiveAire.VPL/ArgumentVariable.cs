using System;
using CaptiveAire.VPL.Interfaces;

namespace CaptiveAire.VPL
{
    public class ArgumentVariable : Variable
    {
        private readonly IArgument _argument;

        public ArgumentVariable(IElementOwner owner, IVplType type, IArgument argument) 
            : base(owner, type, argument.Id)
        {
            if (argument == null) throw new ArgumentNullException(nameof(argument));

            _argument = argument;

            argument.PropertyChanged += Argument_PropertyChanged;
        }

        private void Argument_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IArgument.Name))
            {
                RaisePropertyChanged(nameof(Name));
            }
        }

        protected IArgument Argument
        {
            get { return _argument; }
        }

        protected override bool CanRename()
        {
            return false;
        }

        public override string Name
        {
            get { return _argument.Name; }
            set {  }
        }

        /// <summary>
        /// Argument variables are created by their argument definitions.
        /// </summary>
        public override bool Persist
        {
            get { return false; }
        }
    }
}