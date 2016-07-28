using System;
using CaptiveAire.VPL.Interfaces;

namespace CaptiveAire.VPL.Model
{
    internal class ElementCreationContext : IElementCreationContext
    {
        private readonly IElementOwner _owner;
        private readonly string _data;

        public ElementCreationContext(IElementOwner owner, string data)
        {
            if (owner == null) throw new ArgumentNullException(nameof(owner));

            _owner = owner;
            _data = data;
        }

        public IElementOwner Owner
        {
            get { return _owner; }
        }

        public string Data
        {
            get { return _data; }
        }
    }
}