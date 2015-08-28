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

namespace muParserNETTests
{
    [TestFixture]
    public class ParserConstsTests
    {
        [Test]
        public void TestDefineConst()
        {
            // cria o parser
            Parser parser = new Parser();
            parser.Expr = "myConst";

            // define a constante
            parser.DefineConst("myConst", 50.0);

            double r = parser.Eval();

            // verifica se foi retornado o valor da constante
            Assert.AreEqual(50.0, r);
        }

        [Test]
        public void TestGetConsts()
        {
            // cria o parser
            Parser parser = new Parser();

            // define a constante
            parser.DefineConst("myConst", 50.0);

            // indica se encontrou a constante que foi criada
            bool found = false;
            // busca a constante
            foreach(var constant in parser.Consts)
            {
                if (constant.Key == "myConst")
                    found = true;
                Console.WriteLine("{0} = {1}", constant.Key, constant.Value);
            }

            // falha o teste se não tiver encontrada a constante
            Assert.True(found);
        }
    }
}
