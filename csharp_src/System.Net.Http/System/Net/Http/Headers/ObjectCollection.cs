using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace System.Net.Http.Headers;

internal sealed class ObjectCollection<T> : Collection<T> where T : class
{
	private static readonly Action<T> s_defaultValidator = CheckNotNull;

	private readonly Action<T> _validator;

	public ObjectCollection()
		: this(s_defaultValidator)
	{
	}

	public ObjectCollection(Action<T> validator)
		: base((IList<T>)new List<T>())
	{
		_validator = validator;
	}

	public new List<T>.Enumerator GetEnumerator()
	{
		return ((List<T>)base.Items).GetEnumerator();
	}

	protected override void InsertItem(int index, T item)
	{
		_validator(item);
		base.InsertItem(index, item);
	}

	protected override void SetItem(int index, T item)
	{
		_validator(item);
		base.SetItem(index, item);
	}

	private static void CheckNotNull(T item)
	{
		if (item == null)
		{
			throw new ArgumentNullException("item");
		}
	}
}
