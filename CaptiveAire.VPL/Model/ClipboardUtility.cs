using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using CaptiveAire.VPL.Interfaces;
using CaptiveAire.VPL.Metadata;
using Newtonsoft.Json;

namespace CaptiveAire.VPL.Model
{
    internal static class ClipboardUtility
    {
        /// <summary>
        /// The format to use when using the clipboard.
        /// </summary>
        internal const string CopyPasteFormat = "CaptiveAire.VPL.CopyPasteFormat";

        internal static void Copy(IEnumerable<IElement> elements)
        {
            var elementMetadatas = elements
              .ToMetadata()
              .ToArray();

            if (elementMetadatas.Any())
            {
                var json = JsonConvert.SerializeObject(elementMetadatas);

                Clipboard.SetData(CopyPasteFormat, json);
            }
        }

        internal static bool CanPaste()
        {
            return Clipboard.ContainsData(CopyPasteFormat);
        }

        internal static IElementClipboardData Paste()
        {
            try
            {
                var json = Clipboard.GetData(CopyPasteFormat) as string;

                if (!string.IsNullOrWhiteSpace(json))
                {
                    var elementMetadatas = JsonConvert.DeserializeObject<ElementMetadata[]>(json);

                    if (elementMetadatas.Any())
                    {
                        return new ElementClipboardData(elementMetadatas);
                    }
                }
            }
            catch (Exception)
            {
            }

            return null;
        }
    }
}