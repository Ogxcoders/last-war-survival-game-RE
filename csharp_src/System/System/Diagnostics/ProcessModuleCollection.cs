using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace System.Diagnostics;

public class ProcessModuleCollection : ReadOnlyCollectionBase
{
	public ProcessModule this[int index] => (ProcessModule)base.InnerList[index];

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

	protected ProcessModuleCollection()
	{
	}

	public ProcessModuleCollection(ProcessModule[] processModules)
	{
		base.InnerList.AddRange(processModules);
	}

	public int IndexOf(ProcessModule module)
	{
		return base.InnerList.IndexOf(module);
	}

	public bool Contains(ProcessModule module)
	{
		return base.InnerList.Contains(module);
	}

	public void CopyTo(ProcessModule[] array, int index)
	{
		base.InnerList.CopyTo(array, index);
	}

	[Obsolete("This API is no longer available", true)]
	public void Add(ProcessModule item)
	{
		throw new NotSupportedException();
	}

	[Obsolete("This API is no longer available", true)]
	public void AddRange(IEnumerable<ProcessModule> collection)
	{
		throw new NotSupportedException();
	}

	[Obsolete("This API is no longer available", true)]
	public ReadOnlyCollection<ProcessModule> AsReadOnly()
	{
		throw new NotSupportedException();
	}

	[Obsolete("This API is no longer available", true)]
	public int BinarySearch(int index, int count, ProcessModule item, IComparer<ProcessModule> comparer)
	{
		throw new NotSupportedException();
	}

	[Obsolete("This API is no longer available", true)]
	public int BinarySearch(ProcessModule item)
	{
		throw new NotSupportedException();
	}

	[Obsolete("This API is no longer available", true)]
	public int BinarySearch(ProcessModule item, IComparer<ProcessModule> comparer)
	{
		throw new NotSupportedException();
	}

	[Obsolete("This API is no longer available", true)]
	public void Clear()
	{
		throw new NotSupportedException();
	}

	[Obsolete("This API is no longer available", true)]
	public List<TOutput> ConvertAll<TOutput>(Converter<ProcessModule, TOutput> converter)
	{
		throw new NotSupportedException();
	}

	[Obsolete("This API is no longer available", true)]
	public void CopyTo(ProcessModule[] array)
	{
		throw new NotSupportedException();
	}

	[Obsolete("This API is no longer available", true)]
	public void CopyTo(int index, ProcessModule[] array, int arrayIndex, int count)
	{
		throw new NotSupportedException();
	}

	[Obsolete("This API is no longer available", true)]
	public bool Exists(Predicate<ProcessModule> match)
	{
		throw new NotSupportedException();
	}

	[Obsolete("This API is no longer available", true)]
	public ProcessModule Find(Predicate<ProcessModule> match)
	{
		throw new NotSupportedException();
	}

	[Obsolete("This API is no longer available", true)]
	public List<ProcessModule> FindAll(Predicate<ProcessModule> match)
	{
		throw new NotSupportedException();
	}

	[Obsolete("This API is no longer available", true)]
	public int FindIndex(Predicate<ProcessModule> match)
	{
		throw new NotSupportedException();
	}

	[Obsolete("This API is no longer available", true)]
	public int FindIndex(int startIndex, Predicate<ProcessModule> match)
	{
		throw new NotSupportedException();
	}

	[Obsolete("This API is no longer available", true)]
	public int FindIndex(int startIndex, int count, Predicate<ProcessModule> match)
	{
		throw new NotSupportedException();
	}

	[Obsolete("This API is no longer available", true)]
	public ProcessModule FindLast(Predicate<ProcessModule> match)
	{
		throw new NotSupportedException();
	}

	[Obsolete("This API is no longer available", true)]
	public int FindLastIndex(Predicate<ProcessModule> match)
	{
		throw new NotSupportedException();
	}

	[Obsolete("This API is no longer available", true)]
	public int FindLastIndex(int startIndex, Predicate<ProcessModule> match)
	{
		throw new NotSupportedException();
	}

	[Obsolete("This API is no longer available", true)]
	public int FindLastIndex(int startIndex, int count, Predicate<ProcessModule> match)
	{
		throw new NotSupportedException();
	}

	[Obsolete("This API is no longer available", true)]
	public void ForEach(Action<ProcessModule> action)
	{
		throw new NotSupportedException();
	}

	[Obsolete("This API is no longer available", true)]
	public List<ProcessModule> GetRange(int index, int count)
	{
		throw new NotSupportedException();
	}

	[Obsolete("This API is no longer available", true)]
	public int IndexOf(ProcessModule item, int index)
	{
		throw new NotSupportedException();
	}

	[Obsolete("This API is no longer available", true)]
	public int IndexOf(ProcessModule item, int index, int count)
	{
		throw new NotSupportedException();
	}

	[Obsolete("This API is no longer available", true)]
	public void Insert(int index, ProcessModule item)
	{
		throw new NotSupportedException();
	}

	[Obsolete("This API is no longer available", true)]
	public void InsertRange(int index, IEnumerable<ProcessModule> collection)
	{
		throw new NotSupportedException();
	}

	[Obsolete("This API is no longer available", true)]
	public int LastIndexOf(ProcessModule item)
	{
		throw new NotSupportedException();
	}

	[Obsolete("This API is no longer available", true)]
	public int LastIndexOf(ProcessModule item, int index)
	{
		throw new NotSupportedException();
	}

	[Obsolete("This API is no longer available", true)]
	public int LastIndexOf(ProcessModule item, int index, int count)
	{
		throw new NotSupportedException();
	}

	[Obsolete("This API is no longer available", true)]
	public bool Remove(ProcessModule item)
	{
		throw new NotSupportedException();
	}

	[Obsolete("This API is no longer available", true)]
	public int RemoveAll(Predicate<ProcessModule> match)
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
	public void Sort(IComparer<ProcessModule> comparer)
	{
		throw new NotSupportedException();
	}

	[Obsolete("This API is no longer available", true)]
	public void Sort(int index, int count, IComparer<ProcessModule> comparer)
	{
		throw new NotSupportedException();
	}

	[Obsolete("This API is no longer available", true)]
	public void Sort(Comparison<ProcessModule> comparison)
	{
		throw new NotSupportedException();
	}

	[Obsolete("This API is no longer available", true)]
	public ProcessModule[] ToArray()
	{
		throw new NotSupportedException();
	}

	[Obsolete("This API is no longer available", true)]
	public void TrimExcess()
	{
		throw new NotSupportedException();
	}

	[Obsolete("This API is no longer available", true)]
	public bool TrueForAll(Predicate<ProcessModule> match)
	{
		throw new NotSupportedException();
	}
}
