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
