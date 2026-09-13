namespace System.Runtime.InteropServices;

[Serializable]
[Obsolete("Use System.Runtime.InteropServices.ComTypes.FUNCKIND instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
public enum FUNCKIND
{
	FUNC_VIRTUAL,
	FUNC_PUREVIRTUAL,
	FUNC_NONVIRTUAL,
	FUNC_STATIC,
	FUNC_DISPATCH
}
