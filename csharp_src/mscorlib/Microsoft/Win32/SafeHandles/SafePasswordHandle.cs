using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Microsoft.Win32.SafeHandles;

internal sealed class SafePasswordHandle : SafeHandle
{
	public override bool IsInvalid => handle == (IntPtr)(-1);

	public static SafePasswordHandle InvalidHandle => SafeHandleCache<SafePasswordHandle>.GetInvalidHandle(() => new SafePasswordHandle((string)null)
	{
		handle = (IntPtr)(-1)
	});

	private IntPtr CreateHandle(string password)
	{
		return Marshal.StringToHGlobalAnsi(password);
	}

	private IntPtr CreateHandle(SecureString password)
	{
		return Marshal.SecureStringToGlobalAllocAnsi(password);
	}

	private void FreeHandle()
	{
		Marshal.ZeroFreeGlobalAllocAnsi(handle);
	}

	public SafePasswordHandle(string password)
		: base(IntPtr.Zero, ownsHandle: true)
	{
		if (password != null)
		{
			SetHandle(CreateHandle(password));
		}
	}

	public SafePasswordHandle(SecureString password)
		: base(IntPtr.Zero, ownsHandle: true)
	{
		if (password != null)
		{
			SetHandle(CreateHandle(password));
		}
	}

	protected override bool ReleaseHandle()
	{
		if (handle != IntPtr.Zero)
		{
			FreeHandle();
		}
		SetHandle((IntPtr)(-1));
		return true;
	}

	protected override void Dispose(bool disposing)
	{
		if (!disposing || !SafeHandleCache<SafePasswordHandle>.IsCachedInvalidHandle(this))
		{
			base.Dispose(disposing);
		}
	}

	internal string Mono_DangerousGetString()
	{
		return Marshal.PtrToStringAnsi(DangerousGetHandle());
	}
}
