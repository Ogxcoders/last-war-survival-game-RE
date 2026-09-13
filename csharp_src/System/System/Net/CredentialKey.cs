using System.Globalization;

namespace System.Net;

internal class CredentialKey
{
	internal Uri UriPrefix;

	internal int UriPrefixLength = -1;

	internal string AuthenticationType;

	private int m_HashCode;

	private bool m_ComputedHashCode;

	internal CredentialKey(Uri uriPrefix, string authenticationType)
	{
		UriPrefix = uriPrefix;
		UriPrefixLength = UriPrefix.ToString().Length;
		AuthenticationType = authenticationType;
	}

	internal bool Match(Uri uri, string authenticationType)
	{
		if (uri == null || authenticationType == null)
		{
			return false;
		}
		if (string.Compare(authenticationType, AuthenticationType, StringComparison.OrdinalIgnoreCase) != 0)
		{
			return false;
		}
		return IsPrefix(uri, UriPrefix);
	}

	internal bool IsPrefix(Uri uri, Uri prefixUri)
	{
		if (prefixUri.Scheme != uri.Scheme || prefixUri.Host != uri.Host || prefixUri.Port != uri.Port)
		{
			return false;
		}
		int num = prefixUri.AbsolutePath.LastIndexOf('/');
		if (num > uri.AbsolutePath.LastIndexOf('/'))
		{
			return false;
		}
		return string.Compare(uri.AbsolutePath, 0, prefixUri.AbsolutePath, 0, num, StringComparison.OrdinalIgnoreCase) == 0;
	}

	public override int GetHashCode()
	{
		if (!m_ComputedHashCode)
		{
			m_HashCode = AuthenticationType.ToUpperInvariant().GetHashCode() + UriPrefixLength + UriPrefix.GetHashCode();
			m_ComputedHashCode = true;
		}
		return m_HashCode;
	}

	public override bool Equals(object comparand)
	{
		CredentialKey credentialKey = comparand as CredentialKey;
		if (comparand == null)
		{
			return false;
		}
		if (string.Compare(AuthenticationType, credentialKey.AuthenticationType, StringComparison.OrdinalIgnoreCase) == 0)
		{
			return UriPrefix.Equals(credentialKey.UriPrefix);
		}
		return false;
	}

	public override string ToString()
	{
		return "[" + UriPrefixLength.ToString(NumberFormatInfo.InvariantInfo) + "]:" + ValidationHelper.ToString(UriPrefix) + ":" + ValidationHelper.ToString(AuthenticationType);
	}
}
