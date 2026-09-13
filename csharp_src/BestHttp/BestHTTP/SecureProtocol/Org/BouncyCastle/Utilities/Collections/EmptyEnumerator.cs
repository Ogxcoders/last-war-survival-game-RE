using System;
using System.Collections;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Collections;

public sealed class EmptyEnumerator : IEnumerator
{
	public static readonly IEnumerator Instance = new EmptyEnumerator();

	public object Current
	{
		get
		{
			throw new InvalidOperationException("No elements");
		}
	}

	private EmptyEnumerator()
	{
	}

	public bool MoveNext()
	{
		return false;
	}

	public void Reset()
	{
	}
}
