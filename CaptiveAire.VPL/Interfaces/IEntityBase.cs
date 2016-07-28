namespace CaptiveAire.VPL.Interfaces
{
    public interface IEntityBase
    {
        /// <summary>
        /// Gets the next element in the element chain.
        /// </summary>
        /// <returns></returns>
        IElement GetNext();

        /// <summary>
        /// Sets the next element.
        /// </summary>
        /// <param name="element"></param>
        void SetNext(IElement element);

        /// <summary>
        /// Gets the previous element
        /// </summary>
        /// <returns></returns>
        IElement GetPrevious();

        /// <summary>
        /// Sets the previous element.
        /// </summary>
        /// <param name="element"></param>
        void SetPrevious(IElement element);

        /// <summary>
        /// Gets the owner of this element.
        /// </summary>
        IElementOwner Owner { get; }

    }
}