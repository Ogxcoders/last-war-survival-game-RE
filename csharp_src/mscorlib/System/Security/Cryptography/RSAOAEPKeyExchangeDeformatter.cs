using System.Runtime.InteropServices;

namespace System.Security.Cryptography;

[ComVisible(true)]
public class RSAOAEPKeyExchangeDeformatter : AsymmetricKeyExchangeDeformatter
{
	private RSA _rsaKey;

	private bool? _rsaOverridesDecrypt;

	public override string Parameters
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	private bool OverridesDecrypt
	{
		get
		{
			if (!_rsaOverridesDecrypt.HasValue)
			{
				_rsaOverridesDecrypt = Utils.DoesRsaKeyOverride(_rsaKey, "Decrypt", new Type[2]
				{
					typeof(byte[]),
					typeof(RSAEncryptionPadding)
				});
			}
			return _rsaOverridesDecrypt.Value;
		}
	}

	public RSAOAEPKeyExchangeDeformatter()
	{
	}

	public RSAOAEPKeyExchangeDeformatter(AsymmetricAlgorithm key)
	{
		if (key == null)
		{
			throw new ArgumentNullException("key");
		}
		_rsaKey = (RSA)key;
	}

	[SecuritySafeCritical]
	public override byte[] DecryptKeyExchange(byte[] rgbData)
	{
		if (_rsaKey == null)
		{
			throw new CryptographicUnexpectedOperationException(Environment.GetResourceString("No asymmetric key object has been associated with this formatter object."));
		}
		if (OverridesDecrypt)
		{
			return _rsaKey.Decrypt(rgbData, RSAEncryptionPadding.OaepSHA1);
		}
		return Utils.RsaOaepDecrypt(_rsaKey, SHA1.Create(), new PKCS1MaskGenerationMethod(), rgbData);
	}

	public override void SetKey(AsymmetricAlgorithm key)
	{
		if (key == null)
		{
			throw new ArgumentNullException("key");
		}
		_rsaKey = (RSA)key;
		_rsaOverridesDecrypt = null;
	}
}
