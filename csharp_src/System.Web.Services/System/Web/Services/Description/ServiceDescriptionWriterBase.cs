using System.Collections;
using System.Reflection;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace System.Web.Services.Description;

internal class ServiceDescriptionWriterBase : XmlSerializationWriter
{
	private const string xmlNamespace = "http://www.w3.org/2000/xmlns/";

	private static readonly MethodInfo toBinHexStringMethod = typeof(XmlConvert).GetMethod("ToBinHexString", BindingFlags.Static | BindingFlags.NonPublic, null, new Type[1] { typeof(byte[]) }, null);

	private static string ToBinHexString(byte[] input)
	{
		if (input != null)
		{
			return (string)toBinHexStringMethod.Invoke(null, new object[1] { input });
		}
		return null;
	}

	public void WriteRoot_ServiceDescription(object o)
	{
		WriteStartDocument();
		ServiceDescription ob = (ServiceDescription)o;
		TopLevelElement();
		WriteObject_ServiceDescription(ob, "definitions", "http://schemas.xmlsoap.org/wsdl/", isNullable: true, needType: false, writeWrappingElem: true);
	}

	private void WriteObject_ServiceDescription(ServiceDescription ob, string element, string namesp, bool isNullable, bool needType, bool writeWrappingElem)
	{
		if (ob == null)
		{
			if (isNullable)
			{
				WriteNullTagLiteral(element, namesp);
			}
			return;
		}
		if (!(ob.GetType() == typeof(ServiceDescription)))
		{
			throw CreateUnknownTypeException(ob);
		}
		if (writeWrappingElem)
		{
			WriteStartElement(element, namesp, ob);
		}
		if (needType)
		{
			WriteXsiType("ServiceDescription", "http://schemas.xmlsoap.org/wsdl/");
		}
		WriteNamespaceDeclarations(ob.Namespaces);
		ICollection extensibleAttributes = ob.ExtensibleAttributes;
		if (extensibleAttributes != null)
		{
			foreach (XmlAttribute item in extensibleAttributes)
			{
				if (item.NamespaceURI != "http://www.w3.org/2000/xmlns/")
				{
					WriteXmlAttribute(item, ob);
				}
			}
		}
		WriteAttribute("name", "", ob.Name);
		WriteAttribute("targetNamespace", "", ob.TargetNamespace);
		ServiceDescription.WriteExtensions(base.Writer, ob);
		if (ob.DocumentationElement != null)
		{
			XmlNode documentationElement = ob.DocumentationElement;
			if (!(documentationElement is XmlElement))
			{
				throw CreateUnknownAnyElementException(documentationElement.Name, documentationElement.NamespaceURI);
			}
			if (!(documentationElement.LocalName == "documentation") || !(documentationElement.NamespaceURI == "http://schemas.xmlsoap.org/wsdl/"))
			{
				documentationElement.WriteTo(base.Writer);
			}
			WriteElementLiteral(documentationElement, "", "", isNullable: false, any: true);
		}
		if (ob.Imports != null)
		{
			for (int i = 0; i < ob.Imports.Count; i++)
			{
				WriteObject_Import(ob.Imports[i], "import", "http://schemas.xmlsoap.org/wsdl/", isNullable: false, needType: false, writeWrappingElem: true);
			}
		}
		WriteObject_Types(ob.Types, "types", "http://schemas.xmlsoap.org/wsdl/", isNullable: false, needType: false, writeWrappingElem: true);
		if (ob.Messages != null)
		{
			for (int j = 0; j < ob.Messages.Count; j++)
			{
				WriteObject_Message(ob.Messages[j], "message", "http://schemas.xmlsoap.org/wsdl/", isNullable: false, needType: false, writeWrappingElem: true);
			}
		}
		if (ob.PortTypes != null)
		{
			for (int k = 0; k < ob.PortTypes.Count; k++)
			{
				WriteObject_PortType(ob.PortTypes[k], "portType", "http://schemas.xmlsoap.org/wsdl/", isNullable: false, needType: false, writeWrappingElem: true);
			}
		}
		if (ob.Bindings != null)
		{
			for (int l = 0; l < ob.Bindings.Count; l++)
			{
				WriteObject_Binding(ob.Bindings[l], "binding", "http://schemas.xmlsoap.org/wsdl/", isNullable: false, needType: false, writeWrappingElem: true);
			}
		}
		if (ob.Services != null)
		{
			for (int m = 0; m < ob.Services.Count; m++)
			{
				WriteObject_Service(ob.Services[m], "service", "http://schemas.xmlsoap.org/wsdl/", isNullable: false, needType: false, writeWrappingElem: true);
			}
		}
		if (writeWrappingElem)
		{
			WriteEndElement(ob);
		}
	}

