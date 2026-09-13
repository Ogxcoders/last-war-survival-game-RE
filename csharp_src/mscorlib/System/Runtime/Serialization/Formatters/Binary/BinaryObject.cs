using System.Diagnostics;
using System.Security;

namespace System.Runtime.Serialization.Formatters.Binary;

internal sealed class BinaryObject : IStreamable
{
	internal int objectId;

	internal int mapId;

	internal BinaryObject()
	{
	}

	internal void Set(int objectId, int mapId)
	{
		this.objectId = objectId;
		this.mapId = mapId;
	}

	public void Write(__BinaryWriter sout)
	{
		sout.WriteByte(1);
		sout.WriteInt32(objectId);
		sout.WriteInt32(mapId);
	}

	[SecurityCritical]
	public void Read(__BinaryParser input)
	{
		objectId = input.ReadInt32();
		mapId = input.ReadInt32();
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
