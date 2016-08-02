namespace CaptiveAire.VPL.Interfaces
{
    public interface IDeleteable
    {
        /// <summary>
        /// Returns true if the item was deleted, false otherwise.
        /// </summary>
        /// <returns></returns>
        bool Delete();

        /// <summary>
        /// True if the item can be deleted, false otherwise.
        /// </summary>
        /// <returns></returns>
        bool CanDelete();
    }
}