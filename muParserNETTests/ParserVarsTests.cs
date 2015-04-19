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
