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

#include "ParserVariable.h"

namespace muParserNET
{
	String ^ParserVariable::Name::get()
	{
		return this->name;
	}

	double ParserVariable::Value::get()
	{
		// retorna o primeiro valor do array
		return this->valueArray[0];
	}

	void ParserVariable::Value::set(double value)
	{
		this->valueArray[0] = value;
	}

	array<double> ^ParserVariable::ValueArray::get()
	{
		return this->valueArray;
	}

	IntPtr ParserVariable::Pointer::get()
	{
		// cria um ponteiro para a primeira posição do vetor
		pin_ptr<double> ptr = &this->valueArray[0];

		return IntPtr(ptr);
	}

	ParserVariable::ParserVariable(String ^name)
		: name(name)
	{
		this->valueArray = gcnew array<double>(1);
		this->valueArray[0] = 0.0;

		// evita que o objeto do array seja movido
		this->ptrValueArray = GCHandle::Alloc(this->valueArray, GCHandleType::Pinned);
	}

	ParserVariable::ParserVariable(String ^name, double value)
		: name(name)
	{
		this->valueArray = gcnew array<double>(1);
		this->valueArray[0] = value;

		// evita que o objeto do array seja movido
		this->ptrValueArray = GCHandle::Alloc(this->valueArray, GCHandleType::Pinned);
	}

	ParserVariable::ParserVariable(String ^name, array<double> ^valueArray)
		: name(name)
		, valueArray(valueArray)
	{
		// evita que o objeto do array seja movido
		this->ptrValueArray = GCHandle::Alloc(this->valueArray, GCHandleType::Pinned);
	}

	ParserVariable::~ParserVariable()
	{
		// libera o vetor
		this->ptrValueArray.Free();
	}

}
