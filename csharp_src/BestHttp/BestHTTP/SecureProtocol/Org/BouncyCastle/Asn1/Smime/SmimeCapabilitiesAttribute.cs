using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Smime;

public class SmimeCapabilitiesAttribute : AttributeX509
{
	public SmimeCapabilitiesAttribute(SmimeCapabilityVector capabilities)
		: base(SmimeAttributes.SmimeCapabilities, new DerSet(new DerSequence(capabilities.ToAsn1EncodableVector())))
	{
	}
}
