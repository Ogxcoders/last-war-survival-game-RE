using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Channels;

[Serializable]
[ComVisible(true)]
public enum ServerProcessing
{
	Complete,
	OneWay,
	Async
}