	private void WriteObject_Import(Import ob, string element, string namesp, bool isNullable, bool needType, bool writeWrappingElem)
	{
		if (ob == null)
		{
			if (isNullable)
			{
				WriteNullTagLiteral(element, namesp);
			}
			return;
		}
		if (!(ob.GetType() == typeof(Import)))
		{
			throw CreateUnknownTypeException(ob);
		}
		if (writeWrappingElem)
		{
			WriteStartElement(element, namesp, ob);
		}
		if (needType)
		{
			WriteXsiType("Import", "http://schemas.xmlsoap.org/wsdl/");
		}
		WriteNamespaceDeclarations(ob.Namespaces);
		ICollection extensibleAttributes = ob.ExtensibleAttributes;
		if (extensibleAttributes != null)
		{
			foreach (XmlAttribute item in extensibleAttributes)
			{
				if (item.NamespaceURI != "http://www.w3.org/2000/xmlns/")
				{
					WriteXmlAttribute(item, ob);
				}
			}
		}
		WriteAttribute("location", "", ob.Location);
		WriteAttribute("namespace", "", ob.Namespace);
		ServiceDescription.WriteExtensions(base.Writer, ob);
		if (ob.DocumentationElement != null)
		{
			XmlNode documentationElement = ob.DocumentationElement;
			if (!(documentationElement is XmlElement))
			{
				throw CreateUnknownAnyElementException(documentationElement.Name, documentationElement.NamespaceURI);
			}
			if (!(documentationElement.LocalName == "documentation") || !(documentationElement.NamespaceURI == "http://schemas.xmlsoap.org/wsdl/"))
			{
				documentationElement.WriteTo(base.Writer);
			}
			WriteElementLiteral(documentationElement, "", "", isNullable: false, any: true);
		}
		if (writeWrappingElem)
		{
			WriteEndElement(ob);
		}
	}

	private void WriteObject_Types(Types ob, string element, string namesp, bool isNullable, bool needType, bool writeWrappingElem)
	{
		if (ob == null)
		{
			if (isNullable)
			{
				WriteNullTagLiteral(element, namesp);
			}
			return;
		}
		if (!(ob.GetType() == typeof(Types)))
		{
			throw CreateUnknownTypeException(ob);
		}
		if (writeWrappingElem)
		{
			WriteStartElement(element, namesp, ob);
		}
		if (needType)
		{
			WriteXsiType("Types", "http://schemas.xmlsoap.org/wsdl/");
		}
		WriteNamespaceDeclarations(ob.Namespaces);
		ICollection extensibleAttributes = ob.ExtensibleAttributes;
		if (extensibleAttributes != null)
		{
			foreach (XmlAttribute item in extensibleAttributes)
			{
				if (item.NamespaceURI != "http://www.w3.org/2000/xmlns/")
				{
					WriteXmlAttribute(item, ob);
				}
			}
		}
		ServiceDescription.WriteExtensions(base.Writer, ob);
		if (ob.DocumentationElement != null)
		{
			XmlNode documentationElement = ob.DocumentationElement;
			if (!(documentationElement is XmlElement))
			{
				throw CreateUnknownAnyElementException(documentationElement.Name, documentationElement.NamespaceURI);
			}
			if (!(documentationElement.LocalName == "documentation") || !(documentationElement.NamespaceURI == "http://schemas.xmlsoap.org/wsdl/"))
			{
				documentationElement.WriteTo(base.Writer);
			}
			WriteElementLiteral(documentationElement, "", "", isNullable: false, any: true);
		}
		if (ob.Schemas != null)
		{
			for (int i = 0; i < ob.Schemas.Count; i++)
			{
				WriteObject_XmlSchema(ob.Schemas[i], "schema", "http://www.w3.org/2001/XMLSchema", isNullable: false, needType: false, writeWrappingElem: true);
			}
		}
		if (writeWrappingElem)
		{
			WriteEndElement(ob);
		}
	}

