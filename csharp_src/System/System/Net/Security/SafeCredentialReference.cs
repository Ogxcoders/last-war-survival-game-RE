using Microsoft.Win32.SafeHandles;

namespace System.Net.Security;

internal sealed class SafeCredentialReference : CriticalHandleMinusOneIsInvalid
{
	internal SafeFreeCredentials Target;

	internal static SafeCredentialReference CreateReference(SafeFreeCredentials target)
	{
		SafeCredentialReference safeCredentialReference = new SafeCredentialReference(target);
		if (safeCredentialReference.IsInvalid)
		{
			return null;
		}
		return safeCredentialReference;
	}

	private SafeCredentialReference(SafeFreeCredentials target)
	{
		bool success = false;
		target.DangerousAddRef(ref success);
		Target = target;
		SetHandle(new IntPtr(0));
	}

	protected override bool ReleaseHandle()
	{
		Target?.DangerousRelease();
		Target = null;
		return true;
	}
}
