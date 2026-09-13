namespace System.Runtime.InteropServices;

internal static class AddrofIntrinsics
{
	internal static IntPtr AddrOf<T>(T ftn)
	{
		return Marshal.GetFunctionPointerForDelegate(ftn);
	}
}
