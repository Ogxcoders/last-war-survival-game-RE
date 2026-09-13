using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;

public class TweakableBlockCipherParameters : ICipherParameters
{
	private readonly byte[] tweak;

	private readonly KeyParameter key;

	public KeyParameter Key => key;

	public byte[] Tweak => tweak;

	public TweakableBlockCipherParameters(KeyParameter key, byte[] tweak)
	{
		this.key = key;
		this.tweak = Arrays.Clone(tweak);
	}
}
