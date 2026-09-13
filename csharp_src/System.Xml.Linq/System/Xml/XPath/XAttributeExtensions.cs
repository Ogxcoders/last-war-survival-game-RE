using System.Xml.Linq;

namespace System.Xml.XPath;

internal static class XAttributeExtensions
{
	public static string GetPrefixOfNamespace(this XAttribute attribute, XNamespace ns)
	{
		string namespaceName = ns.NamespaceName;
		if (namespaceName.Length == 0)
		{
			return string.Empty;
		}
		if (attribute.GetParent() != null)
		{
			return ((XElement)attribute.GetParent()).GetPrefixOfNamespace(ns);
		}
		if ((object)namespaceName == XNodeNavigator.xmlPrefixNamespace)
		{
			return "xml";
		}
		if ((object)namespaceName == XNodeNavigator.xmlnsPrefixNamespace)
		{
			return "xmlns";
		}
		return null;
	}
}
