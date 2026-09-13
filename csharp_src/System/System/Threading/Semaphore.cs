using System.IO.Ports;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.AccessControl;
using System.Security.Permissions;
using Microsoft.Win32.SafeHandles;

namespace System.Threading;

[ComVisible(false)]
[HostProtection(SecurityAction.LinkDemand, Synchronization = true, ExternalThreading = true)]
public sealed class Semaphore : WaitHandle
{
	private new enum OpenExistingResult
	{
		Success,
		NameNotFound,
		PathNotFound,
		NameInvalid
	}

	private const int MAX_PATH = 260;

	[SecuritySafeCritical]
	public Semaphore(int initialCount, int maximumCount)
		: this(initialCount, maximumCount, null)
	{
	}

	[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public Semaphore(int initialCount, int maximumCount, string name)
	{
		if (initialCount < 0)
		{
			throw new ArgumentOutOfRangeException("initialCount", global::SR.GetString("Non-negative number required."));
		}
		if (maximumCount < 1)
		{
			throw new ArgumentOutOfRangeException("maximumCount", global::SR.GetString("Positive number required."));
		}
		if (initialCount > maximumCount)
		{
			throw new ArgumentException(global::SR.GetString("The initial count for the semaphore must be greater than or equal to zero and less than the maximum count."));
		}
		if (name != null && 260 < name.Length)
		{
			throw new ArgumentException(global::SR.GetString("The name can be no more than 260 characters in length."));
		}
		int errorCode;
		SafeWaitHandle safeWaitHandle = new SafeWaitHandle(CreateSemaphore_internal(initialCount, maximumCount, name, out errorCode), ownsHandle: true);
		if (safeWaitHandle.IsInvalid)
		{
			if (name != null && name.Length != 0 && 6 == errorCode)
			{
				throw new WaitHandleCannotBeOpenedException(global::SR.GetString("A WaitHandle with system-wide name '{0}' cannot be created. A WaitHandle of a different type might have the same name.", name));
			}
			InternalResources.WinIOError(errorCode, "");
		}
		base.SafeWaitHandle = safeWaitHandle;
	}

	[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public Semaphore(int initialCount, int maximumCount, string name, out bool createdNew)
		: this(initialCount, maximumCount, name, out createdNew, null)
	{
	}

	[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public Semaphore(int initialCount, int maximumCount, string name, out bool createdNew, SemaphoreSecurity semaphoreSecurity)
	{
		if (initialCount < 0)
		{
			throw new ArgumentOutOfRangeException("initialCount", global::SR.GetString("Non-negative number required."));
		}
		if (maximumCount < 1)
		{
			throw new ArgumentOutOfRangeException("maximumCount", global::SR.GetString("Non-negative number required."));
		}
		if (initialCount > maximumCount)
		{
			throw new ArgumentException(global::SR.GetString("The initial count for the semaphore must be greater than or equal to zero and less than the maximum count."));
		}
		if (name != null && 260 < name.Length)
		{
			throw new ArgumentException(global::SR.GetString("The name can be no more than 260 characters in length."));
		}
		int errorCode;
		SafeWaitHandle safeWaitHandle = new SafeWaitHandle(CreateSemaphore_internal(initialCount, maximumCount, name, out errorCode), ownsHandle: true);
		if (safeWaitHandle.IsInvalid)
		{
			if (name != null && name.Length != 0 && 6 == errorCode)
			{
				throw new WaitHandleCannotBeOpenedException(global::SR.GetString("A WaitHandle with system-wide name '{0}' cannot be created. A WaitHandle of a different type might have the same name.", name));
			}
			InternalResources.WinIOError(errorCode, "");
		}
		createdNew = errorCode != 183;
		base.SafeWaitHandle = safeWaitHandle;
	}

	private Semaphore(SafeWaitHandle handle)
	{
		base.SafeWaitHandle = handle;
	}

	[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public static Semaphore OpenExisting(string name)
	{
		return OpenExisting(name, SemaphoreRights.Modify | SemaphoreRights.Synchronize);
	}

	[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public static Semaphore OpenExisting(string name, SemaphoreRights rights)
	{
		Semaphore result;
		switch (OpenExistingWorker(name, rights, out result))
		{
		case OpenExistingResult.NameNotFound:
			throw new WaitHandleCannotBeOpenedException();
		case OpenExistingResult.NameInvalid:
			throw new WaitHandleCannotBeOpenedException(global::SR.GetString("A WaitHandle with system-wide name '{0}' cannot be created. A WaitHandle of a different type might have the same name.", name));
		case OpenExistingResult.PathNotFound:
			InternalResources.WinIOError(3, string.Empty);
			return result;
		default:
			return result;
		}
	}

	[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public static bool TryOpenExisting(string name, out Semaphore result)
	{
		return OpenExistingWorker(name, SemaphoreRights.Modify | SemaphoreRights.Synchronize, out result) == OpenExistingResult.Success;
	}

	[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public static bool TryOpenExisting(string name, SemaphoreRights rights, out Semaphore result)
	{
		return OpenExistingWorker(name, rights, out result) == OpenExistingResult.Success;
	}

	[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	private static OpenExistingResult OpenExistingWorker(string name, SemaphoreRights rights, out Semaphore result)
	{
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		if (name.Length == 0)
		{
			throw new ArgumentException(global::SR.GetString("Argument {0} cannot be null or zero-length.", "name"), "name");
		}
		if (name != null && 260 < name.Length)
		{
			throw new ArgumentException(global::SR.GetString("The name can be no more than 260 characters in length."));
		}
		result = null;
		throw new NotSupportedException();
	}

	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
	[PrePrepareMethod]
	public int Release()
	{
		return Release(1);
	}

	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
	public int Release(int releaseCount)
	{
		if (releaseCount < 1)
		{
			throw new ArgumentOutOfRangeException("releaseCount", global::SR.GetString("Non-negative number required."));
		}
		if (!ReleaseSemaphore_internal(base.SafeWaitHandle.DangerousGetHandle(), releaseCount, out var previousCount))
		{
			throw new SemaphoreFullException();
		}
		return previousCount;
	}

	public SemaphoreSecurity GetAccessControl()
	{
		return new SemaphoreSecurity(base.SafeWaitHandle, AccessControlSections.Access | AccessControlSections.Owner | AccessControlSections.Group);
	}

	public void SetAccessControl(SemaphoreSecurity semaphoreSecurity)
	{
		if (semaphoreSecurity == null)
		{
			throw new ArgumentNullException("semaphoreSecurity");
		}
		semaphoreSecurity.Persist(base.SafeWaitHandle);
	}

	internal unsafe static IntPtr CreateSemaphore_internal(int initialCount, int maximumCount, string name, out int errorCode)
	{
		fixed (char* name2 = name)
		{
			return CreateSemaphore_icall(initialCount, maximumCount, name2, name?.Length ?? 0, out errorCode);
		}
	}

	private unsafe static IntPtr OpenSemaphore_internal(string name, SemaphoreRights rights, out int errorCode)
	{
		fixed (char* name2 = name)
		{
			return OpenSemaphore_icall(name2, name?.Length ?? 0, rights, out errorCode);
		}
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	private unsafe static extern IntPtr CreateSemaphore_icall(int initialCount, int maximumCount, char* name, int name_length, out int errorCode);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private unsafe static extern IntPtr OpenSemaphore_icall(char* name, int name_length, SemaphoreRights rights, out int errorCode);

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern bool ReleaseSemaphore_internal(IntPtr handle, int releaseCount, out int previousCount);
}
