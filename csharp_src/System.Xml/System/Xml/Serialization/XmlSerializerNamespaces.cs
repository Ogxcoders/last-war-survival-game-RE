using System.Collections;

namespace System.Xml.Serialization;

public class XmlSerializerNamespaces
{
	private Hashtable namespaces;

	public int Count => Namespaces.Count;

	internal ArrayList NamespaceList
	{
		get
		{
			if (namespaces == null || namespaces.Count == 0)
			{
				return null;
			}
			ArrayList arrayList = new ArrayList();
			foreach (string key in Namespaces.Keys)
			{
				arrayList.Add(new XmlQualifiedName(key, (string)Namespaces[key]));
			}
			return arrayList;
		}
	}

	internal Hashtable Namespaces
	{
		get
		{
			if (namespaces == null)
			{
				namespaces = new Hashtable();
			}
			return namespaces;
		}
		set
		{
			namespaces = value;
		}
	}

	public XmlSerializerNamespaces()
	{
	}

	public XmlSerializerNamespaces(XmlSerializerNamespaces namespaces)
	{
		this.namespaces = (Hashtable)namespaces.Namespaces.Clone();
	}

	public XmlSerializerNamespaces(XmlQualifiedName[] namespaces)
	{
		foreach (XmlQualifiedName xmlQualifiedName in namespaces)
		{
			Add(xmlQualifiedName.Name, xmlQualifiedName.Namespace);
		}
	}

	public void Add(string prefix, string ns)
	{
		if (prefix != null && prefix.Length > 0)
		{
			XmlConvert.VerifyNCName(prefix);
		}
		if (ns != null && ns.Length > 0)
		{
			XmlConvert.ToUri(ns);
		}
		AddInternal(prefix, ns);
	}

	internal void AddInternal(string prefix, string ns)
	{
		Namespaces[prefix] = ns;
	}

	public XmlQualifiedName[] ToArray()
	{
		if (NamespaceList == null)
		{
			return new XmlQualifiedName[0];
		}
		return (XmlQualifiedName[])NamespaceList.ToArray(typeof(XmlQualifiedName));
	}

	internal string LookupPrefix(string ns)
	{
		if (string.IsNullOrEmpty(ns))
		{
			return null;
		}
		if (namespaces == null || namespaces.Count == 0)
		{
			return null;
		}
		foreach (string key in namespaces.Keys)
		{
			if (!string.IsNullOrEmpty(key) && (string)namespaces[key] == ns)
			{
				return key;
			}
		}
		return null;
	}
}
