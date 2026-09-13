using System;
using System.Collections.Generic;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls;

namespace Org.BouncyCastle.Crypto.Tls;

public sealed class LegacyTlsClient : DefaultTlsClient
{
	private readonly Uri TargetUri;

	private readonly ICertificateVerifyer verifyer;

	private readonly IClientCredentialsProvider credProvider;

	public LegacyTlsClient(Uri targetUri, ICertificateVerifyer verifyer, IClientCredentialsProvider prov, List<string> hostNames)
	{
		TargetUri = targetUri;
		this.verifyer = verifyer;
		credProvider = prov;
		base.HostNames = hostNames;
	}

	public override TlsAuthentication GetAuthentication()
	{
		return new LegacyTlsAuthentication(TargetUri, verifyer, credProvider);
	}
}
