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
                new ElementClipboardDataItem(new ElementMetadata()
                {
                    ElementTypeId  = elementFactory.ElementTypeId
                }) 
            };
        }

        public ElementClipboardData(IEnumerable<IElement> elements)
        {
            _items = elements
                .Select(e => (IClipboardDataItem)new ElementClipboardDataItem(e.ToMetadata()))
                .ToArray();
        }

        public ElementClipboardData(IEnumerable<ElementMetadata> elementMetadatas)
        {
            _items = elementMetadatas
                .Select(m => new ElementClipboardDataItem(m))
                .ToArray();
        }

        public IClipboardDataItem[] Items
        {
            get { return _items; }
        }
    }

    [Serializable]
    internal class ElementClipboardDataItem : IClipboardDataItem
    {
        private readonly ElementMetadata _elementMetadata;

        internal ElementClipboardDataItem(ElementMetadata elementMetadata)
        {
            if (elementMetadata == null) throw new ArgumentNullException(nameof(elementMetadata));

            _elementMetadata = elementMetadata;
        }

        public ElementMetadata ElementMetadata
        {
            get { return _elementMetadata; }
        }
    }
}