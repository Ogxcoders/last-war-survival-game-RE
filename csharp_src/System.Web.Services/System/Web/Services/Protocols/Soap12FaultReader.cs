using System.Xml;
using System.Xml.Serialization;

namespace System.Web.Services.Protocols;

internal class Soap12FaultReader : XmlSerializationReader
{
	public object ReadRoot_Soap12Fault()
	{
		base.Reader.MoveToContent();
		if (base.Reader.LocalName != "Fault" || base.Reader.NamespaceURI != "http://www.w3.org/2003/05/soap-envelope")
		{
			throw CreateUnknownNodeException();
		}
		return ReadObject_Fault(isNullable: true, checkType: true);
	}

	public Soap12Fault ReadObject_Fault(bool isNullable, bool checkType)
	{
		Soap12Fault soap12Fault = null;
		if (isNullable && ReadNull())
		{
			return null;
		}
		if (checkType)
		{
			XmlQualifiedName xsiType = GetXsiType();
			if (!(xsiType == null) && (xsiType.Name != "Fault" || xsiType.Namespace != "http://www.w3.org/2003/05/soap-envelope"))
			{
				throw CreateUnknownTypeException(xsiType);
			}
		}
		soap12Fault = new Soap12Fault();
		base.Reader.MoveToElement();
		while (base.Reader.MoveToNextAttribute())
		{
			if (!IsXmlnsAttribute(base.Reader.Name))
			{
				UnknownNode(soap12Fault);
			}
		}
		base.Reader.MoveToElement();
		if (base.Reader.IsEmptyElement)
		{
			base.Reader.Skip();
			return soap12Fault;
		}
		base.Reader.ReadStartElement();
		base.Reader.MoveToContent();
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = false;
		bool flag5 = false;
		while (base.Reader.NodeType != XmlNodeType.EndElement)
		{
			if (base.Reader.NodeType == XmlNodeType.Element)
			{
				if (base.Reader.LocalName == "Role" && base.Reader.NamespaceURI == "http://www.w3.org/2003/05/soap-envelope" && !flag4)
				{
					flag4 = true;
					soap12Fault.Role = base.Reader.ReadElementString();
				}
				else if (base.Reader.LocalName == "Detail" && base.Reader.NamespaceURI == "http://www.w3.org/2003/05/soap-envelope" && !flag5)
				{
					flag5 = true;
					soap12Fault.Detail = ReadObject_Detail(isNullable: false, checkType: true);
				}
				else if (base.Reader.LocalName == "Code" && base.Reader.NamespaceURI == "http://www.w3.org/2003/05/soap-envelope" && !flag)
				{
					flag = true;
					soap12Fault.Code = ReadObject_Code(isNullable: false, checkType: true);
				}
				else if (base.Reader.LocalName == "Node" && base.Reader.NamespaceURI == "http://www.w3.org/2003/05/soap-envelope" && !flag3)
				{
					flag3 = true;
					soap12Fault.Node = base.Reader.ReadElementString();
				}
				else if (base.Reader.LocalName == "Reason" && base.Reader.NamespaceURI == "http://www.w3.org/2003/05/soap-envelope" && !flag2)
				{
					flag2 = true;
					soap12Fault.Reason = ReadObject_Reason(isNullable: false, checkType: true);
				}
				else
				{
					UnknownNode(soap12Fault);
				}
			}
			else
			{
				UnknownNode(soap12Fault);
			}
			base.Reader.MoveToContent();
		}
		ReadEndElement();
		return soap12Fault;
	}

