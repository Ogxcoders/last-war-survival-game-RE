using System.Runtime.InteropServices;

namespace System.Net.Security;

internal abstract class SafeDeleteContext : SafeHandle
{
	private SafeFreeCredentials _credential;

	public override bool IsInvalid => _credential == null;

	protected SafeDeleteContext(SafeFreeCredentials credential)
		: base(IntPtr.Zero, ownsHandle: true)
	{
		bool success = false;
		_credential = credential;
		_credential.DangerousAddRef(ref success);
	}

	protected override bool ReleaseHandle()
	{
		_credential.DangerousRelease();
		_credential = null;
		return true;
	}
}
