using System.ComponentModel;
using System.Web.Services.Configuration;
using System.Xml;
using System.Xml.Serialization;

namespace System.Web.Services.Description;

[XmlFormatExtension("header", "http://schemas.xmlsoap.org/wsdl/soap/", typeof(InputBinding), typeof(OutputBinding))]
public class SoapHeaderBinding : ServiceDescriptionFormatExtension
{
	private string encoding;

	private bool mapToProperty;

	private XmlQualifiedName message;

	private string ns;

	private string part;

	private SoapBindingUse use;

	[DefaultValue("")]
	[XmlAttribute("encodingStyle")]
	public string Encoding
	{
		get
		{
			return encoding;
		}
		set
		{
			encoding = value;
		}
	}

	[XmlIgnore]
	public bool MapToProperty
	{
		get
		{
			return mapToProperty;
		}
		set
		{
			mapToProperty = value;
		}
	}

	[XmlAttribute("message")]
	public XmlQualifiedName Message
	{
		get
		{
			return message;
		}
		set
		{
			message = value;
		}
	}

	[DefaultValue("")]
	[XmlAttribute("namespace")]
	public string Namespace
	{
		get
		{
			return ns;
		}
		set
		{
			ns = value;
		}
	}

	[XmlAttribute("part")]
	public string Part
	{
		get
		{
			return part;
		}
		set
		{
			part = value;
		}
	}

	[DefaultValue(SoapBindingUse.Default)]
	[XmlAttribute("use")]
	public SoapBindingUse Use
	{
		get
		{
			return use;
		}
		set
		{
			use = value;
		}
	}

	[System.MonoTODO]
	[XmlElement("headerfault")]
	public SoapHeaderFaultBinding Fault
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public SoapHeaderBinding()
	{
		encoding = string.Empty;
		mapToProperty = false;
		message = XmlQualifiedName.Empty;
		ns = string.Empty;
		part = string.Empty;
		use = SoapBindingUse.Default;
	}
}
