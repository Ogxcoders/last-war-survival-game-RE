using System;

namespace Mono.Btls;

[Flags]
internal enum MonoBtlsX509VerifyFlags
{
	DEFAULT = 0,
	CRL_CHECK = 1,
	CRL_CHECK_ALL = 2,
	X509_STRIC = 4
}
