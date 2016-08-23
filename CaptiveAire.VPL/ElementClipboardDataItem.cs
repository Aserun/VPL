using System;
using CaptiveAire.VPL.Interfaces;
using CaptiveAire.VPL.Metadata;

namespace CaptiveAire.VPL
{
    [Serializable]
    internal class ElementClipboardDataItem : IClipboardDataItem
    {
        private readonly IElementFactory _elementFactory;
        private readonly ElementMetadata _elementMetadata;

        internal ElementClipboardDataItem(IElementFactory elementFactory, ElementMetadata elementMetadata)
        {
            if (elementMetadata == null && elementFactory == null)
                throw new ArgumentException("Both arguments cannot not be null.");

            _elementFactory = elementFactory;
            _elementMetadata = elementMetadata;
        }

        public ElementMetadata ElementMetadata
        {
            get { return _elementMetadata; }
        }

        public IElementFactory Factory
        {
            get { return _elementFactory; }
        }
    }
}