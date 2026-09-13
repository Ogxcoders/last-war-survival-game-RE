using System;
using System.Reflection;

namespace Microsoft.SqlServer.Server;

internal sealed class FieldInfoEx : IComparable
{
	internal readonly int Offset;

	internal readonly FieldInfo FieldInfo;

	internal readonly Normalizer Normalizer;

	internal FieldInfoEx(FieldInfo fi, int offset, Normalizer normalizer)
	{
		FieldInfo = fi;
		Offset = offset;
		Normalizer = normalizer;
	}

	public int CompareTo(object other)
	{
		if (!(other is FieldInfoEx fieldInfoEx))
		{
			return -1;
		}
		return Offset.CompareTo(fieldInfoEx.Offset);
	}
}
