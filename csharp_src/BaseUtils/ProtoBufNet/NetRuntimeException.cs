using System;

namespace ProtoBufNet;

[Serializable]
internal class NetRuntimeException : SystemException
{
	public NetRuntimeException(string message)
		: base(message)
	{
	}
}
