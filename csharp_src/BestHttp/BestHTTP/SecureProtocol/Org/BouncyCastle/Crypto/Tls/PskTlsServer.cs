using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Agreement;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls;

public class PskTlsServer : AbstractTlsServer
{
	protected TlsPskIdentityManager mPskIdentityManager;

	public PskTlsServer(TlsPskIdentityManager pskIdentityManager)
		: this(new DefaultTlsCipherFactory(), pskIdentityManager)
	{
	}

	public PskTlsServer(TlsCipherFactory cipherFactory, TlsPskIdentityManager pskIdentityManager)
		: base(cipherFactory)
	{
		mPskIdentityManager = pskIdentityManager;
	}

	protected virtual TlsEncryptionCredentials GetRsaEncryptionCredentials()
	{
		throw new TlsFatalAlert(80);
	}

	protected virtual DHParameters GetDHParameters()
	{
		return DHStandardGroups.rfc7919_ffdhe2048;
	}

	protected override int[] GetCipherSuites()
	{
		return new int[4] { 49207, 49205, 178, 144 };
	}

	public override TlsCredentials GetCredentials()
	{
		switch (TlsUtilities.GetKeyExchangeAlgorithm(mSelectedCipherSuite))
		{
		case 13:
		case 14:
		case 24:
			return null;
		case 15:
			return GetRsaEncryptionCredentials();
		default:
			throw new TlsFatalAlert(80);
		}
	}

	public override TlsKeyExchange GetKeyExchange()
	{
		int keyExchangeAlgorithm = TlsUtilities.GetKeyExchangeAlgorithm(mSelectedCipherSuite);
		if ((uint)(keyExchangeAlgorithm - 13) <= 2u || keyExchangeAlgorithm == 24)
		{
			return CreatePskKeyExchange(keyExchangeAlgorithm);
		}
		throw new TlsFatalAlert(80);
	}

	protected virtual TlsKeyExchange CreatePskKeyExchange(int keyExchange)
	{
		return new TlsPskKeyExchange(keyExchange, mSupportedSignatureAlgorithms, null, mPskIdentityManager, null, GetDHParameters(), mNamedCurves, mClientECPointFormats, mServerECPointFormats);
	}
}
