using System;
using System.Collections.Generic;
using System.Linq;
using CaptiveAire.VPL.Interfaces;
using CaptiveAire.VPL.Metadata;

namespace CaptiveAire.VPL.Model
{
    [Serializable]
    internal class ElementClipboardData : IElementClipboardData
    {
        private readonly IClipboardDataItem[] _items;

        public ElementClipboardData(IElementFactory elementFactory)
        {
            _items = new IClipboardDataItem[]
            {
                new ElementClipboardDataItem(elementFactory, null)
            };
        }

        public ElementClipboardData(IEnumerable<IElement> elements)
        {
            _items = elements
                .Select(e => (IClipboardDataItem)new ElementClipboardDataItem(e.Factory, e.ToMetadata()))
                .ToArray();
        }

        public ElementClipboardData(IEnumerable<ElementMetadata> elementMetadatas)
        {
            _items = elementMetadatas
                .Select(m => new ElementClipboardDataItem(null, m))
                .ToArray();
        }

        public IClipboardDataItem[] Items
        {
            get { return _items; }
        }
    }
}