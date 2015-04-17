#pragma once

#include "ParserVariable.h"

using namespace System;
using namespace System::Collections::Generic;
using namespace System::Runtime::InteropServices;

namespace muParserNET
{

	public ref class Parser
	{
	public:
	//	delegate ParserVariable ^FactoryFunction(String ^name, Object ^userData);
		[UnmanagedFunctionPointerAttribute(CallingConvention::Cdecl)]
		delegate bool IdentFunction(String ^remainingExpr, int %pos, double %value);
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
		// Atributos de apoio as funções de identificação
		
		// armazena os delegates para eles não serem deletados
		List<GCHandle> ^identFunctionsPtrs;

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

		//void SetVarFactory(FactoryFunction ^func, Object ^userData);

		double Eval();
		array<double> ^EvalBulk(int bulkSize);
		array<double> ^EvalMulti();
	};

}