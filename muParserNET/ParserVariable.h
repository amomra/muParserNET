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

This software uses and contains parts copied from muParser library.
muParser library - Copyright (C) 2013 Ingo Berg
*/

#pragma once

using namespace System;
using namespace System::Runtime::InteropServices;

namespace muParserNET
{
	/// <summary>
	/// Class of the parser variable.
	/// </summary>
	public ref class ParserVariable
	{
	private:
		String ^name;
		array<double> ^valueArray;

		// este ponteiro é necessário para evitar que o GC mova o array
		GCHandle ptrValueArray;

	public:
		/// <summary>
		/// Gets the variable name.
		/// </summary>
		property String ^Name
		{
			String ^get();
		}

		/// <summary>
		/// Gets os sets the variable value. If the variable is an array, it
		/// gets or sets the first value.
		/// </summary>
		property double Value
		{
			double get();
			void set(double value);
		}

		/// <summary>
		/// Gets os sets the variable value as an array. If the variable isn't an array, it
		/// gets the value as an array with a single value.
		/// </summary>
		property array<double> ^ValueArray
		{
			array<double> ^get();
		}

		/// <summary>
		/// Gets the variable pointer. This will be used to create variables in
		/// muParser structure.
		/// </summary>
		property IntPtr Pointer
		{
			IntPtr get();
		}

	public:
		/// <summary>
		/// Class constructor. It creates a variable with a single default
		/// value.
		/// </summary>
		/// <param name="name">The variable name</param>
		ParserVariable(String ^name);

		/// <summary>
		/// Class constructor. It creates a variable with a single value.
		/// </summary>
		/// <param name="name">The variable name</param>
		/// <param name="value">The variable initial value</param>
		ParserVariable(String ^name, double value);

		/// <summary>
		/// Class constructor. It creates a variable with multiples values.
		/// </summary>
		/// <param name="name">The variable name</param>
		/// <param name="valueArray">The array with the variable values. The
		/// array's memory will be pinned to avoid the garbage collector to move
		/// it and causing memory problems</param>
		ParserVariable(String ^name, array<double> ^valueArray);

		/// <summary>
		/// Class destructor.
		/// </summary>
		~ParserVariable();
	};

}
