using System.Reflection;
using System.Text;

namespace System.Xml.Serialization;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.ReturnValue, AllowMultiple = false)]
public class XmlChoiceIdentifierAttribute : Attribute
{
	private string memberName;

	private MemberInfo member;

	public string MemberName
	{
		get
		{
			if (memberName == null)
			{
				return string.Empty;
			}
			return memberName;
		}
		set
		{
			memberName = value;
		}
	}

	internal MemberInfo MemberInfo
	{
		get
		{
			return member;
		}
		set
		{
			MemberName = ((value != null) ? value.Name : null);
			member = value;
		}
	}

	public XmlChoiceIdentifierAttribute()
	{
	}

	public XmlChoiceIdentifierAttribute(string name)
	{
		memberName = name;
	}

	internal void AddKeyHash(StringBuilder sb)
	{
		sb.Append("XCA ");
		KeyHelper.AddField(sb, 1, memberName);
		sb.Append('|');
	}
}
