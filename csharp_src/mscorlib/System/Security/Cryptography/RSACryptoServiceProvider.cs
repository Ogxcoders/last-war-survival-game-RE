using System.IO;
using System.Runtime.InteropServices;
using Mono.Security.Cryptography;

namespace System.Security.Cryptography;

[ComVisible(true)]
public sealed class RSACryptoServiceProvider : RSA, ICspAsymmetricAlgorithm
{
	private static volatile CspProviderFlags s_UseMachineKeyStore;

	private const int PROV_RSA_FULL = 1;

	private const int AT_KEYEXCHANGE = 1;

	private const int AT_SIGNATURE = 2;

	private KeyPairPersistence store;

	private bool persistKey;

	private bool persisted;

	private bool privateKeyExportable = true;

	private bool m_disposed;

	private RSAManaged rsa;

	public override string SignatureAlgorithm => "http://www.w3.org/2000/09/xmldsig#rsa-sha1";

	public static bool UseMachineKeyStore
	{
		get
		{
			return s_UseMachineKeyStore == CspProviderFlags.UseMachineKeyStore;
		}
		set
		{
			s_UseMachineKeyStore = (value ? CspProviderFlags.UseMachineKeyStore : CspProviderFlags.NoFlags);
		}
	}

	public override string KeyExchangeAlgorithm => "RSA-PKCS1-KeyEx";

	public override int KeySize
	{
		get
		{
			if (rsa == null)
			{
				return KeySizeValue;
			}
			return rsa.KeySize;
		}
	}

	public bool PersistKeyInCsp
	{
		get
		{
			return persistKey;
		}
		set
		{
			persistKey = value;
			if (persistKey)
			{
				OnKeyGenerated(rsa, null);
			}
		}
	}

	[ComVisible(false)]
	public bool PublicOnly => rsa.PublicOnly;

	[ComVisible(false)]
	public CspKeyContainerInfo CspKeyContainerInfo => new CspKeyContainerInfo(store.Parameters);

	[SecuritySafeCritical]
	protected override byte[] HashData(byte[] data, int offset, int count, HashAlgorithmName hashAlgorithm)
	{
		return HashAlgorithm.Create(hashAlgorithm.Name).ComputeHash(data, offset, count);
	}

	[SecuritySafeCritical]
	protected override byte[] HashData(Stream data, HashAlgorithmName hashAlgorithm)
	{
		return HashAlgorithm.Create(hashAlgorithm.Name).ComputeHash(data);
	}

	private static int GetAlgorithmId(HashAlgorithmName hashAlgorithm)
	{
		return hashAlgorithm.Name switch
		{
			"MD5" => 32771, 
			"SHA1" => 32772, 
			"SHA256" => 32780, 
			"SHA384" => 32781, 
			"SHA512" => 32782, 
			_ => throw new CryptographicException(Environment.GetResourceString("'{0}' is not a known hash algorithm.", hashAlgorithm.Name)), 
		};
	}

	public override byte[] Encrypt(byte[] data, RSAEncryptionPadding padding)
	{
		if (data == null)
		{
			throw new ArgumentNullException("data");
		}
		if (padding == null)
		{
			throw new ArgumentNullException("padding");
		}
		if (padding == RSAEncryptionPadding.Pkcs1)
		{
			return Encrypt(data, fOAEP: false);
		}
		if (padding == RSAEncryptionPadding.OaepSHA1)
		{
			return Encrypt(data, fOAEP: true);
		}
		throw PaddingModeNotSupported();
	}

	public override byte[] Decrypt(byte[] data, RSAEncryptionPadding padding)
	{
		if (data == null)
		{
			throw new ArgumentNullException("data");
		}
		if (padding == null)
		{
			throw new ArgumentNullException("padding");
		}
		if (padding == RSAEncryptionPadding.Pkcs1)
		{
			return Decrypt(data, fOAEP: false);
		}
		if (padding == RSAEncryptionPadding.OaepSHA1)
		{
			return Decrypt(data, fOAEP: true);
		}
		throw PaddingModeNotSupported();
	}

