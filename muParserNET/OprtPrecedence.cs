/*
muParserNET - muParser library wrapper for .NET Framework

Copyright (c) 2015 Luiz Carlos Viana Melo

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.

This software uses and contains parts copied from muParser library.
muParser library - Copyright (C) 2013 Ingo Berg
*/

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
