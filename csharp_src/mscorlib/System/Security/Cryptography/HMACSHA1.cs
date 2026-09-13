using System.Runtime.InteropServices;

namespace System.Security.Cryptography;

[ComVisible(true)]
public class HMACSHA1 : HMAC
{
	public HMACSHA1()
		: this(Utils.GenerateRandom(64))
	{
	}

	public HMACSHA1(byte[] key)
		: this(key, useManagedSha1: false)
	{
	}

	public HMACSHA1(byte[] key, bool useManagedSha1)
	{
		m_hashName = "SHA1";
		if (useManagedSha1)
		{
			m_hash1 = new SHA1Managed();
			m_hash2 = new SHA1Managed();
		}
		else
		{
			m_hash1 = new SHA1CryptoServiceProvider();
			m_hash2 = new SHA1CryptoServiceProvider();
		}
		HashSizeValue = 160;
		InitializeKey(key);
	}
}
