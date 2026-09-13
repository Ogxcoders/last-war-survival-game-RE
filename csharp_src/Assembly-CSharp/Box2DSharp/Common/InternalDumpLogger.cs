using System;

namespace Box2DSharp.Common;

public class InternalDumpLogger : IDumpLogger
{
	public void Log(string message)
	{
		Console.WriteLine(message);
	}
}
