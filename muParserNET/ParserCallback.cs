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
using System.Runtime.InteropServices;

namespace muParserNET
{
    /// <summary>
    /// Class of the parser callbacks.
    /// </summary>
    public class ParserCallback
    {
        private Delegate func;

        /*
		 * Quando a função passa a ser um callback da biblioteca o objeto do
		 * delegate não deve ser apagado pelo GC.
		 */
        private GCHandle ptrFunc;

        /// <summary>
		/// Gets the delegate which represents the callback.
		/// </summary>
		public Delegate Function
		{
			get
            {
                return this.func;
            }
		}

        /// <summary>
		/// Class constructor. It receives a delegate that will represent the
		/// callback triggered by the muParser library. It also blocks the
		/// garbage colletor to destroy the delegate object.
		/// </summary>
		/// <param name="func">The callback delegate object</param>
		public ParserCallback(Delegate func)
        {
            this.func = func;

            // bloqueia o GC de apagar o objeto do delegate
		    this.ptrFunc = GCHandle.Alloc(func);
        }

		/// <summary>
		/// Class destructor. It releases the delegate object to be removed by
		/// the garbage collector.
		/// </summary>
		~ParserCallback()
        {
            // libera o GC para apagar o objeto
            this.ptrFunc.Free();
        }
    }
}
