using System.Runtime.InteropServices;

namespace System.Security.Cryptography;

[ComVisible(true)]
public class DSASignatureFormatter : AsymmetricSignatureFormatter
{
	private DSA _dsaKey;

	private string _oid;

	public DSASignatureFormatter()
	{
		_oid = CryptoConfig.MapNameToOID("SHA1");
	}

	public DSASignatureFormatter(AsymmetricAlgorithm key)
		: this()
	{
		if (key == null)
		{
			throw new ArgumentNullException("key");
		}
		_dsaKey = (DSA)key;
	}

	public override void SetKey(AsymmetricAlgorithm key)
	{
		if (key == null)
		{
			throw new ArgumentNullException("key");
		}
		_dsaKey = (DSA)key;
	}

	public override void SetHashAlgorithm(string strName)
	{
		if (CryptoConfig.MapNameToOID(strName) != _oid)
		{
			throw new CryptographicUnexpectedOperationException(Environment.GetResourceString("This operation is not supported for this class."));
		}
	}

	public override byte[] CreateSignature(byte[] rgbHash)
	{
		if (rgbHash == null)
		{
			throw new ArgumentNullException("rgbHash");
		}
		if (_oid == null)
		{
			throw new CryptographicUnexpectedOperationException(Environment.GetResourceString("Required object identifier (OID) cannot be found."));
		}
		if (_dsaKey == null)
		{
			throw new CryptographicUnexpectedOperationException(Environment.GetResourceString("No asymmetric key object has been associated with this formatter object."));
		}
		return _dsaKey.CreateSignature(rgbHash);
	}
}
