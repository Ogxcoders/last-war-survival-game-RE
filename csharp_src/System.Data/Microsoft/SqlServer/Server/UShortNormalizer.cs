using System;
using System.IO;
using System.Reflection;

namespace Microsoft.SqlServer.Server;

internal sealed class UShortNormalizer : Normalizer
{
	internal override int Size => 2;

	internal override void Normalize(FieldInfo fi, object obj, Stream s)
	{
		byte[] bytes = BitConverter.GetBytes((ushort)GetValue(fi, obj));
		if (!_skipNormalize)
		{
			Array.Reverse(bytes);
		}
		s.Write(bytes, 0, bytes.Length);
	}

	internal override void DeNormalize(FieldInfo fi, object recvr, Stream s)
	{
		byte[] array = new byte[2];
		s.Read(array, 0, array.Length);
		if (!_skipNormalize)
		{
			Array.Reverse(array);
		}
		SetValue(fi, recvr, BitConverter.ToUInt16(array, 0));
	}
}
