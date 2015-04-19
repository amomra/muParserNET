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

#include "stdafx.h"

#include "Parser.h"

#define __DEFINE_FUN_IMPL(funType) \
	void Parser::DefineFun(String ^name, funType ^func, bool allowOpt) { this->IntDefineFun(name, func, allowOpt); }

namespace muParserNET
{
	/*
	 * Todas as funções fazem a chamada da função genérica.
	 */
	__DEFINE_FUN_IMPL(FunType0)
	__DEFINE_FUN_IMPL(FunType1)
	__DEFINE_FUN_IMPL(FunType2)
	__DEFINE_FUN_IMPL(FunType3)
	__DEFINE_FUN_IMPL(FunType4)
	__DEFINE_FUN_IMPL(FunType5)
	__DEFINE_FUN_IMPL(FunType6)
	__DEFINE_FUN_IMPL(FunType7)
	__DEFINE_FUN_IMPL(FunType8)
	__DEFINE_FUN_IMPL(FunType9)
	__DEFINE_FUN_IMPL(FunType10)

	__DEFINE_FUN_IMPL(BulkFunType0)
	__DEFINE_FUN_IMPL(BulkFunType1)
	__DEFINE_FUN_IMPL(BulkFunType2)
	__DEFINE_FUN_IMPL(BulkFunType3)
	__DEFINE_FUN_IMPL(BulkFunType4)
	__DEFINE_FUN_IMPL(BulkFunType5)
	__DEFINE_FUN_IMPL(BulkFunType6)
	__DEFINE_FUN_IMPL(BulkFunType7)
	__DEFINE_FUN_IMPL(BulkFunType8)
	__DEFINE_FUN_IMPL(BulkFunType9)
	__DEFINE_FUN_IMPL(BulkFunType10)

	__DEFINE_FUN_IMPL(MultFunType)

	__DEFINE_FUN_IMPL(StrFunType1)
	__DEFINE_FUN_IMPL(StrFunType2)
	__DEFINE_FUN_IMPL(StrFunType3)
}