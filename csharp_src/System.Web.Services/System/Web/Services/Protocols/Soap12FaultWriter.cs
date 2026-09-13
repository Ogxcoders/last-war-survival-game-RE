using System.Collections;
using System.Xml;
using System.Xml.Serialization;

namespace System.Web.Services.Protocols;

internal class Soap12FaultWriter : XmlSerializationWriter
{
	private const string xmlNamespace = "http://www.w3.org/2000/xmlns/";

	public void WriteRoot_Soap12Fault(object o)
	{
		WriteStartDocument();
		Soap12Fault ob = (Soap12Fault)o;
		TopLevelElement();
		WriteObject_Fault(ob, "Fault", "http://www.w3.org/2003/05/soap-envelope", isNullable: true, needType: false, writeWrappingElem: true);
	}

	private void WriteObject_Fault(Soap12Fault ob, string element, string namesp, bool isNullable, bool needType, bool writeWrappingElem)
	{
		if (ob == null)
		{
			if (isNullable)
			{
				WriteNullTagLiteral(element, namesp);
			}
			return;
		}
		if (!(ob.GetType() == typeof(Soap12Fault)))
		{
			throw CreateUnknownTypeException(ob);
		}
		if (writeWrappingElem)
		{
			WriteStartElement(element, namesp, ob);
		}
		if (needType)
		{
			WriteXsiType("Fault", "http://www.w3.org/2003/05/soap-envelope");
		}
		WriteObject_Code(ob.Code, "Code", "http://www.w3.org/2003/05/soap-envelope", isNullable: false, needType: false, writeWrappingElem: true);
		WriteObject_Reason(ob.Reason, "Reason", "http://www.w3.org/2003/05/soap-envelope", isNullable: false, needType: false, writeWrappingElem: true);
		WriteElementString("Node", "http://www.w3.org/2003/05/soap-envelope", (ob.Node != null) ? ob.Node.ToString() : null);
		WriteElementString("Role", "http://www.w3.org/2003/05/soap-envelope", (ob.Role != null) ? ob.Role.ToString() : null);
		WriteObject_Detail(ob.Detail, "Detail", "http://www.w3.org/2003/05/soap-envelope", isNullable: false, needType: false, writeWrappingElem: true);
		if (writeWrappingElem)
		{
			WriteEndElement(ob);
		}
	}

	private void WriteObject_Code(Soap12FaultCode ob, string element, string namesp, bool isNullable, bool needType, bool writeWrappingElem)
	{
		if (ob == null)
		{
			if (isNullable)
			{
				WriteNullTagLiteral(element, namesp);
			}
			return;
		}
		if (!(ob.GetType() == typeof(Soap12FaultCode)))
		{
			throw CreateUnknownTypeException(ob);
		}
		if (writeWrappingElem)
		{
			WriteStartElement(element, namesp, ob);
		}
		if (needType)
		{
			WriteXsiType("Code", "http://www.w3.org/2003/05/soap-envelope");
		}
		WriteElementQualifiedName("Value", "http://www.w3.org/2003/05/soap-envelope", ob.Value);
		WriteObject_Code(ob.Subcode, "Subcode", "http://www.w3.org/2003/05/soap-envelope", isNullable: false, needType: false, writeWrappingElem: true);
		if (writeWrappingElem)
		{
			WriteEndElement(ob);
		}
	}

	private void WriteObject_Reason(Soap12FaultReason ob, string element, string namesp, bool isNullable, bool needType, bool writeWrappingElem)
	{
		if (ob == null)
		{
			if (isNullable)
			{
				WriteNullTagLiteral(element, namesp);
			}
			return;
		}
		if (!(ob.GetType() == typeof(Soap12FaultReason)))
		{
			throw CreateUnknownTypeException(ob);
		}
		if (writeWrappingElem)
		{
			WriteStartElement(element, namesp, ob);
		}
		if (needType)
		{
			WriteXsiType("Reason", "http://www.w3.org/2003/05/soap-envelope");
		}
		if (ob.Texts != null)
		{
			for (int i = 0; i < ob.Texts.Length; i++)
			{
				WriteObject_Text(ob.Texts[i], "Text", "http://www.w3.org/2003/05/soap-envelope", isNullable: false, needType: false, writeWrappingElem: true);
			}
		}
		if (writeWrappingElem)
		{
			WriteEndElement(ob);
		}
	}

	private void WriteObject_Detail(Soap12FaultDetail ob, string element, string namesp, bool isNullable, bool needType, bool writeWrappingElem)
	{
		if (ob == null)
		{
			if (isNullable)
			{
				WriteNullTagLiteral(element, namesp);
			}
			return;
		}
		if (!(ob.GetType() == typeof(Soap12FaultDetail)))
		{
			throw CreateUnknownTypeException(ob);
		}
		if (writeWrappingElem)
		{
			WriteStartElement(element, namesp, ob);
		}
		if (needType)
		{
			WriteXsiType("Detail", "http://www.w3.org/2003/05/soap-envelope");
		}
		ICollection attributes = ob.Attributes;
		if (attributes != null)
		{
			foreach (XmlAttribute item in attributes)
			{
				if (item.NamespaceURI != "http://www.w3.org/2000/xmlns/")
				{
					WriteXmlAttribute(item, ob);
				}
			}
		}
		if (ob.Children != null)
		{
			XmlElement[] children = ob.Children;
			foreach (XmlNode xmlNode in children)
			{
				if (!(xmlNode is XmlElement))
				{
					xmlNode.WriteTo(base.Writer);
				}
				WriteElementLiteral(xmlNode, "", "", isNullable: false, any: true);
			}
		}
		WriteValue(ob.Text);
		if (writeWrappingElem)
		{
			WriteEndElement(ob);
		}
	}

	private void WriteObject_Text(Soap12FaultReasonText ob, string element, string namesp, bool isNullable, bool needType, bool writeWrappingElem)
	{
		if (ob == null)
		{
			if (isNullable)
			{
				WriteNullTagLiteral(element, namesp);
			}
			return;
		}
		if (!(ob.GetType() == typeof(Soap12FaultReasonText)))
		{
			throw CreateUnknownTypeException(ob);
		}
		if (writeWrappingElem)
		{
			WriteStartElement(element, namesp, ob);
		}
		if (needType)
		{
			WriteXsiType("Text", "http://www.w3.org/2003/05/soap-envelope");
		}
		WriteAttribute("lang", "http://www.w3.org/XML/1998/namespace", ob.XmlLang);
		WriteValue(ob.Value);
		if (writeWrappingElem)
		{
			WriteEndElement(ob);
		}
	}

	protected override void InitCallbacks()
	{
	}
}
