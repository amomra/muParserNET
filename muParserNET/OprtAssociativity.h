#pragma once

namespace muParserNET
{
	/** \brief Parser operator precedence values. */
	public enum class OprtAssociativity
		: int
	{
		LEFT = 0,
		RIGHT = 1,
		NONE = 2
	};
}