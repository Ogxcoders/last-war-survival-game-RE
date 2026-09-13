using System.Xml;

namespace System.Web.Services.Protocols;

[Serializable]
public class SoapFaultSubCode
{
	private XmlQualifiedName _code;

	private SoapFaultSubCode _subcode;

	public XmlQualifiedName Code => _code;

	public SoapFaultSubCode SubCode => _subcode;

	public SoapFaultSubCode(XmlQualifiedName code)
	{
		_code = code;
	}

	public SoapFaultSubCode(XmlQualifiedName code, SoapFaultSubCode subcode)
	{
		_code = code;
		_subcode = subcode;
	}
}
