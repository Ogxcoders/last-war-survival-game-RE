using System.Diagnostics;
using System.Security;

namespace System.Runtime.Serialization;

internal static class SerializationTrace
{
	[SecurityCritical]
	private static TraceSource codeGen;

	internal static SourceSwitch CodeGenerationSwitch => CodeGenerationTraceSource.Switch;

	private static TraceSource CodeGenerationTraceSource
	{
		[SecuritySafeCritical]
		get
		{
			if (codeGen == null)
			{
				codeGen = new TraceSource("System.Runtime.Serialization.CodeGeneration");
			}
			return codeGen;
		}
	}

	internal static void WriteInstruction(int lineNumber, string instruction)
	{
	}

	internal static void TraceInstruction(string instruction)
	{
	}
}
