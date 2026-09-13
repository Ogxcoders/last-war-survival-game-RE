using System;

namespace Mono.Security.Interface;

public class MonoTlsConnectionInfo
{
	[CLSCompliant(false)]
	public CipherSuiteCode CipherSuiteCode { get; set; }

	public TlsProtocols ProtocolVersion { get; set; }

	public CipherAlgorithmType CipherAlgorithmType { get; set; }

	public HashAlgorithmType HashAlgorithmType { get; set; }

	public ExchangeAlgorithmType ExchangeAlgorithmType { get; set; }

	public string PeerDomainName { get; set; }

	public override string ToString()
	{
		return $"[MonoTlsConnectionInfo: {ProtocolVersion}:{CipherSuiteCode}]";
	}
}
