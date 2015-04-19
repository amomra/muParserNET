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

using muParserNET;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace muParserNETTests
{
    [TestFixture]
    public class ParserOprtTests
    {
        [Test]
        public void TestDefineInfixOprt()
        {
            // cria o parser
            Parser parser = new Parser();
            parser.Expr = "|2";

            // cria o operador 't'
            parser.DefineInfixOprt("|", val =>
            {
                // eleva o número ao cubo
                return val * val * val;
            }, 6, true);

            double res = parser.Eval();

            Assert.AreEqual(8.0, res);
        }

        [Test]
        public void TestDefinePostfixOprt()
        {
            // cria o parser
            Parser parser = new Parser();
            parser.Expr = "2|";

            // cria o operador 't'
            parser.DefinePostfixOprt("|", val =>
            {
                // eleva o número a quarta
                return val * val * val * val;
            }, true);

            double res = parser.Eval();

            Assert.AreEqual(16.0, res);
        }

        [Test]
        public void TestDefineOprt()
        {
            // cria o parser
            Parser parser = new Parser();
            parser.Expr = "2 | 5";

            // cria o operador 't'
            parser.DefineOprt("|", (double val1, double val2) =>
            {
                // multiplica os números
                return val1 * val2;
            }, 6, OprtAssociativity.LEFT, true);

            double res = parser.Eval();

            Assert.AreEqual(10.0, res);
        }
    }
}
