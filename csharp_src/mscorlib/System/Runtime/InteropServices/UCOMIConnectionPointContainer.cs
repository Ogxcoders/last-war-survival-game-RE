namespace System.Runtime.InteropServices;

[ComImport]
[Obsolete]
[Guid("b196b284-bab4-101a-b69c-00aa00341d07")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface UCOMIConnectionPointContainer
{
	void EnumConnectionPoints(out UCOMIEnumConnectionPoints ppEnum);

	void FindConnectionPoint(ref Guid riid, out UCOMIConnectionPoint ppCP);
}
