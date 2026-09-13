using System.ComponentModel;
using System.Xml.Serialization;

namespace System.Web.Services.Description;

public abstract class ServiceDescriptionFormatExtension
{
	private bool handled;

	private object parent;

	private bool required;

	[XmlIgnore]
	public bool Handled
	{
		get
		{
			return handled;
		}
		set
		{
			handled = value;
		}
	}

	public object Parent => parent;

	[DefaultValue(false)]
	[XmlAttribute("required", Namespace = "http://schemas.xmlsoap.org/wsdl/")]
	public bool Required
	{
		get
		{
			return required;
		}
		set
		{
			required = value;
		}
	}

	protected ServiceDescriptionFormatExtension()
	{
		handled = false;
		parent = null;
		required = false;
	}

	internal void SetParent(object value)
	{
		parent = value;
	}
}
