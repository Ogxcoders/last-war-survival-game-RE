using System.Collections;

namespace System.Xml.Schema;

internal class SchemaNamespaceManager : XmlNamespaceManager
{
	private XmlSchemaObject node;

	public SchemaNamespaceManager(XmlSchemaObject node)
	{
		this.node = node;
	}

	public override string LookupNamespace(string prefix)
	{
		if (prefix == "xml")
		{
			return "http://www.w3.org/XML/1998/namespace";
		}
		for (XmlSchemaObject parent = node; parent != null; parent = parent.Parent)
		{
			Hashtable namespaces = parent.Namespaces.Namespaces;
			if (namespaces != null && namespaces.Count > 0)
			{
				object obj = namespaces[prefix];
				if (obj != null)
				{
					return (string)obj;
				}
			}
		}
		if (prefix.Length != 0)
		{
			return null;
		}
		return string.Empty;
	}

	public override string LookupPrefix(string ns)
	{
		if (ns == "http://www.w3.org/XML/1998/namespace")
		{
			return "xml";
		}
		for (XmlSchemaObject parent = node; parent != null; parent = parent.Parent)
		{
			Hashtable namespaces = parent.Namespaces.Namespaces;
			if (namespaces != null && namespaces.Count > 0)
			{
				foreach (DictionaryEntry item in namespaces)
				{
					if (item.Value.Equals(ns))
					{
						return (string)item.Key;
					}
				}
			}
		}
		return null;
	}
}
