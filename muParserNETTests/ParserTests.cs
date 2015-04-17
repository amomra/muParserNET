using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using muParserNET;

namespace muParserNETTests
{
    [TestClass]
    public class ParserTests
    {
        [TestMethod]
        public void TestExpr()
        {
            // cria o parser e tenta ajustar a expressão
            Parser parser = new Parser();
            parser.Expr = "a + b";

            // verifica se a expressão foi ajustada (remove qualquer espaço desnecessário gerado)
            Assert.AreEqual("a + b", parser.Expr.Trim());
        }
    }
}
