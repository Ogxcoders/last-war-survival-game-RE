using System.IO;
using System.Reflection;

namespace Microsoft.SqlServer.Server;

internal sealed class BooleanNormalizer : Normalizer
{
	internal override int Size => 1;

	internal override void Normalize(FieldInfo fi, object obj, Stream s)
	{
		bool flag = (bool)GetValue(fi, obj);
		s.WriteByte((byte)(flag ? 1u : 0u));
	}

	internal override void DeNormalize(FieldInfo fi, object recvr, Stream s)
	{
		byte b = (byte)s.ReadByte();
		SetValue(fi, recvr, b == 1);
	}
}
