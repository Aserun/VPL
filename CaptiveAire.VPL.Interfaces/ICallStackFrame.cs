namespace CaptiveAire.VPL.Interfaces
{
    public interface ICallStackFrame
    {
        int Index { get; }

        string Name { get; }

        IStatement CurrentStatement { get; set; }
    }
}