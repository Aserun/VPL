using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using CaptiveAire.VPL.Factory;
using CaptiveAire.VPL.Interfaces;
using CaptiveAire.VPL.View;
using AnyValueView = CaptiveAire.VPL.Editors.AnyValueView;
using BooleanValueView = CaptiveAire.VPL.Editors.BooleanValueView;
using ByteValueView = CaptiveAire.VPL.Editors.ByteValueView;
using DateTimeValueView = CaptiveAire.VPL.Editors.DateTimeValueView;
using DoubleValueView = CaptiveAire.VPL.Editors.DoubleValueView;
using Int32ValueView = CaptiveAire.VPL.Editors.Int32ValueView;
using TextValueView = CaptiveAire.VPL.Editors.TextValueView;

namespace CaptiveAire.VPL.Model
{
    internal class VplServiceContext : IVplServiceContext
    {
        private readonly IEnumerable<ResourceDictionary> _customResources;
        private readonly IElementFactoryManager _elementFactoryManager;
        private readonly IVplType[] _types;
        private readonly IEnumerable<object> _services;
        private readonly IElementBuilder _elementBuilder;

        public VplServiceContext(IEnumerable<IVplPlugin> plugins = null)
        {
            plugins = plugins ?? new IVplPlugin[] {};

            _customResources = plugins.SelectMany(p => p.Resources);

            var customFactories = plugins.SelectMany(p => p.ElementFactories);

            _elementFactoryManager = new ElementFactoryManager(customFactories);

            //These are the "built-in" types
            var types = new List<IVplType>
            {
                new VplType(VplTypeId.Boolean, "Boolean", () => new BooleanValueView(), false, typeof(bool)),
                new VplType(VplTypeId.Float, "Float", () => new DoubleValueView(), 0.0, typeof(double)),
                new VplType(VplTypeId.Any, "Any", () => new AnyValueView(), null, typeof(object)),
                new VplType(VplTypeId.String, "String", () => new TextValueView(), "", typeof(string)),
                new VplType(VplTypeId.Int, "Int", () => new Int32ValueView(), 0, typeof(int)),
                new VplType(VplTypeId.Byte, "Byte", () => new ByteValueView(), (byte)0, typeof(byte)),
                new VplType(VplTypeId.UInt16, "UInt16", () => new TextValueView(), (ushort)0, typeof(ushort)),
                new VplType(VplTypeId.UInt32, "UInt32", () => new TextValueView(), (uint)0, typeof(uint)),
                new VplType(VplTypeId.Single, "Single", () => new TextValueView(), (float)0, typeof(float)),
                new VplType(VplTypeId.SByte, "Int8", () => new TextValueView(), (sbyte)0, typeof(sbyte)),
                new VplType(VplTypeId.Int16, "Int16", () => new TextValueView(), (short)0, typeof(short)),
                new VplType(VplTypeId.DateTime, "DateTime", () => new DateTimeValueView(), DateTime.Now, typeof(DateTime))
            };

            //Add the plugin types
            foreach (var plugin in plugins)
            {
                types.AddRange(plugin.Types);
            }

            //Create the array of types
            _types = types
                .OrderBy(t => t.Name)
                .ToArray();

            //Create the services
            _services = plugins
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
    }
}