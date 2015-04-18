#pragma once

using namespace System;
using namespace System::Runtime::InteropServices;

namespace muParserNET
{
	public ref class ParserCallback
	{
	private:
		Delegate ^func;

		/*
		 * Quando a fun��o passa a ser um callback da biblioteca o objeto do
		 * delegate n�o deve ser apagado pelo GC.
		 */
		GCHandle ptrFunc;
	public:
		property Delegate ^Function
		{
			Delegate ^get();
		}

		property IntPtr Pointer
		{
			IntPtr get();
		}

	public:
		ParserCallback(Delegate ^func);
		virtual ~ParserCallback();
	};
}
