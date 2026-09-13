using System.ComponentModel;
using System.Web.Services.Configuration;
using System.Xml.Serialization;

namespace System.Web.Services.Description;

[XmlFormatExtension("fault", "http://schemas.xmlsoap.org/wsdl/soap/", typeof(FaultBinding))]
public class SoapFaultBinding : ServiceDescriptionFormatExtension
{
	private string encoding;

	private string ns;

	private SoapBindingUse use;

	private string name;

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

	[XmlAttribute("name")]
	public string Name
	{
		get
		{
			return name;
		}
		set
		{
			name = value;
		}
	}

	public SoapFaultBinding()
	{
		encoding = string.Empty;
		ns = string.Empty;
		use = SoapBindingUse.Default;
	}
}
