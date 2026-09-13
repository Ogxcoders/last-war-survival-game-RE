using System.Collections.Generic;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace System.Net.Security;

public class SslServerAuthenticationOptions
{
	private X509RevocationMode _checkCertificateRevocation;

	private SslProtocols _enabledSslProtocols;

	private EncryptionPolicy _encryptionPolicy;

	private bool _allowRenegotiation = true;

	public bool AllowRenegotiation
	{
		get
		{
			return _allowRenegotiation;
		}
		set
		{
			_allowRenegotiation = value;
		}
	}

	public bool ClientCertificateRequired { get; set; }

	public List<SslApplicationProtocol> ApplicationProtocols { get; set; }

	public RemoteCertificateValidationCallback RemoteCertificateValidationCallback { get; set; }

	public ServerCertificateSelectionCallback ServerCertificateSelectionCallback { get; set; }

	public X509Certificate ServerCertificate { get; set; }

	public SslProtocols EnabledSslProtocols
	{
		get
		{
			return _enabledSslProtocols;
		}
		set
		{
			_enabledSslProtocols = value;
		}
	}

	public X509RevocationMode CertificateRevocationCheckMode
	{
		get
		{
			return _checkCertificateRevocation;
		}
		set
		{
			if (value != X509RevocationMode.NoCheck && value != X509RevocationMode.Offline && value != X509RevocationMode.Online)
			{
				throw new ArgumentException(global::SR.Format("The specified value is not valid in the '{0}' enumeration.", "X509RevocationMode"), "value");
			}
			_checkCertificateRevocation = value;
		}
	}

	public EncryptionPolicy EncryptionPolicy
	{
		get
		{
			return _encryptionPolicy;
		}
		set
		{
			if (value != EncryptionPolicy.RequireEncryption && value != EncryptionPolicy.AllowNoEncryption && value != EncryptionPolicy.NoEncryption)
			{
				throw new ArgumentException(global::SR.Format("The specified value is not valid in the '{0}' enumeration.", "EncryptionPolicy"), "value");
			}
			_encryptionPolicy = value;
		}
	}
}
