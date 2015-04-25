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

namespace muParserNET
{
    internal static class MuParserFunctions
    {
        /*
         * As funções que retornam string fazem com que o .NET tente liberar o
         * endereço de memória.
         */

        [DllImport("muParser", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr mupCreate(int nBaseType);

        [DllImport("muParser", CallingConvention = CallingConvention.Cdecl)]
        public static extern void mupRelease(IntPtr a_hParser);

        [DllImport("muParser", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr mupGetExpr(IntPtr a_hParser);

        [DllImport("muParser", CallingConvention = CallingConvention.Cdecl)]
        public static extern void mupSetExpr(IntPtr a_hParser, string a_szExpr);
        //public static extern void mupSetVarFactory(IntPtr a_hParser, muFacFun a_pFactory, void* pUserData);

        [DllImport("muParser", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr mupGetVersion(IntPtr a_hParser);

        [DllImport("muParser", CallingConvention = CallingConvention.Cdecl)]
        public static extern double mupEval(IntPtr a_hParser);

        // não funciona tentar fazer direto
        [DllImport("muParser", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr mupEvalMulti(IntPtr a_hParser, ref int nNum);

        [DllImport("muParser", CallingConvention = CallingConvention.Cdecl)]
        public static extern void mupEvalBulk(IntPtr a_hParser, double[] a_fResult, int nSize);

        // Defining callbacks / variables / constants
        [DllImport("muParser", CallingConvention = CallingConvention.Cdecl)]
        public static extern void mupDefineFun0(IntPtr a_hParser, string a_szName, FunType0 a_pFun, bool a_bOptimize);

        [DllImport("muParser", CallingConvention = CallingConvention.Cdecl)]
        public static extern void mupDefineFun1(IntPtr a_hParser, string a_szName, FunType1 a_pFun, bool a_bOptimize);

        [DllImport("muParser", CallingConvention = CallingConvention.Cdecl)]
        public static extern void mupDefineFun2(IntPtr a_hParser, string a_szName, FunType2 a_pFun, bool a_bOptimize);

        [DllImport("muParser", CallingConvention = CallingConvention.Cdecl)]
        public static extern void mupDefineFun3(IntPtr a_hParser, string a_szName, FunType3 a_pFun, bool a_bOptimize);

        [DllImport("muParser", CallingConvention = CallingConvention.Cdecl)]
        public static extern void mupDefineFun4(IntPtr a_hParser, string a_szName, FunType4 a_pFun, bool a_bOptimize);

        [DllImport("muParser", CallingConvention = CallingConvention.Cdecl)]
        public static extern void mupDefineFun5(IntPtr a_hParser, string a_szName, FunType5 a_pFun, bool a_bOptimize);

        [DllImport("muParser", CallingConvention = CallingConvention.Cdecl)]
        public static extern void mupDefineFun6(IntPtr a_hParser, string a_szName, FunType6 a_pFun, bool a_bOptimize);

        [DllImport("muParser", CallingConvention = CallingConvention.Cdecl)]
        public static extern void mupDefineFun7(IntPtr a_hParser, string a_szName, FunType7 a_pFun, bool a_bOptimize);

        [DllImport("muParser", CallingConvention = CallingConvention.Cdecl)]
        public static extern void mupDefineFun8(IntPtr a_hParser, string a_szName, FunType8 a_pFun, bool a_bOptimize);

        [DllImport("muParser", CallingConvention = CallingConvention.Cdecl)]
        public static extern void mupDefineFun9(IntPtr a_hParser, string a_szName, FunType9 a_pFun, bool a_bOptimize);

        [DllImport("muParser", CallingConvention = CallingConvention.Cdecl)]
        public static extern void mupDefineFun10(IntPtr a_hParser, string a_szName, FunType10 a_pFun, bool a_bOptimize);

        // Defining bulkmode functions

        [DllImport("muParser", CallingConvention = CallingConvention.Cdecl)]
        public static extern void mupDefineBulkFun0(IntPtr a_hParser, string a_szName, BulkFunType0 a_pFun);

        [DllImport("muParser", CallingConvention = CallingConvention.Cdecl)]
        public static extern void mupDefineBulkFun1(IntPtr a_hParser, string a_szName, BulkFunType1 a_pFun);

        [DllImport("muParser", CallingConvention = CallingConvention.Cdecl)]
        public static extern void mupDefineBulkFun2(IntPtr a_hParser, string a_szName, BulkFunType2 a_pFun);

        [DllImport("muParser", CallingConvention = CallingConvention.Cdecl)]
        public static extern void mupDefineBulkFun3(IntPtr a_hParser, string a_szName, BulkFunType3 a_pFun);

        [DllImport("muParser", CallingConvention = CallingConvention.Cdecl)]
        public static extern void mupDefineBulkFun4(IntPtr a_hParser, string a_szName, BulkFunType4 a_pFun);

        [DllImport("muParser", CallingConvention = CallingConvention.Cdecl)]
        public static extern void mupDefineBulkFun5(IntPtr a_hParser, string a_szName, BulkFunType5 a_pFun);

        [DllImport("muParser", CallingConvention = CallingConvention.Cdecl)]
        public static extern void mupDefineBulkFun6(IntPtr a_hParser, string a_szName, BulkFunType6 a_pFun);

        [DllImport("muParser", CallingConvention = CallingConvention.Cdecl)]
        public static extern void mupDefineBulkFun7(IntPtr a_hParser, string a_szName, BulkFunType7 a_pFun);

        [DllImport("muParser", CallingConvention = CallingConvention.Cdecl)]
        public static extern void mupDefineBulkFun8(IntPtr a_hParser, string a_szName, BulkFunType8 a_pFun);

        [DllImport("muParser", CallingConvention = CallingConvention.Cdecl)]
        public static extern void mupDefineBulkFun9(IntPtr a_hParser, string a_szName, BulkFunType9 a_pFun);

        [DllImport("muParser", CallingConvention = CallingConvention.Cdecl)]
        public static extern void mupDefineBulkFun10(IntPtr a_hParser, string a_szName, BulkFunType10 a_pFun);

        // string functions
        [DllImport("muParser", CallingConvention = CallingConvention.Cdecl)]
        public static extern void mupDefineStrFun1(IntPtr a_hParser, string a_szName, StrFunType1 a_pFun);

        [DllImport("muParser", CallingConvention = CallingConvention.Cdecl)]
        public static extern void mupDefineStrFun2(IntPtr a_hParser, string a_szName, StrFunType2 a_pFun);

        [DllImport("muParser", CallingConvention = CallingConvention.Cdecl)]
        public static extern void mupDefineStrFun3(IntPtr a_hParser, string a_szName, StrFunType3 a_pFun);

        [DllImport("muParser", CallingConvention = CallingConvention.Cdecl)]
        public static extern void mupDefineMultFun(IntPtr a_hParser,
                                           string a_szName,
                                           MultFunType a_pFun,
                                           bool a_bOptimize);

        [DllImport("muParser", CallingConvention = CallingConvention.Cdecl)]
        public static extern void mupDefineOprt(IntPtr a_hParser,
                                        string a_szName,
                                        FunType2 a_pFun,
                                        uint a_nPrec,
                                        int a_nOprtAsct,
                                        bool a_bOptimize);

        [DllImport("muParser", CallingConvention = CallingConvention.Cdecl)]
        public static extern void mupDefineConst(IntPtr a_hParser,
                                         string a_szName,
                                         double a_fVal);

        [DllImport("muParser", CallingConvention = CallingConvention.Cdecl)]
        public static extern void mupDefineStrConst(IntPtr a_hParser,
                                            string a_szName,
                                            string a_sVal);

        [DllImport("muParser", CallingConvention = CallingConvention.Cdecl)]
        public static extern void mupDefineVar(IntPtr a_hParser,
                                       string a_szName,
                                       IntPtr a_fVar);

        [DllImport("muParser", CallingConvention = CallingConvention.Cdecl)]
        public static extern void mupDefineBulkVar(IntPtr a_hParser,
                                       string a_szName,
                                       IntPtr a_fVar);

        [DllImport("muParser", CallingConvention = CallingConvention.Cdecl)]
        public static extern void mupDefinePostfixOprt(IntPtr a_hParser,
                                               string a_szName,
                                               FunType1 a_pOprt,
                                               bool a_bOptimize);

        [DllImport("muParser", CallingConvention = CallingConvention.Cdecl)]
        public static extern void mupDefineInfixOprt(IntPtr a_hParser,
                                             string a_szName,
                                             FunType1 a_pOprt,
                                             bool a_bOptimize);

        // Define character sets for identifiers
        [DllImport("muParser", CallingConvention = CallingConvention.Cdecl)]
        public static extern void mupDefineNameChars(IntPtr a_hParser, string a_szCharset);

        [DllImport("muParser", CallingConvention = CallingConvention.Cdecl)]
        public static extern void mupDefineOprtChars(IntPtr a_hParser, string a_szCharset);

        [DllImport("muParser", CallingConvention = CallingConvention.Cdecl)]
        public static extern void mupDefineInfixOprtChars(IntPtr a_hParser, string a_szCharset);

        // Remove all / single variables
        [DllImport("muParser", CallingConvention = CallingConvention.Cdecl)]
        public static extern void mupRemoveVar(IntPtr a_hParser, string a_szName);

        [DllImport("muParser", CallingConvention = CallingConvention.Cdecl)]
        public static extern void mupClearVar(IntPtr a_hParser);

        [DllImport("muParser", CallingConvention = CallingConvention.Cdecl)]
        public static extern void mupClearConst(IntPtr a_hParser);

        [DllImport("muParser", CallingConvention = CallingConvention.Cdecl)]
        public static extern void mupClearOprt(IntPtr a_hParser);

        [DllImport("muParser", CallingConvention = CallingConvention.Cdecl)]
        public static extern void mupClearFun(IntPtr a_hParser);

        // Querying variables / expression variables / constants
        [DllImport("muParser", CallingConvention = CallingConvention.Cdecl)]
        public static extern int mupGetExprVarNum(IntPtr a_hParser);

        [DllImport("muParser", CallingConvention = CallingConvention.Cdecl)]
        public static extern int mupGetVarNum(IntPtr a_hParser);

        [DllImport("muParser", CallingConvention = CallingConvention.Cdecl)]
        public static extern int mupGetConstNum(IntPtr a_hParser);

        // essas duas funções são perigosas já que não da para saber o tamanho dos arrays
        // public static extern void mupGetExprVar(IntPtr a_hParser, uint a_iVar, ref string a_pszName, ref double[] a_pVar);
        // public static extern void mupGetVar(IntPtr a_hParser, uint a_iVar, ref string a_pszName, ref double[] a_pVar);
        [DllImport("muParser", CallingConvention = CallingConvention.Cdecl)]
        public static extern void mupGetConst(IntPtr a_hParser, uint a_iVar, ref string a_pszName, ref double a_pVar);

        [DllImport("muParser", CallingConvention = CallingConvention.Cdecl)]
        public static extern void mupSetArgSep(IntPtr a_hParser, char cArgSep);

        [DllImport("muParser", CallingConvention = CallingConvention.Cdecl)]
        public static extern void mupSetDecSep(IntPtr a_hParser, char cArgSep);

        [DllImport("muParser", CallingConvention = CallingConvention.Cdecl)]
        public static extern void mupSetThousandsSep(IntPtr a_hParser, char cArgSep);

        [DllImport("muParser", CallingConvention = CallingConvention.Cdecl)]
        public static extern void mupResetLocale(IntPtr a_hParser);

        // Add value recognition callbacks
        [DllImport("muParser", CallingConvention = CallingConvention.Cdecl)]
        public static extern void mupAddValIdent(IntPtr a_hParser, IdentFunction a_func);

        // Error handling
        [DllImport("muParser", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool mupError(IntPtr a_hParser);

        [DllImport("muParser", CallingConvention = CallingConvention.Cdecl)]
        public static extern void mupErrorReset(IntPtr a_hParser);

        [DllImport("muParser", CallingConvention = CallingConvention.Cdecl)]
        public static extern void mupSetErrorHandler(IntPtr a_hParser, ErrorFuncType a_pErrHandler);

        [DllImport("muParser", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr mupGetErrorMsg(IntPtr a_hParser);

        [DllImport("muParser", CallingConvention = CallingConvention.Cdecl)]
        public static extern int mupGetErrorCode(IntPtr a_hParser);

        [DllImport("muParser", CallingConvention = CallingConvention.Cdecl)]
        public static extern int mupGetErrorPos(IntPtr a_hParser);

        [DllImport("muParser", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr mupGetErrorToken(IntPtr a_hParser);

        #region API adicionada

        /*
         * Adicionando as funções que não são suportadas pela API original da
         * DLL muParser mas que são suportadas pela biblioteca de classes da
         * mesma.
         */

        [DllImport("muParser", CallingConvention = CallingConvention.Cdecl)]
        public static extern void mupClearInfixOprt(IntPtr a_hParser);

        [DllImport("muParser", CallingConvention = CallingConvention.Cdecl)]
        public static extern void mupClearPostfixOprt(IntPtr a_hParser);

        [DllImport("muParser", CallingConvention = CallingConvention.Cdecl)]
        public static extern void mupEnableBuiltInOprt(IntPtr a_hParser, bool oprtEn);

        [DllImport("muParser", CallingConvention = CallingConvention.Cdecl)]
        public static extern void mupEnableOptimizer(IntPtr a_hParser, bool optmEn);

        [DllImport("muParser", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr mupValidNameChars(IntPtr a_hParser);

        [DllImport("muParser", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr mupValidOprtChars(IntPtr a_hParser);

        [DllImport("muParser", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr mupValidInfixOprtChars(IntPtr a_hParser);

        #endregion
    }
}
