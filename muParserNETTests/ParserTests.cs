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

This software contains parts copied from muParser library.
muParser library - Copyright (C) 2013 Ingo Berg
*/

using System;
using muParserNET;
using NUnit.Framework;

namespace muParserNETTests
{
    [TestFixture]
    public class ParserTests
    {
        [Test]
        public void TestExpr()
        {
            // cria o parser e tenta ajustar a expressão
            Parser parser = new Parser();
            parser.Expr = "a + b";

            // verifica se a expressão foi ajustada (remove qualquer espaço desnecessário gerado)
            Assert.AreEqual("a + b", parser.Expr.Trim());
        }

        [Test]
        public void TestParserError()
        {
            try
            {
                // cria o parser e tenta ajustar uma expressão errada
                Parser parser = new Parser();
                parser.Expr = "aaaaaaaaasdasd;~";

                parser.Eval();

                // se passou para cá significa que não ocorreu erro de parser
                Assert.Fail();
            }
            catch (ParserError e)
            {
                // significa que capturou a exceção correta
                Console.WriteLine(e.Message);
            }
        }

        [Test]
        public void TestEval()
        {
            // cria o parser e tenta ajustar a expressão
            Parser parser = new Parser();
            parser.Expr = "2 + 10";

            // faz o cálculo
            double res = parser.Eval();

            Assert.AreEqual(12.0, res);
        }

        [Test]
        public void TestEvalMulti()
        {
            // cria o parser e tenta ajustar a expressão com múltiplas formulas
            Parser parser = new Parser();
            parser.Expr = "2 + 10, 3 + 5";

            double[] res = parser.EvalMulti();

            // verifica se a quantidade de resultados está correta
            Assert.AreEqual(2, res.Length);

            // verifica se o resultado está correto
            Assert.AreEqual(12.0, res[0]);
            Assert.AreEqual(8.0, res[1]);
        }


        [Test]
        public void TestEvalBulk()
        {
            Parser parser = new Parser();
            parser.Expr = "a + b";

            // cria as variáveis
            double[] a = new double[] { 1, 2, 3 };
            double[] b = new double[] { 3, 2, 1 };

            parser.DefineVar("a", a);
            parser.DefineVar("b", b);

            // faz o cálculo
            double[] res = parser.EvalBulk(3);

            // verifica se calculou corretamente
            Assert.AreEqual(3, res.Length);

            foreach (var r in res)
                Assert.AreEqual(4, r);
        }
    }
}
