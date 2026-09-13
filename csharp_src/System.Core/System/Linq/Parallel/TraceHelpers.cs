using System.Diagnostics;

namespace System.Linq.Parallel;

internal static class TraceHelpers
{
	[Conditional("PFXTRACE")]
	internal static void TraceInfo(string msg, params object[] args)
	{
	}
}
