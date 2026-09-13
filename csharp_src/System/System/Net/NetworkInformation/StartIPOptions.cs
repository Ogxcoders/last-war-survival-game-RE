namespace System.Net.NetworkInformation;

[Flags]
internal enum StartIPOptions
{
	Both = 3,
	None = 0,
	StartIPv4 = 1,
	StartIPv6 = 2
}
