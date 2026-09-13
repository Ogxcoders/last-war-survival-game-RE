using System.Runtime.InteropServices;

namespace System.Security.Cryptography;

[ComVisible(true)]
public class SignatureDescription
{
	private string _strKey;

	private string _strDigest;

	private string _strFormatter;

	private string _strDeformatter;

	public string KeyAlgorithm
	{
		get
		{
			return _strKey;
		}
		set
		{
			_strKey = value;
		}
	}

	public string DigestAlgorithm
	{
		get
		{
			return _strDigest;
		}
		set
		{
			_strDigest = value;
		}
	}

	public string FormatterAlgorithm
	{
		get
		{
			return _strFormatter;
		}
		set
		{
			_strFormatter = value;
		}
	}

	public string DeformatterAlgorithm
	{
		get
		{
			return _strDeformatter;
		}
		set
		{
			_strDeformatter = value;
		}
	}

	public SignatureDescription()
	{
	}

	public SignatureDescription(SecurityElement el)
	{
		if (el == null)
		{
			throw new ArgumentNullException("el");
		}
		_strKey = el.SearchForTextOfTag("Key");
		_strDigest = el.SearchForTextOfTag("Digest");
		_strFormatter = el.SearchForTextOfTag("Formatter");
		_strDeformatter = el.SearchForTextOfTag("Deformatter");
	}

	public virtual AsymmetricSignatureDeformatter CreateDeformatter(AsymmetricAlgorithm key)
	{
		AsymmetricSignatureDeformatter obj = (AsymmetricSignatureDeformatter)CryptoConfig.CreateFromName(_strDeformatter);
		obj.SetKey(key);
		return obj;
	}

	public virtual AsymmetricSignatureFormatter CreateFormatter(AsymmetricAlgorithm key)
	{
		AsymmetricSignatureFormatter obj = (AsymmetricSignatureFormatter)CryptoConfig.CreateFromName(_strFormatter);
		obj.SetKey(key);
		return obj;
	}

	public virtual HashAlgorithm CreateDigest()
	{
		return (HashAlgorithm)CryptoConfig.CreateFromName(_strDigest);
	}
}
