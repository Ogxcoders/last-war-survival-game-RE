using System.Collections.Specialized;
using System.IO;
using System.Web.Services.Configuration;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace System.Web.Services.Description;

[XmlFormatExtensionPoint("Extensions")]
[XmlRoot("definitions", Namespace = "http://schemas.xmlsoap.org/wsdl/")]
public sealed class ServiceDescription : NamedItem
{
	internal class ServiceDescriptionSerializer : XmlSerializer
	{
		protected override void Serialize(object o, XmlSerializationWriter writer)
		{
			(writer as ServiceDescriptionWriterBase).WriteRoot_ServiceDescription(o);
		}

		protected override object Deserialize(XmlSerializationReader reader)
		{
			return (reader as ServiceDescriptionReaderBase).ReadRoot_ServiceDescription();
		}

		protected override XmlSerializationWriter CreateWriter()
		{
			return new ServiceDescriptionWriterBase();
		}

		protected override XmlSerializationReader CreateReader()
		{
			return new ServiceDescriptionReaderBase();
		}
	}

	public const string Namespace = "http://schemas.xmlsoap.org/wsdl/";

	private BindingCollection bindings;

	private ServiceDescriptionFormatExtensionCollection extensions;

	private ImportCollection imports;

	private MessageCollection messages;

	private PortTypeCollection portTypes;

	private string retrievalUrl = string.Empty;

	private ServiceDescriptionCollection serviceDescriptions;

	private ServiceCollection services;

	private string targetNamespace;

	private Types types;

	private static ServiceDescriptionSerializer serializer;

	private StringCollection validationWarnings;

	private static XmlSchema schema;

	public static XmlSchema Schema
	{
		get
		{
			if (schema == null)
			{
				schema = XmlSchema.Read(typeof(ServiceDescription).Assembly.GetManifestResourceStream("wsdl-1.1.xsd"), null);
			}
			return schema;
		}
	}

	[XmlElement("import")]
	public ImportCollection Imports => imports;

	[XmlElement("types")]
	public Types Types
	{
		get
		{
			return types;
		}
		set
		{
			types = value;
		}
	}

	[XmlElement("message")]
	public MessageCollection Messages => messages;

	[XmlElement("portType")]
	public PortTypeCollection PortTypes => portTypes;

	[XmlElement("binding")]
	public BindingCollection Bindings => bindings;

	[XmlIgnore]
	public override ServiceDescriptionFormatExtensionCollection Extensions => extensions;

	[XmlIgnore]
	public string RetrievalUrl
	{
		get
		{
			return retrievalUrl;
		}
		set
		{
			retrievalUrl = value;
		}
	}

	[XmlIgnore]
	public static XmlSerializer Serializer => serializer;

	[XmlIgnore]
	public ServiceDescriptionCollection ServiceDescriptions => serviceDescriptions;

	[XmlElement("service")]
	public ServiceCollection Services => services;

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
	public StringCollection ValidationWarnings => validationWarnings;

	static ServiceDescription()
	{
		serializer = new ServiceDescriptionSerializer();
	}

	public ServiceDescription()
	{
		bindings = new BindingCollection(this);
		extensions = new ServiceDescriptionFormatExtensionCollection(this);
		imports = new ImportCollection(this);
		messages = new MessageCollection(this);
		portTypes = new PortTypeCollection(this);
		serviceDescriptions = null;
		services = new ServiceCollection(this);
		targetNamespace = null;
		types = new Types();
	}

	public static bool CanRead(XmlReader reader)
	{
		reader.MoveToContent();
		if (reader.LocalName == "definitions")
		{
			return reader.NamespaceURI == "http://schemas.xmlsoap.org/wsdl/";
		}
		return false;
	}

	public static ServiceDescription Read(string fileName, bool validate)
	{
		if (validate)
		{
			using (XmlReader reader = XmlReader.Create(fileName))
			{
				return Read(reader, validate: true);
			}
		}
		return Read(fileName);
	}

	public static ServiceDescription Read(Stream stream, bool validate)
	{
		if (validate)
		{
			return Read(XmlReader.Create(stream), validate: true);
		}
		return Read(stream);
	}

	public static ServiceDescription Read(TextReader reader, bool validate)
	{
		if (validate)
		{
			return Read(XmlReader.Create(reader), validate: true);
		}
		return Read(reader);
	}

	public static ServiceDescription Read(XmlReader reader, bool validate)
	{
		if (validate)
		{
			StringCollection sc = new StringCollection();
			XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
			xmlReaderSettings.ValidationType = ValidationType.Schema;
			xmlReaderSettings.Schemas.Add(Schema);
			xmlReaderSettings.ValidationEventHandler += delegate(object o, ValidationEventArgs e)
			{
				sc.Add(e.Message);
			};
			ServiceDescription serviceDescription = Read(XmlReader.Create(reader, xmlReaderSettings));
			serviceDescription.validationWarnings = sc;
			return serviceDescription;
		}
		return Read(reader);
	}

	public static ServiceDescription Read(Stream stream)
	{
		return (ServiceDescription)serializer.Deserialize(stream);
	}

	public static ServiceDescription Read(string fileName)
	{
		return Read(new FileStream(fileName, FileMode.Open, FileAccess.Read));
	}

