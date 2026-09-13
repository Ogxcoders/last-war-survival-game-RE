using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Xml;
using System.Xml.Serialization;

namespace System.Web.Services.Protocols;

[SoapType(IncludeInSchema = false)]
[XmlType(IncludeInSchema = false)]
public abstract class SoapHeader
{
	private string actor;

	private bool didUnderstand;

	private bool mustUnderstand;

	private string role;

	private bool relay;

	[DefaultValue("")]
	[SoapAttribute("actor", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
	[XmlAttribute("actor", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
	public string Actor
	{
		get
		{
			return actor;
		}
		set
		{
			actor = value;
		}
	}

	[SoapIgnore]
	[XmlIgnore]
	public bool DidUnderstand
	{
		get
		{
			return didUnderstand;
		}
		set
		{
			didUnderstand = value;
		}
	}

	[DefaultValue("0")]
	[SoapAttribute("mustUnderstand", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
	[XmlAttribute("mustUnderstand", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
	public string EncodedMustUnderstand
	{
		get
		{
			if (!MustUnderstand)
			{
				return "0";
			}
			return "1";
		}
		set
		{
			switch (value)
			{
			case "true":
			case "1":
				MustUnderstand = true;
				break;
			case "false":
			case "0":
				MustUnderstand = false;
				break;
			default:
				throw new ArgumentException();
			}
		}
	}

	[SoapIgnore]
	[XmlIgnore]
	public bool MustUnderstand
	{
		get
		{
			return mustUnderstand;
		}
		set
		{
			mustUnderstand = value;
		}
	}

	[DefaultValue("0")]
	[SoapAttribute("mustUnderstand", Namespace = "http://www.w3.org/2003/05/soap-envelope")]
	[XmlAttribute("mustUnderstand", Namespace = "http://www.w3.org/2003/05/soap-envelope")]
	[ComVisible(false)]
	public string EncodedMustUnderstand12
	{
		get
		{
			if (!MustUnderstand)
			{
				return "0";
			}
			return "1";
		}
		set
		{
			switch (value)
			{
			case "true":
			case "1":
				MustUnderstand = true;
				break;
			case "false":
			case "0":
				MustUnderstand = false;
				break;
			default:
				throw new ArgumentException();
			}
		}
	}

	[DefaultValue("0")]
	[SoapAttribute("relay", Namespace = "http://www.w3.org/2003/05/soap-envelope")]
	[XmlAttribute("relay", Namespace = "http://www.w3.org/2003/05/soap-envelope")]
	[ComVisible(false)]
	public string EncodedRelay
	{
		get
		{
			if (!Relay)
			{
				return "0";
			}
			return "1";
		}
		set
		{
			switch (value)
			{
			case "true":
			case "1":
				Relay = true;
				break;
			case "false":
			case "0":
				Relay = false;
				break;
			default:
				throw new ArgumentException();
			}
		}
	}

	[SoapIgnore]
	[XmlIgnore]
	[ComVisible(false)]
	public bool Relay
	{
		get
		{
			return relay;
		}
		set
		{
			relay = value;
		}
	}

	[DefaultValue("")]
	[SoapAttribute("role", Namespace = "http://www.w3.org/2003/05/soap-envelope")]
	[XmlAttribute("role", Namespace = "http://www.w3.org/2003/05/soap-envelope")]
	[ComVisible(false)]
	public string Role
	{
		get
		{
			return role;
		}
		set
		{
			role = value;
		}
	}

	protected SoapHeader()
	{
		actor = string.Empty;
		didUnderstand = false;
		mustUnderstand = false;
	}

	internal SoapHeader(XmlElement elem)
	{
		actor = elem.GetAttribute("actor", "http://schemas.xmlsoap.org/soap/envelope/");
		string attribute = elem.GetAttribute("mustUnderstand", "http://schemas.xmlsoap.org/soap/envelope/");
		if (attribute != "")
		{
			EncodedMustUnderstand = attribute;
		}
		role = elem.GetAttribute("role", "http://www.w3.org/2003/05/soap-envelope");
		attribute = elem.GetAttribute("mustUnderstand", "http://www.w3.org/2003/05/soap-envelope");
		if (attribute != "")
		{
			EncodedMustUnderstand12 = attribute;
		}
	}
}
