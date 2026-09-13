namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC;

public interface ECLookupTable
{
	int Size { get; }

	ECPoint Lookup(int index);
}
