using System.Runtime.InteropServices;

namespace System.Security.Cryptography;

[ComVisible(true)]
public class RSAOAEPKeyExchangeFormatter : AsymmetricKeyExchangeFormatter
{
	private byte[] ParameterValue;

	private RSA _rsaKey;

	private bool? _rsaOverridesEncrypt;

	private RandomNumberGenerator RngValue;

	public byte[] Parameter
	{
		get
		{
			if (ParameterValue != null)
			{
				return (byte[])ParameterValue.Clone();
			}
			return null;
		}
		set
		{
			if (value != null)
			{
				ParameterValue = (byte[])value.Clone();
			}
			else
			{
				ParameterValue = null;
			}
		}
	}

	public override string Parameters => null;

	public RandomNumberGenerator Rng
	{
		get
		{
			return RngValue;
		}
		set
		{
			RngValue = value;
		}
	}

	private bool OverridesEncrypt
	{
		get
		{
			if (!_rsaOverridesEncrypt.HasValue)
			{
				_rsaOverridesEncrypt = Utils.DoesRsaKeyOverride(_rsaKey, "Encrypt", new Type[2]
				{
					typeof(byte[]),
					typeof(RSAEncryptionPadding)
				});
			}
			return _rsaOverridesEncrypt.Value;
		}
	}

	public RSAOAEPKeyExchangeFormatter()
	{
	}

	public RSAOAEPKeyExchangeFormatter(AsymmetricAlgorithm key)
	{
		if (key == null)
		{
			throw new ArgumentNullException("key");
		}
		_rsaKey = (RSA)key;
	}

	public override void SetKey(AsymmetricAlgorithm key)
	{
		if (key == null)
		{
			throw new ArgumentNullException("key");
		}
		_rsaKey = (RSA)key;
		_rsaOverridesEncrypt = null;
	}

	[SecuritySafeCritical]
	public override byte[] CreateKeyExchange(byte[] rgbData)
	{
		if (_rsaKey == null)
		{
			throw new CryptographicUnexpectedOperationException(Environment.GetResourceString("No asymmetric key object has been associated with this formatter object."));
		}
		if (OverridesEncrypt)
		{
			return _rsaKey.Encrypt(rgbData, RSAEncryptionPadding.OaepSHA1);
		}
		return Utils.RsaOaepEncrypt(_rsaKey, SHA1.Create(), new PKCS1MaskGenerationMethod(), RandomNumberGenerator.Create(), rgbData);
	}

	public override byte[] CreateKeyExchange(byte[] rgbData, Type symAlgType)
	{
		return CreateKeyExchange(rgbData);
	}
}
