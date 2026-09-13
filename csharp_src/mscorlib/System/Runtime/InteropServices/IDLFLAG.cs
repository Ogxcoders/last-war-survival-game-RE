namespace System.Runtime.InteropServices;

[Serializable]
[Obsolete("Use System.Runtime.InteropServices.ComTypes.IDLFLAG instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
[Flags]
public enum IDLFLAG : short
{
	IDLFLAG_NONE = 0,
	IDLFLAG_FIN = 1,
	IDLFLAG_FOUT = 2,
	IDLFLAG_FLCID = 4,
	IDLFLAG_FRETVAL = 8
}
