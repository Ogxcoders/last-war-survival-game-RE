using System.Collections;

namespace System.Xml;

internal class XmlNodeListEnumerator : IEnumerator
{
	private XPathNodeList list;

	private int index;

	private bool valid;

	public object Current
	{
		get
		{
			if (valid)
			{
				return list[index];
			}
			return null;
		}
	}

	public XmlNodeListEnumerator(XPathNodeList list)
	{
		this.list = list;
		index = -1;
		valid = false;
	}

	public void Reset()
	{
		index = -1;
	}

	public bool MoveNext()
	{
		index++;
		if (list.ReadUntil(index + 1) - 1 < index)
		{
			return false;
		}
		valid = list[index] != null;
		return valid;
	}
}
