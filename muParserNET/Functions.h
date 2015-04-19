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

using namespace System;
using namespace System::Runtime::InteropServices;

namespace muParserNET
{
	/*
	 * Definição dos ponteiros para função que serão passado para o muParser.
	 */

	//	delegate ParserVariable ^FactoryFunction(String ^name, Object ^userData);

	[UnmanagedFunctionPointer(CallingConvention::Cdecl)]
	public delegate double FunType0();

	/** \brief Callback type used for functions with a single arguments. */
	[UnmanagedFunctionPointer(CallingConvention::Cdecl)]
	public delegate double FunType1(
		double arg0);

	/** \brief Callback type used for functions with two arguments. */
	[UnmanagedFunctionPointer(CallingConvention::Cdecl)]
	public delegate double FunType2(
		double arg0,
		double arg1);

	/** \brief Callback type used for functions with three arguments. */
	[UnmanagedFunctionPointer(CallingConvention::Cdecl)]
	public delegate double FunType3(
		double arg0,
		double arg1,
		double arg2);

	/** \brief Callback type used for functions with four arguments. */
	[UnmanagedFunctionPointer(CallingConvention::Cdecl)]
	public delegate double FunType4(
		double arg0,
		double arg1,
		double arg2,
		double arg3);

	/** \brief Callback type used for functions with five arguments. */
	[UnmanagedFunctionPointer(CallingConvention::Cdecl)]
	public delegate double FunType5(
		double arg0,
		double arg1,
		double arg2,
		double arg3,
		double arg4);

	/** \brief Callback type used for functions with five arguments. */
	[UnmanagedFunctionPointer(CallingConvention::Cdecl)]
	public delegate double FunType6(
		double arg0,
		double arg1,
		double arg2,
		double arg3,
		double arg4,
		double arg5);

	/** \brief Callback type used for functions with five arguments. */
	[UnmanagedFunctionPointer(CallingConvention::Cdecl)]
	public delegate double FunType7(
		double arg0,
		double arg1,
		double arg2,
		double arg3,
		double arg4,
		double arg5,
		double arg6);

	/** \brief Callback type used for functions with five arguments. */
	[UnmanagedFunctionPointer(CallingConvention::Cdecl)]
	public delegate double FunType8(
		double arg0,
		double arg1,
		double arg2,
		double arg3,
		double arg4,
		double arg5,
		double arg6,
		double arg7);

	/** \brief Callback type used for functions with five arguments. */
	[UnmanagedFunctionPointer(CallingConvention::Cdecl)]
	public delegate double FunType9(
		double arg0,
		double arg1,
		double arg2,
		double arg3,
		double arg4,
		double arg5,
		double arg6,
		double arg7,
		double arg8);

	/** \brief Callback type used for functions with five arguments. */
	[UnmanagedFunctionPointer(CallingConvention::Cdecl)]
	public delegate double FunType10(
		double arg0,
		double arg1,
		double arg2,
		double arg3,
		double arg4,
		double arg5,
		double arg6,
		double arg7,
		double arg8,
		double arg9);

	/** \brief Callback type used for functions without arguments. */
	[UnmanagedFunctionPointer(CallingConvention::Cdecl)]
	public delegate double BulkFunType0(int bulkIndex, int threadIndex);

	/** \brief Callback type used for functions with a single arguments. */
	[UnmanagedFunctionPointer(CallingConvention::Cdecl)]
	public delegate double BulkFunType1(int bulkIndex, int threadIndex,
		double arg0);

	/** \brief Callback type used for functions with two arguments. */
	[UnmanagedFunctionPointer(CallingConvention::Cdecl)]
	public delegate double BulkFunType2(int bulkIndex, int threadIndex,
		double arg0,
		double arg1);

	/** \brief Callback type used for functions with three arguments. */
	[UnmanagedFunctionPointer(CallingConvention::Cdecl)]
	public delegate double BulkFunType3(int bulkIndex, int threadIndex,
		double arg0,
		double arg1,
		double arg2);

	/** \brief Callback type used for functions with four arguments. */
	[UnmanagedFunctionPointer(CallingConvention::Cdecl)]
	public delegate double BulkFunType4(int bulkIndex, int threadIndex,
		double arg0,
		double arg1,
		double arg2,
		double arg3);

	/** \brief Callback type used for functions with five arguments. */
	[UnmanagedFunctionPointer(CallingConvention::Cdecl)]
	public delegate double BulkFunType5(int bulkIndex, int threadIndex,
		double arg0,
		double arg1,
		double arg2,
		double arg3,
		double arg4);

	/** \brief Callback type used for functions with five arguments. */
	[UnmanagedFunctionPointer(CallingConvention::Cdecl)]
	public delegate double BulkFunType6(int bulkIndex, int threadIndex,
		double arg0,
		double arg1,
		double arg2,
		double arg3,
		double arg4,
		double arg5);

	/** \brief Callback type used for functions with five arguments. */
	[UnmanagedFunctionPointer(CallingConvention::Cdecl)]
	public delegate double BulkFunType7(int bulkIndex, int threadIndex,
		double arg0,
		double arg1,
		double arg2,
		double arg3,
		double arg4,
		double arg5,
		double arg6);

