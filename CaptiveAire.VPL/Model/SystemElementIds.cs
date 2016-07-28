using System;

namespace CaptiveAire.VPL.Model
{
    /// <summary>
    /// Ids for system element types.
    /// </summary>
    public static class SystemElementIds
    {
        // ---- Statements ----
        public static readonly Guid IfElse = new Guid("37042502-3F84-4BB4-8A06-D8F0D2DC34BE");

        public static readonly Guid Block = new Guid("2D57511A-9139-4D54-8EB2-7734B142F5C2");

        public static readonly Guid Wait = new Guid("863BC6A3-5187-4F1E-A805-634C5BE0EB27");

        public static readonly Guid While = new Guid("467CF5B1-3A83-42CF-8400-58896FD73DC2");

        public static readonly Guid Repeat = new Guid("B4FE4DDE-11DF-4A97-BE95-22D7ED6B814E");

        public static readonly Guid Timeout = new Guid("83851EBA-6B2F-4FCC-BE66-F9F253AFD16A");

        public static readonly Guid Comment = new Guid("F2A4C96B-F048-4E32-A3C3-7595FC0433A7");

        // ---- Variables ----
        public static readonly Guid VariableSetter = new Guid("EDC13576-F2AC-4129-9E6B-D7D0EAC3C655");

        public static readonly Guid VariableGetter = new Guid("A97E6D15-CE98-4B6F-B0E7-EF54097DF821");

        // ---- Operators ----
        public static readonly Guid BinaryLogicOperator = new Guid("0152F207-A0E3-4723-B40B-86C723D998A9");

        public static readonly Guid NotOperator = new Guid("8F9F1012-B123-4BE9-A48C-D8DF7E97BCE8");

        public static readonly Guid BinaryMathOperator = new Guid("00D40D07-8CA4-4FC4-963B-913FA7E705C5");

        public static readonly Guid ComparisonOperator = new Guid("F0575E26-01A0-4122-ADC0-A6FEB5BEFA60");

        // ---- Parameters ----
        public static readonly Guid Parameter = new Guid("86E674D0-B261-456E-869F-546E4F98C069");

        // ---- Functions ----
        public static readonly Guid CallFunction = new Guid("12CE1B54-BBBF-4530-B0C4-7BBEF1108316");

        public static readonly Guid EvaluateFunction = new Guid("48BDDADD-017B-4D55-92EC-DC10B989BC7B");
    }
}