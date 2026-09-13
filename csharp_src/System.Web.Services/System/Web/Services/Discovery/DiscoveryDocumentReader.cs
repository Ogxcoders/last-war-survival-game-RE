using System.Collections;
using System.Xml;
using System.Xml.Serialization;

namespace System.Web.Services.Discovery;

internal class DiscoveryDocumentReader : XmlSerializationReader
{
	public object ReadRoot_DiscoveryDocument()
	{
		base.Reader.MoveToContent();
		if (base.Reader.LocalName != "discovery" || base.Reader.NamespaceURI != "http://schemas.xmlsoap.org/disco/")
		{
			throw CreateUnknownNodeException();
		}
		return ReadObject_DiscoveryDocument(isNullable: true, checkType: true);
	}

	public DiscoveryDocument ReadObject_DiscoveryDocument(bool isNullable, bool checkType)
	{
		DiscoveryDocument discoveryDocument = null;
		if (isNullable && ReadNull())
		{
			return null;
		}
		if (checkType)
		{
			XmlQualifiedName xsiType = GetXsiType();
			if (xsiType != null && (xsiType.Name != "DiscoveryDocument" || xsiType.Namespace != "http://schemas.xmlsoap.org/disco/"))
			{
				throw CreateUnknownTypeException(xsiType);
			}
		}
		discoveryDocument = new DiscoveryDocument();
		base.Reader.MoveToElement();
		while (base.Reader.MoveToNextAttribute())
		{
			if (!IsXmlnsAttribute(base.Reader.Name))
			{
				UnknownNode(discoveryDocument);
			}
		}
		base.Reader.MoveToElement();
		if (base.Reader.IsEmptyElement)
		{
			base.Reader.Skip();
			return discoveryDocument;
		}
		base.Reader.ReadStartElement();
		base.Reader.MoveToContent();
		bool flag = false;
		bool flag2 = false;
		ArrayList arrayList = null;
		ArrayList arrayList2 = null;
		int num = 0;
		int num2 = 0;
		while (base.Reader.NodeType != XmlNodeType.EndElement)
		{
			if (base.Reader.NodeType == XmlNodeType.Element)
			{
				if (base.Reader.LocalName == "discoveryRef" && base.Reader.NamespaceURI == "http://schemas.xmlsoap.org/disco/" && !flag)
				{
					if (arrayList == null)
					{
						arrayList = new ArrayList();
					}
					arrayList.Add(ReadObject_DiscoveryDocumentReference(isNullable: false, checkType: true));
					num++;
				}
				else if (base.Reader.LocalName == "soap" && base.Reader.NamespaceURI == "http://schemas/xmlsoap.org/disco/schema/soap/" && !flag2)
				{
					if (arrayList2 == null)
					{
						arrayList2 = new ArrayList();
					}
					arrayList2.Add(ReadObject_SoapBinding(isNullable: false, checkType: true));
					num2++;
				}
				else if (base.Reader.LocalName == "contractRef" && base.Reader.NamespaceURI == "http://schemas.xmlsoap.org/disco/scl/" && !flag)
				{
					if (arrayList == null)
					{
						arrayList = new ArrayList();
					}
					arrayList.Add(ReadObject_ContractReference(isNullable: false, checkType: true));
					num++;
				}
				else if (base.Reader.LocalName == "schemaRef" && base.Reader.NamespaceURI == "http://schemas.xmlsoap.org/disco/" && !flag)
				{
					if (arrayList == null)
					{
						arrayList = new ArrayList();
					}
					arrayList.Add(ReadObject_SchemaReference(isNullable: false, checkType: true));
					num++;
				}
				else
				{
					UnknownNode(discoveryDocument);
				}
			}
			else
			{
				UnknownNode(discoveryDocument);
			}
			base.Reader.MoveToContent();
		}
		discoveryDocument.references = arrayList;
		discoveryDocument.additionalInfo = arrayList2;
		ReadEndElement();
		return discoveryDocument;
	}

	public DiscoveryDocumentReference ReadObject_DiscoveryDocumentReference(bool isNullable, bool checkType)
	{
		DiscoveryDocumentReference discoveryDocumentReference = null;
		if (isNullable && ReadNull())
		{
			return null;
		}
		if (checkType)
		{
			XmlQualifiedName xsiType = GetXsiType();
			if (xsiType != null && (xsiType.Name != "DiscoveryDocumentReference" || xsiType.Namespace != "http://schemas.xmlsoap.org/disco/"))
			{
				throw CreateUnknownTypeException(xsiType);
			}
		}
		discoveryDocumentReference = new DiscoveryDocumentReference();
		base.Reader.MoveToElement();
		while (base.Reader.MoveToNextAttribute())
		{
			if (base.Reader.LocalName == "ref" && base.Reader.NamespaceURI == "")
			{
				discoveryDocumentReference.Ref = base.Reader.Value;
			}
			else if (!IsXmlnsAttribute(base.Reader.Name))
			{
				UnknownNode(discoveryDocumentReference);
			}
		}
		base.Reader.MoveToElement();
		if (base.Reader.IsEmptyElement)
		{
			base.Reader.Skip();
			return discoveryDocumentReference;
		}
		base.Reader.ReadStartElement();
		base.Reader.MoveToContent();
		while (base.Reader.NodeType != XmlNodeType.EndElement)
		{
			if (base.Reader.NodeType == XmlNodeType.Element)
			{
				UnknownNode(discoveryDocumentReference);
			}
			else
			{
				UnknownNode(discoveryDocumentReference);
			}
			base.Reader.MoveToContent();
		}
		ReadEndElement();
		return discoveryDocumentReference;
	}

