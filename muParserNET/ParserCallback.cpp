#include "stdafx.h"
#include "ParserCallback.h"

namespace muParserNET
{
	Delegate ^ParserCallback::Function::get()
	{
		return this->func;
	}

	IntPtr ParserCallback::Pointer::get()
	{
		// gera o ponteiro
		return Marshal::GetFunctionPointerForDelegate(this->func);
	}

	ParserCallback::ParserCallback(Delegate ^func)
		: func(func)
	{
		// bloqueia o GC de apagar o objeto do delegate
		this->ptrFunc = GCHandle::Alloc(func);
	}

	ParserCallback::~ParserCallback()
	{
		// libera o GC para apagar o objeto
		this->ptrFunc.Free();
	}
}
