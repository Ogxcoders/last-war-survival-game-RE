using System.Runtime.InteropServices;

namespace System.Runtime.CompilerServices;

[Serializable]
[ComVisible(true)]
public enum MethodCodeType
{
	IL,
	Native,
	OPTIL,
	Runtime
}
