using System.ComponentModel;
using System.Text;
using System.Xml.Serialization;

namespace System.Xml.Schema;

public class XmlSchemaAny : XmlSchemaParticle
{
	private string ns;

	private XmlSchemaContentProcessing processContents;

	private NamespaceList namespaceList;

	[XmlAttribute("namespace")]
	public string Namespace
	{
		get
		{
			return ns;
		}
		set
		{
			ns = value;
		}
	}

	[XmlAttribute("processContents")]
	[DefaultValue(XmlSchemaContentProcessing.None)]
	public XmlSchemaContentProcessing ProcessContents
	{
		get
		{
			return processContents;
		}
		set
		{
			processContents = value;
		}
	}

	[XmlIgnore]
	internal NamespaceList NamespaceList => namespaceList;

	[XmlIgnore]
	internal string ResolvedNamespace
	{
		get
		{
			if (ns == null || ns.Length == 0)
			{
				return "##any";
			}
			return ns;
		}
	}

	[XmlIgnore]
	internal XmlSchemaContentProcessing ProcessContentsCorrect
	{
		get
		{
			if (processContents != XmlSchemaContentProcessing.None)
			{
				return processContents;
			}
			return XmlSchemaContentProcessing.Strict;
		}
	}

	internal override string NameString
	{
		get
		{
			switch (namespaceList.Type)
			{
			case NamespaceList.ListType.Any:
				return "##any:*";
			case NamespaceList.ListType.Other:
				return "##other:*";
			case NamespaceList.ListType.Set:
			{
				StringBuilder stringBuilder = new StringBuilder();
				int num = 1;
				foreach (string item in namespaceList.Enumerate)
				{
					stringBuilder.Append(item + ":*");
					if (num < namespaceList.Enumerate.Count)
					{
						stringBuilder.Append(" ");
					}
					num++;
				}
				return stringBuilder.ToString();
			}
			default:
				return string.Empty;
			}
		}
	}

	internal void BuildNamespaceList(string targetNamespace)
	{
		if (ns != null)
		{
			namespaceList = new NamespaceList(ns, targetNamespace);
		}
		else
		{
			namespaceList = new NamespaceList();
		}
	}

	internal void BuildNamespaceListV1Compat(string targetNamespace)
	{
		if (ns != null)
		{
			namespaceList = new NamespaceListV1Compat(ns, targetNamespace);
		}
		else
		{
			namespaceList = new NamespaceList();
		}
	}

	internal bool Allows(XmlQualifiedName qname)
	{
		return namespaceList.Allows(qname.Namespace);
	}
}
