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

#include "ErrorCodes.h"

using namespace System;

namespace muParserNET
{
	/// <summary>
	/// Error class of the parser.
	/// </summary>
	public ref class ParserError
		: public Exception
	{
	public:
		/// <summary>
		/// Gets or sets the expression with error.
		/// </summary>
		property String ^Expr;

		/// <summary>
		/// Gets or sets the invalid token.
		/// </summary>
		property String ^Token;

		/// <summary>
		/// Gets or sets the position of the error in expression.
		/// </summary>
		property int Pos;

		/// <summary>
		/// Gets or sets the error code.
		/// </summary>
		property ErrorCodes Code;

	public:
		/// <summary>
		/// Class constructor.
		/// </summary>
		ParserError();

		/// <summary>
		/// Class constructor. Initialize the object properties with the native
		/// exception thrown by the muParser library.
		/// </summary>
		/// <param name="err">The muParser exception</param>
		explicit ParserError(mu::Parser::exception_type &err);

		/// <summary>
		/// Class destructor.
		/// </summary>
		virtual ~ParserError();
	};

}

