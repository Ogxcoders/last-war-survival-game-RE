using Microsoft.Win32.SafeHandles;

namespace System.Net.Security;

internal sealed class SafeFreeNegoCredentials : SafeFreeCredentials
{
	private SafeGssCredHandle _credential;

	private readonly bool _isNtlmOnly;

	private readonly string _userName;

	private readonly bool _isDefault;

	public SafeGssCredHandle GssCredential => _credential;

	public bool IsNtlmOnly => _isNtlmOnly;

	public string UserName => _userName;

	public bool IsDefault => _isDefault;

	public override bool IsInvalid => _credential == null;

	public SafeFreeNegoCredentials(bool isNtlmOnly, string username, string password, string domain)
		: base(IntPtr.Zero, ownsHandle: true)
	{
		int num = username.IndexOf('\\');
		if (num > 0 && username.IndexOf('\\', num + 1) < 0 && string.IsNullOrEmpty(domain))
		{
			domain = username.Substring(0, num);
			username = username.Substring(num + 1);
		}
		if (domain != null)
		{
			domain = domain.Trim();
		}
		username = username.Trim();
		if (username.IndexOf('@') < 0 && !string.IsNullOrEmpty(domain))
		{
			username = username + "@" + domain;
		}
		bool success = false;
		_isNtlmOnly = isNtlmOnly;
		_userName = username;
		_isDefault = string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password);
		_credential = SafeGssCredHandle.Create(username, password, isNtlmOnly);
		_credential.DangerousAddRef(ref success);
	}

	protected override bool ReleaseHandle()
	{
		_credential.DangerousRelease();
		_credential = null;
		return true;
	}
}
