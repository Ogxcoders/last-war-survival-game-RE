using System.Diagnostics;
using System.Security;

namespace System.Runtime.Serialization.Formatters.Binary;

internal sealed class BinaryCrossAppDomainMap : IStreamable
{
	internal int crossAppDomainArrayIndex;

	internal BinaryCrossAppDomainMap()
	{
	}

	public void Write(__BinaryWriter sout)
	{
		sout.WriteByte(18);
		sout.WriteInt32(crossAppDomainArrayIndex);
	}

	[SecurityCritical]
	public void Read(__BinaryParser input)
	{
		crossAppDomainArrayIndex = input.ReadInt32();
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