	public Soap12FaultDetail ReadObject_Detail(bool isNullable, bool checkType)
	{
		Soap12FaultDetail soap12FaultDetail = null;
		if (isNullable && ReadNull())
		{
			return null;
		}
		if (checkType)
		{
			XmlQualifiedName xsiType = GetXsiType();
			if (!(xsiType == null) && (xsiType.Name != "Detail" || xsiType.Namespace != "http://www.w3.org/2003/05/soap-envelope"))
			{
				throw CreateUnknownTypeException(xsiType);
			}
		}
		soap12FaultDetail = new Soap12FaultDetail();
		base.Reader.MoveToElement();
		int num = 0;
		XmlAttribute[] array = null;
		while (base.Reader.MoveToNextAttribute())
		{
			if (!IsXmlnsAttribute(base.Reader.Name))
			{
				XmlAttribute xmlAttribute = (XmlAttribute)base.Document.ReadNode(base.Reader);
				array = (XmlAttribute[])EnsureArrayIndex(array, num, typeof(XmlAttribute));
				array[num] = xmlAttribute;
				num++;
			}
		}
		array = (XmlAttribute[])ShrinkArray(array, num, typeof(XmlAttribute), isNullable: true);
		soap12FaultDetail.Attributes = array;
		base.Reader.MoveToElement();
		if (base.Reader.IsEmptyElement)
		{
			base.Reader.Skip();
			return soap12FaultDetail;
		}
		base.Reader.ReadStartElement();
		base.Reader.MoveToContent();
		XmlElement[] array2 = null;
		int num2 = 0;
		while (base.Reader.NodeType != XmlNodeType.EndElement)
		{
			if (base.Reader.NodeType == XmlNodeType.Element)
			{
				array2 = (XmlElement[])EnsureArrayIndex(array2, num2, typeof(XmlElement));
				array2[num2] = (XmlElement)ReadXmlNode(wrapped: false);
				num2++;
			}
			else if (base.Reader.NodeType == XmlNodeType.Text || base.Reader.NodeType == XmlNodeType.CDATA)
			{
				soap12FaultDetail.Text = ReadString(soap12FaultDetail.Text);
			}
			else
			{
				UnknownNode(soap12FaultDetail);
			}
			base.Reader.MoveToContent();
		}
		array2 = (XmlElement[])ShrinkArray(array2, num2, typeof(XmlElement), isNullable: true);
		soap12FaultDetail.Children = array2;
		ReadEndElement();
		return soap12FaultDetail;
	}

	public Soap12FaultCode ReadObject_Code(bool isNullable, bool checkType)
	{
		Soap12FaultCode soap12FaultCode = null;
		if (isNullable && ReadNull())
		{
			return null;
		}
		if (checkType)
		{
			XmlQualifiedName xsiType = GetXsiType();
			if (!(xsiType == null) && (xsiType.Name != "Code" || xsiType.Namespace != "http://www.w3.org/2003/05/soap-envelope"))
			{
				throw CreateUnknownTypeException(xsiType);
			}
		}
		soap12FaultCode = new Soap12FaultCode();
		base.Reader.MoveToElement();
		while (base.Reader.MoveToNextAttribute())
		{
			if (!IsXmlnsAttribute(base.Reader.Name))
			{
				UnknownNode(soap12FaultCode);
			}
		}
		base.Reader.MoveToElement();
		if (base.Reader.IsEmptyElement)
		{
			base.Reader.Skip();
			return soap12FaultCode;
		}
		base.Reader.ReadStartElement();
		base.Reader.MoveToContent();
		bool flag = false;
		bool flag2 = false;
		while (base.Reader.NodeType != XmlNodeType.EndElement)
		{
			if (base.Reader.NodeType == XmlNodeType.Element)
			{
				if (base.Reader.LocalName == "Value" && base.Reader.NamespaceURI == "http://www.w3.org/2003/05/soap-envelope" && !flag)
				{
					flag = true;
					soap12FaultCode.Value = ReadElementQualifiedName();
				}
				else if (base.Reader.LocalName == "Subcode" && base.Reader.NamespaceURI == "http://www.w3.org/2003/05/soap-envelope" && !flag2)
				{
					flag2 = true;
					soap12FaultCode.Subcode = ReadObject_Code(isNullable: false, checkType: true);
				}
				else
				{
					UnknownNode(soap12FaultCode);
				}
			}
			else
			{
				UnknownNode(soap12FaultCode);
			}
			base.Reader.MoveToContent();
		}
		ReadEndElement();
		return soap12FaultCode;
	}

