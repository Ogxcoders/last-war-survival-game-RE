using System.Xml.Schema;

namespace System.Xml;

public class XmlDocumentType : XmlLinkedNode
{
	private string name;

	private string publicId;

	private string systemId;

	private string internalSubset;

	private bool namespaces;

	private XmlNamedNodeMap entities;

	private XmlNamedNodeMap notations;

	private SchemaInfo schemaInfo;

	public override string Name => name;

	public override string LocalName => name;

	public override XmlNodeType NodeType => XmlNodeType.DocumentType;

	public override bool IsReadOnly => true;

	public XmlNamedNodeMap Entities
	{
		get
		{
			if (entities == null)
			{
				entities = new XmlNamedNodeMap(this);
			}
			return entities;
		}
	}

	public XmlNamedNodeMap Notations
	{
		get
		{
			if (notations == null)
			{
				notations = new XmlNamedNodeMap(this);
			}
			return notations;
		}
	}

	public string PublicId => publicId;

	public string SystemId => systemId;

	public string InternalSubset => internalSubset;

	internal bool ParseWithNamespaces
	{
		get
		{
			return namespaces;
		}
		set
		{
			namespaces = value;
		}
	}

	internal SchemaInfo DtdSchemaInfo
	{
		get
		{
			return schemaInfo;
		}
		set
		{
			schemaInfo = value;
		}
	}

	protected internal XmlDocumentType(string name, string publicId, string systemId, string internalSubset, XmlDocument doc)
		: base(doc)
	{
		this.name = name;
		this.publicId = publicId;
		this.systemId = systemId;
		namespaces = true;
		this.internalSubset = internalSubset;
		if (!doc.IsLoading)
		{
			doc.IsLoading = true;
			new XmlLoader().ParseDocumentType(this);
			doc.IsLoading = false;
		}
	}

	public override XmlNode CloneNode(bool deep)
	{
		return OwnerDocument.CreateDocumentType(name, publicId, systemId, internalSubset);
	}

	public override void WriteTo(XmlWriter w)
	{
		w.WriteDocType(name, publicId, systemId, internalSubset);
	}

	public override void WriteContentTo(XmlWriter w)
	{
	}
}
