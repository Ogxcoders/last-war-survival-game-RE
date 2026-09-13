using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Xml;

namespace System.Web.Services.Protocols;

[Serializable]
public class SoapException : SystemException
{
	public static readonly XmlQualifiedName ClientFaultCode = new XmlQualifiedName("Client", "http://schemas.xmlsoap.org/soap/envelope/");

	public static readonly XmlQualifiedName DetailElementName = new XmlQualifiedName("detail");

	public static readonly XmlQualifiedName MustUnderstandFaultCode = new XmlQualifiedName("MustUnderstand", "http://schemas.xmlsoap.org/soap/envelope/");

	public static readonly XmlQualifiedName ServerFaultCode = new XmlQualifiedName("Server", "http://schemas.xmlsoap.org/soap/envelope/");

	public static readonly XmlQualifiedName VersionMismatchFaultCode = new XmlQualifiedName("VersionMismatch", "http://schemas.xmlsoap.org/soap/envelope/");

	private string actor;

	private XmlQualifiedName code;

	private XmlNode detail;

	private string lang;

	private string role;

	private SoapFaultSubCode subcode;

	public string Actor => actor;

	public XmlQualifiedName Code => code;

	public XmlNode Detail => detail;

	[ComVisible(false)]
	public string Lang => lang;

	[ComVisible(false)]
	public string Role => role;

	[ComVisible(false)]
	public SoapFaultSubCode SubCode => subcode;

	[ComVisible(false)]
	public string Node => actor;

	public SoapException()
		: this("SOAP error", XmlQualifiedName.Empty)
	{
	}

	public SoapException(string message, XmlQualifiedName code)
		: base(message)
	{
		this.code = code;
	}

	public SoapException(string message, XmlQualifiedName code, Exception innerException)
		: base(message, innerException)
	{
		this.code = code;
	}

	public SoapException(string message, XmlQualifiedName code, string actor)
		: base(message)
	{
		this.code = code;
		this.actor = actor;
	}

	public SoapException(string message, XmlQualifiedName code, string actor, Exception innerException)
		: base(message, innerException)
	{
		this.code = code;
		this.actor = actor;
	}

	public SoapException(string message, XmlQualifiedName code, string actor, XmlNode detail)
		: base(message)
	{
		this.code = code;
		this.actor = actor;
		this.detail = detail;
	}

	public SoapException(string message, XmlQualifiedName code, string actor, XmlNode detail, Exception innerException)
		: base(message, innerException)
	{
		this.code = code;
		this.actor = actor;
		this.detail = detail;
	}

	public SoapException(string message, XmlQualifiedName code, SoapFaultSubCode subcode)
		: base(message)
	{
		this.code = code;
		this.subcode = subcode;
	}

	public SoapException(string message, XmlQualifiedName code, string actor, string role, XmlNode detail, SoapFaultSubCode subcode, Exception innerException)
		: base(message, innerException)
	{
		this.code = code;
		this.subcode = subcode;
		this.detail = detail;
		this.actor = actor;
		this.role = role;
	}

	public SoapException(string message, XmlQualifiedName code, string actor, string role, string lang, XmlNode detail, SoapFaultSubCode subcode, Exception innerException)
		: this(message, code, actor, role, detail, subcode, innerException)
	{
		this.lang = lang;
	}

	protected SoapException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
		actor = info.GetString("actor");
		code = (XmlQualifiedName)info.GetValue("code", typeof(XmlQualifiedName));
		detail = new XmlDocument().ReadNode(XmlReader.Create(new StringReader(info.GetString("detailString"))));
		lang = info.GetString("lang");
		role = info.GetString("role");
		subcode = (SoapFaultSubCode)info.GetValue("subcode", typeof(SoapFaultSubCode));
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		base.GetObjectData(info, context);
		info.AddValue("actor", actor);
		info.AddValue("code", code);
		info.AddValue("detailString", detail.OuterXml);
		info.AddValue("lang", lang);
		info.AddValue("role", role);
		info.AddValue("subcode", subcode);
	}

	public static bool IsClientFaultCode(XmlQualifiedName code)
	{
		if (code == ClientFaultCode)
		{
			return true;
		}
		if (code == Soap12FaultCodes.SenderFaultCode)
		{
			return true;
		}
		return false;
	}

	public static bool IsMustUnderstandFaultCode(XmlQualifiedName code)
	{
		if (code == MustUnderstandFaultCode)
		{
			return true;
		}
		if (code == Soap12FaultCodes.MustUnderstandFaultCode)
		{
			return true;
		}
		return false;
	}

	public static bool IsServerFaultCode(XmlQualifiedName code)
	{
		if (code == ServerFaultCode)
		{
			return true;
		}
		if (code == Soap12FaultCodes.ReceiverFaultCode)
		{
			return true;
		}
		return false;
	}

	public static bool IsVersionMismatchFaultCode(XmlQualifiedName code)
	{
		if (code == VersionMismatchFaultCode)
		{
			return true;
		}
		if (code == Soap12FaultCodes.VersionMismatchFaultCode)
		{
			return true;
		}
		return false;
	}
}
