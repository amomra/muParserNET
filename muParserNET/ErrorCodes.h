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

#pragma once

namespace muParserNET
{
	/** \brief Error codes. */
	public enum class ErrorCodes
		: int
	{
		// Formula syntax errors
		UNEXPECTED_OPERATOR = 0,  ///< Unexpected binary operator found
		UNASSIGNABLE_TOKEN = 1,  ///< Token cant be identified.
		UNEXPECTED_EOF = 2,  ///< Unexpected end of formula. (Example: "2+sin(")
		UNEXPECTED_ARG_SEP = 3,  ///< An unexpected comma has been found. (Example: "1,23")
		UNEXPECTED_ARG = 4,  ///< An unexpected argument has been found
		UNEXPECTED_VAL = 5,  ///< An unexpected value token has been found
		UNEXPECTED_VAR = 6,  ///< An unexpected variable token has been found
		UNEXPECTED_PARENS = 7,  ///< Unexpected Parenthesis, opening or closing
		UNEXPECTED_STR = 8,  ///< A string has been found at an inapropriate position
		STRING_EXPECTED = 9,  ///< A string function has been called with a different type of argument
		VAL_EXPECTED = 10, ///< A numerical function has been called with a non value type of argument
		MISSING_PARENS = 11, ///< Missing parens. (Example: "3*sin(3")
		UNEXPECTED_FUN = 12, ///< Unexpected function found. (Example: "sin(8)cos(9)")
		UNTERMINATED_STRING = 13, ///< unterminated string constant. (Example: "3*valueof("hello)")
		TOO_MANY_PARAMS = 14, ///< Too many function parameters
		TOO_FEW_PARAMS = 15, ///< Too few function parameters. (Example: "ite(1<2,2)")
		OPRT_TYPE_CONFLICT = 16, ///< binary operators may only be applied to value items of the same type
		STR_RESULT = 17, ///< result is a string

		// Invalid Parser input Parameters
		INVALID_NAME = 18, ///< Invalid function, variable or constant name.
		INVALID_BINOP_IDENT = 19, ///< Invalid binary operator identifier
		INVALID_INFIX_IDENT = 20, ///< Invalid function, variable or constant name.
		INVALID_POSTFIX_IDENT = 21, ///< Invalid function, variable or constant name.

		BUILTIN_OVERLOAD = 22, ///< Trying to overload builtin operator
		INVALID_FUN_PTR = 23, ///< Invalid callback function pointer 
		INVALID_VAR_PTR = 24, ///< Invalid variable pointer 
		EMPTY_EXPRESSION = 25, ///< The Expression is empty
		NAME_CONFLICT = 26, ///< Name conflict
		OPT_PRI = 27, ///< Invalid operator priority
		// 
		DOMAIN_ERROR = 28, ///< catch division by zero, sqrt(-1), log(0) (currently unused)
		DIV_BY_ZERO = 29, ///< Division by zero (currently unused)
		GENERIC = 30, ///< Generic error
		LOCALE = 31, ///< Conflict with current locale

		UNEXPECTED_CONDITIONAL = 32,
		MISSING_ELSE_CLAUSE = 33,
		MISPLACED_COLON = 34,

		// internal errors
		INTERNAL_ERROR = 35, ///< Internal error of any kind.

		// The last two are special entries 
		COUNT,                      ///< This is no error code, It just stores just the total number of error codes
		UNDEFINED = -1  ///< Undefined message, placeholder to detect unassigned error messages
	};

}