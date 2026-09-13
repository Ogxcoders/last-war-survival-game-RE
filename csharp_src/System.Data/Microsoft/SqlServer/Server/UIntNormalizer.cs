using System;
using System.IO;
using System.Reflection;

namespace Microsoft.SqlServer.Server;

internal sealed class UIntNormalizer : Normalizer
{
	internal override int Size => 4;

	internal override void Normalize(FieldInfo fi, object obj, Stream s)
	{
		byte[] bytes = BitConverter.GetBytes((uint)GetValue(fi, obj));
		if (!_skipNormalize)
		{
			Array.Reverse(bytes);
		}
		s.Write(bytes, 0, bytes.Length);
	}

	internal override void DeNormalize(FieldInfo fi, object recvr, Stream s)
	{
		byte[] array = new byte[4];
		s.Read(array, 0, array.Length);
		if (!_skipNormalize)
		{
			Array.Reverse(array);
		}
		SetValue(fi, recvr, BitConverter.ToUInt32(array, 0));
	}
}
