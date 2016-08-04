using System;

namespace CaptiveAire.VPL.Plugins
{
    /// <summary>
    /// Ids for system element types.
    /// </summary>
    internal static class PluginElementIds
    {
        // ---- Statements ----
        public static readonly Guid IfElse = new Guid("37042502-3F84-4BB4-8A06-D8F0D2DC34BE");

        public static readonly Guid Wait = new Guid("863BC6A3-5187-4F1E-A805-634C5BE0EB27");

        public static readonly Guid While = new Guid("467CF5B1-3A83-42CF-8400-58896FD73DC2");

        public static readonly Guid Repeat = new Guid("B4FE4DDE-11DF-4A97-BE95-22D7ED6B814E");

        public static readonly Guid Timeout = new Guid("83851EBA-6B2F-4FCC-BE66-F9F253AFD16A");

        // ---- Annotations ----
        public static readonly Guid Comment = new Guid("F2A4C96B-F048-4E32-A3C3-7595FC0433A7");

        public static readonly Guid Annotation = new Guid("18B83A5A-C180-4737-B861-4844660A4E89");

        // ---- Operators ----
        public static readonly Guid BinaryLogicOperator = new Guid("0152F207-A0E3-4723-B40B-86C723D998A9");

        public static readonly Guid NotOperator = new Guid("8F9F1012-B123-4BE9-A48C-D8DF7E97BCE8");

        public static readonly Guid BinaryMathOperator = new Guid("00D40D07-8CA4-4FC4-963B-913FA7E705C5");

        public static readonly Guid ComparisonOperator = new Guid("F0575E26-01A0-4122-ADC0-A6FEB5BEFA60");

        // ---- Functions ----
        public static readonly Guid CallFunction = new Guid("12CE1B54-BBBF-4530-B0C4-7BBEF1108316");

        public static readonly Guid EvaluateFunction = new Guid("48BDDADD-017B-4D55-92EC-DC10B989BC7B");
    }
}