using System.Xml;
using System.Xml.Serialization;

namespace System.Web.Services.Protocols;

[XmlRoot("Fault", Namespace = "http://www.w3.org/2003/05/soap-envelope")]
[XmlType("Fault", Namespace = "http://www.w3.org/2003/05/soap-envelope")]
internal class Soap12Fault
{
	public static XmlSerializer Serializer = new Fault12Serializer();

	public Soap12FaultCode Code;

	public Soap12FaultReason Reason;

	[XmlElement(DataType = "anyURI")]
	public string Node;

	[XmlElement(DataType = "anyURI")]
	public string Role;

	public Soap12FaultDetail Detail;

	public Soap12Fault()
	{
	}

	public Soap12Fault(SoapException ex)
	{
		Code = new Soap12FaultCode();
		Code.Value = ex.Code;
		if (ex.SubCode != null)
		{
			Code.Subcode = CreateFaultCode(ex.SubCode);
		}
		Node = ex.Node;
		Role = ex.Role;
		Reason = new Soap12FaultReason();
		Soap12FaultReasonText soap12FaultReasonText = new Soap12FaultReasonText
		{
			XmlLang = ex.Lang,
			Value = ex.Message
		};
		Reason.Texts = new Soap12FaultReasonText[1] { soap12FaultReasonText };
		if (ex.Detail != null)
		{
			Detail = new Soap12FaultDetail();
			if (ex.Detail.NodeType == XmlNodeType.Attribute)
			{
				Detail.Attributes = new XmlAttribute[1] { (XmlAttribute)ex.Detail };
			}
			else if (ex.Detail.NodeType == XmlNodeType.Element)
			{
				Detail.Children = new XmlElement[1] { (XmlElement)ex.Detail };
			}
			else
			{
				Detail.Text = ex.Detail.Value;
			}
		}
	}

	private static Soap12FaultCode CreateFaultCode(SoapFaultSubCode code)
	{
		if (code == null)
		{
			throw new ArgumentNullException("code");
		}
		Soap12FaultCode soap12FaultCode = new Soap12FaultCode();
		soap12FaultCode.Value = code.Code;
		if (code.SubCode != null)
		{
			soap12FaultCode.Subcode = CreateFaultCode(code.SubCode);
		}
		return soap12FaultCode;
	}

	public static SoapFaultSubCode GetSoapFaultSubCode(Soap12FaultCode src)
	{
		if (src != null)
		{
			return new SoapFaultSubCode(src.Value, GetSoapFaultSubCode(src.Subcode));
		}
		return null;
	}
}
