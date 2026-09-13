using System;

namespace KWSVerification;

[Serializable]
public class AccountData
{
	public AccountInfo accountInfo;

	public bool need_verify;

	public bool can_enter;

	public bool need_account;
}
