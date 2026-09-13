namespace System.Net;

internal enum ChainPolicyType
{
	Base = 1,
	Authenticode,
	Authenticode_TS,
	SSL,
	BasicConstraints,
	NtAuth
}
