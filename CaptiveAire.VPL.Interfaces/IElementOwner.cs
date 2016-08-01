using System.Collections.Generic;

namespace CaptiveAire.VPL.Interfaces
{
    /// <summary>
    /// Represents an entity that owns elements.
    /// </summary>
    public interface IElementOwner
    {
        /// <summary>
        /// Add a root level element.
        /// </summary>
        /// <param name="element"></param>
        void Add(IElement element);

        /// <summary>
        /// Remove a root level element.
        /// </summary>
        /// <param name="element"></param>
        void Remove(IElement element);

        /// <summary>
        /// Gets the variables defined for this scope.
        /// </summary>
        IEnumerable<IVariable> Variables { get; }

        /// <summary>
        /// Adds a variable.
        /// </summary>
        /// <param name="variable"></param>
        void AddVariable(IVariable variable);

        /// <summary>
        /// Removes a variable.
        /// </summary>
        /// <param name="variable"></param>
        void RemoveVariable(IVariable variable);

        /// <summary>
        /// Gets the service context that was used to create this instance..
        /// </summary>
        IVplServiceContext Context { get; }

        /// <summary>
        /// Gets all of the elements in this scope.
        /// </summary>
        /// <returns></returns>
        IEnumerable<IElement> GetAllElements();

        /// <summary>
        /// Gets all of the root level elements.
        /// </summary>
        /// <returns></returns>
        IEnumerable<IElement> GetRootElements();

        /// <summary>
        /// Adds an argument.
        /// </summary>
        /// <param name="argument"></param>
        void AddArgument(IArgument argument);

        /// <summary>
        /// Get the arguments.
        /// </summary>
        /// <returns></returns>
        IEnumerable<IArgument> GetArguments();

        /// <summary>
        /// Marks the scope as dirty.
        /// </summary>
        void MarkDirty();

        /// <summary>
        /// Marks the scope as clean.
        /// </summary>
        void MarkClean();

        /// <summary>
        /// Returns true if changes have been made to the scope since calling MarkClean, false otherwise.
        /// </summary>
        bool IsDirty { get; }

    }
}