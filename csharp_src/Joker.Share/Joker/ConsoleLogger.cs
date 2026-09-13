using System;

namespace Joker;

public class ConsoleLogger : ILogger
{
	public void Debug(string message)
	{
		Console.WriteLine("Debug:" + message);
	}

	public void Info(string message)
	{
		Console.WriteLine("Info:" + message);
	}

	public void Warning(string message)
	{
		Console.WriteLine("Warning:" + message);
	}

	public void Error(string message)
	{
		Console.WriteLine("Error:" + message);
	}

	public void Exception(Exception e)
	{
		Console.WriteLine($"Error:{e}");
	}

	public void Assert(bool condition, object message = null)
	{
		if (!condition)
		{
			string text = ((message == null) ? "failed" : message.ToString());
			Console.WriteLine("Assert:" + text);
		}
	}
}
