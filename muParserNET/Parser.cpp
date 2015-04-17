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

		// inicializa o dicion�rio com as vari�veis
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
			/*
			 * Apenas atualiza o valor da vari�vel se ela j� estiver definida.
			 */
			ParserVariable ^parserVar = nullptr;

			if (this->vars->ContainsKey(name))
			{
				parserVar = this->vars[name];
				parserVar->Value = var;
			}
			else
			{
				// cria e ajusta a vari�vel no muParser
				parserVar = gcnew ParserVariable(name, var);


				mu::string_type strName = msclr::interop::marshal_as<mu::string_type>(name);

				// ajusta a vari�vel
				this->parser->DefineVar(strName, (double *)parserVar->Pointer.ToPointer());
			}

			return parserVar;
		}
		catch (mu::Parser::exception_type &e)
		{
			// lan�a a exce��o do .NET
			throw gcnew ParserError(e);
		}
	}

	ParserVariable ^Parser::DefineVar(String ^name, array<double> ^var)
	{
		try
		{
			/*
			 * Diferente do m�todo que define uma vari�vel com apenas um valor,
			 * este m�todo envolve em modificar o array referenciado por este
			 * objeto, o que faria com que o ponteiro fornecido ao muParser
			 * anteriormente seja invalidado. Para evitar problemas, a vari�vel
			 * ser� recriada.
			 */
			ParserVariable ^parserVar = gcnew ParserVariable(name, var);

			mu::string_type strName = msclr::interop::marshal_as<mu::string_type>(name);

			// ajusta a vari�vel
			this->parser->DefineVar(strName, (double *)parserVar->Pointer.ToPointer());

			return parserVar;
		}
		catch (mu::Parser::exception_type &e)
		{
			// lan�a a exce��o do .NET
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
			// lan�a a exce��o do .NET
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
			// lan�a a exce��o do .NET
			throw gcnew ParserError(e);
		}
	}

	array<double> ^Parser::EvalMulti()
	{
		/*
		 * O vetor retornado pela fun��o Eval � gerenciado pelo pr�prio parser,
		 * n�o sendo necess�rio desaloc�-lo.
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
			// lan�a a exce��o do .NET
			throw gcnew ParserError(e);
		}
	}

}
