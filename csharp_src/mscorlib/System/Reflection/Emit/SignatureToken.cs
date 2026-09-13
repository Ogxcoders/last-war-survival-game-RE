using System.Runtime.InteropServices;

namespace System.Reflection.Emit;

[ComVisible(true)]
public readonly struct SignatureToken : IEquatable<SignatureToken>
{
	internal readonly int tokValue;

	public static readonly SignatureToken Empty;

	public int Token => tokValue;

	internal SignatureToken(int val)
	{
		tokValue = val;
	}

	public override bool Equals(object obj)
	{
		bool flag = obj is SignatureToken;
		if (flag)
		{
			SignatureToken signatureToken = (SignatureToken)obj;
			flag = tokValue == signatureToken.tokValue;
		}
		return flag;
	}

	public bool Equals(SignatureToken obj)
	{
		return tokValue == obj.tokValue;
	}

	public static bool operator ==(SignatureToken a, SignatureToken b)
	{
		return object.Equals(a, b);
	}

	public static bool operator !=(SignatureToken a, SignatureToken b)
	{
		return !object.Equals(a, b);
	}

	public override int GetHashCode()
	{
		return tokValue;
	}
}
