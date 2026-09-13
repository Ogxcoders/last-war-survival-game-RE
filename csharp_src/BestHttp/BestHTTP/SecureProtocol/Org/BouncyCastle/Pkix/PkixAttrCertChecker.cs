using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.X509;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Pkix;

public abstract class PkixAttrCertChecker
{
	public abstract ISet GetSupportedExtensions();

	public abstract void Check(IX509AttributeCertificate attrCert, PkixCertPath certPath, PkixCertPath holderCertPath, ICollection unresolvedCritExts);

	public abstract PkixAttrCertChecker Clone();
}
