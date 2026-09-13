namespace System.Runtime.InteropServices;

[Serializable]
[Obsolete("Use System.Runtime.InteropServices.ComTypes.IMPLTYPEFLAGS instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
[Flags]
public enum IMPLTYPEFLAGS
{
	IMPLTYPEFLAG_FDEFAULT = 1,
	IMPLTYPEFLAG_FSOURCE = 2,
	IMPLTYPEFLAG_FRESTRICTED = 4,
	IMPLTYPEFLAG_FDEFAULTVTABLE = 8
}
