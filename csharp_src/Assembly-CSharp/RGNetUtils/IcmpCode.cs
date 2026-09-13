namespace RGNetUtils;

public enum IcmpCode
{
	NetUnreachable = 0,
	HostUnreachable = 1,
	ProtocolUnreachable = 2,
	PortUnreachable = 3,
	FragmentationNeeded = 4,
	SourceRouteFailed = 5,
	NetworkUnknown = 6,
	HostUnknown = 7,
	SourceHostIsolated = 8,
	NetworkProhibited = 9,
	HostProhibited = 10,
	NetworkUnreachableForTOS = 11,
	HostUnreachableForTOS = 12,
	CommunicationProhibited = 13,
	HostPrecedenceViolation = 14,
	PrecedenceCutoff = 15,
	RedirectForNetwork = 0,
	RedirectForHost = 1,
	RedirectForTOSAndNetwork = 2,
	RedirectForTOSAndHost = 3,
	TTLExceededInTransit = 0,
	FragmentReassemblyTimeExceeded = 1
}
