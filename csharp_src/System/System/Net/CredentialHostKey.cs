using System.Globalization;

namespace System.Net;

internal class CredentialHostKey
{
	internal string Host;

	internal string AuthenticationType;

	internal int Port;

	private int m_HashCode;

	private bool m_ComputedHashCode;

	internal CredentialHostKey(string host, int port, string authenticationType)
	{
		Host = host;
		Port = port;
		AuthenticationType = authenticationType;
	}

	internal bool Match(string host, int port, string authenticationType)
	{
		if (host == null || authenticationType == null)
		{
			return false;
		}
		if (string.Compare(authenticationType, AuthenticationType, StringComparison.OrdinalIgnoreCase) != 0)
		{
			return false;
		}
		if (string.Compare(Host, host, StringComparison.OrdinalIgnoreCase) != 0)
		{
			return false;
		}
		if (port != Port)
		{
			return false;
		}
		return true;
	}

	public override int GetHashCode()
	{
		if (!m_ComputedHashCode)
		{
			m_HashCode = AuthenticationType.ToUpperInvariant().GetHashCode() + Host.ToUpperInvariant().GetHashCode() + Port.GetHashCode();
			m_ComputedHashCode = true;
		}
		return m_HashCode;
	}

	public override bool Equals(object comparand)
	{
		CredentialHostKey credentialHostKey = comparand as CredentialHostKey;
		if (comparand == null)
		{
			return false;
		}
		if (string.Compare(AuthenticationType, credentialHostKey.AuthenticationType, StringComparison.OrdinalIgnoreCase) == 0 && string.Compare(Host, credentialHostKey.Host, StringComparison.OrdinalIgnoreCase) == 0)
		{
			return Port == credentialHostKey.Port;
		}
		return false;
	}

	public override string ToString()
	{
		return "[" + Host.Length.ToString(NumberFormatInfo.InvariantInfo) + "]:" + Host + ":" + Port.ToString(NumberFormatInfo.InvariantInfo) + ":" + ValidationHelper.ToString(AuthenticationType);
	}
}
