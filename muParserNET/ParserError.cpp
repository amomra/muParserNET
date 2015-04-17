#include "stdafx.h"

#include "ParserError.h"

namespace muParserNET
{

	ParserError::ParserError()
		: Exception()
	{
		this->Code = ErrorCodes::UNDEFINED;
	}

	ParserError::ParserError(mu::Parser::exception_type &err)
		: Exception(gcnew String(err.GetMsg().c_str()))
	{
		this->Expr = gcnew String(err.GetExpr().c_str());
		this->Token = gcnew String(err.GetMsg().c_str());
		this->Pos = err.GetPos();
		this->Code = (ErrorCodes)err.GetCode();
	}

	ParserError::~ParserError()
	{
	}

}