using System.IO;
using System.Web.Services.Description;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace System.Web.Services.Discovery;

[XmlRoot("contractRef", Namespace = "http://schemas.xmlsoap.org/disco/scl/", IsNullable = true)]
public class ContractReference : DiscoveryReference
{
	public const string Namespace = "http://schemas.xmlsoap.org/disco/scl/";

	private ServiceDescription contract;

	private string defaultFilename;

	private string docRef;

	private string href;

	[XmlIgnore]
	public ServiceDescription Contract
	{
		get
		{
			if (base.ClientProtocol == null)
			{
				throw new InvalidOperationException("The ClientProtocol property is a null reference");
			}
			return (base.ClientProtocol.Documents[Url] as ServiceDescription) ?? throw new Exception("The Documents property of ClientProtocol does not contain a WSDL document with the url " + Url);
		}
	}

	[XmlIgnore]
	public override string DefaultFilename => DiscoveryReference.FilenameFromUrl(Url) + ".wsdl";

	[XmlAttribute("docRef")]
	public string DocRef
	{
		get
		{
			return docRef;
		}
		set
		{
			docRef = value;
		}
	}

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

	public ContractReference()
	{
	}

	public ContractReference(string href)
		: this()
	{
		this.href = href;
	}

	public ContractReference(string href, string docRef)
	{
		this.href = href;
		this.docRef = docRef;
	}

	public override object ReadDocument(Stream stream)
	{
		return ServiceDescription.Read(stream);
	}

	protected internal override void Resolve(string contentType, Stream stream)
	{
		ServiceDescription serviceDescription = ServiceDescription.Read(stream);
		if (!base.ClientProtocol.References.Contains(Url))
		{
			base.ClientProtocol.References.Add(this);
		}
		base.ClientProtocol.Documents.Add(Url, serviceDescription);
		ResolveInternal(base.ClientProtocol, serviceDescription);
	}

	internal void ResolveInternal(DiscoveryClientProtocol prot, ServiceDescription wsdl)
	{
		if (wsdl.Imports == null)
		{
			return;
		}
		foreach (Import import in wsdl.Imports)
		{
			string url = new Uri(base.BaseUri, import.Location).ToString();
			if (prot.Documents.Contains(url))
			{
				continue;
			}
			try
			{
				string contentType = null;
				Stream input = prot.Download(ref url, ref contentType);
				XmlTextReader xmlTextReader = new XmlTextReader(url, input);
				xmlTextReader.XmlResolver = null;
				xmlTextReader.MoveToContent();
				DiscoveryReference discoveryReference;
				if (ServiceDescription.CanRead(xmlTextReader))
				{
					ServiceDescription serviceDescription = ServiceDescription.Read(xmlTextReader);
					discoveryReference = new ContractReference();
					discoveryReference.ClientProtocol = prot;
					discoveryReference.Url = url;
					((ContractReference)discoveryReference).ResolveInternal(prot, serviceDescription);
					prot.Documents.Add(url, serviceDescription);
				}
				else
				{
					XmlSchema value = XmlSchema.Read(xmlTextReader, null);
					discoveryReference = new SchemaReference();
					discoveryReference.ClientProtocol = prot;
					discoveryReference.Url = url;
					prot.Documents.Add(url, value);
				}
				if (!prot.References.Contains(url))
				{
					prot.References.Add(discoveryReference);
				}
				xmlTextReader.Close();
			}
			catch (Exception ex)
			{
				ReportError(url, ex);
			}
		}
		foreach (XmlSchema schema in wsdl.Types.Schemas)
		{
			string url2 = base.BaseUri.ToString();
			SchemaReference schemaReference = new SchemaReference();
			schemaReference.ClientProtocol = prot;
			schemaReference.Url = url2;
			schemaReference.ResolveInternal(prot, schema);
		}
	}

	public override void WriteDocument(object document, Stream stream)
	{
		((ServiceDescription)document).Write(stream);
	}
}
