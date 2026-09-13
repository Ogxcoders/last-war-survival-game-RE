using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.X509.Store;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Cms;

public class OriginatorInfoGenerator
{
	private readonly IList origCerts;

	private readonly IList origCrls;

	public OriginatorInfoGenerator(X509Certificate origCert)
	{
		origCerts = Platform.CreateArrayList(1);
		origCrls = null;
		origCerts.Add(origCert.CertificateStructure);
	}

	public OriginatorInfoGenerator(IX509Store origCerts)
		: this(origCerts, null)
	{
	}

	public OriginatorInfoGenerator(IX509Store origCerts, IX509Store origCrls)
	{
		this.origCerts = CmsUtilities.GetCertificatesFromStore(origCerts);
		this.origCrls = ((origCrls == null) ? null : CmsUtilities.GetCrlsFromStore(origCrls));
	}

	public virtual OriginatorInfo Generate()
	{
		Asn1Set certs = CmsUtilities.CreateDerSetFromList(origCerts);
		Asn1Set crls = ((origCrls == null) ? null : CmsUtilities.CreateDerSetFromList(origCrls));
		return new OriginatorInfo(certs, crls);
	}
}