	public static ServiceDescription Read(TextReader textReader)
	{
		return (ServiceDescription)serializer.Deserialize(textReader);
	}

	public static ServiceDescription Read(XmlReader reader)
	{
		return (ServiceDescription)serializer.Deserialize(reader);
	}

	public void Write(Stream stream)
	{
		serializer.Serialize(stream, this, GetNamespaceList());
	}

	public void Write(string fileName)
	{
		Write(new FileStream(fileName, FileMode.Create));
	}

	public void Write(TextWriter writer)
	{
		serializer.Serialize(writer, this, GetNamespaceList());
	}

	public void Write(XmlWriter writer)
	{
		serializer.Serialize(writer, this, GetNamespaceList());
	}

	internal void SetParent(ServiceDescriptionCollection serviceDescriptions)
	{
		this.serviceDescriptions = serviceDescriptions;
	}

	private XmlSerializerNamespaces GetNamespaceList()
	{
		XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces();
		xmlSerializerNamespaces.Add("soap", "http://schemas.xmlsoap.org/wsdl/soap/");
		xmlSerializerNamespaces.Add("soap12", "http://schemas.xmlsoap.org/wsdl/soap12/");
		xmlSerializerNamespaces.Add("soapenc", "http://schemas.xmlsoap.org/soap/encoding/");
		xmlSerializerNamespaces.Add("s", "http://www.w3.org/2001/XMLSchema");
		xmlSerializerNamespaces.Add("http", "http://schemas.xmlsoap.org/wsdl/http/");
		xmlSerializerNamespaces.Add("mime", "http://schemas.xmlsoap.org/wsdl/mime/");
		xmlSerializerNamespaces.Add("tm", "http://microsoft.com/wsdl/mime/textMatching/");
		xmlSerializerNamespaces.Add("s0", TargetNamespace);
		AddExtensionNamespaces(xmlSerializerNamespaces, Extensions);
		if (Types != null)
		{
			AddExtensionNamespaces(xmlSerializerNamespaces, Types.Extensions);
		}
		foreach (Service service in Services)
		{
			foreach (Port port in service.Ports)
			{
				AddExtensionNamespaces(xmlSerializerNamespaces, port.Extensions);
			}
		}
		foreach (Binding binding in Bindings)
		{
			AddExtensionNamespaces(xmlSerializerNamespaces, binding.Extensions);
			foreach (OperationBinding operation in binding.Operations)
			{
				AddExtensionNamespaces(xmlSerializerNamespaces, operation.Extensions);
				if (operation.Input != null)
				{
					AddExtensionNamespaces(xmlSerializerNamespaces, operation.Input.Extensions);
				}
				if (operation.Output != null)
				{
					AddExtensionNamespaces(xmlSerializerNamespaces, operation.Output.Extensions);
				}
			}
		}
		return xmlSerializerNamespaces;
	}

	private void AddExtensionNamespaces(XmlSerializerNamespaces ns, ServiceDescriptionFormatExtensionCollection extensions)
	{
		foreach (object extension in extensions)
		{
			if (!(extension is ServiceDescriptionFormatExtension serviceDescriptionFormatExtension))
			{
				continue;
			}
			foreach (XmlQualifiedName namespaceDeclaration in ExtensionManager.GetFormatExtensionInfo(serviceDescriptionFormatExtension.GetType()).NamespaceDeclarations)
			{
				ns.Add(namespaceDeclaration.Name, namespaceDeclaration.Namespace);
			}
		}
	}

	internal static void WriteExtensions(XmlWriter writer, object ob)
	{
		ServiceDescriptionFormatExtensionCollection extensionPoint = ExtensionManager.GetExtensionPoint(ob);
		if (extensionPoint == null)
		{
			return;
		}
		foreach (object item in extensionPoint)
		{
			if (item is ServiceDescriptionFormatExtension)
			{
				WriteExtension(writer, (ServiceDescriptionFormatExtension)item);
			}
			else if (item is XmlElement)
			{
				((XmlElement)item).WriteTo(writer);
			}
		}
	}

	private static void WriteExtension(XmlWriter writer, ServiceDescriptionFormatExtension ext)
	{
		ExtensionInfo formatExtensionInfo = ExtensionManager.GetFormatExtensionInfo(ext.GetType());
		XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces();
		xmlSerializerNamespaces.Add("", "");
		formatExtensionInfo.Serializer.Serialize(writer, ext, xmlSerializerNamespaces);
	}

	internal static void ReadExtension(XmlDocument doc, XmlReader reader, object ob)
	{
		ServiceDescriptionFormatExtensionCollection extensionPoint = ExtensionManager.GetExtensionPoint(ob);
		if (extensionPoint != null)
		{
			ExtensionInfo formatExtensionInfo = ExtensionManager.GetFormatExtensionInfo(reader.LocalName, reader.NamespaceURI);
			if (formatExtensionInfo != null)
			{
				object obj = formatExtensionInfo.Serializer.Deserialize(reader);
				extensionPoint.Add((ServiceDescriptionFormatExtension)obj);
				return;
			}
		}
		if (!(ob is DocumentableItem documentableItem))
		{
			reader.Skip();
		}
		else
		{
			documentableItem.Extensions.Add(doc.ReadNode(reader));
		}
	}
}
