using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata;

[Serializable]
[ComVisible(true)]
public enum XmlFieldOrderOption
{
	All,
	Sequence,
	Choice
}
