using System.Data;

namespace System.Xml;

internal interface IXmlDataVirtualNode
{
	bool IsOnNode(XmlNode nodeToCheck);

	bool IsOnColumn(DataColumn col);

	bool IsInUse();

	void OnFoliated(XmlNode foliatedNode);
}
