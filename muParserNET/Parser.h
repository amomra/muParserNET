#pragma once

using namespace System;
using namespace System::Runtime::InteropServices;

namespace muParserNET
{

	public ref class Parser
	{
	private:
		mu::Parser *parser;
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

	public:
		Parser();
		virtual ~Parser();

		void DefineVar(String ^name, double %var);

		double Eval();
		array<double> ^EvalMulti();
	};

}