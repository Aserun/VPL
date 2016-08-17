namespace CaptiveAire.VPL.Interfaces
{
    /// <summary>
    /// Binary Operator Types
    /// </summary>
    public enum BinaryOperatorOverloadType
    {
        /// <summary>
        /// Equals (==)
        /// </summary>
        Equals = 0,

        /// <summary>
        /// Not Equal (!=)
        /// </summary>
        NotEqual = 1,

        /// <summary>
        /// Less than (&lt;)
        /// </summary>
        LessThan = 2,

        /// <summary>
        /// Less than or equal (&lt;=)
        /// </summary>
        LessThanOrEqual = 3,

        /// <summary>
        /// Greater than (>)
        /// </summary>
        GreaterThan = 4,

        /// <summary>
        /// Greater than or equal (&gt;=)
        /// </summary>
        GreaterThanOrEqual = 5,

        /// <summary>
        /// Addition (+)
        /// </summary>
        Addition = 6,

        /// <summary>
        /// Subtraction (-)
        /// </summary>
        Subtraction = 7,

        /// <summary>
        /// Multiplication (*)
        /// </summary>
        Multiplication = 8,

        /// <summary>
        /// Division operator (/)
        /// </summary>
        Division = 9,

        /// <summary>
        /// Modulus operator %
        /// </summary>
        Modulus = 10,

        /// <summary>
        /// Logical or bitwise AND (&amp;)
        /// </summary>
        LogicalAnd = 11,

        /// <summary>
        /// Logical or bitwise XOR (^)
        /// </summary>
        LogicalXor = 12,

        /// <summary>
        /// Logical or bitwise OR (|)
        /// </summary>
        LogicalOr = 13,

        /// <summary>
        /// Logical AND (&amp;&amp;)
        /// </summary>
        ConditionalAnd = 14,

        /// <summary>
        /// Logical OR (||)
        /// </summary>
        ConditionalOr = 15,
    }
}