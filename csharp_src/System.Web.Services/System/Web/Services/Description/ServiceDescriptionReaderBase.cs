using System.Reflection;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace System.Web.Services.Description;

internal class ServiceDescriptionReaderBase : XmlSerializationReader
{
	private static readonly MethodInfo fromBinHexStringMethod = typeof(XmlConvert).GetMethod("FromBinHexString", BindingFlags.Static | BindingFlags.NonPublic, null, new Type[1] { typeof(string) }, null);

	private static byte[] FromBinHexString(string input)
	{
		if (input != null)
		{
			return (byte[])fromBinHexStringMethod.Invoke(null, new object[1] { input });
		}
		return null;
	}

	public object ReadRoot_ServiceDescription()
	{
		base.Reader.MoveToContent();
		if (base.Reader.LocalName != "definitions" || base.Reader.NamespaceURI != "http://schemas.xmlsoap.org/wsdl/")
		{
			throw CreateUnknownNodeException();
		}
		return ReadObject_ServiceDescription(isNullable: true, checkType: true);
	}

	public ServiceDescription ReadObject_ServiceDescription(bool isNullable, bool checkType)
	{
		ServiceDescription serviceDescription = null;
		if (isNullable && ReadNull())
		{
			return null;
		}
		if (checkType)
		{
			XmlQualifiedName xsiType = GetXsiType();
			if (!(xsiType == null) && (xsiType.Name != "ServiceDescription" || xsiType.Namespace != "http://schemas.xmlsoap.org/wsdl/"))
			{
				throw CreateUnknownTypeException(xsiType);
			}
		}
		serviceDescription = (ServiceDescription)Activator.CreateInstance(typeof(ServiceDescription), nonPublic: true);
		base.Reader.MoveToElement();
		int num = 0;
		XmlAttribute[] array = null;
		while (base.Reader.MoveToNextAttribute())
		{
			if (base.Reader.LocalName == "name" && base.Reader.NamespaceURI == "")
			{
				serviceDescription.Name = base.Reader.Value;
			}
			else if (base.Reader.LocalName == "targetNamespace" && base.Reader.NamespaceURI == "")
			{
				serviceDescription.TargetNamespace = base.Reader.Value;
			}
			else if (IsXmlnsAttribute(base.Reader.Name))
			{
				if (serviceDescription.Namespaces == null)
				{
					serviceDescription.Namespaces = new XmlSerializerNamespaces();
				}
				if (base.Reader.Prefix == "xmlns")
				{
					serviceDescription.Namespaces.Add(base.Reader.LocalName, base.Reader.Value);
				}
				else
				{
					serviceDescription.Namespaces.Add("", base.Reader.Value);
				}
			}
			else
			{
				XmlAttribute xmlAttribute = (XmlAttribute)base.Document.ReadNode(base.Reader);
				array = (XmlAttribute[])EnsureArrayIndex(array, num, typeof(XmlAttribute));
				array[num] = xmlAttribute;
				num++;
			}
		}
		array = (XmlAttribute[])ShrinkArray(array, num, typeof(XmlAttribute), isNullable: true);
		serviceDescription.ExtensibleAttributes = array;
		base.Reader.MoveToElement();
		base.Reader.MoveToElement();
		if (base.Reader.IsEmptyElement)
		{
			base.Reader.Skip();
			return serviceDescription;
		}
		base.Reader.ReadStartElement();
		base.Reader.MoveToContent();
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = false;
		bool flag5 = false;
		bool flag6 = false;
		bool flag7 = false;
		ImportCollection imports = serviceDescription.Imports;
		MessageCollection messages = serviceDescription.Messages;
		PortTypeCollection portTypes = serviceDescription.PortTypes;
		BindingCollection bindings = serviceDescription.Bindings;
		ServiceCollection services = serviceDescription.Services;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		int num6 = 0;
		while (base.Reader.NodeType != XmlNodeType.EndElement)
		{
			if (base.Reader.NodeType == XmlNodeType.Element)
			{
				if (base.Reader.LocalName == "types" && base.Reader.NamespaceURI == "http://schemas.xmlsoap.org/wsdl/" && !flag3)
				{
					flag3 = true;
					serviceDescription.Types = ReadObject_Types(isNullable: false, checkType: true);
				}
				else if (base.Reader.LocalName == "service" && base.Reader.NamespaceURI == "http://schemas.xmlsoap.org/wsdl/" && !flag7)
				{
					if (services == null)
					{
						throw CreateReadOnlyCollectionException("System.Web.Services.Description.ServiceCollection");
					}
					services.Add(ReadObject_Service(isNullable: false, checkType: true));
					num6++;
				}
				else if (base.Reader.LocalName == "message" && base.Reader.NamespaceURI == "http://schemas.xmlsoap.org/wsdl/" && !flag4)
				{
					if (messages == null)
					{
						throw CreateReadOnlyCollectionException("System.Web.Services.Description.MessageCollection");
					}
					messages.Add(ReadObject_Message(isNullable: false, checkType: true));
					num3++;
				}
				else if (base.Reader.LocalName == "documentation" && base.Reader.NamespaceURI == "http://schemas.xmlsoap.org/wsdl/" && !flag)
				{
					serviceDescription.DocumentationElement = (XmlElement)ReadXmlNode(wrapped: false);
				}
				else if (base.Reader.LocalName == "portType" && base.Reader.NamespaceURI == "http://schemas.xmlsoap.org/wsdl/" && !flag5)
				{
					if (portTypes == null)
					{
						throw CreateReadOnlyCollectionException("System.Web.Services.Description.PortTypeCollection");
					}
					portTypes.Add(ReadObject_PortType(isNullable: false, checkType: true));
					num4++;
				}
				else if (base.Reader.LocalName == "import" && base.Reader.NamespaceURI == "http://schemas.xmlsoap.org/wsdl/" && !flag2)
				{
					if (imports == null)
					{
						throw CreateReadOnlyCollectionException("System.Web.Services.Description.ImportCollection");
					}
					imports.Add(ReadObject_Import(isNullable: false, checkType: true));
					num2++;
				}
				else if (base.Reader.LocalName == "binding" && base.Reader.NamespaceURI == "http://schemas.xmlsoap.org/wsdl/" && !flag6)
				{
					if (bindings == null)
					{
						throw CreateReadOnlyCollectionException("System.Web.Services.Description.BindingCollection");
					}
					bindings.Add(ReadObject_Binding(isNullable: false, checkType: true));
					num5++;
				}
				else
				{
					ServiceDescription.ReadExtension(base.Document, base.Reader, serviceDescription);
				}
			}
			else
			{
				UnknownNode(serviceDescription);
			}
			base.Reader.MoveToContent();
		}
		ReadEndElement();
		return serviceDescription;
	}

