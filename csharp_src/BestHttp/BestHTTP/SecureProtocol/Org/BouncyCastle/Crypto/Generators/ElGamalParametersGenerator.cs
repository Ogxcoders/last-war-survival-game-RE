using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Generators;

public class ElGamalParametersGenerator
{
	private int size;

	private int certainty;

	private SecureRandom random;

	public void Init(int size, int certainty, SecureRandom random)
	{
		this.size = size;
		this.certainty = certainty;
		this.random = random;
	}

	public ElGamalParameters GenerateParameters()
	{
		BigInteger[] array = DHParametersHelper.GenerateSafePrimes(size, certainty, random);
		BigInteger p = array[0];
		BigInteger q = array[1];
		BigInteger g = DHParametersHelper.SelectGenerator(p, q, random);
		return new ElGamalParameters(p, g);
	}
}
