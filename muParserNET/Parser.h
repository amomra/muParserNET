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

#pragma once

#include "ParserVariable.h"
#include "ParserCallback.h"
#include "ParserError.h"
#include "OprtAssociativity.h"

#include "Functions.h"

using namespace System;
using namespace System::Collections::Generic;
using namespace System::Runtime::InteropServices;

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
	public ref class Parser
	{
	private:
		mu::Parser *parser;

		// hashmap com as variáveis
		Dictionary<String ^, ParserVariable ^> ^vars;

	private:
		// Atributos de apoio ao factory de variáveis -------------------------------

		// os objetos do delegate e userData não podem ser movidos pelo GC
	//	GCHandle ptrFactoryFunction;
	//	GCHandle ptrUserData;
	//
	//	void AllocFactoryObjects(FactoryFunction ^factoryFunc, Object ^userData);
	//	void FreeFactoryObjects();
	private:
		// Atributos de apoio as funções de identificação de tokens -----------------
		
		// armazena os delegates para eles não serem deletados
		List<ParserCallback ^> ^identFunctionsCallbacks;

	private:
		// Atributos de apoio as funções de definição de funções de cálculo ---------

		// armazena os delegates para eles não serem deletados
		Dictionary<String ^, ParserCallback ^> ^funcCallbacks;

		template <typename FuncType>
		void IntDefineFun(String ^name, FuncType ^func, bool allowOpt)
		{
			try
			{
				mu::string_type strName = msclr::interop::marshal_as<mu::string_type>(name);

				// cria a função
				ParserCallback ^callback = gcnew ParserCallback(func);

				IntPtr ptr = callback->Pointer;

				// adiciona no muParser
				this->parser->DefineFun(strName,
					static_cast<Utils::FunctionType<FuncType>::Value>(ptr.ToPointer()), allowOpt);

				this->funcCallbacks->Add(name, callback);
			}
			catch (mu::Parser::exception_type &e)
			{
				throw gcnew ParserError(e);
			}
		}

	private:
		// Atributos de apoio as funções de definição de operadores -----------------

		Dictionary<String ^, ParserCallback ^> ^infixOprtCallbacks;
		Dictionary<String ^, ParserCallback ^> ^postfixOprtCallbacks;
		Dictionary<String ^, ParserCallback ^> ^oprtCallbacks;

	public:
		/// <summary>
		/// Gets or sets the parser expression.
		/// </summary>
		property String ^Expr
		{
			String ^get();
			void set(String ^value);
		}

		/// <summary>
		/// Gets or sets the list of valid chars to be used in variables names.
		/// </summary>
		property String ^NameChars
		{
			String ^get();
			void set(String ^value);
		}

		/// <summary>
		/// Gets or sets the list of valid chars to be used as binary operators.
		/// </summary>
		property String ^OprtChars
		{
			String ^get();
			void set(String ^value);
		}

		/// <summary>
		/// Gets or sets the list of valid chars to be used as unary infix operators.
		/// </summary>
		property String ^InfixOprtChars
		{
			String ^get();
			void set(String ^value);
		}

		/// <summary>
		/// Gets the list of available variables to be used in expressions.
		/// </summary>
		property Dictionary<String ^, ParserVariable ^> ^Vars
		{
			Dictionary<String ^, ParserVariable ^> ^get();
		}

		/// <summary>
		/// Gets the list of available constants to be used in expressions.
		/// </summary>
		property Dictionary<String ^, double> ^Consts
		{
			Dictionary<String ^, double> ^get();
		}

		/// <summary>
		/// Gets the list of available functions to be used in expressions.
		/// </summary>
		property Dictionary<String ^, ParserCallback ^> ^Functions
		{
			Dictionary<String ^, ParserCallback ^> ^get();
		}

		/// <summary>
		/// Gets the list of built-in operators to be used in expressions.
		/// </summary>
		property List<String ^> ^BuiltInOprts
		{
			List<String ^> ^get();
		}

		/// <summary>
		/// Gets the list of available unary infix operators to be used in expressions.
		/// </summary>
		property Dictionary<String ^, ParserCallback ^> ^InfixOprts
		{
			Dictionary<String ^, ParserCallback ^> ^get();
		}

		/// <summary>
		/// Gets the list of available unary postfix operators to be used in expressions.
		/// </summary>
		property Dictionary<String ^, ParserCallback ^> ^PostfixOprts
		{
			Dictionary<String ^, ParserCallback ^> ^get();
		}

		/// <summary>
		/// Gets the list of available binary operators to be used in expressions.
		/// </summary>
		property Dictionary<String ^, ParserCallback ^> ^Oprts
		{
			Dictionary<String ^, ParserCallback ^> ^get();
		}
	public:
		/// <summary>
		/// Class constructor. It initialize the muParser structures.
		/// </summary>
		Parser();

		/// <summary>
		/// Class destructor.
		/// </summary>
		virtual ~Parser();

		/// <summary>
		/// Defines a parser variable.
		/// </summary>
		/// <param name="name">The variable name</param>
		/// <param name="var">The variable initial value</param>
		/// <returns>The parser variable reference</returns>
		/// <exception cref="ParserError">Throws if any parser error occurs</exception>
		ParserVariable ^DefineVar(String ^name, double var);

		/// <summary>
		/// Defines a parser variable.
		/// </summary>
		/// <param name="name">The variable name</param>
		/// <param name="var">A list of values. They will be used in bulk mode</param>
		/// <returns>The parser variable reference</returns>
		/// <exception cref="ParserError">Throws if any parser error occurs</exception>
		ParserVariable ^DefineVar(String ^name, array<double> ^var);

		/// <summary>
		/// Removes a variable from parser if it exists. If not, nothing will be done.
		/// </summary>
		/// <param name="name">The variable name</param>
		void RemoveVar(String ^name);

		/// <summary>
		/// Removes all variables from parser.
		/// </summary>
		void ClearVar();

		/// <summary>
		/// Defines a parser constant.
		/// </summary>
		/// <param name="name">The constant name</param>
		/// <param name="value">The constant value</param>
		/// <exception cref="ParserError">Throws if the name contains invalid signs</exception>
		void DefineConst(String ^name, double value);

		/// <summary>
		/// Defines a parser string constant.
		/// </summary>
		/// <param name="name">The constant name</param>
		/// <param name="value">The constant string value</param>
		/// <exception cref="ParserError">Throws if the name contains invalid signs</exception>
		void DefineStrConst(String ^name, String ^value);

		/// <summary>
		/// Clears all constants.
		/// </summary>
		void ClearConst();

		/// <summary>
		/// Add a value parsing function. When parsing an expression muParser
		/// tries to detect values in the expression string using different
		/// valident callbacks. Thuis it's possible to parse for hex values
		/// binary values and floating point values.
		/// </summary>
		/// <param name="identFunction">The callback function</param>
		void AddValIdent(IdentFunction ^identFunction);

		/// <summary>
		/// Define a parser function without arguments.
		/// </summary>
		/// <param name="name">The name of the function</param>
		/// <param name="func">The callback function</param>
		/// <param name="allowOpt">A flag indicating this function may be optimized</param>
		/// <exception cref="ParserError">Throws if there is a name conflict</exception>
		void DefineFun(String ^name, FunType0 ^func, bool allowOpt);

		/// <summary>
		/// Define a parser function with one argument.
		/// </summary>
		/// <param name="name">The name of the function</param>
		/// <param name="func">The callback function</param>
		/// <param name="allowOpt">A flag indicating this function may be optimized</param>
		/// <exception cref="ParserError">Throws if there is a name conflict</exception>
		void DefineFun(String ^name, FunType1 ^func, bool allowOpt);

		/// <summary>
		/// Define a parser function with two arguments.
		/// </summary>
		/// <param name="name">The name of the function</param>
		/// <param name="func">The callback function</param>
		/// <param name="allowOpt">A flag indicating this function may be optimized</param>
		/// <exception cref="ParserError">Throws if there is a name conflict</exception>
		void DefineFun(String ^name, FunType2 ^func, bool allowOpt);

		/// <summary>
		/// Define a parser function with three arguments.
		/// </summary>
		/// <param name="name">The name of the function</param>
		/// <param name="func">The callback function</param>
		/// <param name="allowOpt">A flag indicating this function may be optimized</param>
		/// <exception cref="ParserError">Throws if there is a name conflict</exception>
		void DefineFun(String ^name, FunType3 ^func, bool allowOpt);

		/// <summary>
		/// Define a parser function with four arguments.
		/// </summary>
		/// <param name="name">The name of the function</param>
		/// <param name="func">The callback function</param>
		/// <param name="allowOpt">A flag indicating this function may be optimized</param>
		/// <exception cref="ParserError">Throws if there is a name conflict</exception>
		void DefineFun(String ^name, FunType4 ^func, bool allowOpt);

		/// <summary>
		/// Define a parser function with five arguments.
		/// </summary>
		/// <param name="name">The name of the function</param>
		/// <param name="func">The callback function</param>
		/// <param name="allowOpt">A flag indicating this function may be optimized</param>
		/// <exception cref="ParserError">Throws if there is a name conflict</exception>
		void DefineFun(String ^name, FunType5 ^func, bool allowOpt);

		/// <summary>
		/// Define a parser function with six arguments.
		/// </summary>
		/// <param name="name">The name of the function</param>
		/// <param name="func">The callback function</param>
		/// <param name="allowOpt">A flag indicating this function may be optimized</param>
		/// <exception cref="ParserError">Throws if there is a name conflict</exception>
		void DefineFun(String ^name, FunType6 ^func, bool allowOpt);

		/// <summary>
		/// Define a parser function with seven arguments.
		/// </summary>
		/// <param name="name">The name of the function</param>
		/// <param name="func">The callback function</param>
		/// <param name="allowOpt">A flag indicating this function may be optimized</param>
		/// <exception cref="ParserError">Throws if there is a name conflict</exception>
		void DefineFun(String ^name, FunType7 ^func, bool allowOpt);

		/// <summary>
		/// Define a parser function with eight arguments.
		/// </summary>
		/// <param name="name">The name of the function</param>
		/// <param name="func">The callback function</param>
		/// <param name="allowOpt">A flag indicating this function may be optimized</param>
		/// <exception cref="ParserError">Throws if there is a name conflict</exception>
		void DefineFun(String ^name, FunType8 ^func, bool allowOpt);

		/// <summary>
		/// Define a parser function with nine arguments.
		/// </summary>
		/// <param name="name">The name of the function</param>
		/// <param name="func">The callback function</param>
		/// <param name="allowOpt">A flag indicating this function may be optimized</param>
		/// <exception cref="ParserError">Throws if there is a name conflict</exception>
		void DefineFun(String ^name, FunType9 ^func, bool allowOpt);

		/// <summary>
		/// Define a parser function with ten arguments.
		/// </summary>
		/// <param name="name">The name of the function</param>
		/// <param name="func">The callback function</param>
		/// <param name="allowOpt">A flag indicating this function may be optimized</param>
		/// <exception cref="ParserError">Throws if there is a name conflict</exception>
		void DefineFun(String ^name, FunType10 ^func, bool allowOpt);


		/// <summary>
		/// Define a parser function without arguments for bulk mode.
		/// </summary>
		/// <param name="name">The name of the function</param>
		/// <param name="func">The callback function</param>
		/// <param name="allowOpt">A flag indicating this function may be optimized</param>
		/// <exception cref="ParserError">Throws if there is a name conflict</exception>
		void DefineFun(String ^name, BulkFunType0 ^func, bool allowOpt);

		/// <summary>
		/// Define a parser function with one argument for bulk mode.
		/// </summary>
		/// <param name="name">The name of the function</param>
		/// <param name="func">The callback function</param>
		/// <param name="allowOpt">A flag indicating this function may be optimized</param>
		/// <exception cref="ParserError">Throws if there is a name conflict</exception>
		void DefineFun(String ^name, BulkFunType1 ^func, bool allowOpt);

		/// <summary>
		/// Define a parser function with two arguments for bulk mode.
		/// </summary>
		/// <param name="name">The name of the function</param>
		/// <param name="func">The callback function</param>
		/// <param name="allowOpt">A flag indicating this function may be optimized</param>
		/// <exception cref="ParserError">Throws if there is a name conflict</exception>
		void DefineFun(String ^name, BulkFunType2 ^func, bool allowOpt);

		/// <summary>
		/// Define a parser function with three arguments for bulk mode.
		/// </summary>
		/// <param name="name">The name of the function</param>
		/// <param name="func">The callback function</param>
		/// <param name="allowOpt">A flag indicating this function may be optimized</param>
		/// <exception cref="ParserError">Throws if there is a name conflict</exception>
		void DefineFun(String ^name, BulkFunType3 ^func, bool allowOpt);

		/// <summary>
		/// Define a parser function with four arguments for bulk mode.
		/// </summary>
		/// <param name="name">The name of the function</param>
		/// <param name="func">The callback function</param>
		/// <param name="allowOpt">A flag indicating this function may be optimized</param>
		/// <exception cref="ParserError">Throws if there is a name conflict</exception>
		void DefineFun(String ^name, BulkFunType4 ^func, bool allowOpt);

		/// <summary>
		/// Define a parser function with five arguments for bulk mode.
		/// </summary>
		/// <param name="name">The name of the function</param>
		/// <param name="func">The callback function</param>
		/// <param name="allowOpt">A flag indicating this function may be optimized</param>
		/// <exception cref="ParserError">Throws if there is a name conflict</exception>
		void DefineFun(String ^name, BulkFunType5 ^func, bool allowOpt);

		/// <summary>
		/// Define a parser function with six arguments for bulk mode.
		/// </summary>
		/// <param name="name">The name of the function</param>
		/// <param name="func">The callback function</param>
		/// <param name="allowOpt">A flag indicating this function may be optimized</param>
		/// <exception cref="ParserError">Throws if there is a name conflict</exception>
		void DefineFun(String ^name, BulkFunType6 ^func, bool allowOpt);

		/// <summary>
		/// Define a parser function with seven arguments for bulk mode.
		/// </summary>
		/// <param name="name">The name of the function</param>
		/// <param name="func">The callback function</param>
		/// <param name="allowOpt">A flag indicating this function may be optimized</param>
		/// <exception cref="ParserError">Throws if there is a name conflict</exception>
		void DefineFun(String ^name, BulkFunType7 ^func, bool allowOpt);

		/// <summary>
		/// Define a parser function with eight arguments for bulk mode.
		/// </summary>
		/// <param name="name">The name of the function</param>
		/// <param name="func">The callback function</param>
		/// <param name="allowOpt">A flag indicating this function may be optimized</param>
		/// <exception cref="ParserError">Throws if there is a name conflict</exception>
		void DefineFun(String ^name, BulkFunType8 ^func, bool allowOpt);

		/// <summary>
		/// Define a parser function with nine arguments for bulk mode.
		/// </summary>
		/// <param name="name">The name of the function</param>
		/// <param name="func">The callback function</param>
		/// <param name="allowOpt">A flag indicating this function may be optimized</param>
		/// <exception cref="ParserError">Throws if there is a name conflict</exception>
		void DefineFun(String ^name, BulkFunType9 ^func, bool allowOpt);

		/// <summary>
		/// Define a parser function with ten arguments for bulk mode.
		/// </summary>
		/// <param name="name">The name of the function</param>
		/// <param name="func">The callback function</param>
		/// <param name="allowOpt">A flag indicating this function may be optimized</param>
		/// <exception cref="ParserError">Throws if there is a name conflict</exception>
		void DefineFun(String ^name, BulkFunType10 ^func, bool allowOpt);

		/// <summary>
		/// Define a parser function with a variable argument list.
		/// </summary>
		/// <param name="name">The name of the function</param>
		/// <param name="func">The callback function</param>
		/// <param name="allowOpt">A flag indicating this function may be optimized</param>
		/// <exception cref="ParserError">Throws if there is a name conflict</exception>
		void DefineFun(String ^name, MultFunType ^func, bool allowOpt);

		/// <summary>
		/// Define a parser function taking a string as an argument.
		/// </summary>
		/// <param name="name">The name of the function</param>
		/// <param name="func">The callback function</param>
		/// <param name="allowOpt">A flag indicating this function may be optimized</param>
		/// <exception cref="ParserError">Throws if there is a name conflict</exception>
		void DefineFun(String ^name, StrFunType1 ^func, bool allowOpt);

		/// <summary>
		/// Define a parser function taking a string and a value as arguments.
		/// </summary>
		/// <param name="name">The name of the function</param>
		/// <param name="func">The callback function</param>
		/// <param name="allowOpt">A flag indicating this function may be optimized</param>
		/// <exception cref="ParserError">Throws if there is a name conflict</exception>
		void DefineFun(String ^name, StrFunType2 ^func, bool allowOpt);

		/// <summary>
		/// Define a parser function taking a string and two values as arguments.
		/// </summary>
		/// <param name="name">The name of the function</param>
		/// <param name="func">The callback function</param>
		/// <param name="allowOpt">A flag indicating this function may be optimized</param>
		/// <exception cref="ParserError">Throws if there is a name conflict</exception>
		void DefineFun(String ^name, StrFunType3 ^func, bool allowOpt);

		/// <summary>
		/// Clears all functions definitions.
		/// </summary>
		void ClearFun();

		/// <summary>
		/// Define a unary infix operator.
		/// </summary>
		/// <param name="identifier">The operator identifier</param>
		/// <param name="func">The callback function</param>
		/// <param name="precedence">The operator precedence</param>
		/// <param name="allowOpt">A flag indicating this function may be optimized</param>
		/// <exception cref="ParserError">Throws if there is a name conflict</exception>
		void DefineInfixOprt(String ^identifier, FunType1 ^func, unsigned precedence, bool allowOpt);

		/// <summary>
		/// Define a unary postfix operator.
		/// </summary>
		/// <param name="identifier">The operator identifier</param>
		/// <param name="func">The callback function</param>
		/// <param name="allowOpt">A flag indicating this function may be optimized</param>
		/// <exception cref="ParserError">Throws if there is a name conflict</exception>
		void DefinePostfixOprt(String ^identifier, FunType1 ^func, bool allowOpt);

		/// <summary>
		/// Define a binary operator.
		/// </summary>
		/// <param name="identifier">The operator identifier</param>
		/// <param name="func">The callback function</param>
		/// <param name="precedence">The operator precedence</param>
		/// <param name="associativity">The associativity of the operator</param>
		/// <param name="allowOpt">A flag indicating this function may be optimized</param>
		/// <exception cref="ParserError">Throws if there is a name conflict</exception>
		void DefineOprt(String ^identifier, FunType2 ^func, unsigned precedence, OprtAssociativity associativity, bool allowOpt);

		/// <summary>
		/// Clears all infix operators.
		/// </summary>
		void CleanInfixOprt();

		/// <summary>
		/// Clears all postfix operators.
		/// </summary>
		void CleanPostfixOprt();

		/// <summary>
		/// Clears all binary operators.
		/// </summary>
		void CleanOprt();

		/// <summary>
		/// Enable or disable the built in binary operators. If you disable the
		/// built in binary operators there will be no binary operators defined.
		/// Thus you must add them manually one by one. It is not possible to
		/// disable built in operators selectively.
		/// </summary>
		/// <param name="oprtEn">Indicates if the operators will be enable or disabled</param>
		void EnableBuiltInOprt(bool oprtEn);

		/// <summary>
		/// Enable or disable the formula optimization feature. 
		/// </summary>
		/// <param name="optmEn">Indicates if the optimizer will be enable or disabled</param>
		void EnableOptimizer(bool optmEn);

		/// <summary>
		/// Set argument separator.
		/// </summary>
		/// <param name="value">The argument separator character</param>
		void SetArgSep(wchar_t value);

		/// <summary>
		/// Set the decimal separator. By default muparser uses the "C" locale.
		/// The decimal separator of this locale is overwritten by the one
		/// provided here.
		/// </summary>
		/// <param name="value">The decimal separator character</param>
		void SetDecSep(wchar_t value);

		/// <summary>
		/// Sets the thousands operator. By default muparser uses the "C"
		/// locale. The thousands separator of this locale is overwritten by the
		/// one provided here.
		/// </summary>
		/// <param name="value">The thousands separator character</param>
		void SetThousandsSep(wchar_t value);

		/// <summary>
		/// Resets the locale. The default locale used "." as decimal separator,
		/// no thousands separator and "," as function argument separator.
		/// </summary>
		void ResetLocale();

		//void SetVarFactory(FactoryFunction ^func, Object ^userData);

		/// <summary>
		/// Calculate the result from the expression formula.
		/// </summary>
		/// <exception cref="ParserError">Throws if no formula is set or in case
		/// of any other error related to the formula</exception>
		double Eval();

		/// <summary>
		/// Calculate the results from the expression formula.
		/// </summary>
		/// <param name="bulkSize">The number of times that the formula will be
		/// calculated</param>
		/// <returns>The list with the results of the calculation</returns>
		/// <exception cref="ParserError">Throws if no formula is set or in case
		/// of any other error related to the formula</exception>
		array<double> ^EvalBulk(int bulkSize);

		/// <summary>
		/// Calculate the results from the expression with multiples formulas.
		/// </summary>
		/// <returns>The list with the results of the calculations</returns>
		/// <exception cref="ParserError">Throws if no formula is set or in case
		/// of any other error related to the formula</exception>
		array<double> ^EvalMulti();
	};

}