	public Types ReadObject_Types(bool isNullable, bool checkType)
	{
		Types types = null;
		if (isNullable && ReadNull())
		{
			return null;
		}
		if (checkType)
		{
			XmlQualifiedName xsiType = GetXsiType();
			if (!(xsiType == null) && (xsiType.Name != "Types" || xsiType.Namespace != "http://schemas.xmlsoap.org/wsdl/"))
			{
				throw CreateUnknownTypeException(xsiType);
			}
		}
		types = (Types)Activator.CreateInstance(typeof(Types), nonPublic: true);
		base.Reader.MoveToElement();
		int num = 0;
		XmlAttribute[] array = null;
		while (base.Reader.MoveToNextAttribute())
		{
			if (IsXmlnsAttribute(base.Reader.Name))
			{
				if (types.Namespaces == null)
				{
					types.Namespaces = new XmlSerializerNamespaces();
				}
				if (base.Reader.Prefix == "xmlns")
				{
					types.Namespaces.Add(base.Reader.LocalName, base.Reader.Value);
				}
				else
				{
					types.Namespaces.Add("", base.Reader.Value);
				}
			}
			else
			{
				XmlAttribute xmlAttribute = (XmlAttribute)base.Document.ReadNode(base.Reader);
				array = (XmlAttribute[])EnsureArrayIndex(array, num, typeof(XmlAttribute));
				array[num] = xmlAttribute;
				num++;
			}
		}
		array = (XmlAttribute[])ShrinkArray(array, num, typeof(XmlAttribute), isNullable: true);
		types.ExtensibleAttributes = array;
		base.Reader.MoveToElement();
		base.Reader.MoveToElement();
		if (base.Reader.IsEmptyElement)
		{
			base.Reader.Skip();
			return types;
		}
		base.Reader.ReadStartElement();
		base.Reader.MoveToContent();
		bool flag = false;
		bool flag2 = false;
		XmlSchemas schemas = types.Schemas;
		int num2 = 0;
		while (base.Reader.NodeType != XmlNodeType.EndElement)
		{
			if (base.Reader.NodeType == XmlNodeType.Element)
			{
				if (base.Reader.LocalName == "documentation" && base.Reader.NamespaceURI == "http://schemas.xmlsoap.org/wsdl/" && !flag)
				{
					types.DocumentationElement = (XmlElement)ReadXmlNode(wrapped: false);
				}
				else if (base.Reader.LocalName == "schema" && base.Reader.NamespaceURI == "http://www.w3.org/2001/XMLSchema" && !flag2)
				{
					if (schemas == null)
					{
						throw CreateReadOnlyCollectionException("System.Xml.Serialization.XmlSchemas");
					}
					schemas.Add(ReadObject_XmlSchema(isNullable: false, checkType: true));
					num2++;
				}
				else
				{
					ServiceDescription.ReadExtension(base.Document, base.Reader, types);
				}
			}
			else
			{
				UnknownNode(types);
			}
			base.Reader.MoveToContent();
		}
		ReadEndElement();
		return types;
	}

	public Service ReadObject_Service(bool isNullable, bool checkType)
	{
		Service service = null;
		if (isNullable && ReadNull())
		{
			return null;
		}
		if (checkType)
		{
			XmlQualifiedName xsiType = GetXsiType();
			if (!(xsiType == null) && (xsiType.Name != "Service" || xsiType.Namespace != "http://schemas.xmlsoap.org/wsdl/"))
			{
				throw CreateUnknownTypeException(xsiType);
			}
		}
		service = (Service)Activator.CreateInstance(typeof(Service), nonPublic: true);
		base.Reader.MoveToElement();
		int num = 0;
		XmlAttribute[] array = null;
		while (base.Reader.MoveToNextAttribute())
		{
			if (base.Reader.LocalName == "name" && base.Reader.NamespaceURI == "")
			{
				service.Name = base.Reader.Value;
			}
			else if (IsXmlnsAttribute(base.Reader.Name))
			{
				if (service.Namespaces == null)
				{
					service.Namespaces = new XmlSerializerNamespaces();
				}
				if (base.Reader.Prefix == "xmlns")
				{
					service.Namespaces.Add(base.Reader.LocalName, base.Reader.Value);
				}
				else
				{
					service.Namespaces.Add("", base.Reader.Value);
				}
			}
			else
			{
				XmlAttribute xmlAttribute = (XmlAttribute)base.Document.ReadNode(base.Reader);
				array = (XmlAttribute[])EnsureArrayIndex(array, num, typeof(XmlAttribute));
				array[num] = xmlAttribute;
				num++;
			}
		}
		array = (XmlAttribute[])ShrinkArray(array, num, typeof(XmlAttribute), isNullable: true);
		service.ExtensibleAttributes = array;
		base.Reader.MoveToElement();
		base.Reader.MoveToElement();
		if (base.Reader.IsEmptyElement)
		{
			base.Reader.Skip();
			return service;
		}
		base.Reader.ReadStartElement();
		base.Reader.MoveToContent();
		bool flag = false;
		bool flag2 = false;
		PortCollection ports = service.Ports;
		int num2 = 0;
		while (base.Reader.NodeType != XmlNodeType.EndElement)
		{
			if (base.Reader.NodeType == XmlNodeType.Element)
			{
				if (base.Reader.LocalName == "documentation" && base.Reader.NamespaceURI == "http://schemas.xmlsoap.org/wsdl/" && !flag)
				{
					service.DocumentationElement = (XmlElement)ReadXmlNode(wrapped: false);
				}
				else if (base.Reader.LocalName == "port" && base.Reader.NamespaceURI == "http://schemas.xmlsoap.org/wsdl/" && !flag2)
				{
					if (ports == null)
					{
						throw CreateReadOnlyCollectionException("System.Web.Services.Description.PortCollection");
					}
					ports.Add(ReadObject_Port(isNullable: false, checkType: true));
					num2++;
				}
				else
				{
					ServiceDescription.ReadExtension(base.Document, base.Reader, service);
				}
			}
			else
			{
				UnknownNode(service);
			}
			base.Reader.MoveToContent();
		}
		ReadEndElement();
		return service;
	}

