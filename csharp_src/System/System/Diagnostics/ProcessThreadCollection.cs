using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace System.Diagnostics;

public class ProcessThreadCollection : ReadOnlyCollectionBase
{
	public ProcessThread this[int index] => (ProcessThread)base.InnerList[index];

	[Obsolete("This API is no longer available", true)]
	public int Capacity
	{
		get
		{
			throw new NotSupportedException();
		}
		set
		{
			throw new NotSupportedException();
		}
	}

	protected ProcessThreadCollection()
	{
	}

	public ProcessThreadCollection(ProcessThread[] processThreads)
	{
		base.InnerList.AddRange(processThreads);
	}

	public int Add(ProcessThread thread)
	{
		return base.InnerList.Add(thread);
	}

	public void Insert(int index, ProcessThread thread)
	{
		base.InnerList.Insert(index, thread);
	}

	public int IndexOf(ProcessThread thread)
	{
		return base.InnerList.IndexOf(thread);
	}

	public bool Contains(ProcessThread thread)
	{
		return base.InnerList.Contains(thread);
	}

	public void Remove(ProcessThread thread)
	{
		base.InnerList.Remove(thread);
	}

	public void CopyTo(ProcessThread[] array, int index)
	{
		base.InnerList.CopyTo(array, index);
	}

	[Obsolete("This API is no longer available", true)]
	public void AddRange(IEnumerable<ProcessThread> collection)
	{
		throw new NotSupportedException();
	}

	[Obsolete("This API is no longer available", true)]
	public ReadOnlyCollection<ProcessThread> AsReadOnly()
	{
		throw new NotSupportedException();
	}

	[Obsolete("This API is no longer available", true)]
	public int BinarySearch(int index, int count, ProcessThread item, IComparer<ProcessThread> comparer)
	{
		throw new NotSupportedException();
	}

	[Obsolete("This API is no longer available", true)]
	public int BinarySearch(ProcessThread item)
	{
		throw new NotSupportedException();
	}

	[Obsolete("This API is no longer available", true)]
	public int BinarySearch(ProcessThread item, IComparer<ProcessThread> comparer)
	{
		throw new NotSupportedException();
	}

	[Obsolete("This API is no longer available", true)]
	public void Clear()
	{
		throw new NotSupportedException();
	}

	[Obsolete("This API is no longer available", true)]
	public List<TOutput> ConvertAll<TOutput>(Converter<ProcessThread, TOutput> converter)
	{
		throw new NotSupportedException();
	}

	[Obsolete("This API is no longer available", true)]
	public void CopyTo(ProcessThread[] array)
	{
		throw new NotSupportedException();
	}

	[Obsolete("This API is no longer available", true)]
	public void CopyTo(int index, ProcessThread[] array, int arrayIndex, int count)
	{
		throw new NotSupportedException();
	}

	[Obsolete("This API is no longer available", true)]
	public bool Exists(Predicate<ProcessThread> match)
	{
		throw new NotSupportedException();
	}

	[Obsolete("This API is no longer available", true)]
	public ProcessThread Find(Predicate<ProcessThread> match)
	{
		throw new NotSupportedException();
	}

	[Obsolete("This API is no longer available", true)]
	public List<ProcessThread> FindAll(Predicate<ProcessThread> match)
	{
		throw new NotSupportedException();
	}

	[Obsolete("This API is no longer available", true)]
	public int FindIndex(Predicate<ProcessThread> match)
	{
		throw new NotSupportedException();
	}

	[Obsolete("This API is no longer available", true)]
	public int FindIndex(int startIndex, Predicate<ProcessThread> match)
	{
		throw new NotSupportedException();
	}

	[Obsolete("This API is no longer available", true)]
	public int FindIndex(int startIndex, int count, Predicate<ProcessThread> match)
	{
		throw new NotSupportedException();
	}

	[Obsolete("This API is no longer available", true)]
	public ProcessThread FindLast(Predicate<ProcessThread> match)
	{
		throw new NotSupportedException();
	}

	[Obsolete("This API is no longer available", true)]
	public int FindLastIndex(Predicate<ProcessThread> match)
	{
		throw new NotSupportedException();
	}

	[Obsolete("This API is no longer available", true)]
	public int FindLastIndex(int startIndex, Predicate<ProcessThread> match)
	{
		throw new NotSupportedException();
	}

	[Obsolete("This API is no longer available", true)]
	public int FindLastIndex(int startIndex, int count, Predicate<ProcessThread> match)
	{
		throw new NotSupportedException();
	}

	[Obsolete("This API is no longer available", true)]
	public void ForEach(Action<ProcessThread> action)
	{
		throw new NotSupportedException();
	}

	[Obsolete("This API is no longer available", true)]
	public List<ProcessThread> GetRange(int index, int count)
	{
		throw new NotSupportedException();
	}

	[Obsolete("This API is no longer available", true)]
	public int IndexOf(ProcessThread item, int index)
	{
		throw new NotSupportedException();
	}

	[Obsolete("This API is no longer available", true)]
	public int IndexOf(ProcessThread item, int index, int count)
	{
		throw new NotSupportedException();
	}

	[Obsolete("This API is no longer available", true)]
	public void InsertRange(int index, IEnumerable<ProcessThread> collection)
	{
		throw new NotSupportedException();
	}

	[Obsolete("This API is no longer available", true)]
	public int LastIndexOf(ProcessThread item)
	{
		throw new NotSupportedException();
	}

	[Obsolete("This API is no longer available", true)]
	public int LastIndexOf(ProcessThread item, int index)
	{
		throw new NotSupportedException();
	}

	[Obsolete("This API is no longer available", true)]
	public int LastIndexOf(ProcessThread item, int index, int count)
	{
		throw new NotSupportedException();
	}

	[Obsolete("This API is no longer available", true)]
	public int RemoveAll(Predicate<ProcessThread> match)
	{
		throw new NotSupportedException();
	}

	[Obsolete("This API is no longer available", true)]
	public void RemoveAt(int index)
	{
		throw new NotSupportedException();
	}

	[Obsolete("This API is no longer available", true)]
	public void RemoveRange(int index, int count)
	{
		throw new NotSupportedException();
	}

	[Obsolete("This API is no longer available", true)]
	public void Reverse()
	{
		throw new NotSupportedException();
	}

	[Obsolete("This API is no longer available", true)]
	public void Reverse(int index, int count)
	{
		throw new NotSupportedException();
	}

	[Obsolete("This API is no longer available", true)]
	public void Sort()
	{
		throw new NotSupportedException();
	}

	[Obsolete("This API is no longer available", true)]
	public void Sort(IComparer<ProcessThread> comparer)
	{
		throw new NotSupportedException();
	}

	[Obsolete("This API is no longer available", true)]
	public void Sort(int index, int count, IComparer<ProcessThread> comparer)
	{
		throw new NotSupportedException();
	}

	[Obsolete("This API is no longer available", true)]
	public void Sort(Comparison<ProcessThread> comparison)
	{
		throw new NotSupportedException();
	}

	[Obsolete("This API is no longer available", true)]
	public ProcessThread[] ToArray()
	{
		throw new NotSupportedException();
	}

	[Obsolete("This API is no longer available", true)]
	public void TrimExcess()
	{
		throw new NotSupportedException();
	}

	[Obsolete("This API is no longer available", true)]
	public bool TrueForAll(Predicate<ProcessThread> match)
	{
		throw new NotSupportedException();
	}
}
