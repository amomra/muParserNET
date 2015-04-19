/*
muParserNET - muParser library wrapper for .NET Framework

Copyright (c) 2015 Luiz Carlos Viana Melo

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.

This software contains parts copied from muParser library.
muParser library - Copyright (C) 2013 Ingo Berg
*/

#include "stdafx.h"

#include "Parser.h"

namespace muParserNET
{
	/*
	void Parser::AllocFactoryObjects(FactoryFunction ^factoryFunc, Object ^userData)
	{
		this->ptrFactoryFunction = GCHandle::Alloc(factoryFunc, GCHandleType::Pinned);
		this->ptrUserData = GCHandle::Alloc(userData, GCHandleType::Pinned);
	}

	void Parser::FreeFactoryObjects()
	{
		if (this->ptrFactoryFunction.IsAllocated)
			this->ptrFactoryFunction.Free();

		if (this->ptrUserData.IsAllocated)
			this->ptrUserData.Free();
	}*/

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

	Dictionary<String ^, ParserVariable ^> ^Parser::Vars::get()
	{
		/*
		 * A princípio não é para ter variáveis a mais no parser do que tem
		 * nesta lista.
		 */
		return this->vars;
	}

	Dictionary<String ^, double> ^Parser::Consts::get()
	{
		// converte o map do parser
		const mu::valmap_type &consts = this->parser->GetConst();

		Dictionary<String ^, double> ^res = gcnew Dictionary<String ^, double>();

		for (mu::valmap_type::const_iterator i = consts.cbegin(); i != consts.end(); i++)
			res->Add(gcnew String(i->first.c_str()), i->second);

		return res;
	}

	Dictionary<String ^, ParserCallback ^> ^Parser::Functions::get()
	{
		return this->funcCallbacks;
	}

	List<String ^> ^Parser::BuiltInOprts::get()
	{
		/*
		 * A lista de operadores do muParser é composta por string onde o último
		 * elemento terá um nullptr para indicar o fim da lista.
		 */
		List<String ^> ^oprts = gcnew List<String ^>();

		const mu::char_type **dfOprts = this->parser->GetOprtDef();

		for (int i = 0; dfOprts[i] != nullptr; i++)
			oprts->Add(gcnew String(dfOprts[i]));

		return oprts;
	}

	Dictionary<String ^, ParserCallback ^> ^Parser::InfixOprts::get()
	{
		return this->infixOprtCallbacks;
	}

	Dictionary<String ^, ParserCallback ^> ^Parser::PostfixOprts::get()
	{
		return this->postfixOprtCallbacks;
	}

	Dictionary<String ^, ParserCallback ^> ^Parser::Oprts::get()
	{
		return this->oprtCallbacks;
	}

	Parser::Parser()
	{
		// inicializa o parser
		this->parser = new mu::Parser();

		// inicializa o dicionário com as variáveis
		this->vars = gcnew Dictionary<String ^, ParserVariable ^>();

		// inicializa as listas de delegates
		this->identFunctionsCallbacks = gcnew List<ParserCallback ^>();
		this->funcCallbacks = gcnew Dictionary<String ^, ParserCallback ^>();

		this->infixOprtCallbacks = gcnew Dictionary<String ^, ParserCallback ^>();
		this->postfixOprtCallbacks = gcnew Dictionary<String ^, ParserCallback ^>();
		this->oprtCallbacks = gcnew Dictionary<String ^, ParserCallback ^>();
	}

