namespace System.Runtime.InteropServices.ComTypes;

[ComImport]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[Guid("00000103-0000-0000-C000-000000000046")]
public interface IEnumFORMATETC
{
	void Clone(out IEnumFORMATETC newEnum);

	[PreserveSig]
	int Next(int celt, [Out][MarshalAs(UnmanagedType.LPArray)] FORMATETC[] rgelt, [Out][MarshalAs(UnmanagedType.LPArray)] int[] pceltFetched);

	[PreserveSig]
	int Reset();

	[PreserveSig]
	int Skip(int celt);
}
