namespace CaptiveAire.VPL.Interfaces
{
    public interface ICallStackFrame
    {
        int Index { get; }

        string Name { get; }

        IStatement CurrentStatement { get; set; }

        /// <summary>
        /// Gets the function name formatted with its arguments
        /// </summary>
        string Prototype { get; }
    }
}