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

using System;

namespace muParserNET
{
    /// <summary>
    /// Error class of the parser.
    /// </summary>
    public class ParserError : Exception
    {
        /// <summary>
        /// Gets or sets the expression with error.
        /// </summary>
        public string Expr { get; set; }

        /// <summary>
        /// Gets or sets the invalid token.
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets the position of the error in expression.
        /// </summary>
        public int Pos { get; set; }

        /// <summary>
        /// Gets or sets the error code.
        /// </summary>
        public ErrorCodes Code { get; set; }

        /// <summary>
        /// Class constructor.
        /// </summary>
        public ParserError()
        {
        }

        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="message">The error message</param>
        /// <param name="expr">The expression with error</param>
        /// <param name="token">The invalid token</param>
        /// <param name="pos">The position of the error in expression</param>
        /// <param name="code">The error code</param>
        public ParserError(string message,
            string expr,
            string token,
            int pos,
            ErrorCodes code)
            : base(message)
        {
            this.Expr = expr;
            this.Token = token;
            this.Pos = pos;
            this.Code = code;
        }
    }
}
