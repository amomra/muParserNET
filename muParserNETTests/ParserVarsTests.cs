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
    public class ParserVarsTests
    {
        [Test]
        public void TestDefineVar()
        {
            // cria o parser
            Parser parser = new Parser();
            parser.Expr = "a";

            // define a variável 'a'
            ParserVariable a = parser.DefineVar("a", 10.0);

            // calcula
            double res = parser.Eval();

            Assert.AreEqual(10.0, res);

            // muda o valor e testa denovo
            a.Value = 50.0;

            res = parser.Eval();

            Assert.AreEqual(50.0, res);
        }

        [Test]
        public void TestAddIdentFunc()
        {
            // cria o parser
            Parser parser = new Parser();
            parser.Expr = "#10";

            // adiciona a função de parser
            parser.AddValIdent((string remainingExpr, ref int pos, ref double value) =>
            {
                // ignora o token se ele não comecar com '#'
                if (remainingExpr.Length == 0 || remainingExpr[0] != '#')
                    return false;

                // pega a string a frente do simbolo
                string strN = remainingExpr.Substring(1);

                int n = 0;

                if (int.TryParse(strN, out n))
                {
                    value = n;

                    // incrementa a posição para ir para o próximo token
                    pos += remainingExpr.Length;

                    return true;
                }

                return false;
            });

            double res = parser.Eval();

            // verifica se fez o parser corretamente
            Assert.AreEqual(10.0, res);
        }
    }
}
