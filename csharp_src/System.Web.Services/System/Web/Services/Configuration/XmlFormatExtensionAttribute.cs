namespace System.Web.Services.Configuration;

[AttributeUsage(AttributeTargets.Class, Inherited = true)]
public sealed class XmlFormatExtensionAttribute : Attribute
{
	private string elementName;

	private string ns;

	private Type[] extensionPoints;

	public string ElementName
	{
		get
		{
			return elementName;
		}
		set
		{
			elementName = value;
		}
	}

	public Type[] ExtensionPoints
	{
		get
		{
			return extensionPoints;
		}
		set
		{
			extensionPoints = value;
		}
	}

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

	public XmlFormatExtensionAttribute()
	{
	}

	public XmlFormatExtensionAttribute(string elementName, string ns, Type extensionPoint1)
		: this(elementName, ns, new Type[1] { extensionPoint1 })
	{
	}

	public XmlFormatExtensionAttribute(string elementName, string ns, Type[] extensionPoints)
		: this()
	{
		this.elementName = elementName;
		this.ns = ns;
		this.extensionPoints = extensionPoints;
	}

	public XmlFormatExtensionAttribute(string elementName, string ns, Type extensionPoint1, Type extensionPoint2)
		: this(elementName, ns, new Type[2] { extensionPoint1, extensionPoint2 })
	{
	}

	public XmlFormatExtensionAttribute(string elementName, string ns, Type extensionPoint1, Type extensionPoint2, Type extensionPoint3)
		: this(elementName, ns, new Type[3] { extensionPoint1, extensionPoint2, extensionPoint3 })
	{
	}

	public XmlFormatExtensionAttribute(string elementName, string ns, Type extensionPoint1, Type extensionPoint2, Type extensionPoint3, Type extensionPoint4)
		: this(elementName, ns, new Type[4] { extensionPoint1, extensionPoint2, extensionPoint3, extensionPoint4 })
	{
	}
}
