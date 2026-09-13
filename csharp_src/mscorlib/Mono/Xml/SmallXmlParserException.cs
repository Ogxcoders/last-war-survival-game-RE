using System;

namespace Mono.Xml;

internal class SmallXmlParserException : SystemException
{
	private int line;

	private int column;

	public int Line => line;

	public int Column => column;

	public SmallXmlParserException(string msg, int line, int column)
		: base($"{msg}. At ({line},{column})")
	{
		this.line = line;
		this.column = column;
	}
}