	private void WriteObject_Message(Message ob, string element, string namesp, bool isNullable, bool needType, bool writeWrappingElem)
	{
		if (ob == null)
		{
			if (isNullable)
			{
				WriteNullTagLiteral(element, namesp);
			}
			return;
		}
		if (!(ob.GetType() == typeof(Message)))
		{
			throw CreateUnknownTypeException(ob);
		}
		if (writeWrappingElem)
		{
			WriteStartElement(element, namesp, ob);
		}
		if (needType)
		{
			WriteXsiType("Message", "http://schemas.xmlsoap.org/wsdl/");
		}
		WriteNamespaceDeclarations(ob.Namespaces);
		ICollection extensibleAttributes = ob.ExtensibleAttributes;
		if (extensibleAttributes != null)
		{
			foreach (XmlAttribute item in extensibleAttributes)
			{
				if (item.NamespaceURI != "http://www.w3.org/2000/xmlns/")
				{
					WriteXmlAttribute(item, ob);
				}
			}
		}
		WriteAttribute("name", "", ob.Name);
		ServiceDescription.WriteExtensions(base.Writer, ob);
		if (ob.DocumentationElement != null)
		{
			XmlNode documentationElement = ob.DocumentationElement;
			if (!(documentationElement is XmlElement))
			{
				throw CreateUnknownAnyElementException(documentationElement.Name, documentationElement.NamespaceURI);
			}
			if (!(documentationElement.LocalName == "documentation") || !(documentationElement.NamespaceURI == "http://schemas.xmlsoap.org/wsdl/"))
			{
				documentationElement.WriteTo(base.Writer);
			}
			WriteElementLiteral(documentationElement, "", "", isNullable: false, any: true);
		}
		if (ob.Parts != null)
		{
			for (int i = 0; i < ob.Parts.Count; i++)
			{
				WriteObject_MessagePart(ob.Parts[i], "part", "http://schemas.xmlsoap.org/wsdl/", isNullable: false, needType: false, writeWrappingElem: true);
			}
		}
		if (writeWrappingElem)
		{
			WriteEndElement(ob);
		}
	}

	private void WriteObject_PortType(PortType ob, string element, string namesp, bool isNullable, bool needType, bool writeWrappingElem)
	{
		if (ob == null)
		{
			if (isNullable)
			{
				WriteNullTagLiteral(element, namesp);
			}
			return;
		}
		if (!(ob.GetType() == typeof(PortType)))
		{
			throw CreateUnknownTypeException(ob);
		}
		if (writeWrappingElem)
		{
			WriteStartElement(element, namesp, ob);
		}
		if (needType)
		{
			WriteXsiType("PortType", "http://schemas.xmlsoap.org/wsdl/");
		}
		WriteNamespaceDeclarations(ob.Namespaces);
		ICollection extensibleAttributes = ob.ExtensibleAttributes;
		if (extensibleAttributes != null)
		{
			foreach (XmlAttribute item in extensibleAttributes)
			{
				if (item.NamespaceURI != "http://www.w3.org/2000/xmlns/")
				{
					WriteXmlAttribute(item, ob);
				}
			}
		}
		WriteAttribute("name", "", ob.Name);
		ServiceDescription.WriteExtensions(base.Writer, ob);
		if (ob.DocumentationElement != null)
		{
			XmlNode documentationElement = ob.DocumentationElement;
			if (!(documentationElement is XmlElement))
			{
				throw CreateUnknownAnyElementException(documentationElement.Name, documentationElement.NamespaceURI);
			}
			if (!(documentationElement.LocalName == "documentation") || !(documentationElement.NamespaceURI == "http://schemas.xmlsoap.org/wsdl/"))
			{
				documentationElement.WriteTo(base.Writer);
			}
			WriteElementLiteral(documentationElement, "", "", isNullable: false, any: true);
		}
		if (ob.Operations != null)
		{
			for (int i = 0; i < ob.Operations.Count; i++)
			{
				WriteObject_Operation(ob.Operations[i], "operation", "http://schemas.xmlsoap.org/wsdl/", isNullable: false, needType: false, writeWrappingElem: true);
			}
		}
		if (writeWrappingElem)
		{
			WriteEndElement(ob);
		}
	}

	private void WriteObject_Binding(Binding ob, string element, string namesp, bool isNullable, bool needType, bool writeWrappingElem)
	{
		if (ob == null)
		{
			if (isNullable)
			{
				WriteNullTagLiteral(element, namesp);
			}
			return;
		}
		if (!(ob.GetType() == typeof(Binding)))
		{
			throw CreateUnknownTypeException(ob);
		}
		if (writeWrappingElem)
		{
			WriteStartElement(element, namesp, ob);
		}
		if (needType)
		{
			WriteXsiType("Binding", "http://schemas.xmlsoap.org/wsdl/");
		}
		WriteNamespaceDeclarations(ob.Namespaces);
		ICollection extensibleAttributes = ob.ExtensibleAttributes;
		if (extensibleAttributes != null)
		{
			foreach (XmlAttribute item in extensibleAttributes)
			{
				if (item.NamespaceURI != "http://www.w3.org/2000/xmlns/")
				{
					WriteXmlAttribute(item, ob);
				}
			}
		}
		WriteAttribute("name", "", ob.Name);
		WriteAttribute("type", "", FromXmlQualifiedName(ob.Type));
		ServiceDescription.WriteExtensions(base.Writer, ob);
		if (ob.DocumentationElement != null)
		{
			XmlNode documentationElement = ob.DocumentationElement;
			if (!(documentationElement is XmlElement))
			{
				throw CreateUnknownAnyElementException(documentationElement.Name, documentationElement.NamespaceURI);
			}
			if (!(documentationElement.LocalName == "documentation") || !(documentationElement.NamespaceURI == "http://schemas.xmlsoap.org/wsdl/"))
			{
				documentationElement.WriteTo(base.Writer);
			}
			WriteElementLiteral(documentationElement, "", "", isNullable: false, any: true);
		}
		if (ob.Operations != null)
		{
			for (int i = 0; i < ob.Operations.Count; i++)
			{
				WriteObject_OperationBinding(ob.Operations[i], "operation", "http://schemas.xmlsoap.org/wsdl/", isNullable: false, needType: false, writeWrappingElem: true);
			}
		}
		if (writeWrappingElem)
		{
			WriteEndElement(ob);
		}
	}

