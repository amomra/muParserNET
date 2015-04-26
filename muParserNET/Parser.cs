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

        private Dictionary<string, ParserCallback> infixOprtCallbacks;
        private Dictionary<string, ParserCallback> postfixOprtCallbacks;
        private Dictionary<string, ParserCallback> oprtCallbacks;

        #endregion

        #region Propriedades

        /// <summary>
        /// Gets or sets the parser expression.
        /// </summary>
        public string Expr
        {
            get
            {
                return Marshal.PtrToStringAnsi(MuParserLibrary.mupGetExpr(this.parserHandler));
            }
            set
            {
                MuParserLibrary.mupSetExpr(this.parserHandler, value);
            }
        }

        /// <summary>
        /// Gets or sets the list of valid chars to be used in variables names.
        /// </summary>
        /// <remarks>The get function of this property is not supported if using the original
        /// muParser library instead of the muParserNET-compatible library (which
        /// is available at muParserNET repository.</remarks>
        public string NameChars
        {
            get
            {
                // checa se a biblioteca do muParser é compatível
                this.CheckLibraryVersion();

                return Marshal.PtrToStringAnsi(MuParserLibrary.mupValidNameChars(this.parserHandler));
            }
            set
            {
                MuParserLibrary.mupDefineNameChars(this.parserHandler, value);
            }
        }

        /// <summary>
        /// Gets or sets the list of valid chars to be used as binary operators.
        /// </summary>
        /// <remarks>The get function of this property is not supported if using the original
        /// muParser library instead of the muParserNET-compatible library (which
        /// is available at muParserNET repository.</remarks>
        public string OprtChars
        {
            get
            {
                // checa se a biblioteca do muParser é compatível
                this.CheckLibraryVersion();

                return Marshal.PtrToStringAnsi(MuParserLibrary.mupValidOprtChars(this.parserHandler));
            }
            set
            {
                MuParserLibrary.mupDefineOprtChars(this.parserHandler, value);
            }
        }

        /// <summary>
        /// Gets or sets the list of valid chars to be used as unary infix operators.
        /// </summary>
        /// <remarks>The get function of this property is not supported if using the original
        /// muParser library instead of the muParserNET-compatible library (which
        /// is available at muParserNET repository.</remarks>
        public string InfixOprtChars
        {
            get
            {
                // checa se a biblioteca do muParser é compatível
                this.CheckLibraryVersion();

                return Marshal.PtrToStringAnsi(MuParserLibrary.mupValidInfixOprtChars(this.parserHandler));
            }
            set
            {
                MuParserLibrary.mupDefineInfixOprtChars(this.parserHandler, value);
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

                int numConsts = MuParserLibrary.mupGetConstNum(this.parserHandler);
                for (int i = 0; i < numConsts; i++)
                {

                    /*
                     * A biblioteca do muParser conta com um buffer interno que
                     * é preenchido com o nome da constante. A função ajusta o
                     * ponteiro passado para apontar para esse buffer. A gerência
                     * deste buffer é feita pelo próprio muParser.
                     */
                    IntPtr buffConstName = new IntPtr();
                    double value = 0.0;
                    MuParserLibrary.mupGetConst(this.parserHandler, (uint)i, ref buffConstName, ref value);

                    string constName = Marshal.PtrToStringAnsi(buffConstName);

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
		/// Gets the list of available unary infix operators to be used in expressions.
		/// </summary>
		public Dictionary<string, ParserCallback> InfixOprts
		{
            get
            {
                return this.infixOprtCallbacks;
            }
        }

		/// <summary>
		/// Gets the list of available unary postfix operators to be used in expressions.
		/// </summary>
		public Dictionary<string, ParserCallback> PostfixOprts
        {
            get
            {
                return this.postfixOprtCallbacks;
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
            this.parserHandler = MuParserLibrary.mupCreate(0);

            // inicializa o dicionário com as variáveis
            this.vars = new Dictionary<string, ParserVariable>();

            // inicializa as listas de delegates
            this.identFunctionsCallbacks = new List<ParserCallback>();
            this.funcCallbacks = new Dictionary<string, ParserCallback>();

            this.infixOprtCallbacks = new Dictionary<string, ParserCallback>();
            this.postfixOprtCallbacks = new Dictionary<string, ParserCallback>();
            this.oprtCallbacks = new Dictionary<string, ParserCallback>();

            // ajusta a função de tratamento de erros
            MuParserLibrary.mupSetErrorHandler(this.parserHandler, this.ErrorHandler);
        }

        /// <summary>
        /// Class destructor.
        /// </summary>
        ~Parser()
        {
            // finaliza o parser
            MuParserLibrary.mupRelease(this.parserHandler);
        }

        /// <summary>
        /// Checks if the muParser library is compatible with the muParserNET.
        /// If not, it throws an exception.
        /// </summary>
        private void CheckLibraryVersion()
        {
            // esta função não é suportada pela versão original do muParser
            if (!this.GetVersion().Contains("muParserNET"))
                throw new NotSupportedException("This function only works with the muParserNET modified muParser library");
        }

        /// <summary>
        /// Error handler. It loads the ParserError exception.
        /// </summary>
        private void ErrorHandler()
        {
            IntPtr ptrMessage = MuParserLibrary.mupGetErrorMsg(this.parserHandler);
            IntPtr ptrToken = MuParserLibrary.mupGetErrorToken(this.parserHandler);

            string message = Marshal.PtrToStringAnsi(ptrMessage);
            string expr = this.Expr;
            string token = Marshal.PtrToStringAnsi(ptrToken);
            ErrorCodes code = (ErrorCodes)MuParserLibrary.mupGetErrorCode(this.parserHandler);
            int pos = MuParserLibrary.mupGetErrorPos(this.parserHandler);

            // lança a exceção
            throw new ParserError(message, expr, token, pos, code);
        }

        /// <summary>
        /// Returns the muParser version.
        /// </summary>
        /// <returns>The string with the muParser library version. It will contain
        /// the '-muParserNET' string appended if compiled from muParserNET
        /// repository. If using the original muParser library, some functions
        /// will not work</returns>
        public string GetVersion()
        {
            IntPtr ptrVersion = MuParserLibrary.mupGetVersion(this.parserHandler);
            return Marshal.PtrToStringAnsi(ptrVersion);
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
            MuParserLibrary.mupDefineVar(this.parserHandler, name, parserVar.Pointer);

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
            MuParserLibrary.mupDefineVar(this.parserHandler, name, parserVar.Pointer);

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
            MuParserLibrary.mupRemoveVar(this.parserHandler, name);

            // e remove a variável da lista interna
            this.vars.Remove(name);
        }

        /// <summary>
        /// Removes all variables from parser.
        /// </summary>
        public void ClearVar()
        {
            // remove todas as variáveis
            MuParserLibrary.mupClearVar(this.parserHandler);
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
            MuParserLibrary.mupDefineConst(this.parserHandler, name, value);
        }

        /// <summary>
        /// Defines a parser string constant.
        /// </summary>
        /// <param name="name">The constant name</param>
        /// <param name="value">The constant string value</param>
        /// <exception cref="ParserError">Throws if the name contains invalid signs</exception>
        public void DefineStrConst(string name, string value)
        {
            MuParserLibrary.mupDefineStrConst(this.parserHandler, name, value);
        }

        /// <summary>
        /// Clears all constants.
        /// </summary>
        void ClearConst()
        {
            MuParserLibrary.mupClearConst(this.parserHandler);
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
            MuParserLibrary.mupAddValIdent(this.parserHandler, identFunction);

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
            MuParserLibrary.mupDefineFun0(this.parserHandler, name, func, allowOpt);

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
            MuParserLibrary.mupDefineFun1(this.parserHandler, name, func, allowOpt);

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
            MuParserLibrary.mupDefineFun2(this.parserHandler, name, func, allowOpt);

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
            MuParserLibrary.mupDefineFun3(this.parserHandler, name, func, allowOpt);

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
            MuParserLibrary.mupDefineFun4(this.parserHandler, name, func, allowOpt);

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
            MuParserLibrary.mupDefineFun5(this.parserHandler, name, func, allowOpt);

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
            MuParserLibrary.mupDefineFun6(this.parserHandler, name, func, allowOpt);

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
            MuParserLibrary.mupDefineFun7(this.parserHandler, name, func, allowOpt);

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
            MuParserLibrary.mupDefineFun8(this.parserHandler, name, func, allowOpt);

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
            MuParserLibrary.mupDefineFun9(this.parserHandler, name, func, allowOpt);

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
            MuParserLibrary.mupDefineFun10(this.parserHandler, name, func, allowOpt);

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
            MuParserLibrary.mupDefineBulkFun0(this.parserHandler, name, func);

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
            MuParserLibrary.mupDefineBulkFun1(this.parserHandler, name, func);

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
            MuParserLibrary.mupDefineBulkFun2(this.parserHandler, name, func);

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
            MuParserLibrary.mupDefineBulkFun3(this.parserHandler, name, func);

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
            MuParserLibrary.mupDefineBulkFun4(this.parserHandler, name, func);

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
            MuParserLibrary.mupDefineBulkFun5(this.parserHandler, name, func);

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
            MuParserLibrary.mupDefineBulkFun6(this.parserHandler, name, func);

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
            MuParserLibrary.mupDefineBulkFun7(this.parserHandler, name, func);

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
            MuParserLibrary.mupDefineBulkFun8(this.parserHandler, name, func);

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
            MuParserLibrary.mupDefineBulkFun9(this.parserHandler, name, func);

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
            MuParserLibrary.mupDefineBulkFun10(this.parserHandler, name, func);

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
            MuParserLibrary.mupDefineMultFun(this.parserHandler, name, func, allowOpt);

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
            MuParserLibrary.mupDefineStrFun1(this.parserHandler, name, func);

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
            MuParserLibrary.mupDefineStrFun2(this.parserHandler, name, func);

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
            MuParserLibrary.mupDefineStrFun3(this.parserHandler, name, func);

            this.funcCallbacks.Add(name, callback);
        }

        /// <summary>
        /// Clears all functions definitions.
        /// </summary>
        public void ClearFun()
        {
            MuParserLibrary.mupClearFun(this.parserHandler);
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

            MuParserLibrary.mupDefineInfixOprt(this.parserHandler, identifier, func, allowOpt);

            this.infixOprtCallbacks.Add(identifier, callback);
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

            MuParserLibrary.mupDefinePostfixOprt(this.parserHandler, identifier, func, allowOpt);

            this.postfixOprtCallbacks.Add(identifier, callback);
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
        public void DefineOprt(string identifier, FunType2 func, uint precedence = 0, OprtAssociativity associativity = OprtAssociativity.LEFT, bool allowOpt = false)
        {
            // bloqueia o GC de mover o delegate
            ParserCallback callback = new ParserCallback(func);

            MuParserLibrary.mupDefineOprt(this.parserHandler, identifier, func, precedence, (int) associativity, allowOpt);

            this.oprtCallbacks.Add(identifier, callback);
        }

        /// <summary>
        /// Clears all infix operators.
        /// </summary>
        /// <remarks>This function is not supported if using the original
        /// muParser library instead of the muParserNET-compatible library (which
        /// is available at muParserNET repository).</remarks>
        public void CleanInfixOprt()
        {
            // checa se a biblioteca do muParser é compatível
            this.CheckLibraryVersion();

            MuParserLibrary.mupClearInfixOprt(this.parserHandler);
            this.infixOprtCallbacks.Clear();
        }

        /// <summary>
        /// Clears all postfix operators.
        /// </summary>
        /// <remarks>This function is not supported if using the original
        /// muParser library instead of the muParserNET-compatible library (which
        /// is available at muParserNET repository).</remarks>
        public void CleanPostfixOprt()
        {
            // checa se a biblioteca do muParser é compatível
            this.CheckLibraryVersion();

            MuParserLibrary.mupClearPostfixOprt(this.parserHandler);
            this.postfixOprtCallbacks.Clear();
        }

        /// <summary>
        /// Clears all operators.
        /// </summary>
        public void CleanOprt()
        {
            MuParserLibrary.mupClearOprt(this.parserHandler);
            this.oprtCallbacks.Clear();
        }

        /// <summary>
        /// Enable or disable the built in binary operators. If you disable the
        /// built in binary operators there will be no binary operators defined.
        /// Thus you must add them manually one by one. It is not possible to
        /// disable built in operators selectively.
        /// </summary>
        /// <param name="oprtEn">Indicates if the operators will be enable or disabled</param>
        /// <remarks>This function is not supported if using the original
        /// muParser library instead of the muParserNET-compatible library (which
        /// is available at muParserNET repository).</remarks>
        public void EnableBuiltInOprt(bool oprtEn = true)
        {
            // checa se a biblioteca do muParser é compatível
            this.CheckLibraryVersion();

            MuParserLibrary.mupEnableBuiltInOprt(this.parserHandler, oprtEn);
        }

        /// <summary>
        /// Enable or disable the formula optimization feature. 
        /// </summary>
        /// <param name="optmEn">Indicates if the optimizer will be enable or disabled</param>
        /// <remarks>This function is not supported if using the original
        /// muParser library instead of the muParserNET-compatible library (which
        /// is available at muParserNET repository).</remarks>
        public void EnableOptimizer(bool optmEn = true)
        {
            // checa se a biblioteca do muParser é compatível
            this.CheckLibraryVersion();

            MuParserLibrary.mupEnableOptimizer(this.parserHandler, optmEn);
        }

        /// <summary>
        /// Set argument separator.
        /// </summary>
        /// <param name="value">The argument separator character</param>
        public void SetArgSep(char value)
        {
            MuParserLibrary.mupSetArgSep(this.parserHandler, value);
        }

        /// <summary>
        /// Set the decimal separator. By default muparser uses the "C" locale.
        /// The decimal separator of this locale is overwritten by the one
        /// provided here.
        /// </summary>
        /// <param name="value">The decimal separator character</param>
        public void SetDecSep(char value)
        {
            MuParserLibrary.mupSetDecSep(this.parserHandler, value);
        }

        /// <summary>
        /// Sets the thousands operator. By default muparser uses the "C"
        /// locale. The thousands separator of this locale is overwritten by the
        /// one provided here.
        /// </summary>
        /// <param name="value">The thousands separator character</param>
        public void SetThousandsSep(char value = '\0')
        {
            MuParserLibrary.mupSetThousandsSep(this.parserHandler, value);
        }

        /// <summary>
        /// Resets the locale. The default locale used "." as decimal separator,
        /// no thousands separator and "," as function argument separator.
        /// </summary>
        public void ResetLocale()
        {
            MuParserLibrary.mupResetLocale(this.parserHandler);
        }

        //void SetVarFactory(FactoryFunction ^func, Object ^userData){ }

        /// <summary>
        /// Calculate the result from the expression formula.
        /// </summary>
        /// <exception cref="ParserError">Throws if no formula is set or in case
        /// of any other error related to the formula</exception>
        public double Eval()
        {
            return MuParserLibrary.mupEval(this.parserHandler);
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
            MuParserLibrary.mupEvalBulk(this.parserHandler, result, bulkSize);

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

			IntPtr result = MuParserLibrary.mupEvalMulti(this.parserHandler, ref n);

			// aloca o array de retorno do .net
			double[] ret = new double[n];

			// copia o resultado
            Marshal.Copy(result, ret, 0, n);
			return ret;
        }

        #endregion
    }
}
