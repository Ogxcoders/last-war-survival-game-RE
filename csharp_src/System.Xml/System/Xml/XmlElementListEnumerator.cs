using System.Collections;

namespace System.Xml;

internal class XmlElementListEnumerator : IEnumerator
{
	private XmlElementList list;

	private XmlNode curElem;

	private int changeCount;

	public object Current => curElem;

	public XmlElementListEnumerator(XmlElementList list)
	{
		this.list = list;
		curElem = null;
		changeCount = list.ChangeCount;
	}

	public bool MoveNext()
	{
		if (list.ChangeCount != changeCount)
		{
			throw new InvalidOperationException(Res.GetString("The element list has changed. The enumeration operation failed to continue."));
		}
		curElem = list.GetNextNode(curElem);
		return curElem != null;
	}

	public void Reset()
	{
		curElem = null;
		changeCount = list.ChangeCount;
	}
}
