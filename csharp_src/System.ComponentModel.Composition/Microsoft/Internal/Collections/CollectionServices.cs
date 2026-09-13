using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Microsoft.Internal.Collections;

internal static class CollectionServices
{
	private class CollectionOfObjectList : ICollection<object>, IEnumerable<object>, IEnumerable
	{
		private readonly IList _list;

		public int Count => Assumes.NotReachable<int>();

		public bool IsReadOnly => _list.IsReadOnly;

		public CollectionOfObjectList(IList list)
		{
			_list = list;
		}

		public void Add(object item)
		{
			_list.Add(item);
		}

		public void Clear()
		{
			_list.Clear();
		}

		public bool Contains(object item)
		{
			return Assumes.NotReachable<bool>();
		}

		public void CopyTo(object[] array, int arrayIndex)
		{
			Assumes.NotReachable<object>();
		}

		public bool Remove(object item)
		{
			return Assumes.NotReachable<bool>();
		}

		public IEnumerator<object> GetEnumerator()
		{
			return Assumes.NotReachable<IEnumerator<object>>();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return Assumes.NotReachable<IEnumerator>();
		}
	}

	private class CollectionOfObject<T> : ICollection<object>, IEnumerable<object>, IEnumerable
	{
		private readonly ICollection<T> _collectionOfT;

		public int Count => Assumes.NotReachable<int>();

		public bool IsReadOnly => _collectionOfT.IsReadOnly;

		public CollectionOfObject(object collectionOfT)
		{
			_collectionOfT = (ICollection<T>)collectionOfT;
		}

		public void Add(object item)
		{
			_collectionOfT.Add((T)item);
		}

		public void Clear()
		{
			_collectionOfT.Clear();
		}

		public bool Contains(object item)
		{
			return Assumes.NotReachable<bool>();
		}

		public void CopyTo(object[] array, int arrayIndex)
		{
			Assumes.NotReachable<object>();
		}

		public bool Remove(object item)
		{
			return Assumes.NotReachable<bool>();
		}

		public IEnumerator<object> GetEnumerator()
		{
			return Assumes.NotReachable<IEnumerator<object>>();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return Assumes.NotReachable<IEnumerator>();
		}
	}

	private static readonly Type StringType = typeof(string);

	private static readonly Type IEnumerableType = typeof(IEnumerable);

	private static readonly Type IEnumerableOfTType = typeof(IEnumerable<>);

	private static readonly Type ICollectionOfTType = typeof(ICollection<>);

	public static ICollection<object> GetCollectionWrapper(Type itemType, object collectionObject)
	{
		Assumes.NotNull(itemType, collectionObject);
		Type underlyingSystemType = itemType.UnderlyingSystemType;
		if (underlyingSystemType == typeof(object))
		{
			return (ICollection<object>)collectionObject;
		}
		if (typeof(IList).IsAssignableFrom(collectionObject.GetType()))
		{
			return new CollectionOfObjectList((IList)collectionObject);
		}
		return (ICollection<object>)Activator.CreateInstance(typeof(CollectionOfObject<>).MakeGenericType(underlyingSystemType), collectionObject);
	}

	public static bool IsEnumerableOfT(Type type)
	{
		if (type.IsGenericType && type.GetGenericTypeDefinition().UnderlyingSystemType == IEnumerableOfTType)
		{
			return true;
		}
		return false;
	}

	public static Type GetEnumerableElementType(Type type)
	{
		if (type.UnderlyingSystemType == StringType || !IEnumerableType.IsAssignableFrom(type))
		{
			return null;
		}
		if (ReflectionServices.TryGetGenericInterfaceType(type, IEnumerableOfTType, out var targetClosedInterfaceType))
		{
			return targetClosedInterfaceType.GetGenericArguments()[0];
		}
		return null;
	}

	public static Type GetCollectionElementType(Type type)
	{
		if (ReflectionServices.TryGetGenericInterfaceType(type, ICollectionOfTType, out var targetClosedInterfaceType))
		{
			return targetClosedInterfaceType.GetGenericArguments()[0];
		}
		return null;
	}

	public static ReadOnlyCollection<T> ToReadOnlyCollection<T>(this IEnumerable<T> source)
	{
		Assumes.NotNull(source);
		return new ReadOnlyCollection<T>(source.AsArray());
	}

	public static IEnumerable<T> ConcatAllowingNull<T>(this IEnumerable<T> source, IEnumerable<T> second)
	{
		if (second == null || !second.FastAny())
		{
			return source;
		}
		if (source == null || !source.FastAny())
		{
			return second;
		}
		return source.Concat(second);
	}

	public static ICollection<T> ConcatAllowingNull<T>(this ICollection<T> source, ICollection<T> second)
	{
		if (second == null || second.Count == 0)
		{
			return source;
		}
		if (source == null || source.Count == 0)
		{
			return second;
		}
		List<T> list = new List<T>(source);
		list.AddRange(second);
		return list;
	}

	public static List<T> FastAppendToListAllowNulls<T>(this List<T> source, IEnumerable<T> second)
	{
		if (second == null)
		{
			return source;
		}
		if (source == null || source.Count == 0)
		{
			return second.AsList();
		}
		if (second is List<T> list)
		{
			if (list.Count == 0)
			{
				return source;
			}
			if (list.Count == 1)
			{
				source.Add(list[0]);
				return source;
			}
		}
		source.AddRange(second);
		return source;
	}

	public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
	{
		foreach (T item in source)
		{
			action(item);
		}
	}

	public static EnumerableCardinality GetCardinality<T>(this IEnumerable<T> source)
	{
		Assumes.NotNull(source);
		if (source is ICollection { Count: var count })
		{
			return count switch
			{
				0 => EnumerableCardinality.Zero, 
				1 => EnumerableCardinality.One, 
				_ => EnumerableCardinality.TwoOrMore, 
			};
		}
		using IEnumerator<T> enumerator = source.GetEnumerator();
		if (!enumerator.MoveNext())
		{
			return EnumerableCardinality.Zero;
		}
		if (!enumerator.MoveNext())
		{
			return EnumerableCardinality.One;
		}
		return EnumerableCardinality.TwoOrMore;
	}

	public static bool FastAny<T>(this IEnumerable<T> source)
	{
		if (source is ICollection collection)
		{
			return collection.Count > 0;
		}
		return source.Any();
	}

	public static Stack<T> Copy<T>(this Stack<T> stack)
	{
		Assumes.NotNull(stack);
		return new Stack<T>(stack.Reverse());
	}

	public static T[] AsArray<T>(this IEnumerable<T> enumerable)
	{
		if (enumerable is T[] result)
		{
			return result;
		}
		return enumerable.ToArray();
	}

	public static List<T> AsList<T>(this IEnumerable<T> enumerable)
	{
		if (enumerable is List<T> result)
		{
			return result;
		}
		return enumerable.ToList();
	}

	public static bool IsArrayEqual<T>(this T[] thisArray, T[] thatArray)
	{
		if (thisArray.Length != thatArray.Length)
		{
			return false;
		}
		for (int i = 0; i < thisArray.Length; i++)
		{
			ref readonly T reference = ref thisArray[i];
			object obj = thatArray[i];
			if (!reference.Equals(obj))
			{
				return false;
			}
		}
		return true;
	}

	public static bool IsCollectionEqual<T>(this IList<T> thisList, IList<T> thatList)
	{
		if (thisList.Count != thatList.Count)
		{
			return false;
		}
		for (int i = 0; i < thisList.Count; i++)
		{
			if (!thisList[i].Equals(thatList[i]))
			{
				return false;
			}
		}
		return true;
	}
}
