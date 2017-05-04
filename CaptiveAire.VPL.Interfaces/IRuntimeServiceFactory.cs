namespace CaptiveAire.VPL.Interfaces
{
    public interface IRuntimeServiceFactory
    {
        /// <summary>
        /// Creaets one or more runtime services. Note that any services that implement IDisposbie will be disposed along with the 
        /// IExecutionContext instance.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        object[] CreateServices(IVplServiceContext context);
    }
}