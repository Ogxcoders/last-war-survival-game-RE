using System.Collections;

namespace System.Xml;

internal sealed class EmptyEnumerator : IEnumerator
{
	object IEnumerator.Current
	{
		get
		{
			throw new InvalidOperationException(Res.GetString("Operation is not valid due to the current state of the object."));
		}
	}

	bool IEnumerator.MoveNext()
	{
		return false;
	}

	void IEnumerator.Reset()
	{
	}
}
