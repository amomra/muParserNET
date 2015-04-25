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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace muParserNET
{
    /*
     * Definição dos ponteiros para função que serão passado para o muParser.
     */

    /// <summary>
    /// Callback type used for handle parser errors.
    /// </summary>
    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl)]
    public delegate void ErrorFuncType();

    //	delegate ParserVariable ^FactoryFunction(String ^name, Object ^userData);

    /// <summary>
    /// Callback type used for functions without arguments.
    /// </summary>
    /// <returns>The function result</returns>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate double FunType0();

    /// <summary>
    /// Callback type used for functions with a single arguments.
    /// </summary>
    /// <param name="arg0">The first function argument</param>
    /// <returns>The function result</returns>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate double FunType1(
        double arg0);

    /// <summary>
    /// Callback type used for functions with two arguments.
    /// </summary>
    /// <param name="arg0">The first function argument</param>
    /// <param name="arg1">The second function argument</param>
    /// <returns>The function result</returns>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate double FunType2(
        double arg0,
        double arg1);

    /// <summary>
    /// Callback type used for functions with three arguments.
    /// </summary>
    /// <param name="arg0">The first function argument</param>
    /// <param name="arg1">The second function argument</param>
    /// <param name="arg2">The third function argument</param>
    /// <returns>The function result</returns>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate double FunType3(
        double arg0,
        double arg1,
        double arg2);

    /// <summary>
    /// Callback type used for functions with four arguments.
    /// </summary>
    /// <param name="arg0">The first function argument</param>
    /// <param name="arg1">The second function argument</param>
    /// <param name="arg2">The third function argument</param>
    /// <param name="arg3">The fourth function argument</param>
    /// <returns>The function result</returns>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate double FunType4(
        double arg0,
        double arg1,
        double arg2,
        double arg3);

    /// <summary>
    /// Callback type used for functions with five arguments.
    /// </summary>
    /// <param name="arg0">The first function argument</param>
    /// <param name="arg1">The second function argument</param>
    /// <param name="arg2">The third function argument</param>
    /// <param name="arg3">The fourth function argument</param>
    /// <param name="arg4">The fifth function argument</param>
    /// <returns>The function result</returns>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate double FunType5(
        double arg0,
        double arg1,
        double arg2,
        double arg3,
        double arg4);

    /// <summary>
    /// Callback type used for functions with six arguments.
    /// </summary>
    /// <param name="arg0">The first function argument</param>
    /// <param name="arg1">The second function argument</param>
    /// <param name="arg2">The third function argument</param>
    /// <param name="arg3">The fourth function argument</param>
    /// <param name="arg4">The fifth function argument</param>
    /// <param name="arg5">The sixth function argument</param>
    /// <returns>The function result</returns>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate double FunType6(
        double arg0,
        double arg1,
        double arg2,
        double arg3,
        double arg4,
        double arg5);

    /// <summary>
    /// Callback type used for functions with seven arguments.
    /// </summary>
    /// <param name="arg0">The first function argument</param>
    /// <param name="arg1">The second function argument</param>
    /// <param name="arg2">The third function argument</param>
    /// <param name="arg3">The fourth function argument</param>
    /// <param name="arg4">The fifth function argument</param>
    /// <param name="arg5">The sixth function argument</param>
    /// <param name="arg6">The seventh function argument</param>
    /// <returns>The function result</returns>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate double FunType7(
        double arg0,
        double arg1,
        double arg2,
        double arg3,
        double arg4,
        double arg5,
        double arg6);

    /// <summary>
    /// Callback type used for functions with eight arguments.
    /// </summary>
    /// <param name="arg0">The first function argument</param>
    /// <param name="arg1">The second function argument</param>
    /// <param name="arg2">The third function argument</param>
    /// <param name="arg3">The fourth function argument</param>
    /// <param name="arg4">The fifth function argument</param>
    /// <param name="arg5">The sixth function argument</param>
    /// <param name="arg6">The seventh function argument</param>
    /// <param name="arg7">The eighth function argument</param>
    /// <returns>The function result</returns>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate double FunType8(
        double arg0,
        double arg1,
        double arg2,
        double arg3,
        double arg4,
        double arg5,
        double arg6,
        double arg7);

    /// <summary>
    /// Callback type used for functions with nine arguments.
    /// </summary>
    /// <param name="arg0">The first function argument</param>
    /// <param name="arg1">The second function argument</param>
    /// <param name="arg2">The third function argument</param>
    /// <param name="arg3">The fourth function argument</param>
    /// <param name="arg4">The fifth function argument</param>
    /// <param name="arg5">The sixth function argument</param>
    /// <param name="arg6">The seventh function argument</param>
    /// <param name="arg7">The eighth function argument</param>
    /// <param name="arg8">The ninth function argument</param>
    /// <returns>The function result</returns>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
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

    /// <summary>
    /// Callback type used for functions with ten arguments.
    /// </summary>
    /// <param name="arg0">The first function argument</param>
    /// <param name="arg1">The second function argument</param>
    /// <param name="arg2">The third function argument</param>
    /// <param name="arg3">The fourth function argument</param>
    /// <param name="arg4">The fifth function argument</param>
    /// <param name="arg5">The sixth function argument</param>
    /// <param name="arg6">The seventh function argument</param>
    /// <param name="arg7">The eighth function argument</param>
    /// <param name="arg8">The ninth function argument</param>
    /// <param name="arg9">The tenth function argument</param>
    /// <returns>The function result</returns>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
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

    /// <summary>
    /// Callback type used for functions without arguments.
    /// </summary>
    /// <param name="bulkIndex">The current bulk index</param>
    /// <param name="threadIndex">The thread index that are running the callback</param>
    /// <returns>The function result</returns>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate double BulkFunType0(int bulkIndex, int threadIndex);

    /// <summary>
    /// Callback type used for functions with a single argument.
    /// </summary>
    /// <param name="bulkIndex">The current bulk index</param>
    /// <param name="threadIndex">The thread index that are running the callback</param>
    /// <param name="arg0">The first function argument</param>
    /// <returns>The function result</returns>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate double BulkFunType1(int bulkIndex, int threadIndex,
        double arg0);

    /// <summary>
    /// Callback type used for functions with two arguments.
    /// </summary>
    /// <param name="bulkIndex">The current bulk index</param>
    /// <param name="threadIndex">The thread index that are running the callback</param>
    /// <param name="arg0">The first function argument</param>
    /// <param name="arg1">The second function argument</param>
    /// <returns>The function result</returns>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate double BulkFunType2(int bulkIndex, int threadIndex,
        double arg0,
        double arg1);

    /// <summary>
    /// Callback type used for functions with three arguments.
    /// </summary>
    /// <param name="bulkIndex">The current bulk index</param>
    /// <param name="threadIndex">The thread index that are running the callback</param>
    /// <param name="arg0">The first function argument</param>
    /// <param name="arg1">The second function argument</param>
    /// <param name="arg2">The third function argument</param>
    /// <returns>The function result</returns>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate double BulkFunType3(int bulkIndex, int threadIndex,
        double arg0,
        double arg1,
        double arg2);

    /// <summary>
    /// Callback type used for functions with four arguments.
    /// </summary>
    /// <param name="bulkIndex">The current bulk index</param>
    /// <param name="threadIndex">The thread index that are running the callback</param>
    /// <param name="arg0">The first function argument</param>
    /// <param name="arg1">The second function argument</param>
    /// <param name="arg2">The third function argument</param>
    /// <param name="arg3">The fourth function argument</param>
    /// <returns>The function result</returns>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate double BulkFunType4(int bulkIndex, int threadIndex,
        double arg0,
        double arg1,
        double arg2,
        double arg3);

    /// <summary>
    /// Callback type used for functions with five arguments.
    /// </summary>
    /// <param name="bulkIndex">The current bulk index</param>
    /// <param name="threadIndex">The thread index that are running the callback</param>
    /// <param name="arg0">The first function argument</param>
    /// <param name="arg1">The second function argument</param>
    /// <param name="arg2">The third function argument</param>
    /// <param name="arg3">The fourth function argument</param>
    /// <param name="arg4">The fifth function argument</param>
    /// <returns>The function result</returns>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate double BulkFunType5(int bulkIndex, int threadIndex,
        double arg0,
        double arg1,
        double arg2,
        double arg3,
        double arg4);

    /// <summary>
    /// Callback type used for functions with six arguments.
    /// </summary>
    /// <param name="bulkIndex">The current bulk index</param>
    /// <param name="threadIndex">The thread index that are running the callback</param>
    /// <param name="arg0">The first function argument</param>
    /// <param name="arg1">The second function argument</param>
    /// <param name="arg2">The third function argument</param>
    /// <param name="arg3">The fourth function argument</param>
    /// <param name="arg4">The fifth function argument</param>
    /// <param name="arg5">The sixth function argument</param>
    /// <returns>The function result</returns>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate double BulkFunType6(int bulkIndex, int threadIndex,
        double arg0,
        double arg1,
        double arg2,
        double arg3,
        double arg4,
        double arg5);

    /// <summary>
    /// Callback type used for functions with seven arguments.
    /// </summary>
    /// <param name="bulkIndex">The current bulk index</param>
    /// <param name="threadIndex">The thread index that are running the callback</param>
    /// <param name="arg0">The first function argument</param>
    /// <param name="arg1">The second function argument</param>
    /// <param name="arg2">The third function argument</param>
    /// <param name="arg3">The fourth function argument</param>
    /// <param name="arg4">The fifth function argument</param>
    /// <param name="arg5">The sixth function argument</param>
    /// <param name="arg6">The seventh function argument</param>
    /// <returns>The function result</returns>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate double BulkFunType7(int bulkIndex, int threadIndex,
        double arg0,
        double arg1,
        double arg2,
        double arg3,
        double arg4,
        double arg5,
        double arg6);

    /// <summary>
    /// Callback type used for functions with eight arguments.
    /// </summary>
    /// <param name="bulkIndex">The current bulk index</param>
    /// <param name="threadIndex">The thread index that are running the callback</param>
    /// <param name="arg0">The first function argument</param>
    /// <param name="arg1">The second function argument</param>
    /// <param name="arg2">The third function argument</param>
    /// <param name="arg3">The fourth function argument</param>
    /// <param name="arg4">The fifth function argument</param>
    /// <param name="arg5">The sixth function argument</param>
    /// <param name="arg6">The seventh function argument</param>
    /// <param name="arg7">The eighth function argument</param>
    /// <returns>The function result</returns>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate double BulkFunType8(int bulkIndex, int threadIndex,
        double arg0,
        double arg1,
        double arg2,
        double arg3,
        double arg4,
        double arg5,
        double arg6,
        double arg7);

    /// <summary>
    /// Callback type used for functions with nine arguments.
    /// </summary>
    /// <param name="bulkIndex">The current bulk index</param>
    /// <param name="threadIndex">The thread index that are running the callback</param>
    /// <param name="arg0">The first function argument</param>
    /// <param name="arg1">The second function argument</param>
    /// <param name="arg2">The third function argument</param>
    /// <param name="arg3">The fourth function argument</param>
    /// <param name="arg4">The fifth function argument</param>
    /// <param name="arg5">The sixth function argument</param>
    /// <param name="arg6">The seventh function argument</param>
    /// <param name="arg7">The eighth function argument</param>
    /// <param name="arg8">The ninth function argument</param>
    /// <returns>The function result</returns>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
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

    /// <summary>
    /// Callback type used for functions with ten arguments.
    /// </summary>
    /// <param name="bulkIndex">The current bulk index</param>
    /// <param name="threadIndex">The thread index that are running the callback</param>
    /// <param name="arg0">The first function argument</param>
    /// <param name="arg1">The second function argument</param>
    /// <param name="arg2">The third function argument</param>
    /// <param name="arg3">The fourth function argument</param>
    /// <param name="arg4">The fifth function argument</param>
    /// <param name="arg5">The sixth function argument</param>
    /// <param name="arg6">The seventh function argument</param>
    /// <param name="arg7">The eighth function argument</param>
    /// <param name="arg8">The ninth function argument</param>
    /// <param name="arg9">The tenth function argument</param>
    /// <returns>The function result</returns>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
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

    /// <summary>
    /// Callback type used for functions with a variable argument list.
    /// </summary>
    /// <param name="args">The arguments list</param>
    /// <param name="size">The size of arguments list</param>
    /// <returns>The function result</returns>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate double MultFunType([MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] double[] args, int size);

    /// <summary>
    /// Callback type used for functions taking a string as an argument.
    /// </summary>
    /// <param name="str">The string function argument</param>
    /// <returns>The function result</returns>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate double StrFunType1([MarshalAs(UnmanagedType.LPStr)] string str);

    /// <summary>
    /// Callback type used for functions taking a string and a value as arguments.
    /// </summary>
    /// <param name="str">The string function argument</param>
    /// <param name="arg0">The first function argument</param>
    /// <returns>The function result</returns>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate double StrFunType2([MarshalAs(UnmanagedType.LPStr)] string str, double arg0);

    /// <summary>
    /// Callback type used for functions taking a string and two values as arguments.
    /// </summary>
    /// <param name="str">The string function argument</param>
    /// <param name="arg0">The first function argument</param>
    /// <param name="arg1">The second function argument</param>
    /// <returns>The function result</returns>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate double StrFunType3([MarshalAs(UnmanagedType.LPStr)] string str, double arg0, double arg1);

    /// <summary>
    /// Callback used for functions that identify values in a string.
    /// </summary>
    /// <param name="remainingExpr">The string function argument</param>
    /// <param name="pos">The position relative to the first position of the
    /// expression. This must be incremented with the number of characters used
    /// to parser de value.</param>
    /// <param name="value">The variable to receive the parsed value</param>
    /// <returns>Must return <code>true</code> if the value was parsed by the callback.
    /// Otherwise, it must return <code>false</code>.</returns>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate bool IdentFunction([MarshalAs(UnmanagedType.LPStr)] string remainingExpr, ref int pos, ref double value);
}
