using System.Runtime.InteropServices;

namespace System.Reflection.Emit;

[Serializable]
[ComVisible(true)]
public readonly struct PropertyToken : IEquatable<PropertyToken>
{
	internal readonly int tokValue;

	public static readonly PropertyToken Empty;

	public int Token => tokValue;

	internal PropertyToken(int val)
	{
		tokValue = val;
	}

	public override bool Equals(object obj)
	{
		bool flag = obj is PropertyToken;
		if (flag)
		{
			PropertyToken propertyToken = (PropertyToken)obj;
			flag = tokValue == propertyToken.tokValue;
		}
		return flag;
	}

	public bool Equals(PropertyToken obj)
	{
		return tokValue == obj.tokValue;
	}

	public static bool operator ==(PropertyToken a, PropertyToken b)
	{
		return object.Equals(a, b);
	}

	public static bool operator !=(PropertyToken a, PropertyToken b)
	{
		return !object.Equals(a, b);
	}

	public override int GetHashCode()
	{
		return tokValue;
	}
}
