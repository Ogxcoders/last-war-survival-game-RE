using System.Security.Permissions;

namespace System.Security.Cryptography;

[Serializable]
[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
public sealed class CngKeyBlobFormat : IEquatable<CngKeyBlobFormat>
{
	private static volatile CngKeyBlobFormat s_eccPrivate;

	private static volatile CngKeyBlobFormat s_eccPublic;

	private static volatile CngKeyBlobFormat s_eccFullPrivate;

	private static volatile CngKeyBlobFormat s_eccFullPublic;

	private static volatile CngKeyBlobFormat s_genericPrivate;

	private static volatile CngKeyBlobFormat s_genericPublic;

	private static volatile CngKeyBlobFormat s_opaqueTransport;

	private static volatile CngKeyBlobFormat s_pkcs8Private;

	private string m_format;

	public string Format => m_format;

	public static CngKeyBlobFormat EccPrivateBlob
	{
		get
		{
			if (s_eccPrivate == null)
			{
				s_eccPrivate = new CngKeyBlobFormat("ECCPRIVATEBLOB");
			}
			return s_eccPrivate;
		}
	}

	public static CngKeyBlobFormat EccPublicBlob
	{
		get
		{
			if (s_eccPublic == null)
			{
				s_eccPublic = new CngKeyBlobFormat("ECCPUBLICBLOB");
			}
			return s_eccPublic;
		}
	}

	public static CngKeyBlobFormat EccFullPrivateBlob
	{
		get
		{
			if (s_eccFullPrivate == null)
			{
				s_eccFullPrivate = new CngKeyBlobFormat("ECCFULLPRIVATEBLOB");
			}
			return s_eccFullPrivate;
		}
	}

	public static CngKeyBlobFormat EccFullPublicBlob
	{
		get
		{
			if (s_eccFullPublic == null)
			{
				s_eccFullPublic = new CngKeyBlobFormat("ECCFULLPUBLICBLOB");
			}
			return s_eccFullPublic;
		}
	}

	public static CngKeyBlobFormat GenericPrivateBlob
	{
		get
		{
			if (s_genericPrivate == null)
			{
				s_genericPrivate = new CngKeyBlobFormat("PRIVATEBLOB");
			}
			return s_genericPrivate;
		}
	}

	public static CngKeyBlobFormat GenericPublicBlob
	{
		get
		{
			if (s_genericPublic == null)
			{
				s_genericPublic = new CngKeyBlobFormat("PUBLICBLOB");
			}
			return s_genericPublic;
		}
	}

	public static CngKeyBlobFormat OpaqueTransportBlob
	{
		get
		{
			if (s_opaqueTransport == null)
			{
				s_opaqueTransport = new CngKeyBlobFormat("OpaqueTransport");
			}
			return s_opaqueTransport;
		}
	}

	public static CngKeyBlobFormat Pkcs8PrivateBlob
	{
		get
		{
			if (s_pkcs8Private == null)
			{
				s_pkcs8Private = new CngKeyBlobFormat("PKCS8_PRIVATEKEY");
			}
			return s_pkcs8Private;
		}
	}

	public CngKeyBlobFormat(string format)
	{
		if (format == null)
		{
			throw new ArgumentNullException("format");
		}
		if (format.Length == 0)
		{
			throw new ArgumentException(SR.GetString("The key blob format '{0}' is invalid.", format), "format");
		}
		m_format = format;
	}

	public static bool operator ==(CngKeyBlobFormat left, CngKeyBlobFormat right)
	{
		return left?.Equals(right) ?? ((object)right == null);
	}

	public static bool operator !=(CngKeyBlobFormat left, CngKeyBlobFormat right)
	{
		if ((object)left == null)
		{
			return (object)right != null;
		}
		return !left.Equals(right);
	}

	public override bool Equals(object obj)
	{
		return Equals(obj as CngKeyBlobFormat);
	}

	public bool Equals(CngKeyBlobFormat other)
	{
		if ((object)other == null)
		{
			return false;
		}
		return m_format.Equals(other.Format);
	}

	public override int GetHashCode()
	{
		return m_format.GetHashCode();
	}

	public override string ToString()
	{
		return m_format;
	}
}
