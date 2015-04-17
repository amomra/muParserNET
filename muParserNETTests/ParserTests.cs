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
    }
}
