using System.Collections;
using System.Xml.XPath;

namespace System.Xml.Xsl.XsltOld;

internal class Key
{
	private XmlQualifiedName name;

	private int matchKey;

	private int useKey;

	private ArrayList keyNodes;

	public XmlQualifiedName Name => name;

	public int MatchKey => matchKey;

	public int UseKey => useKey;

	public Key(XmlQualifiedName name, int matchkey, int usekey)
	{
		this.name = name;
		matchKey = matchkey;
		useKey = usekey;
		keyNodes = null;
	}

	public void AddKey(XPathNavigator root, Hashtable table)
	{
		if (keyNodes == null)
		{
			keyNodes = new ArrayList();
		}
		keyNodes.Add(new DocumentKeyList(root, table));
	}

	public Hashtable GetKeys(XPathNavigator root)
	{
		if (keyNodes != null)
		{
			for (int i = 0; i < keyNodes.Count; i++)
			{
				if (((DocumentKeyList)keyNodes[i]).RootNav.IsSamePosition(root))
				{
					return ((DocumentKeyList)keyNodes[i]).KeyTable;
				}
			}
		}
		return null;
	}

	public Key Clone()
	{
		return new Key(name, matchKey, useKey);
	}
}
