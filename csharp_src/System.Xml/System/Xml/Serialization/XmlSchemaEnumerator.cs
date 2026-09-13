using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;

namespace System.Xml.Serialization;

public class XmlSchemaEnumerator : IEnumerator<XmlSchema>, IDisposable, IEnumerator
{
	private XmlSchemas list;

	private int idx;

	private int end;

	public XmlSchema Current => list[idx];

	object IEnumerator.Current => list[idx];

	public XmlSchemaEnumerator(XmlSchemas list)
	{
		this.list = list;
		idx = -1;
		end = list.Count - 1;
	}

	public void Dispose()
	{
	}

	public bool MoveNext()
	{
		if (idx >= end)
		{
			return false;
		}
		idx++;
		return true;
	}

	void IEnumerator.Reset()
	{
		idx = -1;
	}
}
