using System.Runtime.InteropServices;

namespace System.Reflection.Emit;

[Serializable]
[ComVisible(true)]
public readonly struct ParameterToken : IEquatable<ParameterToken>
{
	internal readonly int tokValue;

	public static readonly ParameterToken Empty;

	public int Token => tokValue;

	internal ParameterToken(int val)
	{
		tokValue = val;
	}

	public override bool Equals(object obj)
	{
		bool flag = obj is ParameterToken;
		if (flag)
		{
			ParameterToken parameterToken = (ParameterToken)obj;
			flag = tokValue == parameterToken.tokValue;
		}
		return flag;
	}

	public bool Equals(ParameterToken obj)
	{
		return tokValue == obj.tokValue;
	}

	public static bool operator ==(ParameterToken a, ParameterToken b)
	{
		return object.Equals(a, b);
	}

	public static bool operator !=(ParameterToken a, ParameterToken b)
	{
		return !object.Equals(a, b);
	}

	public override int GetHashCode()
	{
		return tokValue;
	}
}
