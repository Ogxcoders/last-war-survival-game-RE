using System.ComponentModel;
using System.Xml;
using System.Xml.Serialization;

namespace System.Web.Services.Description;

public class SoapHeaderFaultBinding : ServiceDescriptionFormatExtension
{
	private string encoding;

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
	public SoapHeaderFaultBinding()
	{
		encoding = string.Empty;
		message = XmlQualifiedName.Empty;
		ns = string.Empty;
		part = string.Empty;
		use = SoapBindingUse.Default;
	}
}
