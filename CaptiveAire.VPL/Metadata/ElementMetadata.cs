using System;
using System.Windows;

namespace CaptiveAire.VPL.Metadata
{
    public class ElementMetadata : ElementMetadataBase
    {
        public Guid ElementTypeId { get; set; }

        public BlockMetadata[] Blocks { get; set; }

        public Point Location { get; set; }
    }
}