	public Soap12FaultReason ReadObject_Reason(bool isNullable, bool checkType)
	{
		Soap12FaultReason soap12FaultReason = null;
		if (isNullable && ReadNull())
		{
			return null;
		}
		if (checkType)
		{
			XmlQualifiedName xsiType = GetXsiType();
			if (!(xsiType == null) && (xsiType.Name != "Reason" || xsiType.Namespace != "http://www.w3.org/2003/05/soap-envelope"))
			{
				throw CreateUnknownTypeException(xsiType);
			}
		}
		soap12FaultReason = new Soap12FaultReason();
		base.Reader.MoveToElement();
		while (base.Reader.MoveToNextAttribute())
		{
			if (!IsXmlnsAttribute(base.Reader.Name))
			{
				UnknownNode(soap12FaultReason);
			}
		}
		base.Reader.MoveToElement();
		if (base.Reader.IsEmptyElement)
		{
			base.Reader.Skip();
			return soap12FaultReason;
		}
		base.Reader.ReadStartElement();
		base.Reader.MoveToContent();
		bool flag = false;
		Soap12FaultReasonText[] array = null;
		int num = 0;
		while (base.Reader.NodeType != XmlNodeType.EndElement)
		{
			if (base.Reader.NodeType == XmlNodeType.Element)
			{
				if (base.Reader.LocalName == "Text" && base.Reader.NamespaceURI == "http://www.w3.org/2003/05/soap-envelope" && !flag)
				{
					array = (Soap12FaultReasonText[])EnsureArrayIndex(array, num, typeof(Soap12FaultReasonText));
					array[num] = ReadObject_Text(isNullable: false, checkType: true);
					num++;
				}
				else
				{
					UnknownNode(soap12FaultReason);
				}
			}
			else
			{
				UnknownNode(soap12FaultReason);
			}
			base.Reader.MoveToContent();
		}
		array = (Soap12FaultReasonText[])ShrinkArray(array, num, typeof(Soap12FaultReasonText), isNullable: true);
		soap12FaultReason.Texts = array;
		ReadEndElement();
		return soap12FaultReason;
	}

	public Soap12FaultReasonText ReadObject_Text(bool isNullable, bool checkType)
	{
		Soap12FaultReasonText soap12FaultReasonText = null;
		if (isNullable && ReadNull())
		{
			return null;
		}
		if (checkType)
		{
			XmlQualifiedName xsiType = GetXsiType();
			if (!(xsiType == null) && (xsiType.Name != "Text" || xsiType.Namespace != "http://www.w3.org/2003/05/soap-envelope"))
			{
				throw CreateUnknownTypeException(xsiType);
			}
		}
		soap12FaultReasonText = new Soap12FaultReasonText();
		base.Reader.MoveToElement();
		while (base.Reader.MoveToNextAttribute())
		{
			if (base.Reader.LocalName == "lang" && base.Reader.NamespaceURI == "http://www.w3.org/XML/1998/namespace")
			{
				soap12FaultReasonText.XmlLang = base.Reader.Value;
			}
			else if (!IsXmlnsAttribute(base.Reader.Name))
			{
				UnknownNode(soap12FaultReasonText);
			}
		}
		base.Reader.MoveToElement();
		if (base.Reader.IsEmptyElement)
		{
			base.Reader.Skip();
			return soap12FaultReasonText;
		}
		base.Reader.ReadStartElement();
		base.Reader.MoveToContent();
		while (base.Reader.NodeType != XmlNodeType.EndElement)
		{
			if (base.Reader.NodeType == XmlNodeType.Element)
			{
				UnknownNode(soap12FaultReasonText);
			}
			else if (base.Reader.NodeType == XmlNodeType.Text || base.Reader.NodeType == XmlNodeType.CDATA)
			{
				soap12FaultReasonText.Value = ReadString(soap12FaultReasonText.Value);
			}
			else
			{
				UnknownNode(soap12FaultReasonText);
			}
			base.Reader.MoveToContent();
		}
		ReadEndElement();
		return soap12FaultReasonText;
	}

	protected override void InitCallbacks()
	{
	}

	protected override void InitIDs()
	{
	}
}
