using System;

public class LuaAdapterException : Exception
{
	public LuaAdapterException(string message)
		: base(message)
	{
	}

	public static string GenMessage(bool isNull, int index, uint arrSize)
	{
		if (!isNull)
		{
			return $"index error {index} {arrSize}";
		}
		return "ptr is null";
	}

	public static void ThrowIfNeeded(bool isNull, int index, uint arrSize)
	{
		if (isNull)
		{
			return;
		}
		throw new LuaAdapterException($"index error {index} {arrSize}");
	}
}
