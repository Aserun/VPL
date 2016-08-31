using System;

namespace CaptiveAire.VPL.Plugins
{
    /// <summary>
    /// Ids for system element types.
    /// </summary>
    internal static class PluginElementIds
    {
        // ---- Control ----
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

        // ---- Dates ----
        public static readonly Guid Now = new Guid("97A059F6-CA40-498F-A0ED-B34B2C6818FE");
        public static readonly Guid UtcNow = new Guid("00A53ABF-469B-47BF-B5D0-7939B585A0A2");
        public static readonly Guid AddToDate = new Guid("B1EB4F3A-307D-48E0-92D2-274E517245C6");

        // ---- Conversion ----
        public static readonly Guid Cast = new Guid("BB8D52BD-36D4-4CA3-9432-FC389CEE2ACC");

        // ---- Trig ----
        public static readonly Guid Acos  = new Guid("C67BFDC8-E91F-476A-8CFE-C164D0C17E7E");
        public static readonly Guid Asin  = new Guid("93321D05-CE0F-4146-878A-0B60EEDA2006");
        public static readonly Guid Atan  = new Guid("046AB48A-5546-4938-8B5B-5308F76A9421");
        public static readonly Guid Cos = new Guid("360CE80D-1E18-4A8A-9E04-F820FD92E22F");
        public static readonly Guid Cosh= new Guid("888E40DB-D937-4C92-83A5-B40FE9E61F5F");
        public static readonly Guid Sin = new Guid("C5397091-B3F9-4D75-A477-DB8A2FF94FCF");
        public static readonly Guid Sinh = new Guid("67B32AC4-6A27-48BE-AD57-05F4D8001AD0");
        public static readonly Guid Tan = new Guid("273AB8D0-544D-4CEE-9FAC-70B05AB8251A");
        public static readonly Guid Tanh = new Guid("4B9ADED8-E6A1-4974-AAFB-0CF7A9D6CFBF");
     
    }
}