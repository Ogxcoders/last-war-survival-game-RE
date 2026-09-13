using System.Security;

namespace System.Diagnostics;

internal static class Assert
{
	internal const int COR_E_FAILFAST = -2146232797;

	private static AssertFilter Filter;

	static Assert()
	{
		Filter = new DefaultFilter();
	}

	internal static void Check(bool condition, string conditionString, string message)
	{
		if (!condition)
		{
			Fail(conditionString, message, null, -2146232797);
		}
	}

	internal static void Check(bool condition, string conditionString, string message, int exitCode)
	{
		if (!condition)
		{
			Fail(conditionString, message, null, exitCode);
		}
	}

	internal static void Fail(string conditionString, string message)
	{
		Fail(conditionString, message, null, -2146232797);
	}

	internal static void Fail(string conditionString, string message, string windowTitle, int exitCode)
	{
		Fail(conditionString, message, windowTitle, exitCode, StackTrace.TraceFormat.Normal, 0);
	}

	internal static void Fail(string conditionString, string message, int exitCode, StackTrace.TraceFormat stackTraceFormat)
	{
		Fail(conditionString, message, null, exitCode, stackTraceFormat, 0);
	}

	[SecuritySafeCritical]
	internal static void Fail(string conditionString, string message, string windowTitle, int exitCode, StackTrace.TraceFormat stackTraceFormat, int numStackFramesToSkip)
	{
		StackTrace location = new StackTrace(numStackFramesToSkip, fNeedFileInfo: true);
		switch (Filter.AssertFailure(conditionString, message, location, stackTraceFormat, windowTitle))
		{
		case AssertFilters.FailDebug:
			if (Debugger.IsAttached)
			{
				Debugger.Break();
			}
			else if (!Debugger.Launch())
			{
				throw new InvalidOperationException(Environment.GetResourceString("Debugger unable to launch."));
			}
			break;
		case AssertFilters.FailTerminate:
			if (Debugger.IsAttached)
			{
				Environment._Exit(exitCode);
			}
			else
			{
				Environment.FailFast(message, (uint)exitCode);
			}
			break;
		}
	}

	internal static int ShowDefaultAssertDialog(string conditionString, string message, string stackTrace, string windowTitle)
	{
		throw new NotImplementedException();
	}
}
