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

	ParserVariable::ParserVariable(String ^name, double value)
		: name(name)
	{
		this->valueArray = gcnew array<double>(1);
		this->valueArray[0] = value;
	}

	ParserVariable::ParserVariable(String ^name, array<double> ^valueArray)
		: name(name)
		, valueArray(valueArray)
	{
	}

	ParserVariable::~ParserVariable()
	{
	}

}
