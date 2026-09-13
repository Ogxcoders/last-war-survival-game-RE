using System.Diagnostics;
using System.Security;

namespace System.Runtime.Serialization.Formatters.Binary;

internal sealed class BinaryCrossAppDomainAssembly : IStreamable
{
	internal int assemId;

	internal int assemblyIndex;

	internal BinaryCrossAppDomainAssembly()
	{
	}

	public void Write(__BinaryWriter sout)
	{
		sout.WriteByte(20);
		sout.WriteInt32(assemId);
		sout.WriteInt32(assemblyIndex);
	}

	[SecurityCritical]
	public void Read(__BinaryParser input)
	{
		assemId = input.ReadInt32();
		assemblyIndex = input.ReadInt32();
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