	public Message ReadObject_Message(bool isNullable, bool checkType)
	{
		Message message = null;
		if (isNullable && ReadNull())
		{
			return null;
		}
		if (checkType)
		{
			XmlQualifiedName xsiType = GetXsiType();
			if (!(xsiType == null) && (xsiType.Name != "Message" || xsiType.Namespace != "http://schemas.xmlsoap.org/wsdl/"))
			{
				throw CreateUnknownTypeException(xsiType);
			}
		}
		message = (Message)Activator.CreateInstance(typeof(Message), nonPublic: true);
		base.Reader.MoveToElement();
		int num = 0;
		XmlAttribute[] array = null;
		while (base.Reader.MoveToNextAttribute())
		{
			if (base.Reader.LocalName == "name" && base.Reader.NamespaceURI == "")
			{
				message.Name = base.Reader.Value;
			}
			else if (IsXmlnsAttribute(base.Reader.Name))
			{
				if (message.Namespaces == null)
				{
					message.Namespaces = new XmlSerializerNamespaces();
				}
				if (base.Reader.Prefix == "xmlns")
				{
					message.Namespaces.Add(base.Reader.LocalName, base.Reader.Value);
				}
				else
				{
					message.Namespaces.Add("", base.Reader.Value);
				}
			}
			else
			{
				XmlAttribute xmlAttribute = (XmlAttribute)base.Document.ReadNode(base.Reader);
				array = (XmlAttribute[])EnsureArrayIndex(array, num, typeof(XmlAttribute));
				array[num] = xmlAttribute;
				num++;
			}
		}
		array = (XmlAttribute[])ShrinkArray(array, num, typeof(XmlAttribute), isNullable: true);
		message.ExtensibleAttributes = array;
		base.Reader.MoveToElement();
		base.Reader.MoveToElement();
		if (base.Reader.IsEmptyElement)
		{
			base.Reader.Skip();
			return message;
		}
		base.Reader.ReadStartElement();
		base.Reader.MoveToContent();
		bool flag = false;
		bool flag2 = false;
		MessagePartCollection parts = message.Parts;
		int num2 = 0;
		while (base.Reader.NodeType != XmlNodeType.EndElement)
		{
			if (base.Reader.NodeType == XmlNodeType.Element)
			{
				if (base.Reader.LocalName == "documentation" && base.Reader.NamespaceURI == "http://schemas.xmlsoap.org/wsdl/" && !flag)
				{
					message.DocumentationElement = (XmlElement)ReadXmlNode(wrapped: false);
				}
				else if (base.Reader.LocalName == "part" && base.Reader.NamespaceURI == "http://schemas.xmlsoap.org/wsdl/" && !flag2)
				{
					if (parts == null)
					{
						throw CreateReadOnlyCollectionException("System.Web.Services.Description.MessagePartCollection");
					}
					parts.Add(ReadObject_MessagePart(isNullable: false, checkType: true));
					num2++;
				}
				else
				{
					ServiceDescription.ReadExtension(base.Document, base.Reader, message);
				}
			}
			else
			{
				UnknownNode(message);
			}
			base.Reader.MoveToContent();
		}
		ReadEndElement();
		return message;
	}

	public PortType ReadObject_PortType(bool isNullable, bool checkType)
	{
		PortType portType = null;
		if (isNullable && ReadNull())
		{
			return null;
		}
		if (checkType)
		{
			XmlQualifiedName xsiType = GetXsiType();
			if (!(xsiType == null) && (xsiType.Name != "PortType" || xsiType.Namespace != "http://schemas.xmlsoap.org/wsdl/"))
			{
				throw CreateUnknownTypeException(xsiType);
			}
		}
		portType = (PortType)Activator.CreateInstance(typeof(PortType), nonPublic: true);
		base.Reader.MoveToElement();
		int num = 0;
		XmlAttribute[] array = null;
		while (base.Reader.MoveToNextAttribute())
		{
			if (base.Reader.LocalName == "name" && base.Reader.NamespaceURI == "")
			{
				portType.Name = base.Reader.Value;
			}
			else if (IsXmlnsAttribute(base.Reader.Name))
			{
				if (portType.Namespaces == null)
				{
					portType.Namespaces = new XmlSerializerNamespaces();
				}
				if (base.Reader.Prefix == "xmlns")
				{
					portType.Namespaces.Add(base.Reader.LocalName, base.Reader.Value);
				}
				else
				{
					portType.Namespaces.Add("", base.Reader.Value);
				}
			}
			else
			{
				XmlAttribute xmlAttribute = (XmlAttribute)base.Document.ReadNode(base.Reader);
				array = (XmlAttribute[])EnsureArrayIndex(array, num, typeof(XmlAttribute));
				array[num] = xmlAttribute;
				num++;
			}
		}
		array = (XmlAttribute[])ShrinkArray(array, num, typeof(XmlAttribute), isNullable: true);
		portType.ExtensibleAttributes = array;
		base.Reader.MoveToElement();
		base.Reader.MoveToElement();
		if (base.Reader.IsEmptyElement)
		{
			base.Reader.Skip();
			return portType;
		}
		base.Reader.ReadStartElement();
		base.Reader.MoveToContent();
		bool flag = false;
		bool flag2 = false;
		OperationCollection operations = portType.Operations;
		int num2 = 0;
		while (base.Reader.NodeType != XmlNodeType.EndElement)
		{
			if (base.Reader.NodeType == XmlNodeType.Element)
			{
				if (base.Reader.LocalName == "documentation" && base.Reader.NamespaceURI == "http://schemas.xmlsoap.org/wsdl/" && !flag)
				{
					portType.DocumentationElement = (XmlElement)ReadXmlNode(wrapped: false);
				}
				else if (base.Reader.LocalName == "operation" && base.Reader.NamespaceURI == "http://schemas.xmlsoap.org/wsdl/" && !flag2)
				{
					if (operations == null)
					{
						throw CreateReadOnlyCollectionException("System.Web.Services.Description.OperationCollection");
					}
					operations.Add(ReadObject_Operation(isNullable: false, checkType: true));
					num2++;
				}
				else
				{
					ServiceDescription.ReadExtension(base.Document, base.Reader, portType);
				}
			}
			else
			{
				UnknownNode(portType);
			}
			base.Reader.MoveToContent();
		}
		ReadEndElement();
		return portType;
	}

	public Import ReadObject_Import(bool isNullable, bool checkType)
	{
		Import import = null;
		if (isNullable && ReadNull())
		{
			return null;
		}
		if (checkType)
		{
			XmlQualifiedName xsiType = GetXsiType();
			if (!(xsiType == null) && (xsiType.Name != "Import" || xsiType.Namespace != "http://schemas.xmlsoap.org/wsdl/"))
			{
				throw CreateUnknownTypeException(xsiType);
			}
		}
		import = (Import)Activator.CreateInstance(typeof(Import), nonPublic: true);
		base.Reader.MoveToElement();
		int num = 0;
		XmlAttribute[] array = null;
		while (base.Reader.MoveToNextAttribute())
		{
			if (base.Reader.LocalName == "location" && base.Reader.NamespaceURI == "")
			{
				import.Location = base.Reader.Value;
			}
			else if (base.Reader.LocalName == "namespace" && base.Reader.NamespaceURI == "")
			{
				import.Namespace = base.Reader.Value;
			}
			else if (IsXmlnsAttribute(base.Reader.Name))
			{
				if (import.Namespaces == null)
				{
					import.Namespaces = new XmlSerializerNamespaces();
				}
				if (base.Reader.Prefix == "xmlns")
				{
					import.Namespaces.Add(base.Reader.LocalName, base.Reader.Value);
				}
				else
				{
					import.Namespaces.Add("", base.Reader.Value);
				}
			}
			else
			{
				XmlAttribute xmlAttribute = (XmlAttribute)base.Document.ReadNode(base.Reader);
				array = (XmlAttribute[])EnsureArrayIndex(array, num, typeof(XmlAttribute));
				array[num] = xmlAttribute;
				num++;
			}
		}
		array = (XmlAttribute[])ShrinkArray(array, num, typeof(XmlAttribute), isNullable: true);
		import.ExtensibleAttributes = array;
		base.Reader.MoveToElement();
		base.Reader.MoveToElement();
		if (base.Reader.IsEmptyElement)
		{
			base.Reader.Skip();
			return import;
		}
		base.Reader.ReadStartElement();
		base.Reader.MoveToContent();
		bool flag = false;
		while (base.Reader.NodeType != XmlNodeType.EndElement)
		{
			if (base.Reader.NodeType == XmlNodeType.Element)
			{
				if (base.Reader.LocalName == "documentation" && base.Reader.NamespaceURI == "http://schemas.xmlsoap.org/wsdl/" && !flag)
				{
					import.DocumentationElement = (XmlElement)ReadXmlNode(wrapped: false);
				}
				else
				{
					ServiceDescription.ReadExtension(base.Document, base.Reader, import);
				}
			}
			else
			{
				UnknownNode(import);
			}
			base.Reader.MoveToContent();
		}
		ReadEndElement();
		return import;
	}

