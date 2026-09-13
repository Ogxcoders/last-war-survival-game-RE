using System.Xml;
using System.Xml.Serialization;

namespace System.Web.Services.Protocols;

internal class FaultReader : XmlSerializationReader
{
	public object ReadRoot_Fault()
	{
		base.Reader.MoveToContent();
		if (base.Reader.LocalName != "Fault" || base.Reader.NamespaceURI != "http://schemas.xmlsoap.org/soap/envelope/")
		{
			throw CreateUnknownNodeException();
		}
		return ReadObject_Fault(isNullable: true, checkType: true);
	}

	public Fault ReadObject_Fault(bool isNullable, bool checkType)
	{
		Fault fault = null;
		if (isNullable && ReadNull())
		{
			return null;
		}
		if (checkType)
		{
			XmlQualifiedName xsiType = GetXsiType();
			if (xsiType != null && (xsiType.Name != "Fault" || xsiType.Namespace != "http://schemas.xmlsoap.org/soap/envelope/"))
			{
				throw CreateUnknownTypeException(xsiType);
			}
		}
		fault = new Fault();
		base.Reader.MoveToElement();
		while (base.Reader.MoveToNextAttribute())
		{
			if (!IsXmlnsAttribute(base.Reader.Name))
			{
				UnknownNode(fault);
			}
		}
		base.Reader.MoveToElement();
		if (base.Reader.IsEmptyElement)
		{
			base.Reader.Skip();
			return fault;
		}
		base.Reader.ReadStartElement();
		base.Reader.MoveToContent();
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = false;
		while (base.Reader.NodeType != XmlNodeType.EndElement)
		{
			if (base.Reader.NodeType == XmlNodeType.Element)
			{
				if (base.Reader.NamespaceURI == string.Empty || base.Reader.NamespaceURI == "http://schemas.xmlsoap.org/soap/envelope/")
				{
					if (base.Reader.LocalName == "faultcode" && !flag)
					{
						flag = true;
						fault.faultcode = ReadElementQualifiedName();
					}
					else if (base.Reader.LocalName == "faultstring" && !flag2)
					{
						flag2 = true;
						fault.faultstring = base.Reader.ReadElementString();
					}
					else if (base.Reader.LocalName == "detail" && !flag4)
					{
						flag4 = true;
						fault.detail = ReadXmlNode(wrapped: false);
					}
					else if (base.Reader.LocalName == "faultactor" && !flag3)
					{
						flag3 = true;
						fault.faultactor = base.Reader.ReadElementString();
					}
					else
					{
						UnknownNode(fault);
					}
				}
				else
				{
					UnknownNode(fault);
				}
			}
			else
			{
				UnknownNode(fault);
			}
			base.Reader.MoveToContent();
		}
		ReadEndElement();
		return fault;
	}

	protected override void InitCallbacks()
	{
	}

	protected override void InitIDs()
	{
	}
}
