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

	array<double> ^Parser::EvalMulti()
	{
		/*
		 * O vetor retornado pela função Eval é gerenciado pelo próprio parser,
		 * não sendo necessário desalocá-lo.
		 */
		try
		{
			int n = 0;
			mu::value_type *result = this->parser->Eval(n);

			// aloca o array de retorno do .net
			array<double> ^ret = gcnew array<double>(n);

			// copia o resultado
			IntPtr ptrResult = IntPtr(result);

			Marshal::Copy(ptrResult, ret, 0, n);
			return ret;
		}
		catch (mu::Parser::exception_type &e)
		{
			// lança a exceção do .NET
			throw gcnew ParserError(e);
		}
	}

}