	public Binding ReadObject_Binding(bool isNullable, bool checkType)
	{
		Binding binding = null;
		if (isNullable && ReadNull())
		{
			return null;
		}
		if (checkType)
		{
			XmlQualifiedName xsiType = GetXsiType();
			if (!(xsiType == null) && (xsiType.Name != "Binding" || xsiType.Namespace != "http://schemas.xmlsoap.org/wsdl/"))
			{
				throw CreateUnknownTypeException(xsiType);
			}
		}
		binding = (Binding)Activator.CreateInstance(typeof(Binding), nonPublic: true);
		base.Reader.MoveToElement();
		int num = 0;
		XmlAttribute[] array = null;
		while (base.Reader.MoveToNextAttribute())
		{
			if (base.Reader.LocalName == "name" && base.Reader.NamespaceURI == "")
			{
				binding.Name = base.Reader.Value;
			}
			else if (base.Reader.LocalName == "type" && base.Reader.NamespaceURI == "")
			{
				binding.Type = ToXmlQualifiedName(base.Reader.Value);
			}
			else if (IsXmlnsAttribute(base.Reader.Name))
			{
				if (binding.Namespaces == null)
				{
					binding.Namespaces = new XmlSerializerNamespaces();
				}
				if (base.Reader.Prefix == "xmlns")
				{
					binding.Namespaces.Add(base.Reader.LocalName, base.Reader.Value);
				}
				else
				{
					binding.Namespaces.Add("", base.Reader.Value);
				}
			}
			else
			{
				XmlAttribute xmlAttribute = (XmlAttribute)base.Document.ReadNode(base.Reader);
				array = (XmlAttribute[])EnsureArrayIndex(array, num, typeof(XmlAttribute));
				array[num] = xmlAttribute;
				num++;
			}
		}
		array = (XmlAttribute[])ShrinkArray(array, num, typeof(XmlAttribute), isNullable: true);
		binding.ExtensibleAttributes = array;
		base.Reader.MoveToElement();
		base.Reader.MoveToElement();
		if (base.Reader.IsEmptyElement)
		{
			base.Reader.Skip();
			return binding;
		}
		base.Reader.ReadStartElement();
		base.Reader.MoveToContent();
		bool flag = false;
		bool flag2 = false;
		OperationBindingCollection operations = binding.Operations;
		int num2 = 0;
		while (base.Reader.NodeType != XmlNodeType.EndElement)
		{
			if (base.Reader.NodeType == XmlNodeType.Element)
			{
				if (base.Reader.LocalName == "documentation" && base.Reader.NamespaceURI == "http://schemas.xmlsoap.org/wsdl/" && !flag)
				{
					binding.DocumentationElement = (XmlElement)ReadXmlNode(wrapped: false);
				}
				else if (base.Reader.LocalName == "operation" && base.Reader.NamespaceURI == "http://schemas.xmlsoap.org/wsdl/" && !flag2)
				{
					if (operations == null)
					{
						throw CreateReadOnlyCollectionException("System.Web.Services.Description.OperationBindingCollection");
					}
					operations.Add(ReadObject_OperationBinding(isNullable: false, checkType: true));
					num2++;
				}
				else
				{
					ServiceDescription.ReadExtension(base.Document, base.Reader, binding);
				}
			}
			else
			{
				UnknownNode(binding);
			}
			base.Reader.MoveToContent();
		}
		ReadEndElement();
		return binding;
	}

	public XmlSchema ReadObject_XmlSchema(bool isNullable, bool checkType)
	{
		XmlSchema result = XmlSchema.Read(base.Reader, null);
		base.Reader.Read();
		return result;
	}

	public Port ReadObject_Port(bool isNullable, bool checkType)
	{
		Port port = null;
		if (isNullable && ReadNull())
		{
			return null;
		}
		if (checkType)
		{
			XmlQualifiedName xsiType = GetXsiType();
			if (!(xsiType == null) && (xsiType.Name != "Port" || xsiType.Namespace != "http://schemas.xmlsoap.org/wsdl/"))
			{
				throw CreateUnknownTypeException(xsiType);
			}
		}
		port = (Port)Activator.CreateInstance(typeof(Port), nonPublic: true);
		base.Reader.MoveToElement();
		int num = 0;
		XmlAttribute[] array = null;
		while (base.Reader.MoveToNextAttribute())
		{
			if (base.Reader.LocalName == "name" && base.Reader.NamespaceURI == "")
			{
				port.Name = base.Reader.Value;
			}
			else if (base.Reader.LocalName == "binding" && base.Reader.NamespaceURI == "")
			{
				port.Binding = ToXmlQualifiedName(base.Reader.Value);
			}
			else if (IsXmlnsAttribute(base.Reader.Name))
			{
				if (port.Namespaces == null)
				{
					port.Namespaces = new XmlSerializerNamespaces();
				}
				if (base.Reader.Prefix == "xmlns")
				{
					port.Namespaces.Add(base.Reader.LocalName, base.Reader.Value);
				}
				else
				{
					port.Namespaces.Add("", base.Reader.Value);
				}
			}
			else
			{
				XmlAttribute xmlAttribute = (XmlAttribute)base.Document.ReadNode(base.Reader);
				array = (XmlAttribute[])EnsureArrayIndex(array, num, typeof(XmlAttribute));
				array[num] = xmlAttribute;
				num++;
			}
		}
		array = (XmlAttribute[])ShrinkArray(array, num, typeof(XmlAttribute), isNullable: true);
		port.ExtensibleAttributes = array;
		base.Reader.MoveToElement();
		base.Reader.MoveToElement();
		if (base.Reader.IsEmptyElement)
		{
			base.Reader.Skip();
			return port;
		}
		base.Reader.ReadStartElement();
		base.Reader.MoveToContent();
		bool flag = false;
		while (base.Reader.NodeType != XmlNodeType.EndElement)
		{
			if (base.Reader.NodeType == XmlNodeType.Element)
			{
				if (base.Reader.LocalName == "documentation" && base.Reader.NamespaceURI == "http://schemas.xmlsoap.org/wsdl/" && !flag)
				{
					port.DocumentationElement = (XmlElement)ReadXmlNode(wrapped: false);
				}
				else
				{
					ServiceDescription.ReadExtension(base.Document, base.Reader, port);
				}
			}
			else
			{
				UnknownNode(port);
			}
			base.Reader.MoveToContent();
		}
		ReadEndElement();
		return port;
	}

