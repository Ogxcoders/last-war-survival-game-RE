namespace Mono.Security.Protocol.Ntlm;

public enum NtlmAuthLevel
{
	LM_and_NTLM,
	LM_and_NTLM_and_try_NTLMv2_Session,
	NTLM_only,
	NTLMv2_only
}
