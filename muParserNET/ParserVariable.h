#pragma once

using namespace System;

namespace muParserNET
{
	public ref class ParserVariable
	{
	private:
		String ^name;
		array<double> ^valueArray;

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
