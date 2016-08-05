using System;
using CaptiveAire.VPL.Interfaces;

namespace CaptiveAire.VPL.Model
{
    internal class ElementCreationContext : IElementCreationContext
    {
        private readonly IElementOwner _owner;
        private readonly string _data;
        private readonly IElementFactory _factory;

        public ElementCreationContext(IElementOwner owner, string data, IElementFactory factory)
        {
            if (owner == null) throw new ArgumentNullException(nameof(owner));
            if (factory == null) throw new ArgumentNullException(nameof(factory));

            _owner = owner;
            _data = data;
            _factory = factory;
        }

        public IElementOwner Owner
        {
            get { return _owner; }
        }

        public string Data
        {
            get { return _data; }
        }

        public IElementFactory Factory
        {
            get { return _factory; }
        }
    }
}