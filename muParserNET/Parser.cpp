#include "stdafx.h"
#include "Parser.h"

namespace muParserNET
{

	String^ Parser::Expr::get()
	{
		return "";
	}

	void Parser::Expr::set(String^ value)
	{
	}

	Parser::Parser()
	{
		// inicializa o parser
		this->parser = new mu::Parser();
	}

	Parser::~Parser()
	{
		// finaliza o parser
		delete this->parser;
	}

}
