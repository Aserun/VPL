namespace CaptiveAire.VPL.Interfaces
{
    /// <summary>
    /// A type converter for VplTypes
    /// </summary>
    public interface IVplTypeConverter
    {
        /// <summary>
        /// 
        /// </summary>
        IVplType InType { get; } 

        /// <summary>
        /// Gets the output type for this conversion
        /// </summary>
        IVplType OutType { get; }

        /// <summary>
        /// Converts the specified value
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        object Convert(IExecutionContext executionContext, object value);
    }
}