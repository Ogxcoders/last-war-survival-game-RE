using System.Collections;

namespace System.Xml.Schema;

public class XmlSchemaObjectEnumerator : IEnumerator
{
	private IEnumerator enumerator;

	public XmlSchemaObject Current => (XmlSchemaObject)enumerator.Current;

	object IEnumerator.Current => enumerator.Current;

	internal XmlSchemaObjectEnumerator(IEnumerator enumerator)
	{
		this.enumerator = enumerator;
	}

	public void Reset()
	{
		enumerator.Reset();
	}

	public bool MoveNext()
	{
		return enumerator.MoveNext();
	}

	void IEnumerator.Reset()
	{
		enumerator.Reset();
	}

	bool IEnumerator.MoveNext()
	{
		return enumerator.MoveNext();
	}
}
