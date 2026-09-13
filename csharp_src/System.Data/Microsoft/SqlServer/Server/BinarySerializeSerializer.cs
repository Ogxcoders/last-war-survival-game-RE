using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace Microsoft.SqlServer.Server;

internal sealed class BinarySerializeSerializer : Serializer
{
	internal BinarySerializeSerializer(Type t)
		: base(t)
	{
	}

	public override void Serialize(Stream s, object o)
	{
		BinaryWriter w = new BinaryWriter(s);
		((IBinarySerialize)o).Write(w);
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override object Deserialize(Stream s)
	{
		object obj = Activator.CreateInstance(_type);
		BinaryReader r = new BinaryReader(s);
		((IBinarySerialize)obj).Read(r);
		return obj;
	}
}
