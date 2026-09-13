using System.Collections;
using System.Collections.Generic;

namespace System.Data.SqlClient;

[Serializable]
public sealed class SqlErrorCollection : ICollection, IEnumerable
{
	private readonly List<object> _errors = new List<object>();

	public int Count => _errors.Count;

	object ICollection.SyncRoot => this;

	bool ICollection.IsSynchronized => false;

	public SqlError this[int index] => (SqlError)_errors[index];

	internal SqlErrorCollection()
	{
	}

	public void CopyTo(Array array, int index)
	{
		((ICollection)_errors).CopyTo(array, index);
	}

	public void CopyTo(SqlError[] array, int index)
	{
		_errors.CopyTo(array, index);
	}

	public IEnumerator GetEnumerator()
	{
		return _errors.GetEnumerator();
	}

	internal void Add(SqlError error)
	{
		_errors.Add(error);
	}
}
