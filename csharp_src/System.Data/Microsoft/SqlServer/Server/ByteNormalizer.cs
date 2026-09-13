using System.IO;
using System.Reflection;

namespace Microsoft.SqlServer.Server;

internal sealed class ByteNormalizer : Normalizer
{
	internal override int Size => 1;

	internal override void Normalize(FieldInfo fi, object obj, Stream s)
	{
		byte value = (byte)GetValue(fi, obj);
		s.WriteByte(value);
	}

	internal override void DeNormalize(FieldInfo fi, object recvr, Stream s)
	{
		byte b = (byte)s.ReadByte();
		SetValue(fi, recvr, b);
	}
}