	private void WriteObject_Service(Service ob, string element, string namesp, bool isNullable, bool needType, bool writeWrappingElem)
	{
		if (ob == null)
		{
			if (isNullable)
			{
				WriteNullTagLiteral(element, namesp);
			}
			return;
		}
		if (!(ob.GetType() == typeof(Service)))
		{
			throw CreateUnknownTypeException(ob);
		}
		if (writeWrappingElem)
		{
			WriteStartElement(element, namesp, ob);
		}
		if (needType)
		{
			WriteXsiType("Service", "http://schemas.xmlsoap.org/wsdl/");
		}
		WriteNamespaceDeclarations(ob.Namespaces);
		ICollection extensibleAttributes = ob.ExtensibleAttributes;
		if (extensibleAttributes != null)
		{
			foreach (XmlAttribute item in extensibleAttributes)
			{
				if (item.NamespaceURI != "http://www.w3.org/2000/xmlns/")
				{
					WriteXmlAttribute(item, ob);
				}
			}
		}
		WriteAttribute("name", "", ob.Name);
		ServiceDescription.WriteExtensions(base.Writer, ob);
		if (ob.DocumentationElement != null)
		{
			XmlNode documentationElement = ob.DocumentationElement;
			if (!(documentationElement is XmlElement))
			{
				throw CreateUnknownAnyElementException(documentationElement.Name, documentationElement.NamespaceURI);
			}
			if (!(documentationElement.LocalName == "documentation") || !(documentationElement.NamespaceURI == "http://schemas.xmlsoap.org/wsdl/"))
			{
				documentationElement.WriteTo(base.Writer);
			}
			WriteElementLiteral(documentationElement, "", "", isNullable: false, any: true);
		}
		if (ob.Ports != null)
		{
			for (int i = 0; i < ob.Ports.Count; i++)
			{
				WriteObject_Port(ob.Ports[i], "port", "http://schemas.xmlsoap.org/wsdl/", isNullable: false, needType: false, writeWrappingElem: true);
			}
		}
		if (writeWrappingElem)
		{
			WriteEndElement(ob);
		}
	}

	private void WriteObject_XmlSchema(XmlSchema ob, string element, string namesp, bool isNullable, bool needType, bool writeWrappingElem)
	{
		ob.Write(base.Writer);
	}

	private void WriteObject_MessagePart(MessagePart ob, string element, string namesp, bool isNullable, bool needType, bool writeWrappingElem)
	{
		if (ob == null)
		{
			if (isNullable)
			{
				WriteNullTagLiteral(element, namesp);
			}
			return;
		}
		if (!(ob.GetType() == typeof(MessagePart)))
		{
			throw CreateUnknownTypeException(ob);
		}
		if (writeWrappingElem)
		{
			WriteStartElement(element, namesp, ob);
		}
		if (needType)
		{
			WriteXsiType("MessagePart", "http://schemas.xmlsoap.org/wsdl/");
		}
		WriteNamespaceDeclarations(ob.Namespaces);
		ICollection extensibleAttributes = ob.ExtensibleAttributes;
		if (extensibleAttributes != null)
		{
			foreach (XmlAttribute item in extensibleAttributes)
			{
				if (item.NamespaceURI != "http://www.w3.org/2000/xmlns/")
				{
					WriteXmlAttribute(item, ob);
				}
			}
		}
		WriteAttribute("name", "", ob.Name);
		WriteAttribute("element", "", FromXmlQualifiedName(ob.Element));
		WriteAttribute("type", "", FromXmlQualifiedName(ob.Type));
		ServiceDescription.WriteExtensions(base.Writer, ob);
		if (ob.DocumentationElement != null)
		{
			XmlNode documentationElement = ob.DocumentationElement;
			if (!(documentationElement is XmlElement))
			{
				throw CreateUnknownAnyElementException(documentationElement.Name, documentationElement.NamespaceURI);
			}
			if (!(documentationElement.LocalName == "documentation") || !(documentationElement.NamespaceURI == "http://schemas.xmlsoap.org/wsdl/"))
			{
				documentationElement.WriteTo(base.Writer);
			}
			WriteElementLiteral(documentationElement, "", "", isNullable: false, any: true);
		}
		if (writeWrappingElem)
		{
			WriteEndElement(ob);
		}
	}

