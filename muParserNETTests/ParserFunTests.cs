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
