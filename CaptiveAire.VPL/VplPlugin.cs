using System.Collections.Generic;
using System.Windows;
using CaptiveAire.VPL.Interfaces;

namespace CaptiveAire.VPL
{
    public class VplPlugin : IVplPlugin
    {
        private readonly string _name;
        private readonly IEnumerable<ResourceDictionary> _resources;
        private readonly IEnumerable<object> _services;
        private readonly IEnumerable<IVplType> _types;
        private readonly IEnumerable<IElementFactory> _elementFactories;

        public VplPlugin(
            string name, 
            IEnumerable<IElementFactory> elementFactories = null, 
            IEnumerable<ResourceDictionary> resources = null,
            IEnumerable<IVplType> types = null,
            IEnumerable<object> services = null)
        {
            _name = name;
            _elementFactories = elementFactories ?? new IElementFactory[] {};
            _resources = resources ?? new ResourceDictionary[] {};
            _services = services ?? new object[] {};
            _types = types ?? new IVplType[] { };
        }

        public string Name
        {
            get { return _name; }
        }

        public IEnumerable<ResourceDictionary> Resources
        {
            get { return _resources; }
        }

        public IEnumerable<IElementFactory> ElementFactories
        {
            get { return _elementFactories; }
        }

        public IEnumerable<IVplType> Types
        {
            get { return _types; }
        }

        public IEnumerable<object> Services
        {
            get { return _services; }
        }
    }
}