	public override byte[] SignHash(byte[] hash, HashAlgorithmName hashAlgorithm, RSASignaturePadding padding)
	{
		if (hash == null)
		{
			throw new ArgumentNullException("hash");
		}
		if (string.IsNullOrEmpty(hashAlgorithm.Name))
		{
			throw RSA.HashAlgorithmNameNullOrEmpty();
		}
		if (padding == null)
		{
			throw new ArgumentNullException("padding");
		}
		if (padding != RSASignaturePadding.Pkcs1)
		{
			throw PaddingModeNotSupported();
		}
		return SignHash(hash, GetAlgorithmId(hashAlgorithm));
	}

	public override bool VerifyHash(byte[] hash, byte[] signature, HashAlgorithmName hashAlgorithm, RSASignaturePadding padding)
	{
		if (hash == null)
		{
			throw new ArgumentNullException("hash");
		}
		if (signature == null)
		{
			throw new ArgumentNullException("signature");
		}
		if (string.IsNullOrEmpty(hashAlgorithm.Name))
		{
			throw RSA.HashAlgorithmNameNullOrEmpty();
		}
		if (padding == null)
		{
			throw new ArgumentNullException("padding");
		}
		if (padding != RSASignaturePadding.Pkcs1)
		{
			throw PaddingModeNotSupported();
		}
		return VerifyHash(hash, GetAlgorithmId(hashAlgorithm), signature);
	}

	private static Exception PaddingModeNotSupported()
	{
		return new CryptographicException(Environment.GetResourceString("Specified padding mode is not valid for this algorithm."));
	}

	public RSACryptoServiceProvider()
		: this(1024)
	{
	}

	public RSACryptoServiceProvider(CspParameters parameters)
		: this(1024, parameters)
	{
	}

	public RSACryptoServiceProvider(int dwKeySize)
	{
		Common(dwKeySize, parameters: false);
	}

	public RSACryptoServiceProvider(int dwKeySize, CspParameters parameters)
	{
		bool flag = parameters != null;
		Common(dwKeySize, flag);
		if (flag)
		{
			Common(parameters);
		}
	}

	private void Common(int dwKeySize, bool parameters)
	{
		LegalKeySizesValue = new KeySizes[1];
		LegalKeySizesValue[0] = new KeySizes(384, 16384, 8);
		base.KeySize = dwKeySize;
		rsa = new RSAManaged(KeySize);
		rsa.KeyGenerated += OnKeyGenerated;
		persistKey = parameters;
		if (!parameters)
		{
			CspParameters cspParameters = new CspParameters(1);
			if (UseMachineKeyStore)
			{
				cspParameters.Flags |= CspProviderFlags.UseMachineKeyStore;
			}
			store = new KeyPairPersistence(cspParameters);
		}
	}

	private void Common(CspParameters p)
	{
		store = new KeyPairPersistence(p);
		bool flag = store.Load();
		bool num = (p.Flags & CspProviderFlags.UseExistingKey) != 0;
		privateKeyExportable = (p.Flags & CspProviderFlags.UseNonExportableKey) == 0;
		if (num && !flag)
		{
			throw new CryptographicException("Keyset does not exist");
		}
		if (store.KeyValue != null)
		{
			persisted = true;
			FromXmlString(store.KeyValue);
		}
	}

	~RSACryptoServiceProvider()
	{
		Dispose(disposing: false);
	}

	public byte[] Decrypt(byte[] rgb, bool fOAEP)
	{
		if (rgb == null)
		{
			throw new ArgumentNullException("rgb");
		}
		if (rgb.Length > KeySize / 8)
		{
			throw new CryptographicException(Environment.GetResourceString("The data to be decrypted exceeds the maximum for this modulus of {0} bytes.", KeySize / 8));
		}
		if (m_disposed)
		{
			throw new ObjectDisposedException("rsa");
		}
		AsymmetricKeyExchangeDeformatter asymmetricKeyExchangeDeformatter = null;
		asymmetricKeyExchangeDeformatter = ((!fOAEP) ? ((AsymmetricKeyExchangeDeformatter)new RSAPKCS1KeyExchangeDeformatter(rsa)) : ((AsymmetricKeyExchangeDeformatter)new RSAOAEPKeyExchangeDeformatter(rsa)));
		return asymmetricKeyExchangeDeformatter.DecryptKeyExchange(rgb);
	}

