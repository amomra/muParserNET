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
    }
}
