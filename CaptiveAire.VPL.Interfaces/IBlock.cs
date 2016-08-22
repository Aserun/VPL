namespace CaptiveAire.VPL.Interfaces
{
    /// <summary>
    /// A block of statements in a compond component.
    /// </summary>
    public interface IBlock : IStatement
    {
        /// <summary>
        /// The id of this block. This is used to persist / match up parameter settings.
        /// </summary>
        string Id { get; }
        
        IElements Elements { get; }        
    }
}