using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using CaptiveAire.VPL.Editors;
using CaptiveAire.VPL.Factory;
using CaptiveAire.VPL.Interfaces;

namespace CaptiveAire.VPL.Model
{
    internal class VplServiceContext : IVplServiceContext
    {
        private readonly IVplService _vplService;
        private readonly IEnumerable<ResourceDictionary> _customResources;
        private readonly IElementFactoryManager _elementFactoryManager;
        private readonly IVplType[] _types;
        private readonly IEnumerable<object> _services;
        private readonly IElementBuilder _elementBuilder;
        private readonly IVplPlugin[] _plugins;

        public VplServiceContext(IVplService vplService, IEnumerable<IVplPlugin> plugins = null)
        {
            if (vplService == null) throw new ArgumentNullException(nameof(vplService));
            _vplService = vplService;
            _plugins = plugins?.ToArray() ?? new IVplPlugin[] {};

            _customResources = _plugins.SelectMany(p => p.Resources);

            var customFactories = _plugins.SelectMany(p => p.ElementFactories);

            _elementFactoryManager = new ElementFactoryManager(customFactories);

            //These are the "built-in" types
            var types = new List<IVplType>
            {
                new VplType(VplTypeId.Boolean, "Boolean", () => new BooleanValueCheckBoxView(), false, typeof(bool)),
                new VplType(VplTypeId.Float, "Float", () => new DoubleValueView(), 0.0, typeof(double)),
                new VplType(VplTypeId.Any, "Any", () => new TextValueView(), null, typeof(object)),
                new VplType(VplTypeId.String, "String", () => new TextValueView(), "", typeof(string)),
                new VplType(VplTypeId.Int, "Int", () => new Int32ValueView(), 0, typeof(int)),
                new VplType(VplTypeId.Byte, "Byte", () => new ByteValueView(), (byte)0, typeof(byte)),
                new VplType(VplTypeId.UInt16, "UInt16", () => new TextValueView(), (ushort)0, typeof(ushort)),
                new VplType(VplTypeId.UInt32, "UInt32", () => new TextValueView(), (uint)0, typeof(uint)),
                new VplType(VplTypeId.Single, "Single", () => new TextValueView(), (float)0, typeof(float)),
                new VplType(VplTypeId.SByte, "Int8", () => new TextValueView(), (sbyte)0, typeof(sbyte)),
                new VplType(VplTypeId.Int16, "Int16", () => new Int16ValueView(), (short)0, typeof(short)),
                new VplType(VplTypeId.DateTime, "DateTime", () => new DateTimeValueView(), DateTime.Now, typeof(DateTime)),
                new VplType(VplTypeId.UInt64, "UInt64", () => new TextValueView(), (ulong)0, typeof(ulong)),
                new VplType(VplTypeId.Int64, "Int64", () => new TextValueView(), (long)0, typeof(long)),
                new VplType(VplTypeId.Decimal, "Decimal", () => new TextValueView(), (decimal)0, typeof(decimal))
            };

            //Add the plugin types
            foreach (var plugin in _plugins)
            {
                types.AddRange(plugin.Types);
            }

            //Create the array of types
            _types = types
                .OrderBy(t => t.Name)
                .ToArray();

            //Create the services
            _services = _plugins
                .SelectMany(p => p.Services)
                .ToArray();

            //Create an element builder
            _elementBuilder = new ElementBuilder(_elementFactoryManager, this);
        }

        public IElementFactoryManager ElementFactoryManager
        {
            get { return _elementFactoryManager; }
        }

        public IEnumerable<ResourceDictionary> CustomResources
        {
            get { return _customResources; }
        }

        public IEnumerable<object> Services
        {
            get { return _services; }
        }

        public IEnumerable<IVplType> Types
        {
            get { return _types; }
        }

        public IElementBuilder ElementBuilder
        {
            get { return _elementBuilder; }
        }

        public IVplService VplService
        {
            get { return _vplService; }
        }
    }
}