using System.Collections;

namespace System.Xml;

internal class XmlElementList : XmlNodeList
{
	private string asterisk;

	private int changeCount;

	private string name;

	private string localName;

	private string namespaceURI;

	private XmlNode rootNode;

	private int curInd;

	private XmlNode curElem;

	private bool empty;

	private bool atomized;

	private int matchCount;

	private WeakReference listener;

	internal int ChangeCount => changeCount;

	public override int Count
	{
		get
		{
			if (empty)
			{
				return 0;
			}
			if (matchCount < 0)
			{
				int num = 0;
				int num2 = changeCount;
				XmlNode matchingNode = rootNode;
				while ((matchingNode = GetMatchingNode(matchingNode, bNext: true)) != null)
				{
					num++;
				}
				if (num2 != changeCount)
				{
					return num;
				}
				matchCount = num;
			}
			return matchCount;
		}
	}

	private XmlElementList(XmlNode parent)
	{
		rootNode = parent;
		curInd = -1;
		curElem = rootNode;
		changeCount = 0;
		empty = false;
		atomized = true;
		matchCount = -1;
		listener = new WeakReference(new XmlElementListListener(parent.Document, this));
	}

	~XmlElementList()
	{
		Dispose(disposing: false);
	}

	internal void ConcurrencyCheck(XmlNodeChangedEventArgs args)
	{
		if (!atomized)
		{
			XmlNameTable nameTable = rootNode.Document.NameTable;
			localName = nameTable.Add(localName);
			namespaceURI = nameTable.Add(namespaceURI);
			atomized = true;
		}
		if (IsMatch(args.Node))
		{
			changeCount++;
			curInd = -1;
			curElem = rootNode;
			if (args.Action == XmlNodeChangedAction.Insert)
			{
				empty = false;
			}
		}
		matchCount = -1;
	}

	internal XmlElementList(XmlNode parent, string name)
		: this(parent)
	{
		XmlNameTable nameTable = parent.Document.NameTable;
		asterisk = nameTable.Add("*");
		this.name = nameTable.Add(name);
		localName = null;
		namespaceURI = null;
	}

	internal XmlElementList(XmlNode parent, string localName, string namespaceURI)
		: this(parent)
	{
		XmlNameTable nameTable = parent.Document.NameTable;
		asterisk = nameTable.Add("*");
		this.localName = nameTable.Get(localName);
		this.namespaceURI = nameTable.Get(namespaceURI);
		if (this.localName == null || this.namespaceURI == null)
		{
			empty = true;
			atomized = false;
			this.localName = localName;
			this.namespaceURI = namespaceURI;
		}
		name = null;
	}

	private XmlNode NextElemInPreOrder(XmlNode curNode)
	{
		XmlNode xmlNode = curNode.FirstChild;
		if (xmlNode == null)
		{
			xmlNode = curNode;
			while (xmlNode != null && xmlNode != rootNode && xmlNode.NextSibling == null)
			{
				xmlNode = xmlNode.ParentNode;
			}
			if (xmlNode != null && xmlNode != rootNode)
			{
				xmlNode = xmlNode.NextSibling;
			}
		}
		if (xmlNode == rootNode)
		{
			xmlNode = null;
		}
		return xmlNode;
	}

	private XmlNode PrevElemInPreOrder(XmlNode curNode)
	{
		XmlNode xmlNode = curNode.PreviousSibling;
		while (xmlNode != null && xmlNode.LastChild != null)
		{
			xmlNode = xmlNode.LastChild;
		}
		if (xmlNode == null)
		{
			xmlNode = curNode.ParentNode;
		}
		if (xmlNode == rootNode)
		{
			xmlNode = null;
		}
		return xmlNode;
	}

	private bool IsMatch(XmlNode curNode)
	{
		if (curNode.NodeType == XmlNodeType.Element)
		{
			if (name != null)
			{
				if (Ref.Equal(name, asterisk) || Ref.Equal(curNode.Name, name))
				{
					return true;
				}
			}
			else if ((Ref.Equal(localName, asterisk) || Ref.Equal(curNode.LocalName, localName)) && (Ref.Equal(namespaceURI, asterisk) || curNode.NamespaceURI == namespaceURI))
			{
				return true;
			}
		}
		return false;
	}

	private XmlNode GetMatchingNode(XmlNode n, bool bNext)
	{
		XmlNode xmlNode = n;
		do
		{
			xmlNode = ((!bNext) ? PrevElemInPreOrder(xmlNode) : NextElemInPreOrder(xmlNode));
		}
		while (xmlNode != null && !IsMatch(xmlNode));
		return xmlNode;
	}

	private XmlNode GetNthMatchingNode(XmlNode n, bool bNext, int nCount)
	{
		XmlNode xmlNode = n;
		for (int i = 0; i < nCount; i++)
		{
			xmlNode = GetMatchingNode(xmlNode, bNext);
			if (xmlNode == null)
			{
				return null;
			}
		}
		return xmlNode;
	}

	public XmlNode GetNextNode(XmlNode n)
	{
		if (empty)
		{
			return null;
		}
		XmlNode n2 = ((n == null) ? rootNode : n);
		return GetMatchingNode(n2, bNext: true);
	}

	public override XmlNode Item(int index)
	{
		if (rootNode == null || index < 0)
		{
			return null;
		}
		if (empty)
		{
			return null;
		}
		if (curInd == index)
		{
			return curElem;
		}
		int num = index - curInd;
		bool bNext = num > 0;
		if (num < 0)
		{
			num = -num;
		}
		XmlNode nthMatchingNode;
		if ((nthMatchingNode = GetNthMatchingNode(curElem, bNext, num)) != null)
		{
			curInd = index;
			curElem = nthMatchingNode;
			return curElem;
		}
		return null;
	}

	public override IEnumerator GetEnumerator()
	{
		if (empty)
		{
			return new XmlEmptyElementListEnumerator(this);
		}
		return new XmlElementListEnumerator(this);
	}

	protected override void PrivateDisposeNodeList()
	{
		GC.SuppressFinalize(this);
		Dispose(disposing: true);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (listener != null)
		{
			((XmlElementListListener)listener.Target)?.Unregister();
			listener = null;
		}
	}
}
