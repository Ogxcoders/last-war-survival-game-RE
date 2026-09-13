namespace System.Security.Cryptography;

public sealed class RSASignaturePadding : IEquatable<RSASignaturePadding>
{
	private static readonly RSASignaturePadding s_pkcs1 = new RSASignaturePadding(RSASignaturePaddingMode.Pkcs1);

	private static readonly RSASignaturePadding s_pss = new RSASignaturePadding(RSASignaturePaddingMode.Pss);

	private readonly RSASignaturePaddingMode _mode;

	public static RSASignaturePadding Pkcs1 => s_pkcs1;

	public static RSASignaturePadding Pss => s_pss;

	public RSASignaturePaddingMode Mode => _mode;

	private RSASignaturePadding(RSASignaturePaddingMode mode)
	{
		_mode = mode;
	}

	public override int GetHashCode()
	{
		return _mode.GetHashCode();
	}

	public override bool Equals(object obj)
	{
		return Equals(obj as RSASignaturePadding);
	}

	public bool Equals(RSASignaturePadding other)
	{
		if (other != null)
		{
			return _mode == other._mode;
		}
		return false;
	}

	public static bool operator ==(RSASignaturePadding left, RSASignaturePadding right)
	{
		return left?.Equals(right) ?? ((object)right == null);
	}

	public static bool operator !=(RSASignaturePadding left, RSASignaturePadding right)
	{
		return !(left == right);
	}

	public override string ToString()
	{
		return _mode.ToString();
	}
}
