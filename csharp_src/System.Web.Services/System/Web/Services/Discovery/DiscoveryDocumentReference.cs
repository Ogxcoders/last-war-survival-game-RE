using System.IO;
using System.Xml.Serialization;

namespace System.Web.Services.Discovery;

[XmlRoot("discoveryRef", Namespace = "http://schemas.xmlsoap.org/disco/", IsNullable = true)]
public sealed class DiscoveryDocumentReference : DiscoveryReference
{
	private DiscoveryDocument document;

	private string defaultFilename;

	private string href;

	[XmlIgnore]
	public DiscoveryDocument Document
	{
		get
		{
			if (base.ClientProtocol == null)
			{
				throw new InvalidOperationException("The ClientProtocol property is a null reference");
			}
			return (base.ClientProtocol.Documents[Url] as DiscoveryDocument) ?? throw new Exception("The Documents property of ClientProtocol does not contain a discovery document with the url " + Url);
		}
	}

	[XmlIgnore]
	public override string DefaultFilename => DiscoveryReference.FilenameFromUrl(Url) + ".disco";

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

	public DiscoveryDocumentReference()
	{
		href = string.Empty;
	}

	public DiscoveryDocumentReference(string href)
	{
		this.href = href;
	}

	public override object ReadDocument(Stream stream)
	{
		return DiscoveryDocument.Read(stream);
	}

	protected internal override void Resolve(string contentType, Stream stream)
	{
		DiscoveryDocument discoveryDocument = DiscoveryDocument.Read(stream);
		base.ClientProtocol.Documents.Add(Url, discoveryDocument);
		if (!base.ClientProtocol.References.Contains(Url))
		{
			base.ClientProtocol.References.Add(this);
		}
		foreach (DiscoveryReference reference in discoveryDocument.References)
		{
			reference.ClientProtocol = base.ClientProtocol;
			base.ClientProtocol.References.Add(reference.Url, reference);
		}
	}

	public void ResolveAll()
	{
		if (base.ClientProtocol.Documents.Contains(Url))
		{
			return;
		}
		Resolve();
		foreach (DiscoveryReference reference in document.References)
		{
			try
			{
				if (reference is DiscoveryDocumentReference)
				{
					((DiscoveryDocumentReference)reference).ResolveAll();
				}
				else
				{
					reference.Resolve();
				}
			}
			catch (Exception ex)
			{
				ReportError(reference.Url, ex);
			}
		}
	}

	public override void WriteDocument(object document, Stream stream)
	{
		((DiscoveryDocument)document).Write(stream);
	}
}
