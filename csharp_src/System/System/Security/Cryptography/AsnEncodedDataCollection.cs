using System.Collections;
using System.Collections.Generic;

namespace System.Security.Cryptography;

public sealed class AsnEncodedDataCollection : ICollection, IEnumerable
{
	private readonly List<AsnEncodedData> _list;

	public AsnEncodedData this[int index] => _list[index];

	public int Count => _list.Count;

	public bool IsSynchronized => false;

	public object SyncRoot => this;

	public AsnEncodedDataCollection()
	{
		_list = new List<AsnEncodedData>();
	}

	public AsnEncodedDataCollection(AsnEncodedData asnEncodedData)
		: this()
	{
		_list.Add(asnEncodedData);
	}

	public int Add(AsnEncodedData asnEncodedData)
	{
		if (asnEncodedData == null)
		{
			throw new ArgumentNullException("asnEncodedData");
		}
		int count = _list.Count;
		_list.Add(asnEncodedData);
		return count;
	}

	public void Remove(AsnEncodedData asnEncodedData)
	{
		if (asnEncodedData == null)
		{
			throw new ArgumentNullException("asnEncodedData");
		}
		_list.Remove(asnEncodedData);
	}

	public AsnEncodedDataEnumerator GetEnumerator()
	{
		return new AsnEncodedDataEnumerator(this);
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	void ICollection.CopyTo(Array array, int index)
	{
		if (array == null)
		{
			throw new ArgumentNullException("array");
		}
		if (array.Rank != 1)
		{
			throw new ArgumentException("Only single dimensional arrays are supported for the requested action.");
		}
		if (index < 0 || index >= array.Length)
		{
			throw new ArgumentOutOfRangeException("index", "Index was out of range. Must be non-negative and less than the size of the collection.");
		}
		if (Count > array.Length - index)
		{
			throw new ArgumentException("Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection.");
		}
		for (int i = 0; i < Count; i++)
		{
			array.SetValue(this[i], index);
			index++;
		}
	}

	public void CopyTo(AsnEncodedData[] array, int index)
	{
		if (array == null)
		{
			throw new ArgumentNullException("array");
		}
		if (index < 0 || index >= array.Length)
		{
			throw new ArgumentOutOfRangeException("index", "Index was out of range. Must be non-negative and less than the size of the collection.");
		}
		_list.CopyTo(array, index);
	}
}
