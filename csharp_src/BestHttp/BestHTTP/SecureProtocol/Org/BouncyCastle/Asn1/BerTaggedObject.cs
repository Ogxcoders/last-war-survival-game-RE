using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;

public class BerTaggedObject : DerTaggedObject
{
	public BerTaggedObject(int tagNo, Asn1Encodable obj)
		: base(tagNo, obj)
	{
	}

	public BerTaggedObject(bool explicitly, int tagNo, Asn1Encodable obj)
		: base(explicitly, tagNo, obj)
	{
	}

	public BerTaggedObject(int tagNo)
		: base(explicitly: false, tagNo, BerSequence.Empty)
	{
	}

	internal override void Encode(DerOutputStream derOut)
	{
		if (derOut is Asn1OutputStream || derOut is BerOutputStream)
		{
			derOut.WriteTag(160, tagNo);
			derOut.WriteByte(128);
			if (!IsEmpty())
			{
				if (!explicitly)
				{
					IEnumerable enumerable;
					if (obj is Asn1OctetString)
					{
						enumerable = ((!(obj is BerOctetString)) ? new BerOctetString(((Asn1OctetString)obj).GetOctets()) : ((BerOctetString)obj));
					}
					else if (obj is Asn1Sequence)
					{
						enumerable = (Asn1Sequence)obj;
					}
					else
					{
						if (!(obj is Asn1Set))
						{
							throw Platform.CreateNotImplementedException(Platform.GetTypeName(obj));
						}
						enumerable = (Asn1Set)obj;
					}
					foreach (Asn1Encodable item in enumerable)
					{
						derOut.WriteObject(item);
					}
				}
				else
				{
					derOut.WriteObject(obj);
				}
			}
			derOut.WriteByte(0);
			derOut.WriteByte(0);
		}
		else
		{
			base.Encode(derOut);
		}
	}
}
