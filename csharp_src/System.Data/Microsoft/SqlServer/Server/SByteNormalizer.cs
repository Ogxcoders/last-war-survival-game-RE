using System.IO;
using System.Reflection;

namespace Microsoft.SqlServer.Server;

internal sealed class SByteNormalizer : Normalizer
{
	internal override int Size => 1;

	internal override void Normalize(FieldInfo fi, object obj, Stream s)
	{
		byte b = (byte)(sbyte)GetValue(fi, obj);
		if (!_skipNormalize)
		{
			b ^= 0x80;
		}
		s.WriteByte(b);
	}

	internal override void DeNormalize(FieldInfo fi, object recvr, Stream s)
	{
		byte b = (byte)s.ReadByte();
		if (!_skipNormalize)
		{
			b ^= 0x80;
		}
		sbyte b2 = (sbyte)b;
		SetValue(fi, recvr, b2);
	}
}
