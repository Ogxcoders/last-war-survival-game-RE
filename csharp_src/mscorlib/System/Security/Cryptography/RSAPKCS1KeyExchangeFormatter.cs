using System.Runtime.InteropServices;

namespace System.Security.Cryptography;

[ComVisible(true)]
public class RSAPKCS1KeyExchangeFormatter : AsymmetricKeyExchangeFormatter
{
	private RandomNumberGenerator RngValue;

	private RSA _rsaKey;

	private bool? _rsaOverridesEncrypt;

	public override string Parameters => "<enc:KeyEncryptionMethod enc:Algorithm=\"http://www.microsoft.com/xml/security/algorithm/PKCS1-v1.5-KeyEx\" xmlns:enc=\"http://www.microsoft.com/xml/security/encryption/v1.0\" />";

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

	public RSAPKCS1KeyExchangeFormatter()
	{
	}

	public RSAPKCS1KeyExchangeFormatter(AsymmetricAlgorithm key)
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

	public override byte[] CreateKeyExchange(byte[] rgbData)
	{
		if (rgbData == null)
		{
			throw new ArgumentNullException("rgbData");
		}
		if (_rsaKey == null)
		{
			throw new CryptographicUnexpectedOperationException(Environment.GetResourceString("No asymmetric key object has been associated with this formatter object."));
		}
		if (OverridesEncrypt)
		{
			return _rsaKey.Encrypt(rgbData, RSAEncryptionPadding.Pkcs1);
		}
		int num = _rsaKey.KeySize / 8;
		if (rgbData.Length + 11 > num)
		{
			throw new CryptographicException(Environment.GetResourceString("The data to be encrypted exceeds the maximum for this modulus of {0} bytes.", num - 11));
		}
		byte[] array = new byte[num];
		if (RngValue == null)
		{
			RngValue = RandomNumberGenerator.Create();
		}
		Rng.GetNonZeroBytes(array);
		array[0] = 0;
		array[1] = 2;
		array[num - rgbData.Length - 1] = 0;
		Buffer.InternalBlockCopy(rgbData, 0, array, num - rgbData.Length, rgbData.Length);
		return _rsaKey.EncryptValue(array);
	}

	public override byte[] CreateKeyExchange(byte[] rgbData, Type symAlgType)
	{
		return CreateKeyExchange(rgbData);
	}
}
