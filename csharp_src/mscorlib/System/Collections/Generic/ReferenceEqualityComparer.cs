using System.Runtime.CompilerServices;

namespace System.Collections.Generic;

internal sealed class ReferenceEqualityComparer<T> : IEqualityComparer<T> where T : class
{
	internal static readonly ReferenceEqualityComparer<T> Instance = new ReferenceEqualityComparer<T>();

	private ReferenceEqualityComparer()
	{
	}

	public bool Equals(T x, T y)
	{
		return x == y;
	}

	public int GetHashCode(T obj)
	{
		return RuntimeHelpers.GetHashCode(obj);
	}
}
