using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;

public class CrlNumber : DerInteger
{
	public BigInteger Number => base.PositiveValue;

	public CrlNumber(BigInteger number)
		: base(number)
	{
	}

	public override string ToString()
	{
		return "CRLNumber: " + Number;
	}
}
