namespace CaptiveAire.VPL.Interfaces
{
    public interface IError
    {
        /// <summary>
        /// Gets the source element.
        /// </summary>
        IErrorSource Source { get; }

        /// <summary>
        /// Gets the message of this item.
        /// </summary>
        string Message { get; }

        /// <summary>
        /// Gets the error level associated with this item.
        /// </summary>
        ErrorLevel Level { get; }
    }
}