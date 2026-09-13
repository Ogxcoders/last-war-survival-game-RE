namespace System.Runtime.InteropServices.ComTypes;

[ComImport]
[Guid("0000010e-0000-0000-C000-000000000046")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IDataObject
{
	[PreserveSig]
	int DAdvise([In] ref FORMATETC pFormatetc, ADVF advf, IAdviseSink adviseSink, out int connection);

	void DUnadvise(int connection);

	[PreserveSig]
	int EnumDAdvise(out IEnumSTATDATA enumAdvise);

	IEnumFORMATETC EnumFormatEtc(DATADIR direction);

	[PreserveSig]
	int GetCanonicalFormatEtc([In] ref FORMATETC formatIn, out FORMATETC formatOut);

	void GetData([In] ref FORMATETC format, out STGMEDIUM medium);

	void GetDataHere([In] ref FORMATETC format, ref STGMEDIUM medium);

	[PreserveSig]
	int QueryGetData([In] ref FORMATETC format);

	void SetData([In] ref FORMATETC formatIn, [In] ref STGMEDIUM medium, [MarshalAs(UnmanagedType.Bool)] bool release);
}
