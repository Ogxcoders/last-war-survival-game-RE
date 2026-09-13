namespace System.Web.Services.Configuration;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public sealed class XmlFormatExtensionPrefixAttribute : Attribute
{
	private string prefix;

	private string ns;

	public string Prefix
	{
		get
		{
			return prefix;
		}
		set
		{
			prefix = value;
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

	public XmlFormatExtensionPrefixAttribute()
	{
	}

	public XmlFormatExtensionPrefixAttribute(string prefix, string ns)
		: this()
	{
		this.prefix = prefix;
		this.ns = ns;
	}
}
