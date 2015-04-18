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