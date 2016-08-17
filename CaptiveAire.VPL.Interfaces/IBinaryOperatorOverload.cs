namespace CaptiveAire.VPL.Interfaces
{
    /// <summary>
    /// A binary operater definition. Similar to operator overloading in .NET
    /// </summary>
    public interface IBinaryOperatorOverload
    {
        /// <summary>
        /// The type of the left hand operator
        /// </summary>
        IVplType TypeA { get; }

        /// <summary>
        /// The type of the right hand operator
        /// </summary>
        IVplType TypeB { get; }

        /// <summary>
        /// The return type
        /// </summary>
        IVplType ReturnType { get; }

        /// <summary>
        /// Execute the operator.
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="b">Right hand argument.</param>
        /// <param name="a">Left hand argument</param>
        /// <returns></returns>
        object Evaluate(IExecutionContext executionContext, object a, object b);
    }
}