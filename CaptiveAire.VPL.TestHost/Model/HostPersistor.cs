using System.IO;
using CaptiveAire.VPL.TestHost.Metadata;
using Newtonsoft.Json;

namespace CaptiveAire.VPL.TestHost.Model
{
    public class HostPersistor
    {
        public static void SaveToFile(string path, RootMetadata metadata)
        {
            var json = JsonConvert.SerializeObject(metadata, Formatting.Indented);

            File.WriteAllText(path, json);
        }

        public static RootMetadata LoadFromFile(string path)
        {
            var json = File.ReadAllText(path);

            var metadata = JsonConvert.DeserializeObject<RootMetadata>(json);

            return metadata;

        }
    }
}