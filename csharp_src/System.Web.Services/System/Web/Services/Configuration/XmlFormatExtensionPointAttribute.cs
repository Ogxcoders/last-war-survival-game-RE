namespace System.Web.Services.Configuration;

[AttributeUsage(AttributeTargets.Class, Inherited = true)]
public sealed class XmlFormatExtensionPointAttribute : Attribute
{
	private bool allowElements;

	private string memberName;

	public bool AllowElements
	{
		get
		{
			return allowElements;
		}
		set
		{
			allowElements = value;
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

	public XmlFormatExtensionPointAttribute(string memberName)
	{
		this.memberName = memberName;
		allowElements = true;
	}
}
