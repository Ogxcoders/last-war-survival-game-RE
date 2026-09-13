using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.IO;

public class SignerSink : BaseOutputStream
{
	private readonly ISigner mSigner;

	public virtual ISigner Signer => mSigner;

	public SignerSink(ISigner signer)
	{
		mSigner = signer;
	}

	public override void WriteByte(byte b)
	{
		mSigner.Update(b);
	}

	public override void Write(byte[] buf, int off, int len)
	{
		if (len > 0)
		{
			mSigner.BlockUpdate(buf, off, len);
		}
	}
}
