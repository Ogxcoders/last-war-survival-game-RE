namespace BestHTTP;

internal enum SOCKSReplies : byte
{
	Succeeded,
	GeneralSOCKSServerFailure,
	ConnectionNotAllowedByRuleset,
	NetworkUnreachable,
	HostUnreachable,
	ConnectionRefused,
	TTLExpired,
	CommandNotSupported,
	AddressTypeNotSupported
}
