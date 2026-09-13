using System.Xml.Serialization;

namespace System.Web.Services.Discovery;

internal class DiscoveryDocumentWriter : XmlSerializationWriter
{
	public void WriteRoot_DiscoveryDocument(object o)
	{
		WriteStartDocument();
		DiscoveryDocument ob = (DiscoveryDocument)o;
		TopLevelElement();
		WriteObject_DiscoveryDocument(ob, "discovery", "http://schemas.xmlsoap.org/disco/", isNullable: true, needType: false, writeWrappingElem: true);
	}

	private void WriteObject_DiscoveryDocument(DiscoveryDocument ob, string element, string namesp, bool isNullable, bool needType, bool writeWrappingElem)
	{
		if (ob == null)
		{
			if (isNullable)
			{
				WriteNullTagLiteral(element, namesp);
			}
			return;
		}
		if (writeWrappingElem)
		{
			WriteStartElement(element, namesp, ob);
		}
		if (needType)
		{
			WriteXsiType("DiscoveryDocument", "http://schemas.xmlsoap.org/disco/");
		}
		if (ob.references != null)
		{
			for (int i = 0; i < ob.references.Count; i++)
			{
				if (ob.references[i] == null)
				{
					continue;
				}
				if (ob.references[i].GetType() == typeof(SchemaReference))
				{
					WriteObject_SchemaReference((SchemaReference)ob.references[i], "schemaRef", "http://schemas.xmlsoap.org/disco/", isNullable: false, needType: false, writeWrappingElem: true);
					continue;
				}
				if (ob.references[i].GetType() == typeof(DiscoveryDocumentReference))
				{
					WriteObject_DiscoveryDocumentReference((DiscoveryDocumentReference)ob.references[i], "discoveryRef", "http://schemas.xmlsoap.org/disco/", isNullable: false, needType: false, writeWrappingElem: true);
					continue;
				}
				if (ob.references[i].GetType() == typeof(ContractReference))
				{
					WriteObject_ContractReference((ContractReference)ob.references[i], "contractRef", "http://schemas.xmlsoap.org/disco/scl/", isNullable: false, needType: false, writeWrappingElem: true);
					continue;
				}
				throw CreateUnknownTypeException(ob.references[i]);
			}
		}
		if (ob.additionalInfo != null)
		{
			for (int j = 0; j < ob.additionalInfo.Count; j++)
			{
				WriteObject_SoapBinding((SoapBinding)ob.additionalInfo[j], "soap", "http://schemas/xmlsoap.org/disco/schema/soap/", isNullable: false, needType: false, writeWrappingElem: true);
			}
		}
		if (writeWrappingElem)
		{
			WriteEndElement(ob);
		}
	}

	private void WriteObject_SchemaReference(SchemaReference ob, string element, string namesp, bool isNullable, bool needType, bool writeWrappingElem)
	{
		if (ob == null)
		{
			if (isNullable)
			{
				WriteNullTagLiteral(element, namesp);
			}
			return;
		}
		if (writeWrappingElem)
		{
			WriteStartElement(element, namesp, ob);
		}
		if (needType)
		{
			WriteXsiType("SchemaReference", "http://schemas/xmlsoap.org/disco/schema/");
		}
		if (ob.TargetNamespace != "")
		{
			WriteAttribute("targetNamespace", "", ob.TargetNamespace);
		}
		WriteAttribute("ref", "", ob.Ref);
		if (writeWrappingElem)
		{
			WriteEndElement(ob);
		}
	}

	private void WriteObject_DiscoveryDocumentReference(DiscoveryDocumentReference ob, string element, string namesp, bool isNullable, bool needType, bool writeWrappingElem)
	{
		if (ob == null)
		{
			if (isNullable)
			{
				WriteNullTagLiteral(element, namesp);
			}
			return;
		}
		if (writeWrappingElem)
		{
			WriteStartElement(element, namesp, ob);
		}
		if (needType)
		{
			WriteXsiType("DiscoveryDocumentReference", "http://schemas.xmlsoap.org/disco/");
		}
		WriteAttribute("ref", "", ob.Ref);
		if (writeWrappingElem)
		{
			WriteEndElement(ob);
		}
	}

	private void WriteObject_ContractReference(ContractReference ob, string element, string namesp, bool isNullable, bool needType, bool writeWrappingElem)
	{
		if (ob == null)
		{
			if (isNullable)
			{
				WriteNullTagLiteral(element, namesp);
			}
			return;
		}
		if (writeWrappingElem)
		{
			WriteStartElement(element, namesp, ob);
		}
		if (needType)
		{
			WriteXsiType("ContractReference", "http://schemas.xmlsoap.org/disco/scl/");
		}
		WriteAttribute("docRef", "", ob.DocRef);
		WriteAttribute("ref", "", ob.Ref);
		if (writeWrappingElem)
		{
			WriteEndElement(ob);
		}
	}

	private void WriteObject_SoapBinding(SoapBinding ob, string element, string namesp, bool isNullable, bool needType, bool writeWrappingElem)
	{
		if (ob == null)
		{
			if (isNullable)
			{
				WriteNullTagLiteral(element, namesp);
			}
			return;
		}
		if (writeWrappingElem)
		{
			WriteStartElement(element, namesp, ob);
		}
		if (needType)
		{
			WriteXsiType("SoapBinding", "http://schemas/xmlsoap.org/disco/schema/soap/");
		}
		WriteAttribute("binding", "", FromXmlQualifiedName(ob.Binding));
		WriteAttribute("address", "", ob.Address);
		if (writeWrappingElem)
		{
			WriteEndElement(ob);
		}
	}

	protected override void InitCallbacks()
	{
	}
}