	public override byte[] DecryptValue(byte[] rgb)
	{
		if (!rsa.IsCrtPossible)
		{
			throw new CryptographicException("Incomplete private key - missing CRT.");
		}
		return rsa.DecryptValue(rgb);
	}

	public byte[] Encrypt(byte[] rgb, bool fOAEP)
	{
		AsymmetricKeyExchangeFormatter asymmetricKeyExchangeFormatter = null;
		asymmetricKeyExchangeFormatter = ((!fOAEP) ? ((AsymmetricKeyExchangeFormatter)new RSAPKCS1KeyExchangeFormatter(rsa)) : ((AsymmetricKeyExchangeFormatter)new RSAOAEPKeyExchangeFormatter(rsa)));
		return asymmetricKeyExchangeFormatter.CreateKeyExchange(rgb);
	}

	public override byte[] EncryptValue(byte[] rgb)
	{
		return rsa.EncryptValue(rgb);
	}

	public override RSAParameters ExportParameters(bool includePrivateParameters)
	{
		if (includePrivateParameters && !privateKeyExportable)
		{
			throw new CryptographicException("cannot export private key");
		}
		RSAParameters result = rsa.ExportParameters(includePrivateParameters);
		if (includePrivateParameters)
		{
			if (result.D == null)
			{
				throw new ArgumentNullException("Missing D parameter for the private key.");
			}
			if (result.P == null || result.Q == null || result.DP == null || result.DQ == null || result.InverseQ == null)
			{
				throw new CryptographicException("Missing some CRT parameters for the private key.");
			}
		}
		return result;
	}

	public override void ImportParameters(RSAParameters parameters)
	{
		rsa.ImportParameters(parameters);
	}

	private HashAlgorithm GetHash(object halg)
	{
		if (halg == null)
		{
			throw new ArgumentNullException("halg");
		}
		HashAlgorithm hashAlgorithm = null;
		if (halg is string)
		{
			hashAlgorithm = GetHashFromString((string)halg);
		}
		else if (halg is HashAlgorithm)
		{
			hashAlgorithm = (HashAlgorithm)halg;
		}
		else
		{
			if (!(halg is Type))
			{
				throw new ArgumentException("halg");
			}
			hashAlgorithm = (HashAlgorithm)Activator.CreateInstance((Type)halg);
		}
		if (hashAlgorithm == null)
		{
			throw new ArgumentException("Could not find provider for halg='" + halg?.ToString() + "'.", "halg");
		}
		return hashAlgorithm;
	}

	private HashAlgorithm GetHashFromString(string name)
	{
		HashAlgorithm hashAlgorithm = HashAlgorithm.Create(name);
		if (hashAlgorithm != null)
		{
			return hashAlgorithm;
		}
		try
		{
			return HashAlgorithm.Create(GetHashNameFromOID(name));
		}
		catch (CryptographicException ex)
		{
			throw new ArgumentException(ex.Message, "halg", ex);
		}
	}

	public byte[] SignData(byte[] buffer, object halg)
	{
		if (buffer == null)
		{
			throw new ArgumentNullException("buffer");
		}
		return SignData(buffer, 0, buffer.Length, halg);
	}

	public byte[] SignData(Stream inputStream, object halg)
	{
		HashAlgorithm hash = GetHash(halg);
		byte[] hashValue = hash.ComputeHash(inputStream);
		return PKCS1.Sign_v15(this, hash, hashValue);
	}

	public byte[] SignData(byte[] buffer, int offset, int count, object halg)
	{
		HashAlgorithm hash = GetHash(halg);
		byte[] hashValue = hash.ComputeHash(buffer, offset, count);
		return PKCS1.Sign_v15(this, hash, hashValue);
	}

	private string GetHashNameFromOID(string oid)
	{
		return oid switch
		{
			"1.3.14.3.2.26" => "SHA1", 
			"1.2.840.113549.2.5" => "MD5", 
			"2.16.840.1.101.3.4.2.1" => "SHA256", 
			"2.16.840.1.101.3.4.2.2" => "SHA384", 
			"2.16.840.1.101.3.4.2.3" => "SHA512", 
			_ => throw new CryptographicException(oid + " is an unsupported hash algorithm for RSA signing"), 
		};
	}

