using System;

internal class DiagnosticListener
{
	internal static bool DiagnosticListenerEnabled;

	internal DiagnosticListener(string s)
	{
	}

	internal bool IsEnabled(string s)
	{
		return DiagnosticListenerEnabled;
	}

	internal void Write(string s1, object s2)
	{
		Console.WriteLine($"|| {s1},  {s2}");
	}
}
