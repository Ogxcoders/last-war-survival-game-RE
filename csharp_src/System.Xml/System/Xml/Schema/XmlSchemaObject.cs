using System.Security.Permissions;
using System.Xml.Serialization;

namespace System.Xml.Schema;

[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
public abstract class XmlSchemaObject
{
	private int lineNum;

	private int linePos;

	private string sourceUri;

	private XmlSerializerNamespaces namespaces;

	private XmlSchemaObject parent;

	private bool isProcessing;

	[XmlIgnore]
	public int LineNumber
	{
		get
		{
			return lineNum;
		}
		set
		{
			lineNum = value;
		}
	}

	[XmlIgnore]
	public int LinePosition
	{
		get
		{
			return linePos;
		}
		set
		{
			linePos = value;
		}
	}

	[XmlIgnore]
	public string SourceUri
	{
		get
		{
			return sourceUri;
		}
		set
		{
			sourceUri = value;
		}
	}

	[XmlIgnore]
	public XmlSchemaObject Parent
	{
		get
		{
			return parent;
		}
		set
		{
			parent = value;
		}
	}

	[XmlNamespaceDeclarations]
	public XmlSerializerNamespaces Namespaces
	{
		get
		{
			if (namespaces == null)
			{
				namespaces = new XmlSerializerNamespaces();
			}
			return namespaces;
		}
		set
		{
			namespaces = value;
		}
	}

	[XmlIgnore]
	internal virtual string IdAttribute
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	[XmlIgnore]
	internal virtual string NameAttribute
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	[XmlIgnore]
	internal bool IsProcessing
	{
		get
		{
			return isProcessing;
		}
		set
		{
			isProcessing = value;
		}
	}

	internal virtual void OnAdd(XmlSchemaObjectCollection container, object item)
	{
	}

	internal virtual void OnRemove(XmlSchemaObjectCollection container, object item)
	{
	}

	internal virtual void OnClear(XmlSchemaObjectCollection container)
	{
	}

	internal virtual void SetUnhandledAttributes(XmlAttribute[] moreAttributes)
	{
	}

	internal virtual void AddAnnotation(XmlSchemaAnnotation annotation)
	{
	}

	internal virtual XmlSchemaObject Clone()
	{
		return (XmlSchemaObject)MemberwiseClone();
	}
}
