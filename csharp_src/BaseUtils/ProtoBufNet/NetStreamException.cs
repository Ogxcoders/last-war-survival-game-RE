using System;

namespace ProtoBufNet;

[Serializable]
internal class NetStreamException : NetRuntimeException
{
	public object witch { get; private set; }

	public NetStreamException(string message, object witch)
		: base(message)
	{
		this.witch = witch;
	}
}
