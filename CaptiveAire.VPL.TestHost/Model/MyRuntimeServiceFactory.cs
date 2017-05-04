using CaptiveAire.VPL.Interfaces;

namespace CaptiveAire.VPL.TestHost.Model
{
    public class MyRuntimeServiceFactory : IRuntimeServiceFactory
    {
        public object[] CreateServices(IVplServiceContext context)
        {
            return new object[]
            {
                new MyRuntimeService(), 
            };
        }
    }
}