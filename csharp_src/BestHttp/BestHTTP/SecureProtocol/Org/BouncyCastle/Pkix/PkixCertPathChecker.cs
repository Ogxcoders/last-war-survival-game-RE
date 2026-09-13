using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.X509;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Pkix;

public abstract class PkixCertPathChecker
{
	public abstract void Init(bool forward);

	public abstract bool IsForwardCheckingSupported();

	public abstract ISet GetSupportedExtensions();

	public abstract void Check(X509Certificate cert, ISet unresolvedCritExts);

	public virtual object Clone()
	{
		return MemberwiseClone();
	}
}
