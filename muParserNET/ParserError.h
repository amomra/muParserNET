#pragma once

#include "ErrorCodes.h"

using namespace System;

namespace muParserNET
{

	public ref class ParserError
		: public Exception
	{
	public:
		property String ^Expr;
		property String ^Token;
		property int Pos;
		property ErrorCodes Code;

	public:
		ParserError();
		explicit ParserError(mu::Parser::exception_type &err);
		virtual ~ParserError();
	};

}