	private void WriteObject_Operation(Operation ob, string element, string namesp, bool isNullable, bool needType, bool writeWrappingElem)
	{
		if (ob == null)
		{
			if (isNullable)
			{
				WriteNullTagLiteral(element, namesp);
			}
			return;
		}
		if (!(ob.GetType() == typeof(Operation)))
		{
			throw CreateUnknownTypeException(ob);
		}
		if (writeWrappingElem)
		{
			WriteStartElement(element, namesp, ob);
		}
		if (needType)
		{
			WriteXsiType("Operation", "http://schemas.xmlsoap.org/wsdl/");
		}
		WriteNamespaceDeclarations(ob.Namespaces);
		ICollection extensibleAttributes = ob.ExtensibleAttributes;
		if (extensibleAttributes != null)
		{
			foreach (XmlAttribute item in extensibleAttributes)
			{
				if (item.NamespaceURI != "http://www.w3.org/2000/xmlns/")
				{
					WriteXmlAttribute(item, ob);
				}
			}
		}
		WriteAttribute("name", "", ob.Name);
		if (ob.ParameterOrderString != "")
		{
			WriteAttribute("parameterOrder", "", ob.ParameterOrderString);
		}
		ServiceDescription.WriteExtensions(base.Writer, ob);
		if (ob.DocumentationElement != null)
		{
			XmlNode documentationElement = ob.DocumentationElement;
			if (!(documentationElement is XmlElement))
			{
				throw CreateUnknownAnyElementException(documentationElement.Name, documentationElement.NamespaceURI);
			}
			if (!(documentationElement.LocalName == "documentation") || !(documentationElement.NamespaceURI == "http://schemas.xmlsoap.org/wsdl/"))
			{
				documentationElement.WriteTo(base.Writer);
			}
			WriteElementLiteral(documentationElement, "", "", isNullable: false, any: true);
		}
		if (ob.Faults != null)
		{
			for (int i = 0; i < ob.Faults.Count; i++)
			{
				WriteObject_OperationFault(ob.Faults[i], "fault", "http://schemas.xmlsoap.org/wsdl/", isNullable: false, needType: false, writeWrappingElem: true);
			}
		}
		if (ob.Messages != null)
		{
			for (int j = 0; j < ob.Messages.Count; j++)
			{
				if (ob.Messages[j] == null)
				{
					continue;
				}
				if (ob.Messages[j].GetType() == typeof(OperationOutput))
				{
					WriteObject_OperationOutput((OperationOutput)ob.Messages[j], "output", "http://schemas.xmlsoap.org/wsdl/", isNullable: false, needType: false, writeWrappingElem: true);
					continue;
				}
				if (ob.Messages[j].GetType() == typeof(OperationInput))
				{
					WriteObject_OperationInput((OperationInput)ob.Messages[j], "input", "http://schemas.xmlsoap.org/wsdl/", isNullable: false, needType: false, writeWrappingElem: true);
					continue;
				}
				throw CreateUnknownTypeException(ob.Messages[j]);
			}
		}
		if (writeWrappingElem)
		{
			WriteEndElement(ob);
		}
	}

	private void WriteObject_OperationBinding(OperationBinding ob, string element, string namesp, bool isNullable, bool needType, bool writeWrappingElem)
	{
		if (ob == null)
		{
			if (isNullable)
			{
				WriteNullTagLiteral(element, namesp);
			}
			return;
		}
		if (!(ob.GetType() == typeof(OperationBinding)))
		{
			throw CreateUnknownTypeException(ob);
		}
		if (writeWrappingElem)
		{
			WriteStartElement(element, namesp, ob);
		}
		if (needType)
		{
			WriteXsiType("OperationBinding", "http://schemas.xmlsoap.org/wsdl/");
		}
		WriteNamespaceDeclarations(ob.Namespaces);
		ICollection extensibleAttributes = ob.ExtensibleAttributes;
		if (extensibleAttributes != null)
		{
			foreach (XmlAttribute item in extensibleAttributes)
			{
				if (item.NamespaceURI != "http://www.w3.org/2000/xmlns/")
				{
					WriteXmlAttribute(item, ob);
				}
			}
		}
		WriteAttribute("name", "", ob.Name);
		ServiceDescription.WriteExtensions(base.Writer, ob);
		if (ob.DocumentationElement != null)
		{
			XmlNode documentationElement = ob.DocumentationElement;
			if (!(documentationElement is XmlElement))
			{
				throw CreateUnknownAnyElementException(documentationElement.Name, documentationElement.NamespaceURI);
			}
			if (!(documentationElement.LocalName == "documentation") || !(documentationElement.NamespaceURI == "http://schemas.xmlsoap.org/wsdl/"))
			{
				documentationElement.WriteTo(base.Writer);
			}
			WriteElementLiteral(documentationElement, "", "", isNullable: false, any: true);
		}
		if (ob.Faults != null)
		{
			for (int i = 0; i < ob.Faults.Count; i++)
			{
				WriteObject_FaultBinding(ob.Faults[i], "fault", "http://schemas.xmlsoap.org/wsdl/", isNullable: false, needType: false, writeWrappingElem: true);
			}
		}
		WriteObject_InputBinding(ob.Input, "input", "http://schemas.xmlsoap.org/wsdl/", isNullable: false, needType: false, writeWrappingElem: true);
		WriteObject_OutputBinding(ob.Output, "output", "http://schemas.xmlsoap.org/wsdl/", isNullable: false, needType: false, writeWrappingElem: true);
		if (writeWrappingElem)
		{
			WriteEndElement(ob);
		}
	}

