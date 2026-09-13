using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Date;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Prng;

public class X931SecureRandomBuilder
{
	private readonly SecureRandom mRandom;

	private IEntropySourceProvider mEntropySourceProvider;

	private byte[] mDateTimeVector;

	public X931SecureRandomBuilder()
		: this(new SecureRandom(), predictionResistant: false)
	{
	}

	public X931SecureRandomBuilder(SecureRandom entropySource, bool predictionResistant)
	{
		mRandom = entropySource;
		mEntropySourceProvider = new BasicEntropySourceProvider(mRandom, predictionResistant);
	}

	public X931SecureRandomBuilder(IEntropySourceProvider entropySourceProvider)
	{
		mRandom = null;
		mEntropySourceProvider = entropySourceProvider;
	}

	public X931SecureRandomBuilder SetDateTimeVector(byte[] dateTimeVector)
	{
		mDateTimeVector = dateTimeVector;
		return this;
	}

	public X931SecureRandom Build(IBlockCipher engine, KeyParameter key, bool predictionResistant)
	{
		if (mDateTimeVector == null)
		{
			mDateTimeVector = new byte[engine.GetBlockSize()];
			Pack.UInt64_To_BE((ulong)DateTimeUtilities.CurrentUnixMs(), mDateTimeVector, 0);
		}
		engine.Init(forEncryption: true, key);
		return new X931SecureRandom(mRandom, new X931Rng(engine, mDateTimeVector, mEntropySourceProvider.Get(engine.GetBlockSize() * 8)), predictionResistant);
	}
}
