using System.Security.Permissions;

namespace System.Security.Cryptography;

[Serializable]
[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
public sealed class CngProvider : IEquatable<CngProvider>
{
	private static volatile CngProvider s_msSmartCardKsp;

	private static volatile CngProvider s_msSoftwareKsp;

	private string m_provider;

	public string Provider => m_provider;

	public static CngProvider MicrosoftSmartCardKeyStorageProvider
	{
		get
		{
			if (s_msSmartCardKsp == null)
			{
				s_msSmartCardKsp = new CngProvider("Microsoft Smart Card Key Storage Provider");
			}
			return s_msSmartCardKsp;
		}
	}

	public static CngProvider MicrosoftSoftwareKeyStorageProvider
	{
		get
		{
			if (s_msSoftwareKsp == null)
			{
				s_msSoftwareKsp = new CngProvider("Microsoft Software Key Storage Provider");
			}
			return s_msSoftwareKsp;
		}
	}

	public CngProvider(string provider)
	{
		if (provider == null)
		{
			throw new ArgumentNullException("provider");
		}
		if (provider.Length == 0)
		{
			throw new ArgumentException(SR.GetString("The provider name '{0}' is invalid.", provider), "provider");
		}
		m_provider = provider;
	}

	public static bool operator ==(CngProvider left, CngProvider right)
	{
		return left?.Equals(right) ?? ((object)right == null);
	}

	public static bool operator !=(CngProvider left, CngProvider right)
	{
		if ((object)left == null)
		{
			return (object)right != null;
		}
		return !left.Equals(right);
	}

	public override bool Equals(object obj)
	{
		return Equals(obj as CngProvider);
	}

	public bool Equals(CngProvider other)
	{
		if ((object)other == null)
		{
			return false;
		}
		return m_provider.Equals(other.Provider);
	}

	public override int GetHashCode()
	{
		return m_provider.GetHashCode();
	}

	public override string ToString()
	{
		return m_provider.ToString();
	}
}
