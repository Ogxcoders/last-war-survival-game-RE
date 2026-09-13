using System;

namespace Joker;

public interface ILogger
{
	void Debug(string message);

	void Info(string message);

	void Warning(string message);

	void Error(string message);

	void Exception(Exception e);

	void Assert(bool condition, object message = null);
}
