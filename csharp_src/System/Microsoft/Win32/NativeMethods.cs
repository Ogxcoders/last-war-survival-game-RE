using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace Microsoft.Win32;

internal static class NativeMethods
{
	public const int E_ABORT = -2147467260;

	public const int PROCESS_TERMINATE = 1;

	public const int PROCESS_CREATE_THREAD = 2;

	public const int PROCESS_SET_SESSIONID = 4;

	public const int PROCESS_VM_OPERATION = 8;

	public const int PROCESS_VM_READ = 16;

	public const int PROCESS_VM_WRITE = 32;

	public const int PROCESS_DUP_HANDLE = 64;

	public const int PROCESS_CREATE_PROCESS = 128;

	public const int PROCESS_SET_QUOTA = 256;

	public const int PROCESS_SET_INFORMATION = 512;

	public const int PROCESS_QUERY_INFORMATION = 1024;

	public const int PROCESS_QUERY_LIMITED_INFORMATION = 4096;

	public const int STANDARD_RIGHTS_REQUIRED = 983040;

	public const int SYNCHRONIZE = 1048576;

	public const int PROCESS_ALL_ACCESS = 2035711;

	public const int DUPLICATE_CLOSE_SOURCE = 1;

	public const int DUPLICATE_SAME_ACCESS = 2;

	public const int STILL_ACTIVE = 259;

	public const int WAIT_OBJECT_0 = 0;

	public const int WAIT_FAILED = -1;

	public const int WAIT_TIMEOUT = 258;

	public const int WAIT_ABANDONED = 128;

	public const int WAIT_ABANDONED_0 = 128;

	public const int ERROR_FILE_NOT_FOUND = 2;

	public const int ERROR_PATH_NOT_FOUND = 3;

	public const int ERROR_ACCESS_DENIED = 5;

	public const int ERROR_INVALID_HANDLE = 6;

	public const int ERROR_SHARING_VIOLATION = 32;

	public const int ERROR_INVALID_NAME = 123;

	public const int ERROR_ALREADY_EXISTS = 183;

	public const int ERROR_FILENAME_EXCED_RANGE = 206;

	public static bool DuplicateHandle(HandleRef hSourceProcessHandle, SafeHandle hSourceHandle, HandleRef hTargetProcess, out SafeWaitHandle targetHandle, int dwDesiredAccess, bool bInheritHandle, int dwOptions)
	{
		bool success = false;
		try
		{
			hSourceHandle.DangerousAddRef(ref success);
			IntPtr target_handle;
			MonoIOError error;
			bool result = MonoIO.DuplicateHandle(hSourceProcessHandle.Handle, hSourceHandle.DangerousGetHandle(), hTargetProcess.Handle, out target_handle, dwDesiredAccess, bInheritHandle ? 1 : 0, dwOptions, out error);
			if (error != MonoIOError.ERROR_SUCCESS)
			{
				throw MonoIO.GetException(error);
			}
			targetHandle = new SafeWaitHandle(target_handle, ownsHandle: true);
			return result;
		}
		finally
		{
			if (success)
			{
				hSourceHandle.DangerousRelease();
			}
		}
	}

	public static bool DuplicateHandle(HandleRef hSourceProcessHandle, HandleRef hSourceHandle, HandleRef hTargetProcess, out SafeProcessHandle targetHandle, int dwDesiredAccess, bool bInheritHandle, int dwOptions)
	{
		IntPtr target_handle;
		MonoIOError error;
		bool result = MonoIO.DuplicateHandle(hSourceProcessHandle.Handle, hSourceHandle.Handle, hTargetProcess.Handle, out target_handle, dwDesiredAccess, bInheritHandle ? 1 : 0, dwOptions, out error);
		if (error != MonoIOError.ERROR_SUCCESS)
		{
			throw MonoIO.GetException(error);
		}
		targetHandle = new SafeProcessHandle(target_handle, ownsHandle: true);
		return result;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern IntPtr GetCurrentProcess();

	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern bool GetExitCodeProcess(IntPtr processHandle, out int exitCode);

	public static bool GetExitCodeProcess(SafeProcessHandle processHandle, out int exitCode)
	{
		bool success = false;
		try
		{
			processHandle.DangerousAddRef(ref success);
			return GetExitCodeProcess(processHandle.DangerousGetHandle(), out exitCode);
		}
		finally
		{
			if (success)
			{
				processHandle.DangerousRelease();
			}
		}
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern bool TerminateProcess(IntPtr processHandle, int exitCode);

	public static bool TerminateProcess(SafeProcessHandle processHandle, int exitCode)
	{
		bool success = false;
		try
		{
			processHandle.DangerousAddRef(ref success);
			return TerminateProcess(processHandle.DangerousGetHandle(), exitCode);
		}
		finally
		{
			if (success)
			{
				processHandle.DangerousRelease();
			}
		}
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern int WaitForInputIdle(IntPtr handle, int milliseconds);

	public static int WaitForInputIdle(SafeProcessHandle handle, int milliseconds)
	{
		bool success = false;
		try
		{
			handle.DangerousAddRef(ref success);
			return WaitForInputIdle(handle.DangerousGetHandle(), milliseconds);
		}
		finally
		{
			if (success)
			{
				handle.DangerousRelease();
			}
		}
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern bool GetProcessWorkingSetSize(IntPtr handle, out IntPtr min, out IntPtr max);

	public static bool GetProcessWorkingSetSize(SafeProcessHandle handle, out IntPtr min, out IntPtr max)
	{
		bool success = false;
		try
		{
			handle.DangerousAddRef(ref success);
			return GetProcessWorkingSetSize(handle.DangerousGetHandle(), out min, out max);
		}
		finally
		{
			if (success)
			{
				handle.DangerousRelease();
			}
		}
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern bool SetProcessWorkingSetSize(IntPtr handle, IntPtr min, IntPtr max);

	public static bool SetProcessWorkingSetSize(SafeProcessHandle handle, IntPtr min, IntPtr max)
	{
		bool success = false;
		try
		{
			handle.DangerousAddRef(ref success);
			return SetProcessWorkingSetSize(handle.DangerousGetHandle(), min, max);
		}
		finally
		{
			if (success)
			{
				handle.DangerousRelease();
			}
		}
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern bool GetProcessTimes(IntPtr handle, out long creation, out long exit, out long kernel, out long user);

	public static bool GetProcessTimes(SafeProcessHandle handle, out long creation, out long exit, out long kernel, out long user)
	{
		bool success = false;
		try
		{
			handle.DangerousAddRef(ref success);
			return GetProcessTimes(handle.DangerousGetHandle(), out creation, out exit, out kernel, out user);
		}
		finally
		{
			if (success)
			{
				handle.DangerousRelease();
			}
		}
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern int GetCurrentProcessId();

	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern int GetPriorityClass(IntPtr handle);

	public static int GetPriorityClass(SafeProcessHandle handle)
	{
		bool success = false;
		try
		{
			handle.DangerousAddRef(ref success);
			return GetPriorityClass(handle.DangerousGetHandle());
		}
		finally
		{
			if (success)
			{
				handle.DangerousRelease();
			}
		}
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern bool SetPriorityClass(IntPtr handle, int priorityClass);

	public static bool SetPriorityClass(SafeProcessHandle handle, int priorityClass)
	{
		bool success = false;
		try
		{
			handle.DangerousAddRef(ref success);
			return SetPriorityClass(handle.DangerousGetHandle(), priorityClass);
		}
		finally
		{
			if (success)
			{
				handle.DangerousRelease();
			}
		}
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern bool CloseProcess(IntPtr handle);
}
