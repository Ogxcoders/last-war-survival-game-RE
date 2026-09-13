using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Operators;

public class DefaultVerifierCalculator : IStreamCalculator
{
	private readonly SignerSink mSignerSink;

	public Stream Stream => mSignerSink;

	public DefaultVerifierCalculator(ISigner signer)
	{
		mSignerSink = new SignerSink(signer);
	}

	public object GetResult()
	{
		return new DefaultVerifierResult(mSignerSink.Signer);
	}
}
