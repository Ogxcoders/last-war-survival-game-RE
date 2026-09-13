namespace System.Runtime.InteropServices.ComTypes;

[ComImport]
[Guid("00000103-0000-0000-C000-000000000046")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IEnumSTATDATA
{
	void Clone(out IEnumSTATDATA newEnum);

	[PreserveSig]
	int Next(int celt, [Out][MarshalAs(UnmanagedType.LPArray)] STATDATA[] rgelt, [Out][MarshalAs(UnmanagedType.LPArray)] int[] pceltFetched);

	[PreserveSig]
	int Reset();

	[PreserveSig]
	int Skip(int celt);
}