	private void WriteObject_Port(Port ob, string element, string namesp, bool isNullable, bool needType, bool writeWrappingElem)
	{
		if (ob == null)
		{
			if (isNullable)
			{
				WriteNullTagLiteral(element, namesp);
			}
			return;
		}
		if (!(ob.GetType() == typeof(Port)))
		{
			throw CreateUnknownTypeException(ob);
		}
		if (writeWrappingElem)
		{
			WriteStartElement(element, namesp, ob);
		}
		if (needType)
		{
			WriteXsiType("Port", "http://schemas.xmlsoap.org/wsdl/");
		}
		WriteNamespaceDeclarations(ob.Namespaces);
		ICollection extensibleAttributes = ob.ExtensibleAttributes;
		if (extensibleAttributes != null)
		{
			foreach (XmlAttribute item in extensibleAttributes)
			{
				if (item.NamespaceURI != "http://www.w3.org/2000/xmlns/")
				{
					WriteXmlAttribute(item, ob);
				}
			}
		}
		WriteAttribute("name", "", ob.Name);
		WriteAttribute("binding", "", FromXmlQualifiedName(ob.Binding));
		ServiceDescription.WriteExtensions(base.Writer, ob);
		if (ob.DocumentationElement != null)
		{
			XmlNode documentationElement = ob.DocumentationElement;
			if (!(documentationElement is XmlElement))
			{
				throw CreateUnknownAnyElementException(documentationElement.Name, documentationElement.NamespaceURI);
			}
			if (!(documentationElement.LocalName == "documentation") || !(documentationElement.NamespaceURI == "http://schemas.xmlsoap.org/wsdl/"))
			{
				documentationElement.WriteTo(base.Writer);
			}
			WriteElementLiteral(documentationElement, "", "", isNullable: false, any: true);
		}
		if (writeWrappingElem)
		{
			WriteEndElement(ob);
		}
	}

	private void WriteObject_OperationFault(OperationFault ob, string element, string namesp, bool isNullable, bool needType, bool writeWrappingElem)
	{
		if (ob == null)
		{
			if (isNullable)
			{
				WriteNullTagLiteral(element, namesp);
			}
			return;
		}
		if (!(ob.GetType() == typeof(OperationFault)))
		{
			throw CreateUnknownTypeException(ob);
		}
		if (writeWrappingElem)
		{
			WriteStartElement(element, namesp, ob);
		}
		if (needType)
		{
			WriteXsiType("OperationFault", "http://schemas.xmlsoap.org/wsdl/");
		}
		WriteNamespaceDeclarations(ob.Namespaces);
		ICollection extensibleAttributes = ob.ExtensibleAttributes;
		if (extensibleAttributes != null)
		{
			foreach (XmlAttribute item in extensibleAttributes)
			{
				if (item.NamespaceURI != "http://www.w3.org/2000/xmlns/")
				{
					WriteXmlAttribute(item, ob);
				}
			}
		}
		WriteAttribute("name", "", ob.Name);
		WriteAttribute("message", "", FromXmlQualifiedName(ob.Message));
		ServiceDescription.WriteExtensions(base.Writer, ob);
		if (ob.DocumentationElement != null)
		{
			XmlNode documentationElement = ob.DocumentationElement;
			if (!(documentationElement is XmlElement))
			{
				throw CreateUnknownAnyElementException(documentationElement.Name, documentationElement.NamespaceURI);
			}
			if (!(documentationElement.LocalName == "documentation") || !(documentationElement.NamespaceURI == "http://schemas.xmlsoap.org/wsdl/"))
			{
				documentationElement.WriteTo(base.Writer);
			}
			WriteElementLiteral(documentationElement, "", "", isNullable: false, any: true);
		}
		if (writeWrappingElem)
		{
			WriteEndElement(ob);
		}
	}

