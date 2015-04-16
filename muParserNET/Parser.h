#pragma once

using namespace System;

namespace muParserNET
{

	public ref class Parser
	{
	private:
		mu::Parser *parser;
	public:
		property String^ Expr
		{
			String^ get();
			void set(String^ value);
		}

	public:
		Parser();
		virtual ~Parser();
	};

}