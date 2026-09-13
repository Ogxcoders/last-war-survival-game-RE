using System.Runtime.CompilerServices;

namespace Microsoft.Win32;

internal static class NativeMethods
{
	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern int GetCurrentProcessId();
}
