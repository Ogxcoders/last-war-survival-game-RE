using System.Collections;
using System.Security.Permissions;

namespace System.ComponentModel;

[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
internal class ArraySubsetEnumerator : IEnumerator
{
	private Array array;

	private int total;

	private int current;

	public object Current
	{
		get
		{
			if (current == -1)
			{
				throw new InvalidOperationException();
			}
			return array.GetValue(current);
		}
	}

	public ArraySubsetEnumerator(Array array, int count)
	{
		this.array = array;
		total = count;
		current = -1;
	}

	public bool MoveNext()
	{
		if (current < total - 1)
		{
			current++;
			return true;
		}
		return false;
	}

	public void Reset()
	{
		current = -1;
	}
}
