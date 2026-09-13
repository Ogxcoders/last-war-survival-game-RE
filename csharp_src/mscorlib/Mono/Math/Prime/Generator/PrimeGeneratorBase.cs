namespace Mono.Math.Prime.Generator;

internal abstract class PrimeGeneratorBase
{
	public virtual ConfidenceFactor Confidence => ConfidenceFactor.Medium;

	public virtual PrimalityTest PrimalityTest => PrimalityTests.RabinMillerTest;

	public virtual int TrialDivisionBounds => 4000;

	protected bool PostTrialDivisionTests(BigInteger bi)
	{
		return PrimalityTest(bi, Confidence);
	}

	public abstract BigInteger GenerateNewPrime(int bits);
}
