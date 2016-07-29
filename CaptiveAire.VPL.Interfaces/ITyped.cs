namespace CaptiveAire.VPL.Interfaces
{
    /// <summary>
    /// Represents something that has a VplType.
    /// </summary>
    public interface ITyped
    {
        /// <summary>
        /// Gets the VplType for this item.
        /// </summary>
        IVplType Type { get; } 
    }
}