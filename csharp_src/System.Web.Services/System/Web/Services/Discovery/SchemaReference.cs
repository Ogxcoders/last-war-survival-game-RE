using System.ComponentModel;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace System.Web.Services.Discovery;

[XmlRoot("schemaRef", Namespace = "http://schemas.xmlsoap.org/disco/schema/", IsNullable = true)]
public sealed class SchemaReference : DiscoveryReference
{
	public const string Namespace = "http://schemas.xmlsoap.org/disco/schema/";

	private string defaultFilename;

	private string href;

	private string targetNamespace;

	private XmlSchema schema;

	[XmlIgnore]
	public override string DefaultFilename => DiscoveryReference.FilenameFromUrl(Url) + ".xsd";

	[XmlAttribute("ref")]
	public string Ref
	{
		get
		{
			return href;
		}
		set
		{
			href = value;
		}
	}

	[XmlIgnore]
	public override string Url
	{
		get
		{
			return href;
		}
		set
		{
			href = value;
		}
	}

	[DefaultValue(null)]
	[XmlAttribute("targetNamespace")]
	public string TargetNamespace
	{
		get
		{
			return targetNamespace;
		}
		set
		{
			targetNamespace = value;
		}
	}

	[XmlIgnore]
	public XmlSchema Schema
	{
		get
		{
			if (base.ClientProtocol == null)
			{
				throw new InvalidOperationException("The ClientProtocol property is a null reference");
			}
			return (base.ClientProtocol.Documents[Url] as XmlSchema) ?? throw new Exception("The Documents property of ClientProtocol does not contain a schema with the url " + Url);
		}
	}

	public SchemaReference()
	{
	}

	public SchemaReference(string href)
		: this()
	{
		this.href = href;
	}

	public override object ReadDocument(Stream stream)
	{
		return XmlSchema.Read(stream, null);
	}

	protected internal override void Resolve(string contentType, Stream stream)
	{
		XmlSchema value = XmlSchema.Read(stream, null);
		base.ClientProtocol.Documents.Add(Url, value);
		if (!base.ClientProtocol.References.Contains(Url))
		{
			base.ClientProtocol.References.Add(this);
		}
	}

	internal void ResolveInternal(DiscoveryClientProtocol prot, XmlSchema xsd)
	{
		if (xsd.Includes.Count == 0)
		{
			return;
		}
		foreach (XmlSchemaExternal include in xsd.Includes)
		{
			if (include.SchemaLocation == null)
			{
				continue;
			}
			string url = new Uri(base.BaseUri, include.SchemaLocation).ToString();
			if (prot.Documents.Contains(url))
			{
				continue;
			}
			try
			{
				string contentType = null;
				Stream stream = prot.Download(ref url, ref contentType);
				XmlTextReader xmlTextReader = new XmlTextReader(url, stream);
				xmlTextReader.XmlResolver = null;
				xmlTextReader.MoveToContent();
				XmlSchema xmlSchema = XmlSchema.Read(xmlTextReader, null);
				DiscoveryReference discoveryReference = new SchemaReference();
				discoveryReference.ClientProtocol = prot;
				discoveryReference.Url = url;
				prot.Documents.Add(url, xmlSchema);
				((SchemaReference)discoveryReference).ResolveInternal(prot, xmlSchema);
				if (!prot.References.Contains(url))
				{
					prot.References.Add(discoveryReference);
				}
				stream.Close();
			}
			catch (Exception ex)
			{
				ReportError(url, ex);
			}
		}
	}

	public override void WriteDocument(object document, Stream stream)
	{
		((XmlSchema)document).Write(stream);
	}
}
