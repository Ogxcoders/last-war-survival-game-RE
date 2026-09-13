using System;

namespace Joker;

internal class ConfigInvalidException : Exception
{
	public ConfigInvalidException(string message)
		: base(message)
	{
	}
}