	public MessagePart ReadObject_MessagePart(bool isNullable, bool checkType)
	{
		MessagePart messagePart = null;
		if (isNullable && ReadNull())
		{
			return null;
		}
		if (checkType)
		{
			XmlQualifiedName xsiType = GetXsiType();
			if (!(xsiType == null) && (xsiType.Name != "MessagePart" || xsiType.Namespace != "http://schemas.xmlsoap.org/wsdl/"))
			{
				throw CreateUnknownTypeException(xsiType);
			}
		}
		messagePart = (MessagePart)Activator.CreateInstance(typeof(MessagePart), nonPublic: true);
		base.Reader.MoveToElement();
		int num = 0;
		XmlAttribute[] array = null;
		while (base.Reader.MoveToNextAttribute())
		{
			if (base.Reader.LocalName == "name" && base.Reader.NamespaceURI == "")
			{
				messagePart.Name = base.Reader.Value;
			}
			else if (base.Reader.LocalName == "element" && base.Reader.NamespaceURI == "")
			{
				messagePart.Element = ToXmlQualifiedName(base.Reader.Value);
			}
			else if (base.Reader.LocalName == "type" && base.Reader.NamespaceURI == "")
			{
				messagePart.Type = ToXmlQualifiedName(base.Reader.Value);
			}
			else if (IsXmlnsAttribute(base.Reader.Name))
			{
				if (messagePart.Namespaces == null)
				{
					messagePart.Namespaces = new XmlSerializerNamespaces();
				}
				if (base.Reader.Prefix == "xmlns")
				{
					messagePart.Namespaces.Add(base.Reader.LocalName, base.Reader.Value);
				}
				else
				{
					messagePart.Namespaces.Add("", base.Reader.Value);
				}
			}
			else
			{
				XmlAttribute xmlAttribute = (XmlAttribute)base.Document.ReadNode(base.Reader);
				array = (XmlAttribute[])EnsureArrayIndex(array, num, typeof(XmlAttribute));
				array[num] = xmlAttribute;
				num++;
			}
		}
		array = (XmlAttribute[])ShrinkArray(array, num, typeof(XmlAttribute), isNullable: true);
		messagePart.ExtensibleAttributes = array;
		base.Reader.MoveToElement();
		base.Reader.MoveToElement();
		if (base.Reader.IsEmptyElement)
		{
			base.Reader.Skip();
			return messagePart;
		}
		base.Reader.ReadStartElement();
		base.Reader.MoveToContent();
		bool flag = false;
		while (base.Reader.NodeType != XmlNodeType.EndElement)
		{
			if (base.Reader.NodeType == XmlNodeType.Element)
			{
				if (base.Reader.LocalName == "documentation" && base.Reader.NamespaceURI == "http://schemas.xmlsoap.org/wsdl/" && !flag)
				{
					messagePart.DocumentationElement = (XmlElement)ReadXmlNode(wrapped: false);
				}
				else
				{
					ServiceDescription.ReadExtension(base.Document, base.Reader, messagePart);
				}
			}
			else
			{
				UnknownNode(messagePart);
			}
			base.Reader.MoveToContent();
		}
		ReadEndElement();
		return messagePart;
	}

	public Operation ReadObject_Operation(bool isNullable, bool checkType)
	{
		Operation operation = null;
		if (isNullable && ReadNull())
		{
			return null;
		}
		if (checkType)
		{
			XmlQualifiedName xsiType = GetXsiType();
			if (!(xsiType == null) && (xsiType.Name != "Operation" || xsiType.Namespace != "http://schemas.xmlsoap.org/wsdl/"))
			{
				throw CreateUnknownTypeException(xsiType);
			}
		}
		operation = (Operation)Activator.CreateInstance(typeof(Operation), nonPublic: true);
		base.Reader.MoveToElement();
		int num = 0;
		XmlAttribute[] array = null;
		while (base.Reader.MoveToNextAttribute())
		{
			if (base.Reader.LocalName == "name" && base.Reader.NamespaceURI == "")
			{
				operation.Name = base.Reader.Value;
			}
			else if (base.Reader.LocalName == "parameterOrder" && base.Reader.NamespaceURI == "")
			{
				operation.ParameterOrderString = base.Reader.Value;
			}
			else if (IsXmlnsAttribute(base.Reader.Name))
			{
				if (operation.Namespaces == null)
				{
					operation.Namespaces = new XmlSerializerNamespaces();
				}
				if (base.Reader.Prefix == "xmlns")
				{
					operation.Namespaces.Add(base.Reader.LocalName, base.Reader.Value);
				}
				else
				{
					operation.Namespaces.Add("", base.Reader.Value);
				}
			}
			else
			{
				XmlAttribute xmlAttribute = (XmlAttribute)base.Document.ReadNode(base.Reader);
				array = (XmlAttribute[])EnsureArrayIndex(array, num, typeof(XmlAttribute));
				array[num] = xmlAttribute;
				num++;
			}
		}
		array = (XmlAttribute[])ShrinkArray(array, num, typeof(XmlAttribute), isNullable: true);
		operation.ExtensibleAttributes = array;
		base.Reader.MoveToElement();
		base.Reader.MoveToElement();
		if (base.Reader.IsEmptyElement)
		{
			base.Reader.Skip();
			return operation;
		}
		base.Reader.ReadStartElement();
		base.Reader.MoveToContent();
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		OperationFaultCollection faults = operation.Faults;
		OperationMessageCollection messages = operation.Messages;
		int num2 = 0;
		int num3 = 0;
		while (base.Reader.NodeType != XmlNodeType.EndElement)
		{
			if (base.Reader.NodeType == XmlNodeType.Element)
			{
				if (base.Reader.LocalName == "output" && base.Reader.NamespaceURI == "http://schemas.xmlsoap.org/wsdl/" && !flag3)
				{
					if (messages == null)
					{
						throw CreateReadOnlyCollectionException("System.Web.Services.Description.OperationMessageCollection");
					}
					messages.Add(ReadObject_OperationOutput(isNullable: false, checkType: true));
					num3++;
				}
				else if (base.Reader.LocalName == "input" && base.Reader.NamespaceURI == "http://schemas.xmlsoap.org/wsdl/" && !flag3)
				{
					if (messages == null)
					{
						throw CreateReadOnlyCollectionException("System.Web.Services.Description.OperationMessageCollection");
					}
					messages.Add(ReadObject_OperationInput(isNullable: false, checkType: true));
					num3++;
				}
				else if (base.Reader.LocalName == "documentation" && base.Reader.NamespaceURI == "http://schemas.xmlsoap.org/wsdl/" && !flag)
				{
					operation.DocumentationElement = (XmlElement)ReadXmlNode(wrapped: false);
				}
				else if (base.Reader.LocalName == "fault" && base.Reader.NamespaceURI == "http://schemas.xmlsoap.org/wsdl/" && !flag2)
				{
					if (faults == null)
					{
						throw CreateReadOnlyCollectionException("System.Web.Services.Description.OperationFaultCollection");
					}
					faults.Add(ReadObject_OperationFault(isNullable: false, checkType: true));
					num2++;
				}
				else
				{
					ServiceDescription.ReadExtension(base.Document, base.Reader, operation);
				}
			}
			else
			{
				UnknownNode(operation);
			}
			base.Reader.MoveToContent();
		}
		ReadEndElement();
		return operation;
	}

