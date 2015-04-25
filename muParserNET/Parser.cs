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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace muParserNET
{
    /// <summary>
    /// Mathematical expressions parser (base parser engine).
    /// 
    /// This is the implementation of a bytecode based mathematical expressions
    /// parser. The formula will be parsed from string and converted into a bytecode.
    /// Future calculations will be done with the bytecode instead the formula string
    /// resulting in a significant performance increase. Complementary to a set
    /// of internally implemented functions the parser is able to handle
    /// user defined functions and variables.
    /// </summary>
    public partial class Parser
    {
        // handler para o parser
        private IntPtr parserHandler;

        // hashmap com as variáveis
        private Dictionary<string, ParserVariable> vars;

        #region Atributos de apoio as funções de identificação de tokens

        // armazena os delegates para eles não serem deletados
        private List<ParserCallback> identFunctionsCallbacks;

        #endregion

        #region Atributos de apoio as funções de definição de funções de cálculo

        // armazena os delegates para eles não serem deletados
        private Dictionary<string, ParserCallback> funcCallbacks;

        #endregion

        #region Atributos de apoio as funções de definição de operadores

        private Dictionary<string, ParserCallback> oprtCallbacks;

        #endregion

        #region Proprieades

        /// <summary>
        /// Gets or sets the parser expression.
        /// </summary>
        public string Expr
        {
            get
            {
                return Marshal.PtrToStringAnsi(MuParserFunctions.mupGetExpr(this.parserHandler));
            }
            set
            {
                MuParserFunctions.mupSetExpr(this.parserHandler, value);
            }
        }

        /// <summary>
        /// Gets the list of available variables to be used in expressions.
        /// </summary>
        public Dictionary<string, ParserVariable> Vars
        {
            get
            {
                /*
		         * A princípio não é para ter variáveis a mais no parser do que tem
		         * nesta lista.
		         */
                return this.vars;
            }
        }

        /// <summary>
        /// Gets the list of available constants to be used in expressions.
        /// </summary>
        public Dictionary<string, double> Consts
        {
            get
            {
                // lista de consts
                var consts = new Dictionary<string, double>();

                int numConsts = MuParserFunctions.mupGetConstNum(this.parserHandler);
                for (int i = 0; i < numConsts; i++)
                {
                    string constName = "";
                    double value = 0.0;
                    MuParserFunctions.mupGetConst(this.parserHandler, (uint)i, ref constName, ref value);

                    consts[constName] = value;
                }

                return consts;
            }
        }

        /// <summary>
        /// Gets the list of available functions to be used in expressions.
        /// </summary>
        public Dictionary<string, ParserCallback> Functions
        {
            get
            {
                return this.funcCallbacks;
            }
        }

        /// <summary>
        /// Gets the list of available binary operators to be used in expressions.
        /// </summary>
        public Dictionary<string, ParserCallback> Oprts
        {
            get
            {
                return this.oprtCallbacks;
            }
        }

        #endregion

        #region Funções

        /// <summary>
        /// Class constructor. It initialize the muParser structures.
        /// </summary>
        public Parser()
        {
            // inicializa o parser
            this.parserHandler = MuParserFunctions.mupCreate(0);

            // inicializa o dicionário com as variáveis
            this.vars = new Dictionary<string, ParserVariable>();

            // inicializa as listas de delegates
            this.identFunctionsCallbacks = new List<ParserCallback>();
            this.funcCallbacks = new Dictionary<string, ParserCallback>();
            this.oprtCallbacks = new Dictionary<string, ParserCallback>();
        }

        /// <summary>
        /// Class destructor.
        /// </summary>
        ~Parser()
        {
            // finaliza o parser
            MuParserFunctions.mupRelease(this.parserHandler);
        }

        /// <summary>
        /// Defines a parser variable.
        /// </summary>
        /// <param name="name">The variable name</param>
        /// <param name="var">The variable initial value</param>
        /// <returns>The parser variable reference</returns>
        /// <exception cref="ParserError">Throws if any parser error occurs</exception>
        public ParserVariable DefineVar(string name, double var)
        {
            // cria a variável
			ParserVariable parserVar = new ParserVariable(name, var);

			// ajusta a variável
            MuParserFunctions.mupDefineVar(this.parserHandler, name, parserVar.Pointer);

			// adiciona a variável na lista de variáveis
			this.vars[name] = parserVar;

			return parserVar;
        }

        /// <summary>
        /// Defines a parser variable.
        /// </summary>
        /// <param name="name">The variable name</param>
        /// <param name="var">A list of values. They will be used in bulk mode</param>
        /// <returns>The parser variable reference</returns>
        /// <exception cref="ParserError">Throws if any parser error occurs</exception>
        public ParserVariable DefineVar(string name, double[] var)
        {
            // cria a variável
            ParserVariable parserVar = new ParserVariable(name, var);

            // ajusta a variável
            MuParserFunctions.mupDefineVar(this.parserHandler, name, parserVar.Pointer);

            // adiciona a variável na lista de variáveis
            this.vars[name] = parserVar;

            return parserVar;
        }

        /// <summary>
        /// Removes a variable from parser if it exists. If not, nothing will be done.
        /// </summary>
        /// <param name="name">The variable name</param>
        public void RemoveVar(string name)
        {
            // remove a variável
            MuParserFunctions.mupRemoveVar(this.parserHandler, name);

            // e remove a variável da lista interna
            this.vars.Remove(name);
        }

        /// <summary>
        /// Removes all variables from parser.
        /// </summary>
        public void ClearVar()
        {
            // remove todas as variáveis
            MuParserFunctions.mupClearVar(this.parserHandler);
            this.vars.Clear();
        }

        /// <summary>
        /// Defines a parser constant.
        /// </summary>
        /// <param name="name">The constant name</param>
        /// <param name="value">The constant value</param>
        /// <exception cref="ParserError">Throws if the name contains invalid signs</exception>
        public void DefineConst(string name, double value)
        {
            MuParserFunctions.mupDefineConst(this.parserHandler, name, value);
        }

        /// <summary>
        /// Defines a parser string constant.
        /// </summary>
        /// <param name="name">The constant name</param>
        /// <param name="value">The constant string value</param>
        /// <exception cref="ParserError">Throws if the name contains invalid signs</exception>
        public void DefineStrConst(string name, string value)
        {
            MuParserFunctions.mupDefineStrConst(this.parserHandler, name, value);
        }

        /// <summary>
        /// Clears all constants.
        /// </summary>
        void ClearConst()
        {
            MuParserFunctions.mupClearConst(this.parserHandler);
        }

        /// <summary>
        /// Add a value parsing function. When parsing an expression muParser
        /// tries to detect values in the expression string using different
        /// valident callbacks. Thuis it's possible to parse for hex values
        /// binary values and floating point values.
        /// </summary>
        /// <param name="identFunction">The callback function</param>
        public void AddValIdent(IdentFunction identFunction)
        {
            // bloqueia o GC de mover o delegate
            ParserCallback callback = new ParserCallback(identFunction);

            // passa a chamada
            MuParserFunctions.mupAddValIdent(this.parserHandler, identFunction);

            this.identFunctionsCallbacks.Add(callback);
        }

        /// <summary>
        /// Define a parser function without arguments.
        /// </summary>
        /// <param name="name">The name of the function</param>
        /// <param name="func">The callback function</param>
        /// <param name="allowOpt">A flag indicating this function may be optimized</param>
        /// <exception cref="ParserError">Throws if there is a name conflict</exception>
        public void DefineFun(string name, FunType0 func, bool allowOpt = true)
        {
            // cria a função
            ParserCallback callback = new ParserCallback(func);

            // adiciona no muParser
            MuParserFunctions.mupDefineFun0(this.parserHandler, name, func, allowOpt);

            this.funcCallbacks.Add(name, callback);
        }

        /// <summary>
        /// Define a parser function with one argument.
        /// </summary>
        /// <param name="name">The name of the function</param>
        /// <param name="func">The callback function</param>
        /// <param name="allowOpt">A flag indicating this function may be optimized</param>
        /// <exception cref="ParserError">Throws if there is a name conflict</exception>
        public void DefineFun(string name, FunType1 func, bool allowOpt = true)
        {
            // cria a função
            ParserCallback callback = new ParserCallback(func);

            // adiciona no muParser
            MuParserFunctions.mupDefineFun1(this.parserHandler, name, func, allowOpt);

            this.funcCallbacks.Add(name, callback);
        }

        /// <summary>
        /// Define a parser function with two arguments.
        /// </summary>
        /// <param name="name">The name of the function</param>
        /// <param name="func">The callback function</param>
        /// <param name="allowOpt">A flag indicating this function may be optimized</param>
        /// <exception cref="ParserError">Throws if there is a name conflict</exception>
        public void DefineFun(string name, FunType2 func, bool allowOpt = true)
        {
            // cria a função
            ParserCallback callback = new ParserCallback(func);

            // adiciona no muParser
            MuParserFunctions.mupDefineFun2(this.parserHandler, name, func, allowOpt);

            this.funcCallbacks.Add(name, callback);
        }

        /// <summary>
        /// Define a parser function with three arguments.
        /// </summary>
        /// <param name="name">The name of the function</param>
        /// <param name="func">The callback function</param>
        /// <param name="allowOpt">A flag indicating this function may be optimized</param>
        /// <exception cref="ParserError">Throws if there is a name conflict</exception>
        public void DefineFun(string name, FunType3 func, bool allowOpt = true)
        {
            // cria a função
            ParserCallback callback = new ParserCallback(func);

            // adiciona no muParser
            MuParserFunctions.mupDefineFun3(this.parserHandler, name, func, allowOpt);

            this.funcCallbacks.Add(name, callback);
        }

        /// <summary>
        /// Define a parser function with four arguments.
        /// </summary>
        /// <param name="name">The name of the function</param>
        /// <param name="func">The callback function</param>
        /// <param name="allowOpt">A flag indicating this function may be optimized</param>
        /// <exception cref="ParserError">Throws if there is a name conflict</exception>
        public void DefineFun(string name, FunType4 func, bool allowOpt = true)
        {
            // cria a função
            ParserCallback callback = new ParserCallback(func);

            // adiciona no muParser
            MuParserFunctions.mupDefineFun4(this.parserHandler, name, func, allowOpt);

            this.funcCallbacks.Add(name, callback);
        }

        /// <summary>
        /// Define a parser function with five arguments.
        /// </summary>
        /// <param name="name">The name of the function</param>
        /// <param name="func">The callback function</param>
        /// <param name="allowOpt">A flag indicating this function may be optimized</param>
        /// <exception cref="ParserError">Throws if there is a name conflict</exception>
        public void DefineFun(string name, FunType5 func, bool allowOpt = true)
        {
            // cria a função
            ParserCallback callback = new ParserCallback(func);

            // adiciona no muParser
            MuParserFunctions.mupDefineFun5(this.parserHandler, name, func, allowOpt);

            this.funcCallbacks.Add(name, callback);
        }

        /// <summary>
        /// Define a parser function with six arguments.
        /// </summary>
        /// <param name="name">The name of the function</param>
        /// <param name="func">The callback function</param>
        /// <param name="allowOpt">A flag indicating this function may be optimized</param>
        /// <exception cref="ParserError">Throws if there is a name conflict</exception>
        public void DefineFun(string name, FunType6 func, bool allowOpt = true)
        {
            // cria a função
            ParserCallback callback = new ParserCallback(func);

            // adiciona no muParser
            MuParserFunctions.mupDefineFun6(this.parserHandler, name, func, allowOpt);

            this.funcCallbacks.Add(name, callback);
        }

        /// <summary>
        /// Define a parser function with seven arguments.
        /// </summary>
        /// <param name="name">The name of the function</param>
        /// <param name="func">The callback function</param>
        /// <param name="allowOpt">A flag indicating this function may be optimized</param>
        /// <exception cref="ParserError">Throws if there is a name conflict</exception>
        public void DefineFun(string name, FunType7 func, bool allowOpt = true)
        {
            // cria a função
            ParserCallback callback = new ParserCallback(func);

            // adiciona no muParser
            MuParserFunctions.mupDefineFun7(this.parserHandler, name, func, allowOpt);

            this.funcCallbacks.Add(name, callback);
        }

        /// <summary>
        /// Define a parser function with eight arguments.
        /// </summary>
        /// <param name="name">The name of the function</param>
        /// <param name="func">The callback function</param>
        /// <param name="allowOpt">A flag indicating this function may be optimized</param>
        /// <exception cref="ParserError">Throws if there is a name conflict</exception>
        public void DefineFun(string name, FunType8 func, bool allowOpt = true)
        {
            // cria a função
            ParserCallback callback = new ParserCallback(func);

            // adiciona no muParser
            MuParserFunctions.mupDefineFun8(this.parserHandler, name, func, allowOpt);

            this.funcCallbacks.Add(name, callback);
        }

        /// <summary>
        /// Define a parser function with nine arguments.
        /// </summary>
        /// <param name="name">The name of the function</param>
        /// <param name="func">The callback function</param>
        /// <param name="allowOpt">A flag indicating this function may be optimized</param>
        /// <exception cref="ParserError">Throws if there is a name conflict</exception>
        public void DefineFun(string name, FunType9 func, bool allowOpt = true)
        {
            // cria a função
            ParserCallback callback = new ParserCallback(func);

            // adiciona no muParser
            MuParserFunctions.mupDefineFun9(this.parserHandler, name, func, allowOpt);

            this.funcCallbacks.Add(name, callback);
        }

        /// <summary>
        /// Define a parser function with ten arguments.
        /// </summary>
        /// <param name="name">The name of the function</param>
        /// <param name="func">The callback function</param>
        /// <param name="allowOpt">A flag indicating this function may be optimized</param>
        /// <exception cref="ParserError">Throws if there is a name conflict</exception>
        public void DefineFun(string name, FunType10 func, bool allowOpt = true)
        {
            // cria a função
            ParserCallback callback = new ParserCallback(func);

            // adiciona no muParser
            MuParserFunctions.mupDefineFun10(this.parserHandler, name, func, allowOpt);

            this.funcCallbacks.Add(name, callback);
        }


        /// <summary>
        /// Define a parser function without arguments for bulk mode.
        /// </summary>
        /// <param name="name">The name of the function</param>
        /// <param name="func">The callback function</param>
        /// <exception cref="ParserError">Throws if there is a name conflict</exception>
        public void DefineFun(string name, BulkFunType0 func)
        {
            // cria a função
            ParserCallback callback = new ParserCallback(func);

            // adiciona no muParser
            MuParserFunctions.mupDefineBulkFun0(this.parserHandler, name, func);

            this.funcCallbacks.Add(name, callback);
        }

        /// <summary>
        /// Define a parser function with one argument for bulk mode.
        /// </summary>
        /// <param name="name">The name of the function</param>
        /// <param name="func">The callback function</param>
        /// <exception cref="ParserError">Throws if there is a name conflict</exception>
        public void DefineFun(string name, BulkFunType1 func)
        {
            // cria a função
            ParserCallback callback = new ParserCallback(func);

            // adiciona no muParser
            MuParserFunctions.mupDefineBulkFun1(this.parserHandler, name, func);

            this.funcCallbacks.Add(name, callback);
        }

        /// <summary>
        /// Define a parser function with two arguments for bulk mode.
        /// </summary>
        /// <param name="name">The name of the function</param>
        /// <param name="func">The callback function</param>
        /// <exception cref="ParserError">Throws if there is a name conflict</exception>
        public void DefineFun(string name, BulkFunType2 func)
        {
            // cria a função
            ParserCallback callback = new ParserCallback(func);

            // adiciona no muParser
            MuParserFunctions.mupDefineBulkFun2(this.parserHandler, name, func);

            this.funcCallbacks.Add(name, callback);
        }

        /// <summary>
        /// Define a parser function with three arguments for bulk mode.
        /// </summary>
        /// <param name="name">The name of the function</param>
        /// <param name="func">The callback function</param>
        /// <exception cref="ParserError">Throws if there is a name conflict</exception>
        public void DefineFun(string name, BulkFunType3 func)
        {
            // cria a função
            ParserCallback callback = new ParserCallback(func);

            // adiciona no muParser
            MuParserFunctions.mupDefineBulkFun3(this.parserHandler, name, func);

            this.funcCallbacks.Add(name, callback);
        }

        /// <summary>
        /// Define a parser function with four arguments for bulk mode.
        /// </summary>
        /// <param name="name">The name of the function</param>
        /// <param name="func">The callback function</param>
        /// <exception cref="ParserError">Throws if there is a name conflict</exception>
        public void DefineFun(string name, BulkFunType4 func)
        {
            // cria a função
            ParserCallback callback = new ParserCallback(func);

            // adiciona no muParser
            MuParserFunctions.mupDefineBulkFun4(this.parserHandler, name, func);

            this.funcCallbacks.Add(name, callback);
        }

        /// <summary>
        /// Define a parser function with five arguments for bulk mode.
        /// </summary>
        /// <param name="name">The name of the function</param>
        /// <param name="func">The callback function</param>
        /// <exception cref="ParserError">Throws if there is a name conflict</exception>
        public void DefineFun(string name, BulkFunType5 func)
        {
            // cria a função
            ParserCallback callback = new ParserCallback(func);

            // adiciona no muParser
            MuParserFunctions.mupDefineBulkFun5(this.parserHandler, name, func);

            this.funcCallbacks.Add(name, callback);
        }

        /// <summary>
        /// Define a parser function with six arguments for bulk mode.
        /// </summary>
        /// <param name="name">The name of the function</param>
        /// <param name="func">The callback function</param>
        /// <exception cref="ParserError">Throws if there is a name conflict</exception>
        public void DefineFun(string name, BulkFunType6 func)
        {
            // cria a função
            ParserCallback callback = new ParserCallback(func);

            // adiciona no muParser
            MuParserFunctions.mupDefineBulkFun6(this.parserHandler, name, func);

            this.funcCallbacks.Add(name, callback);
        }

        /// <summary>
        /// Define a parser function with seven arguments for bulk mode.
        /// </summary>
        /// <param name="name">The name of the function</param>
        /// <param name="func">The callback function</param>
        /// <exception cref="ParserError">Throws if there is a name conflict</exception>
        public void DefineFun(string name, BulkFunType7 func)
        {
            // cria a função
            ParserCallback callback = new ParserCallback(func);

            // adiciona no muParser
            MuParserFunctions.mupDefineBulkFun7(this.parserHandler, name, func);

            this.funcCallbacks.Add(name, callback);
        }

        /// <summary>
        /// Define a parser function with eight arguments for bulk mode.
        /// </summary>
        /// <param name="name">The name of the function</param>
        /// <param name="func">The callback function</param>
        /// <exception cref="ParserError">Throws if there is a name conflict</exception>
        public void DefineFun(string name, BulkFunType8 func)
        {
            // cria a função
            ParserCallback callback = new ParserCallback(func);

            // adiciona no muParser
            MuParserFunctions.mupDefineBulkFun8(this.parserHandler, name, func);

            this.funcCallbacks.Add(name, callback);
        }

        /// <summary>
        /// Define a parser function with nine arguments for bulk mode.
        /// </summary>
        /// <param name="name">The name of the function</param>
        /// <param name="func">The callback function</param>
        /// <exception cref="ParserError">Throws if there is a name conflict</exception>
        public void DefineFun(string name, BulkFunType9 func)
        {
            // cria a função
            ParserCallback callback = new ParserCallback(func);

            // adiciona no muParser
            MuParserFunctions.mupDefineBulkFun9(this.parserHandler, name, func);

            this.funcCallbacks.Add(name, callback);
        }

        /// <summary>
        /// Define a parser function with ten arguments for bulk mode.
        /// </summary>
        /// <param name="name">The name of the function</param>
        /// <param name="func">The callback function</param>
        /// <exception cref="ParserError">Throws if there is a name conflict</exception>
        public void DefineFun(string name, BulkFunType10 func)
        {
            // cria a função
            ParserCallback callback = new ParserCallback(func);

            // adiciona no muParser
            MuParserFunctions.mupDefineBulkFun10(this.parserHandler, name, func);

            this.funcCallbacks.Add(name, callback);
        }

        /// <summary>
        /// Define a parser function with a variable argument list.
        /// </summary>
        /// <param name="name">The name of the function</param>
        /// <param name="func">The callback function</param>
        /// <param name="allowOpt">A flag indicating this function may be optimized</param>
        /// <exception cref="ParserError">Throws if there is a name conflict</exception>
        public void DefineFun(string name, MultFunType func, bool allowOpt = true)
        {
            // cria a função
            ParserCallback callback = new ParserCallback(func);

            // adiciona no muParser
            MuParserFunctions.mupDefineMultFun(this.parserHandler, name, func, allowOpt);

            this.funcCallbacks.Add(name, callback);
        }

        /// <summary>
        /// Define a parser function taking a string as an argument.
        /// </summary>
        /// <param name="name">The name of the function</param>
        /// <param name="func">The callback function</param>
        /// <exception cref="ParserError">Throws if there is a name conflict</exception>
        public void DefineFun(string name, StrFunType1 func)
        {
            // cria a função
            ParserCallback callback = new ParserCallback(func);

            // adiciona no muParser
            MuParserFunctions.mupDefineStrFun1(this.parserHandler, name, func);

            this.funcCallbacks.Add(name, callback);
        }

        /// <summary>
        /// Define a parser function taking a string and a value as arguments.
        /// </summary>
        /// <param name="name">The name of the function</param>
        /// <param name="func">The callback function</param>
        /// <exception cref="ParserError">Throws if there is a name conflict</exception>
        public void DefineFun(string name, StrFunType2 func)
        {
            // cria a função
            ParserCallback callback = new ParserCallback(func);

            // adiciona no muParser
            MuParserFunctions.mupDefineStrFun2(this.parserHandler, name, func);

            this.funcCallbacks.Add(name, callback);
        }

        /// <summary>
        /// Define a parser function taking a string and two values as arguments.
        /// </summary>
        /// <param name="name">The name of the function</param>
        /// <param name="func">The callback function</param>
        /// <exception cref="ParserError">Throws if there is a name conflict</exception>
        public void DefineFun(string name, StrFunType3 func)
        {
            // cria a função
            ParserCallback callback = new ParserCallback(func);

            // adiciona no muParser
            MuParserFunctions.mupDefineStrFun3(this.parserHandler, name, func);

            this.funcCallbacks.Add(name, callback);
        }

        /// <summary>
        /// Clears all functions definitions.
        /// </summary>
        public void ClearFun()
        {
            MuParserFunctions.mupClearFun(this.parserHandler);
            this.funcCallbacks.Clear();
        }

        /// <summary>
        /// Define a unary infix operator.
        /// </summary>
        /// <param name="identifier">The operator identifier</param>
        /// <param name="func">The callback function</param>
        /// <param name="allowOpt">A flag indicating this function may be optimized</param>
        /// <exception cref="ParserError">Throws if there is a name conflict</exception>
        public void DefineInfixOprt(string identifier, FunType1 func, bool allowOpt = true)
        {
            // bloqueia o GC de mover o delegate
			ParserCallback callback = new ParserCallback(func);

            MuParserFunctions.mupDefineInfixOprt(this.parserHandler, identifier, func, allowOpt);

            this.oprtCallbacks.Add(identifier, callback);
        }

        /// <summary>
        /// Define a unary postfix operator.
        /// </summary>
        /// <param name="identifier">The operator identifier</param>
        /// <param name="func">The callback function</param>
        /// <param name="allowOpt">A flag indicating this function may be optimized</param>
        /// <exception cref="ParserError">Throws if there is a name conflict</exception>
        public void DefinePostfixOprt(string identifier, FunType1 func, bool allowOpt = true)
        {
            // bloqueia o GC de mover o delegate
            ParserCallback callback = new ParserCallback(func);

            MuParserFunctions.mupDefinePostfixOprt(this.parserHandler, identifier, func, allowOpt);

            this.oprtCallbacks.Add(identifier, callback);
        }

        /// <summary>
        /// Define a binary operator.
        /// </summary>
        /// <param name="identifier">The operator identifier</param>
        /// <param name="func">The callback function</param>
        /// <param name="precedence">The operator precedence</param>
        /// <param name="associativity">The associativity of the operator</param>
        /// <param name="allowOpt">A flag indicating this function may be optimized</param>
        /// <exception cref="ParserError">Throws if there is a name conflict</exception>
        public void DefineOprt(string identifier, FunType2 func, uint precedence, OprtAssociativity associativity, bool allowOpt = true)
        {
            // bloqueia o GC de mover o delegate
            ParserCallback callback = new ParserCallback(func);

            MuParserFunctions.mupDefineOprt(this.parserHandler, identifier, func, precedence, (int) associativity, allowOpt);

            this.oprtCallbacks.Add(identifier, callback);
        }

        /// <summary>
        /// Clears all operators.
        /// </summary>
        public void CleanOprt()
        {
            MuParserFunctions.mupClearOprt(this.parserHandler);
            this.oprtCallbacks.Clear();
        }

        /// <summary>
        /// Set argument separator.
        /// </summary>
        /// <param name="value">The argument separator character</param>
        public void SetArgSep(char value)
        {
            MuParserFunctions.mupSetArgSep(this.parserHandler, value);
        }

        /// <summary>
        /// Set the decimal separator. By default muparser uses the "C" locale.
        /// The decimal separator of this locale is overwritten by the one
        /// provided here.
        /// </summary>
        /// <param name="value">The decimal separator character</param>
        public void SetDecSep(char value)
        {
            MuParserFunctions.mupSetDecSep(this.parserHandler, value);
        }

        /// <summary>
        /// Sets the thousands operator. By default muparser uses the "C"
        /// locale. The thousands separator of this locale is overwritten by the
        /// one provided here.
        /// </summary>
        /// <param name="value">The thousands separator character</param>
        public void SetThousandsSep(char value)
        {
            MuParserFunctions.mupSetThousandsSep(this.parserHandler, value);
        }

        /// <summary>
        /// Resets the locale. The default locale used "." as decimal separator,
        /// no thousands separator and "," as function argument separator.
        /// </summary>
        public void ResetLocale()
        {
            MuParserFunctions.mupResetLocale(this.parserHandler);
        }

        //void SetVarFactory(FactoryFunction ^func, Object ^userData){ }

        /// <summary>
        /// Calculate the result from the expression formula.
        /// </summary>
        /// <exception cref="ParserError">Throws if no formula is set or in case
        /// of any other error related to the formula</exception>
        public double Eval()
        {
            return MuParserFunctions.mupEval(this.parserHandler);
        }

        /// <summary>
        /// Calculate the results from the expression formula.
        /// </summary>
        /// <param name="bulkSize">The number of times that the formula will be
        /// calculated</param>
        /// <returns>The list with the results of the calculation</returns>
        /// <exception cref="ParserError">Throws if no formula is set or in case
        /// of any other error related to the formula</exception>
        public double[] EvalBulk(int bulkSize)
        {
            // aloca o vetor de resposta
			double[] result = new double[bulkSize];

            /*
             * Os PInvoke já fazem o pin do objeto.
             */

			// executa o comando
            MuParserFunctions.mupEvalBulk(this.parserHandler, result, bulkSize);

			return result;
        }

        /// <summary>
        /// Calculate the results from the expression with multiples formulas.
        /// </summary>
        /// <returns>The list with the results of the calculations</returns>
        /// <exception cref="ParserError">Throws if no formula is set or in case
        /// of any other error related to the formula</exception>
        public double[] EvalMulti()
        {
            /*
		     * O vetor retornado pela função Eval é gerenciado pelo próprio parser,
		     * não sendo necessário desalocá-lo.
		     */

            int n = 0;

			IntPtr result = MuParserFunctions.mupEvalMulti(this.parserHandler, ref n);

			// aloca o array de retorno do .net
			double[] ret = new double[n];

			// copia o resultado
            Marshal.Copy(result, ret, 0, n);
			return ret;
        }

        /// <summary>
        /// Sets the list of valid chars to be used in variables names.
        /// </summary>
        /// <param name="value">The string containing the valid chars</param>
        public void DefineNameChars(string value)
        {
            MuParserFunctions.mupDefineNameChars(this.parserHandler, value);
        }

        /// <summary>
        /// Sets the list of valid chars to be used as binary operators.
        /// </summary>
        /// <param name="value">The string containing the valid chars</param>
        public void DefineOprtChars(string value)
        {
            MuParserFunctions.mupDefineOprtChars(this.parserHandler, value);
        }

        /// <summary>
        /// Sets the list of valid chars to be used as unary infix operators.
        /// </summary>
        /// <param name="value">The string containing the valid chars</param>
        public void DefineInfixOprtChars(string value)
        {
            MuParserFunctions.mupDefineInfixOprtChars(this.parserHandler, value);
        }

        #endregion
    }
}
