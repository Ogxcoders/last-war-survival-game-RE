namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Multiplier;

public interface IPreCompCallback
{
	PreCompInfo Precompute(PreCompInfo existing);
}
