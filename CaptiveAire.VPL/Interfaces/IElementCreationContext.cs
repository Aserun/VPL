namespace CaptiveAire.VPL.Interfaces
{
    /// <summary>
    /// The context for creating an element.
    /// </summary>
    public interface IElementCreationContext
    {
        /// <summary>
        /// Gets the element owner.
        /// </summary>
        IElementOwner Owner { get; }

        /// <summary>
        /// Gets the custom serialization data that was saved for this element.
        /// </summary>
        string Data { get; }
    }
}