using System.ComponentModel;
using System.Xml.Serialization;

namespace System.Xml.Schema;

public class XmlSchemaAnyAttribute : XmlSchemaAnnotated
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

	internal static bool IsSubset(XmlSchemaAnyAttribute sub, XmlSchemaAnyAttribute super)
	{
		return NamespaceList.IsSubset(sub.NamespaceList, super.NamespaceList);
	}

	internal static XmlSchemaAnyAttribute Intersection(XmlSchemaAnyAttribute o1, XmlSchemaAnyAttribute o2, bool v1Compat)
	{
		NamespaceList namespaceList = NamespaceList.Intersection(o1.NamespaceList, o2.NamespaceList, v1Compat);
		if (namespaceList != null)
		{
			return new XmlSchemaAnyAttribute
			{
				namespaceList = namespaceList,
				ProcessContents = o1.ProcessContents,
				Annotation = o1.Annotation
			};
		}
		return null;
	}

	internal static XmlSchemaAnyAttribute Union(XmlSchemaAnyAttribute o1, XmlSchemaAnyAttribute o2, bool v1Compat)
	{
		NamespaceList namespaceList = NamespaceList.Union(o1.NamespaceList, o2.NamespaceList, v1Compat);
		if (namespaceList != null)
		{
			return new XmlSchemaAnyAttribute
			{
				namespaceList = namespaceList,
				processContents = o1.processContents,
				Annotation = o1.Annotation
			};
		}
		return null;
	}
}
