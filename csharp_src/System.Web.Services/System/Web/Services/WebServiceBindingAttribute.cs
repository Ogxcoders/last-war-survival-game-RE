namespace System.Web.Services;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true, Inherited = true)]
public sealed class WebServiceBindingAttribute : Attribute
{
	private string location;

	private string name;

	private string ns;

	private bool emitConformanceClaims;

	private WsiProfiles conformsTo;

	public string Location
	{
		get
		{
			return location;
		}
		set
		{
			location = value;
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

	public bool EmitConformanceClaims
	{
		get
		{
			return emitConformanceClaims;
		}
		set
		{
			emitConformanceClaims = value;
		}
	}

	public WsiProfiles ConformsTo
	{
		get
		{
			return conformsTo;
		}
		set
		{
			conformsTo = value;
		}
	}

	public WebServiceBindingAttribute()
		: this(string.Empty, string.Empty, string.Empty)
	{
	}

	public WebServiceBindingAttribute(string name)
		: this(name, string.Empty, string.Empty)
	{
	}

	public WebServiceBindingAttribute(string name, string ns)
		: this(name, ns, string.Empty)
	{
	}

	public WebServiceBindingAttribute(string name, string ns, string location)
	{
		this.name = name;
		this.ns = ns;
		this.location = location;
	}
}
