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
            parser.Expr = "a = #10";

            // define a variável 'a'
            ParserVariable a = parser.DefineVar("a", 0.0);

            // adiciona a função de parser
            parser.AddValIdent((string remainingExpr, ref int pos, ref double value) =>
            {
                // ignora o token se ele não comecar com '#'
                if (remainingExpr[0] != '#')
                    return false;

                int n = int.Parse(remainingExpr);

                value = n;

                return true;
            });

            parser.Eval();

            // verifica se fez o parser corretamente
            Assert.AreEqual(10.0, a.Value);
        }
    }
}
