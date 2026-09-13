using System.Diagnostics;
using System.Security;

namespace System.Runtime.Serialization.Formatters.Binary;

internal sealed class BinaryCrossAppDomainString : IStreamable
{
	internal int objectId;

	internal int value;

	internal BinaryCrossAppDomainString()
	{
	}

	public void Write(__BinaryWriter sout)
	{
		sout.WriteByte(19);
		sout.WriteInt32(objectId);
		sout.WriteInt32(value);
	}

	[SecurityCritical]
	public void Read(__BinaryParser input)
	{
		objectId = input.ReadInt32();
		value = input.ReadInt32();
	}

	public void Dump()
	{
	}

	[Conditional("_LOGGING")]
	private void DumpInternal()
	{
		BCLDebug.CheckEnabled("BINARY");
	}
}