	public OperationBinding ReadObject_OperationBinding(bool isNullable, bool checkType)
	{
		OperationBinding operationBinding = null;
		if (isNullable && ReadNull())
		{
			return null;
		}
		if (checkType)
		{
			XmlQualifiedName xsiType = GetXsiType();
			if (!(xsiType == null) && (xsiType.Name != "OperationBinding" || xsiType.Namespace != "http://schemas.xmlsoap.org/wsdl/"))
			{
				throw CreateUnknownTypeException(xsiType);
			}
		}
		operationBinding = (OperationBinding)Activator.CreateInstance(typeof(OperationBinding), nonPublic: true);
		base.Reader.MoveToElement();
		int num = 0;
		XmlAttribute[] array = null;
		while (base.Reader.MoveToNextAttribute())
		{
			if (base.Reader.LocalName == "name" && base.Reader.NamespaceURI == "")
			{
				operationBinding.Name = base.Reader.Value;
			}
			else if (IsXmlnsAttribute(base.Reader.Name))
			{
				if (operationBinding.Namespaces == null)
				{
					operationBinding.Namespaces = new XmlSerializerNamespaces();
				}
				if (base.Reader.Prefix == "xmlns")
				{
					operationBinding.Namespaces.Add(base.Reader.LocalName, base.Reader.Value);
				}
				else
				{
					operationBinding.Namespaces.Add("", base.Reader.Value);
				}
			}
			else
			{
				XmlAttribute xmlAttribute = (XmlAttribute)base.Document.ReadNode(base.Reader);
				array = (XmlAttribute[])EnsureArrayIndex(array, num, typeof(XmlAttribute));
				array[num] = xmlAttribute;
				num++;
			}
		}
		array = (XmlAttribute[])ShrinkArray(array, num, typeof(XmlAttribute), isNullable: true);
		operationBinding.ExtensibleAttributes = array;
		base.Reader.MoveToElement();
		base.Reader.MoveToElement();
		if (base.Reader.IsEmptyElement)
		{
			base.Reader.Skip();
			return operationBinding;
		}
		base.Reader.ReadStartElement();
		base.Reader.MoveToContent();
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = false;
		FaultBindingCollection faults = operationBinding.Faults;
		int num2 = 0;
		while (base.Reader.NodeType != XmlNodeType.EndElement)
		{
			if (base.Reader.NodeType == XmlNodeType.Element)
			{
				if (base.Reader.LocalName == "input" && base.Reader.NamespaceURI == "http://schemas.xmlsoap.org/wsdl/" && !flag3)
				{
					flag3 = true;
					operationBinding.Input = ReadObject_InputBinding(isNullable: false, checkType: true);
				}
				else if (base.Reader.LocalName == "output" && base.Reader.NamespaceURI == "http://schemas.xmlsoap.org/wsdl/" && !flag4)
				{
					flag4 = true;
					operationBinding.Output = ReadObject_OutputBinding(isNullable: false, checkType: true);
				}
				else if (base.Reader.LocalName == "documentation" && base.Reader.NamespaceURI == "http://schemas.xmlsoap.org/wsdl/" && !flag)
				{
					operationBinding.DocumentationElement = (XmlElement)ReadXmlNode(wrapped: false);
				}
				else if (base.Reader.LocalName == "fault" && base.Reader.NamespaceURI == "http://schemas.xmlsoap.org/wsdl/" && !flag2)
				{
					if (faults == null)
					{
						throw CreateReadOnlyCollectionException("System.Web.Services.Description.FaultBindingCollection");
					}
					faults.Add(ReadObject_FaultBinding(isNullable: false, checkType: true));
					num2++;
				}
				else
				{
					ServiceDescription.ReadExtension(base.Document, base.Reader, operationBinding);
				}
			}
			else
			{
				UnknownNode(operationBinding);
			}
			base.Reader.MoveToContent();
		}
		ReadEndElement();
		return operationBinding;
	}

	public OperationOutput ReadObject_OperationOutput(bool isNullable, bool checkType)
	{
		OperationOutput operationOutput = null;
		if (isNullable && ReadNull())
		{
			return null;
		}
		if (checkType)
		{
			XmlQualifiedName xsiType = GetXsiType();
			if (!(xsiType == null) && (xsiType.Name != "OperationOutput" || xsiType.Namespace != "http://schemas.xmlsoap.org/wsdl/"))
			{
				throw CreateUnknownTypeException(xsiType);
			}
		}
		operationOutput = (OperationOutput)Activator.CreateInstance(typeof(OperationOutput), nonPublic: true);
		base.Reader.MoveToElement();
		int num = 0;
		XmlAttribute[] array = null;
		while (base.Reader.MoveToNextAttribute())
		{
			if (base.Reader.LocalName == "name" && base.Reader.NamespaceURI == "")
			{
				operationOutput.Name = base.Reader.Value;
			}
			else if (base.Reader.LocalName == "message" && base.Reader.NamespaceURI == "")
			{
				operationOutput.Message = ToXmlQualifiedName(base.Reader.Value);
			}
			else if (IsXmlnsAttribute(base.Reader.Name))
			{
				if (operationOutput.Namespaces == null)
				{
					operationOutput.Namespaces = new XmlSerializerNamespaces();
				}
				if (base.Reader.Prefix == "xmlns")
				{
					operationOutput.Namespaces.Add(base.Reader.LocalName, base.Reader.Value);
				}
				else
				{
					operationOutput.Namespaces.Add("", base.Reader.Value);
				}
			}
			else
			{
				XmlAttribute xmlAttribute = (XmlAttribute)base.Document.ReadNode(base.Reader);
				array = (XmlAttribute[])EnsureArrayIndex(array, num, typeof(XmlAttribute));
				array[num] = xmlAttribute;
				num++;
			}
		}
		array = (XmlAttribute[])ShrinkArray(array, num, typeof(XmlAttribute), isNullable: true);
		operationOutput.ExtensibleAttributes = array;
		base.Reader.MoveToElement();
		base.Reader.MoveToElement();
		if (base.Reader.IsEmptyElement)
		{
			base.Reader.Skip();
			return operationOutput;
		}
		base.Reader.ReadStartElement();
		base.Reader.MoveToContent();
		bool flag = false;
		while (base.Reader.NodeType != XmlNodeType.EndElement)
		{
			if (base.Reader.NodeType == XmlNodeType.Element)
			{
				if (base.Reader.LocalName == "documentation" && base.Reader.NamespaceURI == "http://schemas.xmlsoap.org/wsdl/" && !flag)
				{
					operationOutput.DocumentationElement = (XmlElement)ReadXmlNode(wrapped: false);
				}
				else
				{
					ServiceDescription.ReadExtension(base.Document, base.Reader, operationOutput);
				}
			}
			else
			{
				UnknownNode(operationOutput);
			}
			base.Reader.MoveToContent();
		}
		ReadEndElement();
		return operationOutput;
	}

