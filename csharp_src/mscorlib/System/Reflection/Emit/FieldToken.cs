using System.Runtime.InteropServices;

namespace System.Reflection.Emit;

[Serializable]
[ComVisible(true)]
public readonly struct FieldToken : IEquatable<FieldToken>
{
	internal readonly int tokValue;

	public static readonly FieldToken Empty;

	public int Token => tokValue;

	internal FieldToken(int val)
	{
		tokValue = val;
	}

	public override bool Equals(object obj)
	{
		bool flag = obj is FieldToken;
		if (flag)
		{
			FieldToken fieldToken = (FieldToken)obj;
			flag = tokValue == fieldToken.tokValue;
		}
		return flag;
	}

	public bool Equals(FieldToken obj)
	{
		return tokValue == obj.tokValue;
	}

	public static bool operator ==(FieldToken a, FieldToken b)
	{
		return object.Equals(a, b);
	}

	public static bool operator !=(FieldToken a, FieldToken b)
	{
		return !object.Equals(a, b);
	}

	public override int GetHashCode()
	{
		return tokValue;
	}
}
