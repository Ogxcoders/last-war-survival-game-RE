using System.Collections;

namespace Sfs2XLw.Util;

public class XMLNodeList : ArrayList
{
	public XMLNode Pop()
	{
		XMLNode xMLNode = null;
		xMLNode = (XMLNode)this[Count - 1];
		Remove(xMLNode);
		return xMLNode;
	}

	public int Push(XMLNode item)
	{
		Add(item);
		return Count;
	}
}
