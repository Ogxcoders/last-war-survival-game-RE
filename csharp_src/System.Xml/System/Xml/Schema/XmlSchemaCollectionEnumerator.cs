using System.Collections;

namespace System.Xml.Schema;

public sealed class XmlSchemaCollectionEnumerator : IEnumerator
{
	private IDictionaryEnumerator enumerator;

	object IEnumerator.Current => Current;

	public XmlSchema Current => ((XmlSchemaCollectionNode)enumerator.Value)?.Schema;

	internal XmlSchemaCollectionNode CurrentNode => (XmlSchemaCollectionNode)enumerator.Value;

	internal XmlSchemaCollectionEnumerator(Hashtable collection)
	{
		enumerator = collection.GetEnumerator();
	}

	void IEnumerator.Reset()
	{
		enumerator.Reset();
	}

	bool IEnumerator.MoveNext()
	{
		return enumerator.MoveNext();
	}

	public bool MoveNext()
	{
		return enumerator.MoveNext();
	}
}
