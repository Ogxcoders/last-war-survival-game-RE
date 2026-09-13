namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto;

public interface IVerifierFactory
{
	object AlgorithmDetails { get; }

	IStreamCalculator CreateCalculator();
}
