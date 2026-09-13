using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Collections;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.X509;

public interface IX509Extension
{
	ISet GetCriticalExtensionOids();

	ISet GetNonCriticalExtensionOids();

	[Obsolete("Use version taking a DerObjectIdentifier instead")]
	Asn1OctetString GetExtensionValue(string oid);

	Asn1OctetString GetExtensionValue(DerObjectIdentifier oid);
}
