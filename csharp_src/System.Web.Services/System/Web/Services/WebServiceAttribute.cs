namespace System.Web.Services;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, Inherited = true)]
public sealed class WebServiceAttribute : Attribute
{
	public const string DefaultNamespace = "http://tempuri.org/";

	private string description;

	private string name;

	private string ns;

	public string Description
	{
		get
		{
			return description;
		}
		set
		{
			description = value;
		}
	}

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

	public WebServiceAttribute()
	{
		description = string.Empty;
		name = string.Empty;
		ns = "http://tempuri.org/";
	}
}
