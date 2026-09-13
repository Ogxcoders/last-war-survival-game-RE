using System.Collections;
using MS.Internal.Xml.Cache;

namespace System.Xml.XPath;

internal class XPathNavigatorKeyComparer : IEqualityComparer
{
	bool IEqualityComparer.Equals(object obj1, object obj2)
	{
		XPathNavigator xPathNavigator = obj1 as XPathNavigator;
		XPathNavigator xPathNavigator2 = obj2 as XPathNavigator;
		if (xPathNavigator != null && xPathNavigator2 != null && xPathNavigator.IsSamePosition(xPathNavigator2))
		{
			return true;
		}
		return false;
	}

	int IEqualityComparer.GetHashCode(object obj)
	{
		if (obj == null)
		{
			throw new ArgumentNullException("obj");
		}
		if (obj is XPathDocumentNavigator xPathDocumentNavigator)
		{
			return xPathDocumentNavigator.GetPositionHashCode();
		}
		if (obj is XPathNavigator { UnderlyingObject: var underlyingObject } xPathNavigator)
		{
			if (underlyingObject != null)
			{
				return underlyingObject.GetHashCode();
			}
			int nodeType = (int)xPathNavigator.NodeType;
			nodeType ^= xPathNavigator.LocalName.GetHashCode();
			nodeType ^= xPathNavigator.Prefix.GetHashCode();
			return nodeType ^ xPathNavigator.NamespaceURI.GetHashCode();
		}
		return obj.GetHashCode();
	}
}
