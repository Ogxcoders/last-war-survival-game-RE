namespace BestHTTP;

internal enum SOCKSMethods : byte
{
	NoAuthenticationRequired = 0,
	GSSAPI = 1,
	UsernameAndPassword = 2,
	NoAcceptableMethods = byte.MaxValue
}