	public byte[] SignHash(byte[] rgbHash, string str)
	{
		if (rgbHash == null)
		{
			throw new ArgumentNullException("rgbHash");
		}
		HashAlgorithm hash = HashAlgorithm.Create((str == null) ? "SHA1" : GetHashNameFromOID(str));
		return PKCS1.Sign_v15(this, hash, rgbHash);
	}

	private byte[] SignHash(byte[] rgbHash, int calgHash)
	{
		return PKCS1.Sign_v15(this, InternalHashToHashAlgorithm(calgHash), rgbHash);
	}

	private static HashAlgorithm InternalHashToHashAlgorithm(int calgHash)
	{
		return calgHash switch
		{
			32771 => MD5.Create(), 
			32772 => SHA1.Create(), 
			32780 => SHA256.Create(), 
			32781 => SHA384.Create(), 
			32782 => SHA512.Create(), 
			_ => throw new NotImplementedException(calgHash.ToString()), 
		};
	}

	public bool VerifyData(byte[] buffer, object halg, byte[] signature)
	{
		if (buffer == null)
		{
			throw new ArgumentNullException("buffer");
		}
		if (signature == null)
		{
			throw new ArgumentNullException("signature");
		}
		HashAlgorithm hash = GetHash(halg);
		byte[] hashValue = hash.ComputeHash(buffer);
		return PKCS1.Verify_v15(this, hash, hashValue, signature);
	}

	public bool VerifyHash(byte[] rgbHash, string str, byte[] rgbSignature)
	{
		if (rgbHash == null)
		{
			throw new ArgumentNullException("rgbHash");
		}
		if (rgbSignature == null)
		{
			throw new ArgumentNullException("rgbSignature");
		}
		HashAlgorithm hash = HashAlgorithm.Create((str == null) ? "SHA1" : GetHashNameFromOID(str));
		return PKCS1.Verify_v15(this, hash, rgbHash, rgbSignature);
	}

	private bool VerifyHash(byte[] rgbHash, int calgHash, byte[] rgbSignature)
	{
		return PKCS1.Verify_v15(this, InternalHashToHashAlgorithm(calgHash), rgbHash, rgbSignature);
	}

	protected override void Dispose(bool disposing)
	{
		if (!m_disposed)
		{
			if (persisted && !persistKey)
			{
				store.Remove();
			}
			if (rsa != null)
			{
				rsa.Clear();
			}
			m_disposed = true;
		}
	}

	private void OnKeyGenerated(object sender, EventArgs e)
	{
		if (persistKey && !persisted)
		{
			store.KeyValue = ToXmlString(!rsa.PublicOnly);
			store.Save();
			persisted = true;
		}
	}

	[ComVisible(false)]
	public byte[] ExportCspBlob(bool includePrivateParameters)
	{
		byte[] array = null;
		array = ((!includePrivateParameters) ? CryptoConvert.ToCapiPublicKeyBlob(this) : CryptoConvert.ToCapiPrivateKeyBlob(this));
		array[5] = (byte)((store != null && store.Parameters.KeyNumber == 2) ? 36u : 164u);
		return array;
	}

	[ComVisible(false)]
	public void ImportCspBlob(byte[] keyBlob)
	{
		if (keyBlob == null)
		{
			throw new ArgumentNullException("keyBlob");
		}
		RSA rSA = CryptoConvert.FromCapiKeyBlob(keyBlob);
		if (rSA is RSACryptoServiceProvider)
		{
			RSAParameters parameters = rSA.ExportParameters(!(rSA as RSACryptoServiceProvider).PublicOnly);
			ImportParameters(parameters);
		}
		else
		{
			try
			{
				RSAParameters parameters2 = rSA.ExportParameters(includePrivateParameters: true);
				ImportParameters(parameters2);
			}
			catch
			{
				RSAParameters parameters3 = rSA.ExportParameters(includePrivateParameters: false);
				ImportParameters(parameters3);
			}
		}
		CspParameters cspParameters = new CspParameters(1);
		cspParameters.KeyNumber = ((keyBlob[5] != 36) ? 1 : 2);
		if (UseMachineKeyStore)
		{
			cspParameters.Flags |= CspProviderFlags.UseMachineKeyStore;
		}
		store = new KeyPairPersistence(cspParameters);
	}
}
