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

	IReadOnlyDictionary<String ^, ParserVariable ^> ^Parser::Vars::get()
	{
		/*
		 * A princípio não é para ter variáveis a mais no parser do que tem
		 * nesta lista.
		 */
		return this->vars;
	}

	IReadOnlyDictionary<String ^, double> ^Parser::Consts::get()
	{
		// converte o map do parser
		const mu::valmap_type &consts = this->parser->GetConst();

		Dictionary<String ^, double> ^res = gcnew Dictionary<String ^, double>();

		for (mu::valmap_type::const_iterator i = consts.cbegin(); i != consts.end(); i++)
			res->Add(gcnew String(i->first.c_str()), i->second);

		return res;
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
