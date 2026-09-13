using System.Collections.Generic;
using System.Xml;

namespace System.Runtime.Serialization;

internal class XmlDataNode : DataNode<object>
{
	private IList<XmlAttribute> xmlAttributes;

	private IList<XmlNode> xmlChildNodes;

	private XmlDocument ownerDocument;

	internal IList<XmlAttribute> XmlAttributes
	{
		get
		{
			return xmlAttributes;
		}
		set
		{
			xmlAttributes = value;
		}
	}

	internal IList<XmlNode> XmlChildNodes
	{
		get
		{
			return xmlChildNodes;
		}
		set
		{
			xmlChildNodes = value;
		}
	}

	internal XmlDocument OwnerDocument
	{
		get
		{
			return ownerDocument;
		}
		set
		{
			ownerDocument = value;
		}
	}

	internal XmlDataNode()
	{
		dataType = Globals.TypeOfXmlDataNode;
	}

	public override void Clear()
	{
		base.Clear();
		xmlAttributes = null;
		xmlChildNodes = null;
		ownerDocument = null;
	}
}
