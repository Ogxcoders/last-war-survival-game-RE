using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using Mono.Security.Cryptography;

namespace System.Security.Cryptography;

[ComVisible(true)]
public sealed class DSACryptoServiceProvider : DSA, ICspAsymmetricAlgorithm
{
	private const int PROV_DSS_DH = 13;

	private KeyPairPersistence store;

	private bool persistKey;

	private bool persisted;

	private bool privateKeyExportable = true;

	private bool m_disposed;

	private DSAManaged dsa;

	private static bool useMachineKeyStore;

	public override string KeyExchangeAlgorithm => null;

	public override int KeySize => dsa.KeySize;

	public bool PersistKeyInCsp
	{
		get
		{
			return persistKey;
		}
		set
		{
			persistKey = value;
		}
	}

	[ComVisible(false)]
	public bool PublicOnly => dsa.PublicOnly;

	public override string SignatureAlgorithm => "http://www.w3.org/2000/09/xmldsig#dsa-sha1";

	public static bool UseMachineKeyStore
	{
		get
		{
			return useMachineKeyStore;
		}
		set
		{
			useMachineKeyStore = value;
		}
	}

	[MonoTODO("call into KeyPairPersistence to get details")]
	[ComVisible(false)]
	public CspKeyContainerInfo CspKeyContainerInfo => null;

	public DSACryptoServiceProvider()
		: this(1024)
	{
	}

	public DSACryptoServiceProvider(CspParameters parameters)
		: this(1024, parameters)
	{
	}

	public DSACryptoServiceProvider(int dwKeySize)
	{
		Common(dwKeySize, parameters: false);
	}

	public DSACryptoServiceProvider(int dwKeySize, CspParameters parameters)
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
		LegalKeySizesValue[0] = new KeySizes(512, 1024, 64);
		KeySize = dwKeySize;
		dsa = new DSAManaged(dwKeySize);
		dsa.KeyGenerated += OnKeyGenerated;
		persistKey = parameters;
		if (!parameters)
		{
			CspParameters cspParameters = new CspParameters(13);
			if (useMachineKeyStore)
			{
				cspParameters.Flags |= CspProviderFlags.UseMachineKeyStore;
			}
			store = new KeyPairPersistence(cspParameters);
		}
	}

	private void Common(CspParameters parameters)
	{
		store = new KeyPairPersistence(parameters);
		store.Load();
		if (store.KeyValue != null)
		{
			persisted = true;
			FromXmlString(store.KeyValue);
		}
		privateKeyExportable = (parameters.Flags & CspProviderFlags.UseNonExportableKey) == 0;
	}

	~DSACryptoServiceProvider()
	{
		Dispose(disposing: false);
	}

	public override DSAParameters ExportParameters(bool includePrivateParameters)
	{
		if (includePrivateParameters && !privateKeyExportable)
		{
			throw new CryptographicException(Locale.GetText("Cannot export private key"));
		}
		return dsa.ExportParameters(includePrivateParameters);
	}

	public override void ImportParameters(DSAParameters parameters)
	{
		dsa.ImportParameters(parameters);
	}

	public override byte[] CreateSignature(byte[] rgbHash)
	{
		return dsa.CreateSignature(rgbHash);
	}

	public byte[] SignData(byte[] buffer)
	{
		byte[] rgbHash = SHA1.Create().ComputeHash(buffer);
		return dsa.CreateSignature(rgbHash);
	}

	public byte[] SignData(byte[] buffer, int offset, int count)
	{
		byte[] rgbHash = SHA1.Create().ComputeHash(buffer, offset, count);
		return dsa.CreateSignature(rgbHash);
	}

	public byte[] SignData(Stream inputStream)
	{
		byte[] rgbHash = SHA1.Create().ComputeHash(inputStream);
		return dsa.CreateSignature(rgbHash);
	}

	public byte[] SignHash(byte[] rgbHash, string str)
	{
		if (string.Compare(str, "SHA1", ignoreCase: true, CultureInfo.InvariantCulture) != 0)
		{
			throw new CryptographicException(Locale.GetText("Only SHA1 is supported."));
		}
		return dsa.CreateSignature(rgbHash);
	}

	public bool VerifyData(byte[] rgbData, byte[] rgbSignature)
	{
		byte[] rgbHash = SHA1.Create().ComputeHash(rgbData);
		return dsa.VerifySignature(rgbHash, rgbSignature);
	}

	public bool VerifyHash(byte[] rgbHash, string str, byte[] rgbSignature)
	{
		if (str == null)
		{
			str = "SHA1";
		}
		if (string.Compare(str, "SHA1", ignoreCase: true, CultureInfo.InvariantCulture) != 0)
		{
			throw new CryptographicException(Locale.GetText("Only SHA1 is supported."));
		}
		return dsa.VerifySignature(rgbHash, rgbSignature);
	}

	public override bool VerifySignature(byte[] rgbHash, byte[] rgbSignature)
	{
		return dsa.VerifySignature(rgbHash, rgbSignature);
	}

	protected override byte[] HashData(byte[] data, int offset, int count, HashAlgorithmName hashAlgorithm)
	{
		if (hashAlgorithm != HashAlgorithmName.SHA1)
		{
			throw new CryptographicException(Environment.GetResourceString("'{0}' is not a known hash algorithm.", hashAlgorithm.Name));
		}
		return HashAlgorithm.Create(hashAlgorithm.Name).ComputeHash(data, offset, count);
	}

	protected override byte[] HashData(Stream data, HashAlgorithmName hashAlgorithm)
	{
		if (hashAlgorithm != HashAlgorithmName.SHA1)
		{
			throw new CryptographicException(Environment.GetResourceString("'{0}' is not a known hash algorithm.", hashAlgorithm.Name));
		}
		return HashAlgorithm.Create(hashAlgorithm.Name).ComputeHash(data);
	}

	protected override void Dispose(bool disposing)
	{
		if (!m_disposed)
		{
			if (persisted && !persistKey)
			{
				store.Remove();
			}
			if (dsa != null)
			{
				dsa.Clear();
			}
			m_disposed = true;
		}
	}

	private void OnKeyGenerated(object sender, EventArgs e)
	{
		if (persistKey && !persisted)
		{
			store.KeyValue = ToXmlString(!dsa.PublicOnly);
			store.Save();
			persisted = true;
		}
	}

	[ComVisible(false)]
	public byte[] ExportCspBlob(bool includePrivateParameters)
	{
		byte[] array = null;
		if (includePrivateParameters)
		{
			return CryptoConvert.ToCapiPrivateKeyBlob(this);
		}
		return CryptoConvert.ToCapiPublicKeyBlob(this);
	}

	[ComVisible(false)]
	public void ImportCspBlob(byte[] keyBlob)
	{
		if (keyBlob == null)
		{
			throw new ArgumentNullException("keyBlob");
		}
		DSA dSA = CryptoConvert.FromCapiKeyBlobDSA(keyBlob);
		if (dSA is DSACryptoServiceProvider)
		{
			DSAParameters parameters = dSA.ExportParameters(!(dSA as DSACryptoServiceProvider).PublicOnly);
			ImportParameters(parameters);
			return;
		}
		try
		{
			DSAParameters parameters2 = dSA.ExportParameters(includePrivateParameters: true);
			ImportParameters(parameters2);
		}
		catch
		{
			DSAParameters parameters3 = dSA.ExportParameters(includePrivateParameters: false);
			ImportParameters(parameters3);
		}
	}
}
