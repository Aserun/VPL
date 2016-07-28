namespace CaptiveAire.VPL.Interfaces
{
    /// <summary>
    /// Allows subcomponents of the system editor trigger undo state saves.
    /// </summary>
    public interface IUndoProvider
    {
        void SaveUndoState();
    }
}