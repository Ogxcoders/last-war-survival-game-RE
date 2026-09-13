using System.Security.Cryptography.X509Certificates;

namespace Mono.Security.Interface;

public delegate X509Certificate MonoLocalCertificateSelectionCallback(string targetHost, X509CertificateCollection localCertificates, X509Certificate remoteCertificate, string[] acceptableIssuers);
