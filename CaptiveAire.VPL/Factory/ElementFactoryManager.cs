using System;
using System.Collections.Generic;
using CaptiveAire.VPL.Interfaces;
using CaptiveAire.VPL.Model;
using CaptiveAire.VPL.ViewModel;

namespace CaptiveAire.VPL.Factory
{
    public class ElementFactoryManager : IElementFactoryManager
    {
        private readonly IDictionary<Guid, IElementFactory> _uniqueFactories = new Dictionary<Guid, IElementFactory>();
        private readonly List<IElementFactory> _factories;

        public ElementFactoryManager(IEnumerable<IElementFactory> extensionFactories = null)
        {
            _factories = new List<IElementFactory>(EnumerateFactories(extensionFactories));

            foreach (var factory in _factories)
            {
                if (!_uniqueFactories.ContainsKey(factory.ElementTypeId))
                {
                    _uniqueFactories.Add(factory.ElementTypeId, factory);
                }
            }
        }

        public IEnumerable<IElementFactory> Factories
        {
            get { return _factories; }
        }

        private IEnumerable<IElementFactory> EnumerateFactories(IEnumerable<IElementFactory> extensionFactories)
        {
            yield return new ElementFactory(SystemElementIds.VariableGetter, FactoryCategoryNames.Variable, "Get", context => new VariableGetter(context), typeof(VariableGetter), showInToolbox:false);

            yield return new ElementFactory(SystemElementIds.VariableSetter, FactoryCategoryNames.Variable, "Set", context => new VariableSetter(context), typeof(VariableSetter), showInToolbox: false);

            //See if we have any extension factories
            if (extensionFactories != null)
            {
                //Enumerate the custom extensions last.
                foreach (var extensionFactory in extensionFactories)
                {
                    yield return extensionFactory;
                }
            }
        }

        public IElementFactory GetFactory(Guid elementTypeId)
        {
            return _uniqueFactories[elementTypeId];
        }
    }
}