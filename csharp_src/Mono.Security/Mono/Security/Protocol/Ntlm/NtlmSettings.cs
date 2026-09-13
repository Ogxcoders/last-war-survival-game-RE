namespace Mono.Security.Protocol.Ntlm;

public static class NtlmSettings
{
	private static NtlmAuthLevel defaultAuthLevel = NtlmAuthLevel.LM_and_NTLM_and_try_NTLMv2_Session;

	public static NtlmAuthLevel DefaultAuthLevel
	{
		get
		{
			return defaultAuthLevel;
		}
		set
		{
			defaultAuthLevel = value;
		}
	}
}
