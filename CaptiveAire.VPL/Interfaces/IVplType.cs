using System;
using System.Windows.Media;

namespace CaptiveAire.VPL.Interfaces
{
    public interface IVplType 
    {
        Guid Id { get; }

        string Name { get; }

        Visual CreateVisual();

        object DefaultValue { get; }

        /// <summary>
        /// The .NET type
        /// </summary>
        Type NetType { get; }
    }
}