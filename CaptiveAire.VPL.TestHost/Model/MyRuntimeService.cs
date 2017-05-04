using System;
using System.Diagnostics;

namespace CaptiveAire.VPL.TestHost.Model
{
    public class MyRuntimeService : IDisposable
    {
        public MyRuntimeService()
        {
            Debug.WriteLine("Created my runtime service!");
        }

        public void Dispose()
        {
            Debug.WriteLine("Disposing my runtime service!");
        }
    }
}