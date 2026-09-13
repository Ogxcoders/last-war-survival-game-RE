namespace System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
[Obsolete("Use System.Runtime.InteropServices.ComTypes.ELEMDESC instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
public struct ELEMDESC
{
	[StructLayout(LayoutKind.Explicit, CharSet = CharSet.Unicode)]
	[ComVisible(false)]
	public struct DESCUNION
	{
		[FieldOffset(0)]
		public IDLDESC idldesc;

		[FieldOffset(0)]
		public PARAMDESC paramdesc;
	}

	public TYPEDESC tdesc;

	public DESCUNION desc;
}
