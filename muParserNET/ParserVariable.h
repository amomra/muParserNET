#pragma once

using namespace System;
using namespace System::Runtime::InteropServices;

namespace muParserNET
{
	public ref class ParserVariable
	{
	private:
		String ^name;
		array<double> ^valueArray;

		// este ponteiro é necessário para evitar que o GC mova o array
		GCHandle ptrValueArray;

	public:
		property String ^Name
		{
			String ^get();
		}

		property double Value
		{
			double get();
			void set(double value);
		}

		property array<double> ^ValueArray
		{
			array<double> ^get();
		}

		property IntPtr Pointer
		{
			IntPtr get();
		}

	public:
		ParserVariable(String ^name, double value);
		ParserVariable(String ^name, array<double> ^valueArray);
		~ParserVariable();
	};

}
