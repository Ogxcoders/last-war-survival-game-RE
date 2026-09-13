namespace System.Web.Services.Protocols;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public sealed class SoapHeaderAttribute : Attribute
{
	private SoapHeaderDirection direction;

	private string memberName;

	private bool required;

	public SoapHeaderDirection Direction
	{
		get
		{
			return direction;
		}
		set
		{
			direction = value;
		}
	}

	public string MemberName
	{
		get
		{
			return memberName;
		}
		set
		{
			memberName = value;
		}
	}

	[Obsolete("This property will be removed from a future version. The presence of a particular header in a SOAP message is no longer enforced", false)]
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

	public SoapHeaderAttribute(string memberName)
	{
		direction = SoapHeaderDirection.In;
		this.memberName = memberName;
		required = true;
	}
}
