using System.Security.Cryptography.X509Certificates;

namespace Mono.Security.Interface;

internal delegate X509Certificate MonoServerCertificateSelectionCallback(object sender, string hostName);
