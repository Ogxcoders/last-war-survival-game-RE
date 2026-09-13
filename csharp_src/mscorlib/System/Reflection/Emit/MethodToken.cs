using System.Runtime.InteropServices;

namespace System.Reflection.Emit;

[Serializable]
[ComVisible(true)]
public readonly struct MethodToken : IEquatable<MethodToken>
{
	internal readonly int tokValue;

	public static readonly MethodToken Empty;

	public int Token => tokValue;

	internal MethodToken(int val)
	{
		tokValue = val;
	}

	public override bool Equals(object obj)
	{
		bool flag = obj is MethodToken;
		if (flag)
		{
			MethodToken methodToken = (MethodToken)obj;
			flag = tokValue == methodToken.tokValue;
		}
		return flag;
	}

	public bool Equals(MethodToken obj)
	{
		return tokValue == obj.tokValue;
	}

	public static bool operator ==(MethodToken a, MethodToken b)
	{
		return object.Equals(a, b);
	}

	public static bool operator !=(MethodToken a, MethodToken b)
	{
		return !object.Equals(a, b);
	}

	public override int GetHashCode()
	{
		return tokValue;
	}
}
