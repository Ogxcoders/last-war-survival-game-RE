using System.Runtime.InteropServices;

namespace System.Security.Cryptography;

[ComVisible(true)]
public class HMACSHA384 : HMAC
{
	private bool m_useLegacyBlockSize = Utils._ProduceLegacyHmacValues();

	private int BlockSize
	{
		get
		{
			if (!m_useLegacyBlockSize)
			{
				return 128;
			}
			return 64;
		}
	}

	public bool ProduceLegacyHmacValues
	{
		get
		{
			return m_useLegacyBlockSize;
		}
		set
		{
			m_useLegacyBlockSize = value;
			base.BlockSizeValue = BlockSize;
			InitializeKey(KeyValue);
		}
	}

	public HMACSHA384()
		: this(Utils.GenerateRandom(128))
	{
	}

	[SecuritySafeCritical]
	public HMACSHA384(byte[] key)
	{
		m_hashName = "SHA384";
		m_hash1 = HMAC.GetHashAlgorithmWithFipsFallback(() => new SHA384Managed(), () => HashAlgorithm.Create("System.Security.Cryptography.SHA384CryptoServiceProvider"));
		m_hash2 = HMAC.GetHashAlgorithmWithFipsFallback(() => new SHA384Managed(), () => HashAlgorithm.Create("System.Security.Cryptography.SHA384CryptoServiceProvider"));
		HashSizeValue = 384;
		base.BlockSizeValue = BlockSize;
		InitializeKey(key);
	}
}
