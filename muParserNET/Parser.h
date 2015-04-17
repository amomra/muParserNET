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

	public:
		Parser();
		virtual ~Parser();

		double Eval();
		array<double> ^EvalMulti();
	};

}