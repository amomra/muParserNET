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

namespace muParserNETTests
{
    [TestFixture]
    public class ParserFunTests
    {

        [Test]
        public void TestDefineFun()
        {
            // cria o parser
            Parser parser = new Parser();
            parser.Expr = "testeFun(2)";

            // cria a função
            parser.DefineFun("testeFun", arg =>
            {
                return arg + 10;
            },
            false);

            // executa
            double res = parser.Eval();

            // verifica se a função foi executada
            Assert.AreEqual(12.0, res);
        }

        [Test]
        public void TestDefineFunMult()
        {
            // cria o parser
            Parser parser = new Parser();
            parser.Expr = "testeFun2(1)";

            parser.DefineFun("testeFun2", (double[] args, int n) =>
            {
                // faz a somatória
                double sum = 0.0;

                foreach (var arg in args)
                    sum += arg;
                return sum;
            }, true);

            // executa e verifica o resultado
            double res = parser.Eval();
            Assert.AreEqual(1.0, res);

            parser.Expr = "testeFun2(1, 2, 3)";

            res = parser.Eval();
            Assert.AreEqual(6.0, res);
        }
    }
}