	Parser::~Parser()
	{
		// libera os objetos do factory
		//this->FreeFactoryObjects();

		// libera os objetos dos delegates
		for (int i = 0; i < this->identFunctionsCallbacks->Count; i++)
			delete this->identFunctionsCallbacks[i];
		for each (KeyValuePair<String ^, ParserCallback ^> funcCallback in this->funcCallbacks)
			delete funcCallback.Value;

		for each (KeyValuePair<String ^, ParserCallback ^> infixOprtCallback in this->infixOprtCallbacks)
			delete infixOprtCallback.Value;
		for each (KeyValuePair<String ^, ParserCallback ^> postfixOprtCallback in this->postfixOprtCallbacks)
			delete postfixOprtCallback.Value;
		for each (KeyValuePair<String ^, ParserCallback ^> oprtCallback in this->oprtCallbacks)
			delete oprtCallback.Value;

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

	void Parser::DefineConst(String ^name, double value)
	{
		mu::string_type strName = msclr::interop::marshal_as<mu::string_type>(name);

		this->parser->DefineConst(strName, value);
	}

	void Parser::DefineStrConst(String ^name, String ^value)
	{
		mu::string_type strName = msclr::interop::marshal_as<mu::string_type>(name);
		mu::string_type strValue = msclr::interop::marshal_as<mu::string_type>(value);

		this->parser->DefineStrConst(strName, strValue);
	}

	void Parser::ClearConst()
	{
		this->parser->ClearConst();
	}

	void Parser::AddValIdent(IdentFunction ^identFunction)
	{
		// bloqueia o GC de mover o delegate
		ParserCallback ^callback = gcnew ParserCallback(identFunction);

		// passa a chamada
		this->parser->AddValIdent(static_cast<mu::identfun_type>(callback->Pointer.ToPointer()));

		this->identFunctionsCallbacks->Add(callback);
	}

	void Parser::ClearFun()
	{
		this->parser->ClearFun();
		this->funcCallbacks->Clear();
	}

	void Parser::DefineInfixOprt(String ^identifier, FunType1 ^func, unsigned precedence, bool allowOpt)
	{
		try
		{
			mu::string_type strIdentifier = msclr::interop::marshal_as<mu::string_type>(identifier);

			// bloqueia o GC de mover o delegate
			ParserCallback ^callback = gcnew ParserCallback(func);

			this->parser->DefineInfixOprt(
				strIdentifier,
				static_cast<mu::fun_type1>(callback->Pointer.ToPointer()),
				precedence,
				allowOpt);

			this->infixOprtCallbacks->Add(identifier, callback);
		}
		catch (mu::Parser::exception_type &e)
		{
			// lança a exceção do .NET
			throw gcnew ParserError(e);
		}
	}

	void Parser::DefinePostfixOprt(String ^identifier, FunType1 ^func, bool allowOpt)
	{
		try
		{
			mu::string_type strIdentifier = msclr::interop::marshal_as<mu::string_type>(identifier);

			// bloqueia o GC de mover o delegate
			ParserCallback ^callback = gcnew ParserCallback(func);

			this->parser->DefinePostfixOprt(
				strIdentifier,
				static_cast<mu::fun_type1>(callback->Pointer.ToPointer()),
				allowOpt);

			this->postfixOprtCallbacks->Add(identifier, callback);
		}
		catch (mu::Parser::exception_type &e)
		{
			// lança a exceção do .NET
			throw gcnew ParserError(e);
		}
	}

	void Parser::DefineOprt(String ^identifier, FunType2 ^func, unsigned precedence, OprtAssociativity associativity, bool allowOpt)
	{
		try
		{
			mu::string_type strIdentifier = msclr::interop::marshal_as<mu::string_type>(identifier);

			// bloqueia o GC de mover o delegate
			ParserCallback ^callback = gcnew ParserCallback(func);

			this->parser->DefineOprt(
				strIdentifier,
				static_cast<mu::fun_type2>(callback->Pointer.ToPointer()),
				precedence,
				(mu::EOprtAssociativity)associativity,
				allowOpt);

			this->oprtCallbacks->Add(identifier, callback);
		}
		catch (mu::Parser::exception_type &e)
		{
			// lança a exceção do .NET
			throw gcnew ParserError(e);
		}
	}

	void Parser::CleanInfixOprt()
	{
		this->parser->ClearInfixOprt();
		this->infixOprtCallbacks->Clear();
	}

	void Parser::CleanPostfixOprt()
	{
		this->parser->ClearPostfixOprt();
		this->postfixOprtCallbacks->Clear();
	}

	void Parser::CleanOprt()
	{
		this->parser->ClearOprt();
		this->oprtCallbacks->Clear();
	}

	void Parser::EnableBuiltInOprt(bool oprtEn)
	{
		this->parser->EnableBuiltInOprt(oprtEn);
	}

	void Parser::SetArgSep(wchar_t value)
	{
		this->parser->SetArgSep(value);
	}

	void Parser::SetDecSep(wchar_t value)
	{
		this->parser->SetDecSep(value);
	}

	void Parser::SetThousandsSep(wchar_t value)
	{
		this->parser->SetThousandsSep(value);
	}

	void Parser::ResetLocale()
	{
		this->parser->ResetLocale();
	}

	/* // DEIXAR POR ÚLTIMO!!!

	double *__VariableFactoryCallback(const mu::char_type *a_szName, void *pUserData)
	{

	}

	void Parser::SetVarFactory(FactoryFunction ^func, Object ^userData)
	{
		// libera os objetos anterior
		this->FreeFactoryObjects();

		// ajusta e bloqueia o GC de mover os objetos do delegate e userdata
		this->AllocFactoryObjects(func, userData);

		this->parser->SetVarFactory(__VariableFactoryCallback, this->ptrUserData);
	}
	*/

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
