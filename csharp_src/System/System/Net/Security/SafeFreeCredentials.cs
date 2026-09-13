using System.Runtime.InteropServices;

namespace System.Net.Security;

internal abstract class SafeFreeCredentials : SafeHandle
{
	protected SafeFreeCredentials(IntPtr handle, bool ownsHandle)
		: base(handle, ownsHandle)
	{
	}
}