	private void WriteObject_OperationOutput(OperationOutput ob, string element, string namesp, bool isNullable, bool needType, bool writeWrappingElem)
	{
		if (ob == null)
		{
			if (isNullable)
			{
				WriteNullTagLiteral(element, namesp);
			}
			return;
		}
		if (!(ob.GetType() == typeof(OperationOutput)))
		{
			throw CreateUnknownTypeException(ob);
		}
		if (writeWrappingElem)
		{
			WriteStartElement(element, namesp, ob);
		}
		if (needType)
		{
			WriteXsiType("OperationOutput", "http://schemas.xmlsoap.org/wsdl/");
		}
		WriteNamespaceDeclarations(ob.Namespaces);
		ICollection extensibleAttributes = ob.ExtensibleAttributes;
		if (extensibleAttributes != null)
		{
			foreach (XmlAttribute item in extensibleAttributes)
			{
				if (item.NamespaceURI != "http://www.w3.org/2000/xmlns/")
				{
					WriteXmlAttribute(item, ob);
				}
			}
		}
		WriteAttribute("name", "", ob.Name);
		WriteAttribute("message", "", FromXmlQualifiedName(ob.Message));
		ServiceDescription.WriteExtensions(base.Writer, ob);
		if (ob.DocumentationElement != null)
		{
			XmlNode documentationElement = ob.DocumentationElement;
			if (!(documentationElement is XmlElement))
			{
				throw CreateUnknownAnyElementException(documentationElement.Name, documentationElement.NamespaceURI);
			}
			if (!(documentationElement.LocalName == "documentation") || !(documentationElement.NamespaceURI == "http://schemas.xmlsoap.org/wsdl/"))
			{
				documentationElement.WriteTo(base.Writer);
			}
			WriteElementLiteral(documentationElement, "", "", isNullable: false, any: true);
		}
		if (writeWrappingElem)
		{
			WriteEndElement(ob);
		}
	}

	private void WriteObject_OperationInput(OperationInput ob, string element, string namesp, bool isNullable, bool needType, bool writeWrappingElem)
	{
		if (ob == null)
		{
			if (isNullable)
			{
				WriteNullTagLiteral(element, namesp);
			}
			return;
		}
		if (!(ob.GetType() == typeof(OperationInput)))
		{
			throw CreateUnknownTypeException(ob);
		}
		if (writeWrappingElem)
		{
			WriteStartElement(element, namesp, ob);
		}
		if (needType)
		{
			WriteXsiType("OperationInput", "http://schemas.xmlsoap.org/wsdl/");
		}
		WriteNamespaceDeclarations(ob.Namespaces);
		ICollection extensibleAttributes = ob.ExtensibleAttributes;
		if (extensibleAttributes != null)
		{
			foreach (XmlAttribute item in extensibleAttributes)
			{
				if (item.NamespaceURI != "http://www.w3.org/2000/xmlns/")
				{
					WriteXmlAttribute(item, ob);
				}
			}
		}
		WriteAttribute("name", "", ob.Name);
		WriteAttribute("message", "", FromXmlQualifiedName(ob.Message));
		ServiceDescription.WriteExtensions(base.Writer, ob);
		if (ob.DocumentationElement != null)
		{
			XmlNode documentationElement = ob.DocumentationElement;
			if (!(documentationElement is XmlElement))
			{
				throw CreateUnknownAnyElementException(documentationElement.Name, documentationElement.NamespaceURI);
			}
			if (!(documentationElement.LocalName == "documentation") || !(documentationElement.NamespaceURI == "http://schemas.xmlsoap.org/wsdl/"))
			{
				documentationElement.WriteTo(base.Writer);
			}
			WriteElementLiteral(documentationElement, "", "", isNullable: false, any: true);
		}
		if (writeWrappingElem)
		{
			WriteEndElement(ob);
		}
	}

