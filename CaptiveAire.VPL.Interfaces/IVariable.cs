using System;
using System.Dynamic;

namespace CaptiveAire.VPL.Interfaces
{
    public interface IVariable : IDeleteable
    {
        /// <summary>
        /// To be raised when the name of the variable changes.
        /// </summary>
        event EventHandler NameChanged;

        /// <summary>
        /// The id of the variable
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// Gets or sets the name of the variable
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets the variable's type.
        /// </summary>
        IVplType Type { get; }

        /// <summary>
        /// Gets or sets the value of the variable
        /// </summary>
        object Value { get; set; }
        /// <summary>
        /// 
        /// Gets a flag indicating whether this variable should be persisted on its own.
        /// (e.g. Arguments and Return variables are created automatically)
        /// </summary>
        bool Persist { get; }
    }
}