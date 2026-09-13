using System.Diagnostics;
using System.Security;

namespace System.Runtime.Serialization.Formatters.Binary;

internal sealed class MemberReference : IStreamable
{
	internal int idRef;

	internal MemberReference()
	{
	}

	internal void Set(int idRef)
	{
		this.idRef = idRef;
	}

	public void Write(__BinaryWriter sout)
	{
		sout.WriteByte(9);
		sout.WriteInt32(idRef);
	}

	[SecurityCritical]
	public void Read(__BinaryParser input)
	{
		idRef = input.ReadInt32();
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