	private void WriteObject_FaultBinding(FaultBinding ob, string element, string namesp, bool isNullable, bool needType, bool writeWrappingElem)
	{
		if (ob == null)
		{
			if (isNullable)
			{
				WriteNullTagLiteral(element, namesp);
			}
			return;
		}
		if (!(ob.GetType() == typeof(FaultBinding)))
		{
			throw CreateUnknownTypeException(ob);
		}
		if (writeWrappingElem)
		{
			WriteStartElement(element, namesp, ob);
		}
		if (needType)
		{
			WriteXsiType("FaultBinding", "http://schemas.xmlsoap.org/wsdl/");
		}
		WriteNamespaceDeclarations(ob.Namespaces);
		ICollection extensibleAttributes = ob.ExtensibleAttributes;
		if (extensibleAttributes != null)
		{
			foreach (XmlAttribute item in extensibleAttributes)
			{
				if (item.NamespaceURI != "http://www.w3.org/2000/xmlns/")
				{
					WriteXmlAttribute(item, ob);
				}
			}
		}
		WriteAttribute("name", "", ob.Name);
		ServiceDescription.WriteExtensions(base.Writer, ob);
		if (ob.DocumentationElement != null)
		{
			XmlNode documentationElement = ob.DocumentationElement;
			if (!(documentationElement is XmlElement))
			{
				throw CreateUnknownAnyElementException(documentationElement.Name, documentationElement.NamespaceURI);
			}
			if (!(documentationElement.LocalName == "documentation") || !(documentationElement.NamespaceURI == "http://schemas.xmlsoap.org/wsdl/"))
			{
				documentationElement.WriteTo(base.Writer);
			}
			WriteElementLiteral(documentationElement, "", "", isNullable: false, any: true);
		}
		if (writeWrappingElem)
		{
			WriteEndElement(ob);
		}
	}

	private void WriteObject_InputBinding(InputBinding ob, string element, string namesp, bool isNullable, bool needType, bool writeWrappingElem)
	{
		if (ob == null)
		{
			if (isNullable)
			{
				WriteNullTagLiteral(element, namesp);
			}
			return;
		}
		if (!(ob.GetType() == typeof(InputBinding)))
		{
			throw CreateUnknownTypeException(ob);
		}
		if (writeWrappingElem)
		{
			WriteStartElement(element, namesp, ob);
		}
		if (needType)
		{
			WriteXsiType("InputBinding", "http://schemas.xmlsoap.org/wsdl/");
		}
		WriteNamespaceDeclarations(ob.Namespaces);
		ICollection extensibleAttributes = ob.ExtensibleAttributes;
		if (extensibleAttributes != null)
		{
			foreach (XmlAttribute item in extensibleAttributes)
			{
				if (item.NamespaceURI != "http://www.w3.org/2000/xmlns/")
				{
					WriteXmlAttribute(item, ob);
				}
			}
		}
		WriteAttribute("name", "", ob.Name);
		ServiceDescription.WriteExtensions(base.Writer, ob);
		if (ob.DocumentationElement != null)
		{
			XmlNode documentationElement = ob.DocumentationElement;
			if (!(documentationElement is XmlElement))
			{
				throw CreateUnknownAnyElementException(documentationElement.Name, documentationElement.NamespaceURI);
			}
			if (!(documentationElement.LocalName == "documentation") || !(documentationElement.NamespaceURI == "http://schemas.xmlsoap.org/wsdl/"))
			{
				documentationElement.WriteTo(base.Writer);
			}
			WriteElementLiteral(documentationElement, "", "", isNullable: false, any: true);
		}
		if (writeWrappingElem)
		{
			WriteEndElement(ob);
		}
	}

	private void WriteObject_OutputBinding(OutputBinding ob, string element, string namesp, bool isNullable, bool needType, bool writeWrappingElem)
	{
		if (ob == null)
		{
			if (isNullable)
			{
				WriteNullTagLiteral(element, namesp);
			}
			return;
		}
		if (!(ob.GetType() == typeof(OutputBinding)))
		{
			throw CreateUnknownTypeException(ob);
		}
		if (writeWrappingElem)
		{
			WriteStartElement(element, namesp, ob);
		}
		if (needType)
		{
			WriteXsiType("OutputBinding", "http://schemas.xmlsoap.org/wsdl/");
		}
		WriteNamespaceDeclarations(ob.Namespaces);
		ICollection extensibleAttributes = ob.ExtensibleAttributes;
		if (extensibleAttributes != null)
		{
			foreach (XmlAttribute item in extensibleAttributes)
			{
				if (item.NamespaceURI != "http://www.w3.org/2000/xmlns/")
				{
					WriteXmlAttribute(item, ob);
				}
			}
		}
		WriteAttribute("name", "", ob.Name);
		ServiceDescription.WriteExtensions(base.Writer, ob);
		if (ob.DocumentationElement != null)
		{
			XmlNode documentationElement = ob.DocumentationElement;
			if (!(documentationElement is XmlElement))
			{
				throw CreateUnknownAnyElementException(documentationElement.Name, documentationElement.NamespaceURI);
			}
			if (!(documentationElement.LocalName == "documentation") || !(documentationElement.NamespaceURI == "http://schemas.xmlsoap.org/wsdl/"))
			{
				documentationElement.WriteTo(base.Writer);
			}
			WriteElementLiteral(documentationElement, "", "", isNullable: false, any: true);
		}
		if (writeWrappingElem)
		{
			WriteEndElement(ob);
		}
	}

	protected override void InitCallbacks()
	{
	}
}
