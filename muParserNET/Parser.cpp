#include "stdafx.h"

#include "Parser.h"

#include "ParserError.h"

namespace muParserNET
{

	String ^Parser::Expr::get()
	{
		return gcnew String(this->parser->GetExpr().c_str());
	}

	void Parser::Expr::set(String ^value)
	{
		// converte para o tipo string da std para ajustar no parser
		mu::string_type str = msclr::interop::marshal_as<mu::string_type>(value);
		this->parser->SetExpr(str);
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

	double Parser::Eval()
	{
		try
		{
			return this->parser->Eval();
		}
		catch (mu::Parser::exception_type &e)
		{
			// lança a exceção do .NET
			throw gcnew ParserError(e);
		}
	}

}
