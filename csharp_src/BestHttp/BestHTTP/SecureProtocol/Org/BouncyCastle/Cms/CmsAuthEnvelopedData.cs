using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Cms;

internal class CmsAuthEnvelopedData
{
	private class AuthEnvelopedSecureReadable : CmsSecureReadable
	{
		private readonly CmsAuthEnvelopedData parent;

		public AlgorithmIdentifier Algorithm => parent.authEncAlg;

		public object CryptoObject => null;

		internal AuthEnvelopedSecureReadable(CmsAuthEnvelopedData parent)
		{
			this.parent = parent;
		}

		public CmsReadable GetReadable(KeyParameter key)
		{
			throw new CmsException("AuthEnveloped data decryption not yet implemented");
		}
	}

	internal RecipientInformationStore recipientInfoStore;

	internal ContentInfo contentInfo;

	private OriginatorInfo originator;

	private AlgorithmIdentifier authEncAlg;

	private Asn1Set authAttrs;

	private byte[] mac;

	private Asn1Set unauthAttrs;

	public CmsAuthEnvelopedData(byte[] authEnvData)
		: this(CmsUtilities.ReadContentInfo(authEnvData))
	{
	}

	public CmsAuthEnvelopedData(Stream authEnvData)
		: this(CmsUtilities.ReadContentInfo(authEnvData))
	{
	}

	public CmsAuthEnvelopedData(ContentInfo contentInfo)
	{
		this.contentInfo = contentInfo;
		AuthEnvelopedData instance = AuthEnvelopedData.GetInstance(contentInfo.Content);
		originator = instance.OriginatorInfo;
		Asn1Set recipientInfos = instance.RecipientInfos;
		EncryptedContentInfo authEncryptedContentInfo = instance.AuthEncryptedContentInfo;
		authEncAlg = authEncryptedContentInfo.ContentEncryptionAlgorithm;
		CmsSecureReadable secureReadable = new AuthEnvelopedSecureReadable(this);
		recipientInfoStore = CmsEnvelopedHelper.BuildRecipientInformationStore(recipientInfos, secureReadable);
		authAttrs = instance.AuthAttrs;
		mac = instance.Mac.GetOctets();
		unauthAttrs = instance.UnauthAttrs;
	}
}
