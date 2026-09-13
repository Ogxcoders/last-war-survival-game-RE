using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Cms;

public class CmsAuthenticatedDataParser : CmsContentInfoParser
{
	internal RecipientInformationStore _recipientInfoStore;

	internal AuthenticatedDataParser authData;

	private AlgorithmIdentifier macAlg;

	private byte[] mac;

	private BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.AttributeTable authAttrs;

	private BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.AttributeTable unauthAttrs;

	private bool authAttrNotRead;

	private bool unauthAttrNotRead;

	public AlgorithmIdentifier MacAlgorithmID => macAlg;

	public string MacAlgOid => macAlg.Algorithm.Id;

	public Asn1Object MacAlgParams => macAlg.Parameters?.ToAsn1Object();

	public CmsAuthenticatedDataParser(byte[] envelopedData)
		: this(new MemoryStream(envelopedData, writable: false))
	{
	}

	public CmsAuthenticatedDataParser(Stream envelopedData)
		: base(envelopedData)
	{
		authAttrNotRead = true;
		authData = new AuthenticatedDataParser((Asn1SequenceParser)contentInfo.GetContent(16));
		Asn1Set instance = Asn1Set.GetInstance(authData.GetRecipientInfos().ToAsn1Object());
		macAlg = authData.GetMacAlgorithm();
		CmsReadable readable = new CmsProcessableInputStream(((Asn1OctetStringParser)authData.GetEnapsulatedContentInfo().GetContent(4)).GetOctetStream());
		CmsSecureReadable secureReadable = new CmsEnvelopedHelper.CmsAuthenticatedSecureReadable(macAlg, readable);
		_recipientInfoStore = CmsEnvelopedHelper.BuildRecipientInformationStore(instance, secureReadable);
	}

	public RecipientInformationStore GetRecipientInfos()
	{
		return _recipientInfoStore;
	}

	public byte[] GetMac()
	{
		if (mac == null)
		{
			GetAuthAttrs();
			mac = authData.GetMac().GetOctets();
		}
		return Arrays.Clone(mac);
	}

	public BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.AttributeTable GetAuthAttrs()
	{
		if (authAttrs == null && authAttrNotRead)
		{
			Asn1SetParser asn1SetParser = authData.GetAuthAttrs();
			authAttrNotRead = false;
			if (asn1SetParser != null)
			{
				Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector();
				IAsn1Convertible asn1Convertible;
				while ((asn1Convertible = asn1SetParser.ReadObject()) != null)
				{
					Asn1SequenceParser asn1SequenceParser = (Asn1SequenceParser)asn1Convertible;
					asn1EncodableVector.Add(asn1SequenceParser.ToAsn1Object());
				}
				authAttrs = new BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.AttributeTable(new DerSet(asn1EncodableVector));
			}
		}
		return authAttrs;
	}

	public BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.AttributeTable GetUnauthAttrs()
	{
		if (unauthAttrs == null && unauthAttrNotRead)
		{
			Asn1SetParser asn1SetParser = authData.GetUnauthAttrs();
			unauthAttrNotRead = false;
			if (asn1SetParser != null)
			{
				Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector();
				IAsn1Convertible asn1Convertible;
				while ((asn1Convertible = asn1SetParser.ReadObject()) != null)
				{
					Asn1SequenceParser asn1SequenceParser = (Asn1SequenceParser)asn1Convertible;
					asn1EncodableVector.Add(asn1SequenceParser.ToAsn1Object());
				}
				unauthAttrs = new BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.AttributeTable(new DerSet(asn1EncodableVector));
			}
		}
		return unauthAttrs;
	}
}
