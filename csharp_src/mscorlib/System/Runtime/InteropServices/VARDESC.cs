namespace System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
[Obsolete("Use System.Runtime.InteropServices.ComTypes.VARDESC instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
public struct VARDESC
{
	[StructLayout(LayoutKind.Explicit, CharSet = CharSet.Unicode)]
	[ComVisible(false)]
	public struct DESCUNION
	{
		[FieldOffset(0)]
		public int oInst;

		[FieldOffset(0)]
		public IntPtr lpvarValue;
	}

	public int memid;

	public string lpstrSchema;

	public ELEMDESC elemdescVar;

	public short wVarFlags;

	public VarEnum varkind;
}
