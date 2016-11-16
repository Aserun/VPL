using System;
using System.Collections.Generic;
using System.Linq;
using CaptiveAire.VPL.Interfaces;
using CaptiveAire.VPL.Metadata;
using CaptiveAire.VPL.Model;

namespace CaptiveAire.VPL
{
    [Serializable]
    public class ElementClipboardData : IElementClipboardData
    {
        private readonly Guid? _sourceFunctionId;
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
            _sourceFunctionId = elements
                .FirstOrDefault()?.Owner?.Id;
           
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

        public Guid? SourceFunctionId
        {
            get { return _sourceFunctionId; }
        }
    }
}