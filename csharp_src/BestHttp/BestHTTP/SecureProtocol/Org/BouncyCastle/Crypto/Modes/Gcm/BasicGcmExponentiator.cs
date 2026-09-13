using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Modes.Gcm;

public class BasicGcmExponentiator : IGcmExponentiator
{
	private uint[] x;

	public void Init(byte[] x)
	{
		this.x = GcmUtilities.AsUints(x);
	}

	public void ExponentiateX(long pow, byte[] output)
	{
		uint[] array = GcmUtilities.OneAsUints();
		if (pow > 0)
		{
			uint[] y = Arrays.Clone(x);
			do
			{
				if ((pow & 1) != 0L)
				{
					GcmUtilities.Multiply(array, y);
				}
				GcmUtilities.Multiply(y, y);
				pow >>= 1;
			}
			while (pow > 0);
		}
		GcmUtilities.AsBytes(array, output);
	}
}
