using System.Security.Cryptography.X509Certificates;

namespace Mono.Security.Interface;

public delegate bool MonoRemoteCertificateValidationCallback(string targetHost, X509Certificate certificate, X509Chain chain, MonoSslPolicyErrors sslPolicyErrors);