	public OperationInput ReadObject_OperationInput(bool isNullable, bool checkType)
	{
		OperationInput operationInput = null;
		if (isNullable && ReadNull())
		{
			return null;
		}
		if (checkType)
		{
			XmlQualifiedName xsiType = GetXsiType();
			if (!(xsiType == null) && (xsiType.Name != "OperationInput" || xsiType.Namespace != "http://schemas.xmlsoap.org/wsdl/"))
			{
				throw CreateUnknownTypeException(xsiType);
			}
		}
		operationInput = (OperationInput)Activator.CreateInstance(typeof(OperationInput), nonPublic: true);
		base.Reader.MoveToElement();
		int num = 0;
		XmlAttribute[] array = null;
		while (base.Reader.MoveToNextAttribute())
		{
			if (base.Reader.LocalName == "name" && base.Reader.NamespaceURI == "")
			{
				operationInput.Name = base.Reader.Value;
			}
			else if (base.Reader.LocalName == "message" && base.Reader.NamespaceURI == "")
			{
				operationInput.Message = ToXmlQualifiedName(base.Reader.Value);
			}
			else if (IsXmlnsAttribute(base.Reader.Name))
			{
				if (operationInput.Namespaces == null)
				{
					operationInput.Namespaces = new XmlSerializerNamespaces();
				}
				if (base.Reader.Prefix == "xmlns")
				{
					operationInput.Namespaces.Add(base.Reader.LocalName, base.Reader.Value);
				}
				else
				{
					operationInput.Namespaces.Add("", base.Reader.Value);
				}
			}
			else
			{
				XmlAttribute xmlAttribute = (XmlAttribute)base.Document.ReadNode(base.Reader);
				array = (XmlAttribute[])EnsureArrayIndex(array, num, typeof(XmlAttribute));
				array[num] = xmlAttribute;
				num++;
			}
		}
		array = (XmlAttribute[])ShrinkArray(array, num, typeof(XmlAttribute), isNullable: true);
		operationInput.ExtensibleAttributes = array;
		base.Reader.MoveToElement();
		base.Reader.MoveToElement();
		if (base.Reader.IsEmptyElement)
		{
			base.Reader.Skip();
			return operationInput;
		}
		base.Reader.ReadStartElement();
		base.Reader.MoveToContent();
		bool flag = false;
		while (base.Reader.NodeType != XmlNodeType.EndElement)
		{
			if (base.Reader.NodeType == XmlNodeType.Element)
			{
				if (base.Reader.LocalName == "documentation" && base.Reader.NamespaceURI == "http://schemas.xmlsoap.org/wsdl/" && !flag)
				{
					operationInput.DocumentationElement = (XmlElement)ReadXmlNode(wrapped: false);
				}
				else
				{
					ServiceDescription.ReadExtension(base.Document, base.Reader, operationInput);
				}
			}
			else
			{
				UnknownNode(operationInput);
			}
			base.Reader.MoveToContent();
		}
		ReadEndElement();
		return operationInput;
	}

	public OperationFault ReadObject_OperationFault(bool isNullable, bool checkType)
	{
		OperationFault operationFault = null;
		if (isNullable && ReadNull())
		{
			return null;
		}
		if (checkType)
		{
			XmlQualifiedName xsiType = GetXsiType();
			if (!(xsiType == null) && (xsiType.Name != "OperationFault" || xsiType.Namespace != "http://schemas.xmlsoap.org/wsdl/"))
			{
				throw CreateUnknownTypeException(xsiType);
			}
		}
		operationFault = (OperationFault)Activator.CreateInstance(typeof(OperationFault), nonPublic: true);
		base.Reader.MoveToElement();
		int num = 0;
		XmlAttribute[] array = null;
		while (base.Reader.MoveToNextAttribute())
		{
			if (base.Reader.LocalName == "name" && base.Reader.NamespaceURI == "")
			{
				operationFault.Name = base.Reader.Value;
			}
			else if (base.Reader.LocalName == "message" && base.Reader.NamespaceURI == "")
			{
				operationFault.Message = ToXmlQualifiedName(base.Reader.Value);
			}
			else if (IsXmlnsAttribute(base.Reader.Name))
			{
				if (operationFault.Namespaces == null)
				{
					operationFault.Namespaces = new XmlSerializerNamespaces();
				}
				if (base.Reader.Prefix == "xmlns")
				{
					operationFault.Namespaces.Add(base.Reader.LocalName, base.Reader.Value);
				}
				else
				{
					operationFault.Namespaces.Add("", base.Reader.Value);
				}
			}
			else
			{
				XmlAttribute xmlAttribute = (XmlAttribute)base.Document.ReadNode(base.Reader);
				array = (XmlAttribute[])EnsureArrayIndex(array, num, typeof(XmlAttribute));
				array[num] = xmlAttribute;
				num++;
			}
		}
		array = (XmlAttribute[])ShrinkArray(array, num, typeof(XmlAttribute), isNullable: true);
		operationFault.ExtensibleAttributes = array;
		base.Reader.MoveToElement();
		base.Reader.MoveToElement();
		if (base.Reader.IsEmptyElement)
		{
			base.Reader.Skip();
			return operationFault;
		}
		base.Reader.ReadStartElement();
		base.Reader.MoveToContent();
		bool flag = false;
		while (base.Reader.NodeType != XmlNodeType.EndElement)
		{
			if (base.Reader.NodeType == XmlNodeType.Element)
			{
				if (base.Reader.LocalName == "documentation" && base.Reader.NamespaceURI == "http://schemas.xmlsoap.org/wsdl/" && !flag)
				{
					operationFault.DocumentationElement = (XmlElement)ReadXmlNode(wrapped: false);
				}
				else
				{
					ServiceDescription.ReadExtension(base.Document, base.Reader, operationFault);
				}
			}
			else
			{
				UnknownNode(operationFault);
			}
			base.Reader.MoveToContent();
		}
		ReadEndElement();
		return operationFault;
	}

	public InputBinding ReadObject_InputBinding(bool isNullable, bool checkType)
	{
		InputBinding inputBinding = null;
		if (isNullable && ReadNull())
		{
			return null;
		}
		if (checkType)
		{
			XmlQualifiedName xsiType = GetXsiType();
			if (!(xsiType == null) && (xsiType.Name != "InputBinding" || xsiType.Namespace != "http://schemas.xmlsoap.org/wsdl/"))
			{
				throw CreateUnknownTypeException(xsiType);
			}
		}
		inputBinding = (InputBinding)Activator.CreateInstance(typeof(InputBinding), nonPublic: true);
		base.Reader.MoveToElement();
		int num = 0;
		XmlAttribute[] array = null;
		while (base.Reader.MoveToNextAttribute())
		{
			if (base.Reader.LocalName == "name" && base.Reader.NamespaceURI == "")
			{
				inputBinding.Name = base.Reader.Value;
			}
			else if (IsXmlnsAttribute(base.Reader.Name))
			{
				if (inputBinding.Namespaces == null)
				{
					inputBinding.Namespaces = new XmlSerializerNamespaces();
				}
				if (base.Reader.Prefix == "xmlns")
				{
					inputBinding.Namespaces.Add(base.Reader.LocalName, base.Reader.Value);
				}
				else
				{
					inputBinding.Namespaces.Add("", base.Reader.Value);
				}
			}
			else
			{
				XmlAttribute xmlAttribute = (XmlAttribute)base.Document.ReadNode(base.Reader);
				array = (XmlAttribute[])EnsureArrayIndex(array, num, typeof(XmlAttribute));
				array[num] = xmlAttribute;
				num++;
			}
		}
		array = (XmlAttribute[])ShrinkArray(array, num, typeof(XmlAttribute), isNullable: true);
		inputBinding.ExtensibleAttributes = array;
		base.Reader.MoveToElement();
		base.Reader.MoveToElement();
		if (base.Reader.IsEmptyElement)
		{
			base.Reader.Skip();
			return inputBinding;
		}
		base.Reader.ReadStartElement();
		base.Reader.MoveToContent();
		bool flag = false;
		while (base.Reader.NodeType != XmlNodeType.EndElement)
		{
			if (base.Reader.NodeType == XmlNodeType.Element)
			{
				if (base.Reader.LocalName == "documentation" && base.Reader.NamespaceURI == "http://schemas.xmlsoap.org/wsdl/" && !flag)
				{
					inputBinding.DocumentationElement = (XmlElement)ReadXmlNode(wrapped: false);
				}
				else
				{
					ServiceDescription.ReadExtension(base.Document, base.Reader, inputBinding);
				}
			}
			else
			{
				UnknownNode(inputBinding);
			}
			base.Reader.MoveToContent();
		}
		ReadEndElement();
		return inputBinding;
	}

