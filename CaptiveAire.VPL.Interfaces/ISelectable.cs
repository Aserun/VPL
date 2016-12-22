namespace CaptiveAire.VPL.Interfaces
{
    public interface ISelectable
    {
        bool IsSelected { get; set; }

        /// <summary>
        /// Gets the container of selectables (contains this selectable).
        /// </summary>
        /// <returns></returns>
        ISelectable[] GetSiblings();
    }
}