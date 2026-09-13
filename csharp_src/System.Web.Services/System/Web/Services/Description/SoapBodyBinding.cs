using System.ComponentModel;
using System.Web.Services.Configuration;
using System.Xml.Serialization;

namespace System.Web.Services.Description;

[XmlFormatExtension("body", "http://schemas.xmlsoap.org/wsdl/soap/", typeof(InputBinding), typeof(OutputBinding), typeof(MimePart))]
public class SoapBodyBinding : ServiceDescriptionFormatExtension
{
	private string encoding;

	private string ns;

	private string[] parts;

	private string partsString;

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

	[XmlIgnore]
	public string[] Parts
	{
		get
		{
			return parts;
		}
		set
		{
			parts = value;
			if (value == null)
			{
				partsString = null;
			}
			else
			{
				partsString = string.Join(" ", value);
			}
		}
	}

	[XmlAttribute("parts")]
	public string PartsString
	{
		get
		{
			return partsString;
		}
		set
		{
			partsString = value;
			if (value == null)
			{
				parts = null;
			}
			else
			{
				parts = value.Split(' ');
			}
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

	public SoapBodyBinding()
	{
		encoding = string.Empty;
		ns = string.Empty;
		parts = null;
		partsString = null;
		use = SoapBindingUse.Default;
	}
}
