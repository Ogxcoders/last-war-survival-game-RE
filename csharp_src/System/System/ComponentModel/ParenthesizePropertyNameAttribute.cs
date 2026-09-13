namespace System.ComponentModel;

[AttributeUsage(AttributeTargets.All)]
public sealed class ParenthesizePropertyNameAttribute : Attribute
{
	public static readonly ParenthesizePropertyNameAttribute Default = new ParenthesizePropertyNameAttribute();

	private bool needParenthesis;

	public bool NeedParenthesis => needParenthesis;

	public ParenthesizePropertyNameAttribute()
		: this(needParenthesis: false)
	{
	}

	public ParenthesizePropertyNameAttribute(bool needParenthesis)
	{
		this.needParenthesis = needParenthesis;
	}

	public override bool Equals(object o)
	{
		if (o is ParenthesizePropertyNameAttribute)
		{
			return ((ParenthesizePropertyNameAttribute)o).NeedParenthesis == needParenthesis;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	public override bool IsDefaultAttribute()
	{
		return Equals(Default);
	}
}
