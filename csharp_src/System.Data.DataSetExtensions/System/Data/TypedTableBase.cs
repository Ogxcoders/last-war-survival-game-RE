using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace System.Data;

[Serializable]
public abstract class TypedTableBase<T> : DataTable, IEnumerable<T>, IEnumerable where T : DataRow
{
	protected TypedTableBase()
	{
	}

	protected TypedTableBase(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public IEnumerator<T> GetEnumerator()
	{
		return base.Rows.Cast<T>().GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	public EnumerableRowCollection<TResult> Cast<TResult>()
	{
		return new EnumerableRowCollection<T>(this).Cast<TResult>();
	}
}