	public OutputBinding ReadObject_OutputBinding(bool isNullable, bool checkType)
	{
		OutputBinding outputBinding = null;
		if (isNullable && ReadNull())
		{
			return null;
		}
		if (checkType)
		{
			XmlQualifiedName xsiType = GetXsiType();
			if (!(xsiType == null) && (xsiType.Name != "OutputBinding" || xsiType.Namespace != "http://schemas.xmlsoap.org/wsdl/"))
			{
				throw CreateUnknownTypeException(xsiType);
			}
		}
		outputBinding = (OutputBinding)Activator.CreateInstance(typeof(OutputBinding), nonPublic: true);
		base.Reader.MoveToElement();
		int num = 0;
		XmlAttribute[] array = null;
		while (base.Reader.MoveToNextAttribute())
		{
			if (base.Reader.LocalName == "name" && base.Reader.NamespaceURI == "")
			{
				outputBinding.Name = base.Reader.Value;
			}
			else if (IsXmlnsAttribute(base.Reader.Name))
			{
				if (outputBinding.Namespaces == null)
				{
					outputBinding.Namespaces = new XmlSerializerNamespaces();
				}
				if (base.Reader.Prefix == "xmlns")
				{
					outputBinding.Namespaces.Add(base.Reader.LocalName, base.Reader.Value);
				}
				else
				{
					outputBinding.Namespaces.Add("", base.Reader.Value);
				}
			}
			else
			{
				XmlAttribute xmlAttribute = (XmlAttribute)base.Document.ReadNode(base.Reader);
				array = (XmlAttribute[])EnsureArrayIndex(array, num, typeof(XmlAttribute));
				array[num] = xmlAttribute;
				num++;
			}
		}
		array = (XmlAttribute[])ShrinkArray(array, num, typeof(XmlAttribute), isNullable: true);
		outputBinding.ExtensibleAttributes = array;
		base.Reader.MoveToElement();
		base.Reader.MoveToElement();
		if (base.Reader.IsEmptyElement)
		{
			base.Reader.Skip();
			return outputBinding;
		}
		base.Reader.ReadStartElement();
		base.Reader.MoveToContent();
		bool flag = false;
		while (base.Reader.NodeType != XmlNodeType.EndElement)
		{
			if (base.Reader.NodeType == XmlNodeType.Element)
			{
				if (base.Reader.LocalName == "documentation" && base.Reader.NamespaceURI == "http://schemas.xmlsoap.org/wsdl/" && !flag)
				{
					outputBinding.DocumentationElement = (XmlElement)ReadXmlNode(wrapped: false);
				}
				else
				{
					ServiceDescription.ReadExtension(base.Document, base.Reader, outputBinding);
				}
			}
			else
			{
				UnknownNode(outputBinding);
			}
			base.Reader.MoveToContent();
		}
		ReadEndElement();
		return outputBinding;
	}

	public FaultBinding ReadObject_FaultBinding(bool isNullable, bool checkType)
	{
		FaultBinding faultBinding = null;
		if (isNullable && ReadNull())
		{
			return null;
		}
		if (checkType)
		{
			XmlQualifiedName xsiType = GetXsiType();
			if (!(xsiType == null) && (xsiType.Name != "FaultBinding" || xsiType.Namespace != "http://schemas.xmlsoap.org/wsdl/"))
			{
				throw CreateUnknownTypeException(xsiType);
			}
		}
		faultBinding = (FaultBinding)Activator.CreateInstance(typeof(FaultBinding), nonPublic: true);
		base.Reader.MoveToElement();
		int num = 0;
		XmlAttribute[] array = null;
		while (base.Reader.MoveToNextAttribute())
		{
			if (base.Reader.LocalName == "name" && base.Reader.NamespaceURI == "")
			{
				faultBinding.Name = base.Reader.Value;
			}
			else if (IsXmlnsAttribute(base.Reader.Name))
			{
				if (faultBinding.Namespaces == null)
				{
					faultBinding.Namespaces = new XmlSerializerNamespaces();
				}
				if (base.Reader.Prefix == "xmlns")
				{
					faultBinding.Namespaces.Add(base.Reader.LocalName, base.Reader.Value);
				}
				else
				{
					faultBinding.Namespaces.Add("", base.Reader.Value);
				}
			}
			else
			{
				XmlAttribute xmlAttribute = (XmlAttribute)base.Document.ReadNode(base.Reader);
				array = (XmlAttribute[])EnsureArrayIndex(array, num, typeof(XmlAttribute));
				array[num] = xmlAttribute;
				num++;
			}
		}
		array = (XmlAttribute[])ShrinkArray(array, num, typeof(XmlAttribute), isNullable: true);
		faultBinding.ExtensibleAttributes = array;
		base.Reader.MoveToElement();
		base.Reader.MoveToElement();
		if (base.Reader.IsEmptyElement)
		{
			base.Reader.Skip();
			return faultBinding;
		}
		base.Reader.ReadStartElement();
		base.Reader.MoveToContent();
		bool flag = false;
		while (base.Reader.NodeType != XmlNodeType.EndElement)
		{
			if (base.Reader.NodeType == XmlNodeType.Element)
			{
				if (base.Reader.LocalName == "documentation" && base.Reader.NamespaceURI == "http://schemas.xmlsoap.org/wsdl/" && !flag)
				{
					faultBinding.DocumentationElement = (XmlElement)ReadXmlNode(wrapped: false);
				}
				else
				{
					ServiceDescription.ReadExtension(base.Document, base.Reader, faultBinding);
				}
			}
			else
			{
				UnknownNode(faultBinding);
			}
			base.Reader.MoveToContent();
		}
		ReadEndElement();
		return faultBinding;
	}

	protected override void InitCallbacks()
	{
	}

	protected override void InitIDs()
	{
	}
}
