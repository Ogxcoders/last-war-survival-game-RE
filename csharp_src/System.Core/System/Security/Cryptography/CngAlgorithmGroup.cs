using System.Security.Permissions;

namespace System.Security.Cryptography;

[Serializable]
[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
public sealed class CngAlgorithmGroup : IEquatable<CngAlgorithmGroup>
{
	private static volatile CngAlgorithmGroup s_dh;

	private static volatile CngAlgorithmGroup s_dsa;

	private static volatile CngAlgorithmGroup s_ecdh;

	private static volatile CngAlgorithmGroup s_ecdsa;

	private static volatile CngAlgorithmGroup s_rsa;

	private string m_algorithmGroup;

	public string AlgorithmGroup => m_algorithmGroup;

	public static CngAlgorithmGroup DiffieHellman
	{
		get
		{
			if (s_dh == null)
			{
				s_dh = new CngAlgorithmGroup("DH");
			}
			return s_dh;
		}
	}

	public static CngAlgorithmGroup Dsa
	{
		get
		{
			if (s_dsa == null)
			{
				s_dsa = new CngAlgorithmGroup("DSA");
			}
			return s_dsa;
		}
	}

	public static CngAlgorithmGroup ECDiffieHellman
	{
		get
		{
			if (s_ecdh == null)
			{
				s_ecdh = new CngAlgorithmGroup("ECDH");
			}
			return s_ecdh;
		}
	}

	public static CngAlgorithmGroup ECDsa
	{
		get
		{
			if (s_ecdsa == null)
			{
				s_ecdsa = new CngAlgorithmGroup("ECDSA");
			}
			return s_ecdsa;
		}
	}

	public static CngAlgorithmGroup Rsa
	{
		get
		{
			if (s_rsa == null)
			{
				s_rsa = new CngAlgorithmGroup("RSA");
			}
			return s_rsa;
		}
	}

	public CngAlgorithmGroup(string algorithmGroup)
	{
		if (algorithmGroup == null)
		{
			throw new ArgumentNullException("algorithmGroup");
		}
		if (algorithmGroup.Length == 0)
		{
			throw new ArgumentException(SR.GetString("The algorithm group '{0}' is invalid.", algorithmGroup), "algorithmGroup");
		}
		m_algorithmGroup = algorithmGroup;
	}

	public static bool operator ==(CngAlgorithmGroup left, CngAlgorithmGroup right)
	{
		return left?.Equals(right) ?? ((object)right == null);
	}

	public static bool operator !=(CngAlgorithmGroup left, CngAlgorithmGroup right)
	{
		if ((object)left == null)
		{
			return (object)right != null;
		}
		return !left.Equals(right);
	}

	public override bool Equals(object obj)
	{
		return Equals(obj as CngAlgorithmGroup);
	}

	public bool Equals(CngAlgorithmGroup other)
	{
		if ((object)other == null)
		{
			return false;
		}
		return m_algorithmGroup.Equals(other.AlgorithmGroup);
	}

	public override int GetHashCode()
	{
		return m_algorithmGroup.GetHashCode();
	}

	public override string ToString()
	{
		return m_algorithmGroup;
	}
}
