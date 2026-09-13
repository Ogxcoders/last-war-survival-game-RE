using System;

namespace Mono.Btls;

internal class MonoBtlsX509LookupAndroid : MonoBtlsX509LookupMono
{
	protected override MonoBtlsX509 OnGetBySubject(MonoBtlsX509Name name)
	{
		return AndroidPlatform.CertStoreLookup(name);
	}
}
