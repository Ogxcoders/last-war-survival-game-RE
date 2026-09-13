using System.Xml.Schema;

namespace System.Xml;

internal class DomNameTable
{
	private XmlName[] entries;

	private int count;

	private int mask;

	private XmlDocument ownerDocument;

	private XmlNameTable nameTable;

	private const int InitialSize = 64;

	public DomNameTable(XmlDocument document)
	{
		ownerDocument = document;
		nameTable = document.NameTable;
		entries = new XmlName[64];
		mask = 63;
	}

	public XmlName GetName(string prefix, string localName, string ns, IXmlSchemaInfo schemaInfo)
	{
		if (prefix == null)
		{
			prefix = string.Empty;
		}
		if (ns == null)
		{
			ns = string.Empty;
		}
		int hashCode = XmlName.GetHashCode(localName);
		for (XmlName xmlName = entries[hashCode & mask]; xmlName != null; xmlName = xmlName.next)
		{
			if (xmlName.HashCode == hashCode && ((object)xmlName.LocalName == localName || xmlName.LocalName.Equals(localName)) && ((object)xmlName.Prefix == prefix || xmlName.Prefix.Equals(prefix)) && ((object)xmlName.NamespaceURI == ns || xmlName.NamespaceURI.Equals(ns)) && xmlName.Equals(schemaInfo))
			{
				return xmlName;
			}
		}
		return null;
	}

	public XmlName AddName(string prefix, string localName, string ns, IXmlSchemaInfo schemaInfo)
	{
		if (prefix == null)
		{
			prefix = string.Empty;
		}
		if (ns == null)
		{
			ns = string.Empty;
		}
		int hashCode = XmlName.GetHashCode(localName);
		for (XmlName xmlName = entries[hashCode & mask]; xmlName != null; xmlName = xmlName.next)
		{
			if (xmlName.HashCode == hashCode && ((object)xmlName.LocalName == localName || xmlName.LocalName.Equals(localName)) && ((object)xmlName.Prefix == prefix || xmlName.Prefix.Equals(prefix)) && ((object)xmlName.NamespaceURI == ns || xmlName.NamespaceURI.Equals(ns)) && xmlName.Equals(schemaInfo))
			{
				return xmlName;
			}
		}
		prefix = nameTable.Add(prefix);
		localName = nameTable.Add(localName);
		ns = nameTable.Add(ns);
		int num = hashCode & mask;
		XmlName xmlName2 = XmlName.Create(prefix, localName, ns, hashCode, ownerDocument, entries[num], schemaInfo);
		entries[num] = xmlName2;
		if (count++ == mask)
		{
			Grow();
		}
		return xmlName2;
	}

	private void Grow()
	{
		int num = mask * 2 + 1;
		XmlName[] array = entries;
		XmlName[] array2 = new XmlName[num + 1];
		for (int i = 0; i < array.Length; i++)
		{
			XmlName xmlName = array[i];
			while (xmlName != null)
			{
				int num2 = xmlName.HashCode & num;
				XmlName next = xmlName.next;
				xmlName.next = array2[num2];
				array2[num2] = xmlName;
				xmlName = next;
			}
		}
		entries = array2;
		mask = num;
	}
}
