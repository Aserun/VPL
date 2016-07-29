using System.Windows.Input;

namespace CaptiveAire.VPL.Interfaces
{
    /// <summary>
    /// Represents and action that can be applied to a given element.
    /// </summary>
    public interface IElementAction : ICommand
    {
        /// <summary>
        /// The name of the action (for display to the user)
        /// </summary>
        string Name { get;  }
    }
}