#pragma once

#include "ParserVariable.h"
#include "ParserCallback.h"
#include "ParserError.h"

#include "Functions.h"

using namespace System;
using namespace System::Collections::Generic;
using namespace System::Runtime::InteropServices;

namespace muParserNET
{

	public ref class Parser
	{
	private:
		mu::Parser *parser;

		// hashmap com as variáveis
		Dictionary<String ^, ParserVariable ^> ^vars;

	private:
		// Atributos de apoio ao factory de variáveis

		// os objetos do delegate e userData não podem ser movidos pelo GC
	//	GCHandle ptrFactoryFunction;
	//	GCHandle ptrUserData;
	//
	//	void AllocFactoryObjects(FactoryFunction ^factoryFunc, Object ^userData);
	//	void FreeFactoryObjects();
	private:
		// Atributos de apoio as funções de identificação de tokens
		
		// armazena os delegates para eles não serem deletados
		List<ParserCallback ^> ^identFunctionsCallbacks;

	private:
		// Atributos de apoio as funções de definição de funções de cálculo

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

	public:
		property String ^Expr
		{
			String ^get();
			void set(String ^value);
		}

		property String ^NameChars
		{
			String ^get();
			void set(String ^value);
		}

		property String ^OprtChars
		{
			String ^get();
			void set(String ^value);
		}

		property String ^InfixOprtChars
		{
			String ^get();
			void set(String ^value);
		}

		property IReadOnlyDictionary<String ^, ParserVariable ^> ^Vars
		{
			IReadOnlyDictionary<String ^, ParserVariable ^> ^get();
		}

		property IReadOnlyDictionary<String ^, double> ^Consts
		{
			IReadOnlyDictionary<String ^, double> ^get();
		}
	public:
		Parser();
		virtual ~Parser();

		ParserVariable ^DefineVar(String ^name, double var);
		ParserVariable ^DefineVar(String ^name, array<double> ^var);
		void RemoveVar(String ^name);
		void ClearVar();

		void DefineConst(String ^name, double value);
		void DefineStrConst(String ^name, String ^value);
		void ClearConst();

		void AddValIdent(IdentFunction ^identFunction);
		
		void DefineFun(String ^name, FunType0 ^func, bool allowOpt);
		void DefineFun(String ^name, FunType1 ^func, bool allowOpt);
		void DefineFun(String ^name, FunType2 ^func, bool allowOpt);
		void DefineFun(String ^name, FunType3 ^func, bool allowOpt);
		void DefineFun(String ^name, FunType4 ^func, bool allowOpt);
		void DefineFun(String ^name, FunType5 ^func, bool allowOpt);
		void DefineFun(String ^name, FunType6 ^func, bool allowOpt);
		void DefineFun(String ^name, FunType7 ^func, bool allowOpt);
		void DefineFun(String ^name, FunType8 ^func, bool allowOpt);
		void DefineFun(String ^name, FunType9 ^func, bool allowOpt);
		void DefineFun(String ^name, FunType10 ^func, bool allowOpt);

		void DefineFun(String ^name, BulkFunType0 ^func, bool allowOpt);
		void DefineFun(String ^name, BulkFunType1 ^func, bool allowOpt);
		void DefineFun(String ^name, BulkFunType2 ^func, bool allowOpt);
		void DefineFun(String ^name, BulkFunType3 ^func, bool allowOpt);
		void DefineFun(String ^name, BulkFunType4 ^func, bool allowOpt);
		void DefineFun(String ^name, BulkFunType5 ^func, bool allowOpt);
		void DefineFun(String ^name, BulkFunType6 ^func, bool allowOpt);
		void DefineFun(String ^name, BulkFunType7 ^func, bool allowOpt);
		void DefineFun(String ^name, BulkFunType8 ^func, bool allowOpt);
		void DefineFun(String ^name, BulkFunType9 ^func, bool allowOpt);
		void DefineFun(String ^name, BulkFunType10 ^func, bool allowOpt);

		void DefineFun(String ^name, MultFunType ^func, bool allowOpt);

		void DefineFun(String ^name, StrFunType1 ^func, bool allowOpt);
		void DefineFun(String ^name, StrFunType2 ^func, bool allowOpt);
		void DefineFun(String ^name, StrFunType3 ^func, bool allowOpt);

		//void SetVarFactory(FactoryFunction ^func, Object ^userData);

		double Eval();
		array<double> ^EvalBulk(int bulkSize);
		array<double> ^EvalMulti();
	};

}