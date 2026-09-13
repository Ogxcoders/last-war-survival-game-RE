using System.Diagnostics;
using UnityEngine.Scripting;

namespace UnityEngine.TextCore.LowLevel;

[DebuggerDisplay("{familyName} - {styleName}")]
[UsedByNativeCode]
internal struct FontReference
{
	public string familyName;

	public string styleName;

	public int faceIndex;

	public string filePath;
}
