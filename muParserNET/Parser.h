#pragma once

#include "ParserVariable.h"

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

		property Dictionary<String ^, ParserVariable ^> ^Vars
		{
			Dictionary<String ^, ParserVariable ^> ^get();
		}

	public:
		Parser();
		virtual ~Parser();

		ParserVariable ^DefineVar(String ^name, double var);
		ParserVariable ^DefineVar(String ^name, array<double> ^var);

		double Eval();
		array<double> ^EvalBulk(int bulkSize);
		array<double> ^EvalMulti();
	};

}