	/** \brief Callback type used for functions with five arguments. */
	[UnmanagedFunctionPointer(CallingConvention::Cdecl)]
	public delegate double BulkFunType8(int bulkIndex, int threadIndex,
		double arg0,
		double arg1,
		double arg2,
		double arg3,
		double arg4,
		double arg5,
		double arg6,
		double arg7);

	/** \brief Callback type used for functions with five arguments. */
	[UnmanagedFunctionPointer(CallingConvention::Cdecl)]
	public delegate double BulkFunType9(int bulkIndex, int threadIndex,
		double arg0,
		double arg1,
		double arg2,
		double arg3,
		double arg4,
		double arg5,
		double arg6,
		double arg7,
		double arg8);

	/** \brief Callback type used for functions with five arguments. */
	[UnmanagedFunctionPointer(CallingConvention::Cdecl)]
	public delegate double BulkFunType10(int bulkIndex, int threadIndex,
		double arg0,
		double arg1,
		double arg2,
		double arg3,
		double arg4,
		double arg5,
		double arg6,
		double arg7,
		double arg8,
		double arg9);

	/** \brief Callback type used for functions with a variable argument list. */
	[UnmanagedFunctionPointer(CallingConvention::Cdecl)]
	public delegate double MultFunType([MarshalAs(UnmanagedType::LPArray, SizeParamIndex = 1)] array<double> ^args, int size);

	/** \brief Callback type used for functions taking a string as an argument. */
	[UnmanagedFunctionPointer(CallingConvention::Cdecl)]
	public delegate double StrFunType1([MarshalAs(UnmanagedType::LPWStr)] String ^str);

	/** \brief Callback type used for functions taking a string and a value as arguments. */
	[UnmanagedFunctionPointer(CallingConvention::Cdecl)]
	public delegate double StrFunType2([MarshalAs(UnmanagedType::LPWStr)] String ^str, double arg0);

	/** \brief Callback type used for functions taking a string and two values as arguments. */
	[UnmanagedFunctionPointer(CallingConvention::Cdecl)]
	public delegate double StrFunType3([MarshalAs(UnmanagedType::LPWStr)] String ^str, double arg0, double arg1);

	/** \brief Callback used for functions that identify values in a string. */
	[UnmanagedFunctionPointer(CallingConvention::Cdecl)]
	public delegate bool IdentFunction([MarshalAs(UnmanagedType::LPWStr)] String ^remainingExpr, int %pos, double %value);

	namespace Utils
	{
		template <typename FuncType>
		public value struct FunctionType
		{
			/*
			 * As especializações desta estrutura irão conter os tipos de funções
			 * definidos pelo muParser para cada um dos delegates.
			 */
		};

		template <> public value struct FunctionType < FunType0 > { typedef mu::fun_type0 Value; };
		template <> public value struct FunctionType < FunType1 > { typedef mu::fun_type1 Value; };
		template <> public value struct FunctionType < FunType2 > { typedef mu::fun_type2 Value; };
		template <> public value struct FunctionType < FunType3 > { typedef mu::fun_type3 Value; };
		template <> public value struct FunctionType < FunType4 > { typedef mu::fun_type4 Value; };
		template <> public value struct FunctionType < FunType5 > { typedef mu::fun_type5 Value; };
		template <> public value struct FunctionType < FunType6 > { typedef mu::fun_type6 Value; };
		template <> public value struct FunctionType < FunType7 > { typedef mu::fun_type7 Value; };
		template <> public value struct FunctionType < FunType8 > { typedef mu::fun_type8 Value; };
		template <> public value struct FunctionType < FunType9 > { typedef mu::fun_type9 Value; };
		template <> public value struct FunctionType < FunType10 > { typedef mu::fun_type10 Value; };

		template <> public value struct FunctionType < BulkFunType0 > { typedef mu::bulkfun_type0 Value; };
		template <> public value struct FunctionType < BulkFunType1 > { typedef mu::bulkfun_type1 Value; };
		template <> public value struct FunctionType < BulkFunType2 > { typedef mu::bulkfun_type2 Value; };
		template <> public value struct FunctionType < BulkFunType3 > { typedef mu::bulkfun_type3 Value; };
		template <> public value struct FunctionType < BulkFunType4 > { typedef mu::bulkfun_type4 Value; };
		template <> public value struct FunctionType < BulkFunType5 > { typedef mu::bulkfun_type5 Value; };
		template <> public value struct FunctionType < BulkFunType6 > { typedef mu::bulkfun_type6 Value; };
		template <> public value struct FunctionType < BulkFunType7 > { typedef mu::bulkfun_type7 Value; };
		template <> public value struct FunctionType < BulkFunType8 > { typedef mu::bulkfun_type8 Value; };
		template <> public value struct FunctionType < BulkFunType9 > { typedef mu::bulkfun_type9 Value; };
		template <> public value struct FunctionType < BulkFunType10 > { typedef mu::bulkfun_type10 Value; };

		template <> public value struct FunctionType < MultFunType > { typedef mu::multfun_type Value; };

		template <> public value struct FunctionType < StrFunType1 > { typedef mu::strfun_type1 Value; };
		template <> public value struct FunctionType < StrFunType2 > { typedef mu::strfun_type2 Value; };
		template <> public value struct FunctionType < StrFunType3 > { typedef mu::strfun_type3 Value; };
	}
}