using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Esf;

public class CrlOcspRef : Asn1Encodable
{
	private readonly CrlListID crlids;

	private readonly OcspListID ocspids;

	private readonly OtherRevRefs otherRev;

	public CrlListID CrlIDs => crlids;

	public OcspListID OcspIDs => ocspids;

	public OtherRevRefs OtherRev => otherRev;

	public static CrlOcspRef GetInstance(object obj)
	{
		if (obj == null || obj is CrlOcspRef)
		{
			return (CrlOcspRef)obj;
		}
		if (obj is Asn1Sequence)
		{
			return new CrlOcspRef((Asn1Sequence)obj);
		}
		throw new ArgumentException("Unknown object in 'CrlOcspRef' factory: " + Platform.GetTypeName(obj), "obj");
	}

	private CrlOcspRef(Asn1Sequence seq)
	{
		if (seq == null)
		{
			throw new ArgumentNullException("seq");
		}
		foreach (Asn1TaggedObject item in seq)
		{
			Asn1Object obj2 = item.GetObject();
			switch (item.TagNo)
			{
			case 0:
				crlids = CrlListID.GetInstance(obj2);
				break;
			case 1:
				ocspids = OcspListID.GetInstance(obj2);
				break;
			case 2:
				otherRev = OtherRevRefs.GetInstance(obj2);
				break;
			default:
				throw new ArgumentException("Illegal tag in CrlOcspRef", "seq");
			}
		}
	}

	public CrlOcspRef(CrlListID crlids, OcspListID ocspids, OtherRevRefs otherRev)
	{
		this.crlids = crlids;
		this.ocspids = ocspids;
		this.otherRev = otherRev;
	}

	public override Asn1Object ToAsn1Object()
	{
		Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector();
		if (crlids != null)
		{
			asn1EncodableVector.Add(new DerTaggedObject(explicitly: true, 0, crlids.ToAsn1Object()));
		}
		if (ocspids != null)
		{
			asn1EncodableVector.Add(new DerTaggedObject(explicitly: true, 1, ocspids.ToAsn1Object()));
		}
		if (otherRev != null)
		{
			asn1EncodableVector.Add(new DerTaggedObject(explicitly: true, 2, otherRev.ToAsn1Object()));
		}
		return new DerSequence(asn1EncodableVector);
	}
}
