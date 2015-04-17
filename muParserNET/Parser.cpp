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

	IReadOnlyDictionary<String ^, ParserVariable ^> ^Parser::Vars::get()
	{
		/*
		 * A princípio não é para ter variáveis a mais no parser do que tem
		 * nesta lista.
		 */
		return this->vars;
	}

	Parser::Parser()
	{
		// inicializa o parser
		this->parser = new mu::Parser();

		// inicializa o dicionário com as variáveis
		this->vars = gcnew Dictionary<String ^, ParserVariable ^>();
	}

	Parser::~Parser()
	{
		// finaliza o parser
		delete this->parser;
	}

	ParserVariable ^Parser::DefineVar(String ^name, double var)
	{
		try
		{
			// cria a variável
			ParserVariable ^parserVar = gcnew ParserVariable(name, var);

			mu::string_type strName = msclr::interop::marshal_as<mu::string_type>(name);

			// ajusta a variável
			this->parser->DefineVar(strName, (double *)parserVar->Pointer.ToPointer());

			// adiciona a variável na lista de variáveis
			this->vars[name] = parserVar;

			return parserVar;
		}
		catch (mu::Parser::exception_type &e)
		{
			// lança a exceção do .NET
			throw gcnew ParserError(e);
		}
	}

	ParserVariable ^Parser::DefineVar(String ^name, array<double> ^var)
	{
		try
		{
			// recria a variável
			ParserVariable ^parserVar = gcnew ParserVariable(name, var);

			mu::string_type strName = msclr::interop::marshal_as<mu::string_type>(name);

			// ajusta a variável
			this->parser->DefineVar(strName, (double *)parserVar->Pointer.ToPointer());

			// adiciona a variável na lista de variáveis
			this->vars[name] = parserVar;

			return parserVar;
		}
		catch (mu::Parser::exception_type &e)
		{
			// lança a exceção do .NET
			throw gcnew ParserError(e);
		}
	}

	void Parser::RemoveVar(String ^name)
	{
		mu::string_type strName = msclr::interop::marshal_as<mu::string_type>(name);

		// remove a variável
		this->parser->RemoveVar(strName);

		// e remove a variável da lista interna
		this->vars->Remove(name);
	}

	void Parser::ClearVar()
	{
		// remove todas as variáveis
		this->parser->ClearVar();
		this->vars->Clear();
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

			// pega o ponteiro (e evita do GC mover o objeto enquanto a função estiver em execução)
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
