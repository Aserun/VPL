namespace CaptiveAire.VPL.Interfaces
{
    /// <summary>
    /// An item that can have errors.
    /// </summary>
    public interface IErrorSource
    {
        /// <summary>
        /// Clears the visual display of an error
        /// </summary>
        void ClearErrors();

        /// <summary>
        /// Sets the error for this item.
        /// </summary>
        void SetError(string message);

        /// <summary>
        /// Checks the item for an errors or warnings.
        /// </summary>
        /// <returns></returns>
        IError[] CheckForErrors();

        /// <summary>
        /// True if there is an error, false otherwise.
        /// </summary>
        bool HasError { get; }
    }
}