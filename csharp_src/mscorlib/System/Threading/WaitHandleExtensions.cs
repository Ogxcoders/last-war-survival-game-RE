using System.Security;
using Microsoft.Win32.SafeHandles;

namespace System.Threading;

public static class WaitHandleExtensions
{
	[SecurityCritical]
	public static SafeWaitHandle GetSafeWaitHandle(this WaitHandle waitHandle)
	{
		if (waitHandle == null)
		{
			throw new ArgumentNullException("waitHandle");
		}
		return waitHandle.SafeWaitHandle;
	}

	[SecurityCritical]
	public static void SetSafeWaitHandle(this WaitHandle waitHandle, SafeWaitHandle value)
	{
		if (waitHandle == null)
		{
			throw new ArgumentNullException("waitHandle");
		}
		waitHandle.SafeWaitHandle = value;
	}
}
