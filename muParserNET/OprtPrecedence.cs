using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace muParserNET
{
    /// <summary>
    /// Parser operator precedence values.
    /// </summary>
    public struct OprtPrecedence
    {
        // binary operators

        /// <summary>
        /// Logical OR operator precedence value.
        /// </summary>
        public const uint LOR = 1;

        /// <summary>
        /// Logical AND operator precedence value.
        /// </summary>
        public const uint LAND = 2;

        /// <summary>
        /// Logic operators precedence value.
        /// </summary>
        public const uint LOGIC = 3;

        /// <summary>
        /// Comparsion operators precedence value.
        /// </summary>
        public const uint CMP = 4;

        /// <summary>
        /// Addition and subtraction operators precedence value.
        /// </summary>
        public const uint ADD_SUB = 5;

        /// <summary>
        /// Multiplication and division operators precedence value.
        /// </summary>
        public const uint MUL_DIV = 6;

        /// <summary>
        /// Power operator precedence value (highest).
        /// </summary>
        public const uint POW = 7;

        // infix operators

        /// <summary>
        /// Infix operators precedence value. Signs have a higher priority than
        /// ADD_SUB, but lower than power operator
        /// </summary>
        public const uint INFIX = 6;

        /// <summary>
        /// Postfix operators precedence value.
        /// </summary>
        public const uint POSTFIX = 6;
    }
}
