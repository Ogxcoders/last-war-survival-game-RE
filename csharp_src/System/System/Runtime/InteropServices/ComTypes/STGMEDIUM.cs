namespace System.Runtime.InteropServices.ComTypes;

public struct STGMEDIUM
{
	[MarshalAs(UnmanagedType.IUnknown)]
	public object pUnkForRelease;

	public TYMED tymed;

	public IntPtr unionmember;
}
