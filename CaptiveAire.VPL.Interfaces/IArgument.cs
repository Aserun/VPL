using System;
using System.ComponentModel;

namespace CaptiveAire.VPL.Interfaces
{
    /// <summary>
    /// An argument to a function.
    /// </summary>
    public interface IArgument : INotifyPropertyChanged
    {
        /// <summary>
        /// The unique id of this argument. At runtime, a variable will be created with this id.
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// Gets the name of the argument.
        /// </summary>
        string Name { get; } 

        /// <summary>
        /// Gets the type id of the argument.
        /// </summary>
        Guid TypeId { get; }
    }
}