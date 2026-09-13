using System.Runtime.InteropServices;
using Mono.Security.Cryptography;

namespace System.Security.Cryptography;

[ComVisible(true)]
public class PKCS1MaskGenerationMethod : MaskGenerationMethod
{
	private string HashNameValue;

	public string HashName
	{
		get
		{
			return HashNameValue;
		}
		set
		{
			HashNameValue = value;
			if (HashNameValue == null)
			{
				HashNameValue = "SHA1";
			}
		}
	}

	public PKCS1MaskGenerationMethod()
	{
		HashNameValue = "SHA1";
	}

	public override byte[] GenerateMask(byte[] rgbSeed, int cbReturn)
	{
		return PKCS1.MGF1(HashAlgorithm.Create(HashNameValue), rgbSeed, cbReturn);
	}
}
