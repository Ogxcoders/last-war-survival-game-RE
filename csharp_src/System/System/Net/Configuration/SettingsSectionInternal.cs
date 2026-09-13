using System.Net.Security;
using System.Net.Sockets;

namespace System.Net.Configuration;

internal sealed class SettingsSectionInternal
{
	private static readonly SettingsSectionInternal instance = new SettingsSectionInternal();

	internal readonly bool HttpListenerUnescapeRequestUrl = true;

	internal readonly IPProtectionLevel IPProtectionLevel = IPProtectionLevel.Unspecified;

	internal static SettingsSectionInternal Section => instance;

	internal bool UseNagleAlgorithm { get; set; }

	internal bool Expect100Continue { get; set; }

	internal bool CheckCertificateName { get; private set; }

	internal int DnsRefreshTimeout { get; set; }

	internal bool EnableDnsRoundRobin { get; set; }

	internal bool CheckCertificateRevocationList { get; set; }

	internal EncryptionPolicy EncryptionPolicy { get; private set; }

	internal bool Ipv6Enabled => true;
}
