using System.Collections;
using System.Collections.Generic;

namespace System.Xml.Xsl;

internal struct IListEnumerator<T> : IEnumerator<T>, IDisposable, IEnumerator
{
	private IList<T> sequence;

	private int index;

	private T current;

	public T Current => current;

	object IEnumerator.Current
	{
		get
		{
			if (index == 0)
			{
				throw new InvalidOperationException(Res.GetString("Enumeration has not started. Call MoveNext.", string.Empty));
			}
			if (index > sequence.Count)
			{
				throw new InvalidOperationException(Res.GetString("Enumeration has already finished.", string.Empty));
			}
			return current;
		}
	}

	public IListEnumerator(IList<T> sequence)
	{
		this.sequence = sequence;
		index = 0;
		current = default(T);
	}

	public void Dispose()
	{
	}

	public bool MoveNext()
	{
		if (index < sequence.Count)
		{
			current = sequence[index];
			index++;
			return true;
		}
		current = default(T);
		return false;
	}

	void IEnumerator.Reset()
	{
		index = 0;
		current = default(T);
	}
}
