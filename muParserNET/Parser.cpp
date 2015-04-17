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

	String ^Parser::NameChars::get()
	{
		return gcnew String(this->parser->ValidNameChars());
	}

	void Parser::NameChars::set(String ^value)
	{
		// converte para o tipo string da std para ajustar no parser
		mu::string_type str = msclr::interop::marshal_as<mu::string_type>(value);
		this->parser->DefineNameChars(str.c_str());
	}

	String ^Parser::OprtChars::get()
	{
		return gcnew String(this->parser->ValidOprtChars());
	}

	void Parser::OprtChars::set(String ^value)
	{
		// converte para o tipo string da std para ajustar no parser
		mu::string_type str = msclr::interop::marshal_as<mu::string_type>(value);
		this->parser->DefineOprtChars(str.c_str());
	}

	String ^Parser::InfixOprtChars::get()
	{
		return gcnew String(this->parser->ValidInfixOprtChars());
	}

	void Parser::InfixOprtChars::set(String ^value)
	{
		// converte para o tipo string da std para ajustar no parser
		mu::string_type str = msclr::interop::marshal_as<mu::string_type>(value);
		this->parser->DefineInfixOprtChars(str.c_str());
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

	void Parser::DefineVar(String ^name, double %var)
	{
		try
		{
			mu::string_type strName = msclr::interop::marshal_as<mu::string_type>(name);

			// converte o ponteiro
			pin_ptr<double> ptrVar = &var;

			// ajusta a variável
			this->parser->DefineVar(strName, ptrVar);
		}
		catch (mu::Parser::exception_type &e)
		{
			// lança a exceção do .NET
			throw gcnew ParserError(e);
		}
	}

	void Parser::DefineVar(String ^name, array<double> ^var)
	{
		try
		{
			mu::string_type strName = msclr::interop::marshal_as<mu::string_type>(name);

			// converte o ponteiro
			pin_ptr<double> ptrVar = &var[0];

			// ajusta a variável
			this->parser->DefineVar(strName, ptrVar);
		}
		catch (mu::Parser::exception_type &e)
		{
			// lança a exceção do .NET
			throw gcnew ParserError(e);
		}
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

	array<double> ^Parser::EvalBulk(int bulkSize)
	{
		try
		{
			// aloca o vetor de resposta
			array<double> ^result = gcnew array<double>(bulkSize);

			// pega o ponteiro
			pin_ptr<double> ptrResult = &result[0];

			// executa o comando
			this->parser->Eval(ptrResult, bulkSize);

			return result;
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
