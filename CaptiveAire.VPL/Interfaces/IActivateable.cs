using System;

namespace CaptiveAire.VPL.Interfaces
{
    internal interface IActivateable
    {
        event EventHandler Activated;

        void Activate();
    }
}