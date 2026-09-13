using System;

namespace KWSVerification;

[Serializable]
public class AccountInfoResponse : BaseResponse
{
	public string airKey;

	public AccountData data;
}
