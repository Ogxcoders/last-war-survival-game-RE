using System.Collections.Generic;
using System.Reflection;

public static class HashSetExtensions
{
	private static class HashSetDelegateHolder<T>
	{
		private const BindingFlags Flags = BindingFlags.Instance | BindingFlags.NonPublic;

		public static MethodInfo InitializeMethod { get; } = typeof(HashSet<T>).GetMethod("Initialize", BindingFlags.Instance | BindingFlags.NonPublic);
	}

	public static void SetCapacity<T>(this HashSet<T> hs, int capacity)
	{
		HashSetDelegateHolder<T>.InitializeMethod.Invoke(hs, new object[1] { capacity });
	}

	public static HashSet<T> GetHashSet<T>(int capacity)
	{
		HashSet<T> hashSet = new HashSet<T>();
		hashSet.SetCapacity(capacity);
		return hashSet;
	}
}
