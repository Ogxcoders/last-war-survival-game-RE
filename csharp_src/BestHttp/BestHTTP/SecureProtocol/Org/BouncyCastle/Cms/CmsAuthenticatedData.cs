using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Cms;

public class CmsAuthenticatedData
{
	internal RecipientInformationStore recipientInfoStore;

	internal ContentInfo contentInfo;

	private AlgorithmIdentifier macAlg;

	private Asn1Set authAttrs;

	private Asn1Set unauthAttrs;

	private byte[] mac;

	public AlgorithmIdentifier MacAlgorithmID => macAlg;

	public string MacAlgOid => macAlg.Algorithm.Id;

	public ContentInfo ContentInfo => contentInfo;

	public CmsAuthenticatedData(byte[] authData)
		: this(CmsUtilities.ReadContentInfo(authData))
	{
	}

	public CmsAuthenticatedData(Stream authData)
		: this(CmsUtilities.ReadContentInfo(authData))
	{
	}

	public CmsAuthenticatedData(ContentInfo contentInfo)
	{
		this.contentInfo = contentInfo;
		AuthenticatedData instance = AuthenticatedData.GetInstance(contentInfo.Content);
		Asn1Set recipientInfos = instance.RecipientInfos;
		macAlg = instance.MacAlgorithm;
		CmsReadable readable = new CmsProcessableByteArray(Asn1OctetString.GetInstance(instance.EncapsulatedContentInfo.Content).GetOctets());
		CmsSecureReadable secureReadable = new CmsEnvelopedHelper.CmsAuthenticatedSecureReadable(macAlg, readable);
		recipientInfoStore = CmsEnvelopedHelper.BuildRecipientInformationStore(recipientInfos, secureReadable);
		authAttrs = instance.AuthAttrs;
		mac = instance.Mac.GetOctets();
		unauthAttrs = instance.UnauthAttrs;
	}

	public byte[] GetMac()
	{
		return Arrays.Clone(mac);
	}

	public RecipientInformationStore GetRecipientInfos()
	{
		return recipientInfoStore;
	}

	public BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.AttributeTable GetAuthAttrs()
	{
		if (authAttrs == null)
		{
			return null;
		}
		return new BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.AttributeTable(authAttrs);
	}

	public BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.AttributeTable GetUnauthAttrs()
	{
		if (unauthAttrs == null)
		{
			return null;
		}
		return new BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.AttributeTable(unauthAttrs);
	}

	public byte[] GetEncoded()
	{
		return contentInfo.GetEncoded();
	}
}
