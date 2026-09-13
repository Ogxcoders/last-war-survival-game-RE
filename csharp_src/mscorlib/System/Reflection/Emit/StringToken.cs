using System.Runtime.InteropServices;

namespace System.Reflection.Emit;

[Serializable]
[ComVisible(true)]
public readonly struct StringToken : IEquatable<StringToken>
{
	internal readonly int tokValue;

	public int Token => tokValue;

	internal StringToken(int val)
	{
		tokValue = val;
	}

	public override bool Equals(object obj)
	{
		bool flag = obj is StringToken;
		if (flag)
		{
			StringToken stringToken = (StringToken)obj;
			flag = tokValue == stringToken.tokValue;
		}
		return flag;
	}

	public bool Equals(StringToken obj)
	{
		return tokValue == obj.tokValue;
	}

	public static bool operator ==(StringToken a, StringToken b)
	{
		return object.Equals(a, b);
	}

	public static bool operator !=(StringToken a, StringToken b)
	{
		return !object.Equals(a, b);
	}

	public override int GetHashCode()
	{
		return tokValue;
	}
}