	public SoapBinding ReadObject_SoapBinding(bool isNullable, bool checkType)
	{
		SoapBinding soapBinding = null;
		if (isNullable && ReadNull())
		{
			return null;
		}
		if (checkType)
		{
			XmlQualifiedName xsiType = GetXsiType();
			if (xsiType != null && (xsiType.Name != "SoapBinding" || xsiType.Namespace != "http://schemas/xmlsoap.org/disco/schema/soap/"))
			{
				throw CreateUnknownTypeException(xsiType);
			}
		}
		soapBinding = new SoapBinding();
		base.Reader.MoveToElement();
		while (base.Reader.MoveToNextAttribute())
		{
			if (base.Reader.LocalName == "binding" && base.Reader.NamespaceURI == "")
			{
				soapBinding.Binding = ToXmlQualifiedName(base.Reader.Value);
			}
			else if (base.Reader.LocalName == "address" && base.Reader.NamespaceURI == "")
			{
				soapBinding.Address = base.Reader.Value;
			}
			else if (!IsXmlnsAttribute(base.Reader.Name))
			{
				UnknownNode(soapBinding);
			}
		}
		base.Reader.MoveToElement();
		if (base.Reader.IsEmptyElement)
		{
			base.Reader.Skip();
			return soapBinding;
		}
		base.Reader.ReadStartElement();
		base.Reader.MoveToContent();
		while (base.Reader.NodeType != XmlNodeType.EndElement)
		{
			if (base.Reader.NodeType == XmlNodeType.Element)
			{
				UnknownNode(soapBinding);
			}
			else
			{
				UnknownNode(soapBinding);
			}
			base.Reader.MoveToContent();
		}
		ReadEndElement();
		return soapBinding;
	}

	public ContractReference ReadObject_ContractReference(bool isNullable, bool checkType)
	{
		ContractReference contractReference = null;
		if (isNullable && ReadNull())
		{
			return null;
		}
		if (checkType)
		{
			XmlQualifiedName xsiType = GetXsiType();
			if (xsiType != null && (xsiType.Name != "ContractReference" || xsiType.Namespace != "http://schemas.xmlsoap.org/disco/scl/"))
			{
				throw CreateUnknownTypeException(xsiType);
			}
		}
		contractReference = new ContractReference();
		base.Reader.MoveToElement();
		while (base.Reader.MoveToNextAttribute())
		{
			if (base.Reader.LocalName == "docRef" && base.Reader.NamespaceURI == "")
			{
				contractReference.DocRef = base.Reader.Value;
			}
			else if (base.Reader.LocalName == "ref" && base.Reader.NamespaceURI == "")
			{
				contractReference.Ref = base.Reader.Value;
			}
			else if (!IsXmlnsAttribute(base.Reader.Name))
			{
				UnknownNode(contractReference);
			}
		}
		base.Reader.MoveToElement();
		if (base.Reader.IsEmptyElement)
		{
			base.Reader.Skip();
			return contractReference;
		}
		base.Reader.ReadStartElement();
		base.Reader.MoveToContent();
		while (base.Reader.NodeType != XmlNodeType.EndElement)
		{
			if (base.Reader.NodeType == XmlNodeType.Element)
			{
				UnknownNode(contractReference);
			}
			else
			{
				UnknownNode(contractReference);
			}
			base.Reader.MoveToContent();
		}
		ReadEndElement();
		return contractReference;
	}

	public SchemaReference ReadObject_SchemaReference(bool isNullable, bool checkType)
	{
		SchemaReference schemaReference = null;
		if (isNullable && ReadNull())
		{
			return null;
		}
		if (checkType)
		{
			XmlQualifiedName xsiType = GetXsiType();
			if (xsiType != null && (xsiType.Name != "SchemaReference" || xsiType.Namespace != "http://schemas/xmlsoap.org/disco/schema/"))
			{
				throw CreateUnknownTypeException(xsiType);
			}
		}
		schemaReference = new SchemaReference();
		base.Reader.MoveToElement();
		while (base.Reader.MoveToNextAttribute())
		{
			if (base.Reader.LocalName == "targetNamespace" && base.Reader.NamespaceURI == "")
			{
				schemaReference.TargetNamespace = base.Reader.Value;
			}
			else if (base.Reader.LocalName == "ref" && base.Reader.NamespaceURI == "")
			{
				schemaReference.Ref = base.Reader.Value;
			}
			else if (!IsXmlnsAttribute(base.Reader.Name))
			{
				UnknownNode(schemaReference);
			}
		}
		base.Reader.MoveToElement();
		if (base.Reader.IsEmptyElement)
		{
			base.Reader.Skip();
			return schemaReference;
		}
		base.Reader.ReadStartElement();
		base.Reader.MoveToContent();
		while (base.Reader.NodeType != XmlNodeType.EndElement)
		{
			if (base.Reader.NodeType == XmlNodeType.Element)
			{
				UnknownNode(schemaReference);
			}
			else
			{
				UnknownNode(schemaReference);
			}
			base.Reader.MoveToContent();
		}
		ReadEndElement();
		return schemaReference;
	}

	protected override void InitCallbacks()
	{
	}

	protected override void InitIDs()
	{
	}
}
