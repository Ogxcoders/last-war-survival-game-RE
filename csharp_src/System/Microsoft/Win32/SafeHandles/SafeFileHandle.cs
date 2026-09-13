using System;
using System.Runtime.InteropServices;

namespace Microsoft.Win32.SafeHandles;

internal sealed class SafeFileHandle : SafeHandle
{
	private static readonly IntPtr s_invalidHandle = new IntPtr(-1);

	public override bool IsInvalid
	{
		get
		{
			long num = (long)handle;
			if (num >= 0)
			{
				return num > int.MaxValue;
			}
			return true;
		}
	}

	private SafeFileHandle()
		: this(ownsHandle: true)
	{
	}

	private SafeFileHandle(bool ownsHandle)
		: base(s_invalidHandle, ownsHandle)
	{
	}

	public SafeFileHandle(IntPtr preexistingHandle, bool ownsHandle)
		: this(ownsHandle)
	{
		SetHandle(preexistingHandle);
	}

	internal static SafeFileHandle Open(string path, global::Interop.Sys.OpenFlags flags, int mode)
	{
		bool isDirectory = (flags & global::Interop.Sys.OpenFlags.O_CREAT) != 0;
		SafeFileHandle safeFileHandle = global::Interop.CheckIo(global::Interop.Sys.Open(path, flags, mode), path, isDirectory, (global::Interop.ErrorInfo e) => (e.Error != global::Interop.Error.EISDIR) ? e : global::Interop.Error.EACCES.Info());
		if (global::Interop.Sys.FStat(safeFileHandle, out var output) != 0)
		{
			safeFileHandle.Dispose();
			throw global::Interop.GetExceptionForIoErrno(global::Interop.Sys.GetLastErrorInfo(), path);
		}
		if ((output.Mode & 0xF000) == 16384)
		{
			safeFileHandle.Dispose();
			throw global::Interop.GetExceptionForIoErrno(global::Interop.Error.EACCES.Info(), path, isDirectory: true);
		}
		return safeFileHandle;
	}

	internal static SafeFileHandle Open(Func<SafeFileHandle> fdFunc)
	{
		return global::Interop.CheckIo(fdFunc());
	}

	protected override bool ReleaseHandle()
	{
		global::Interop.Sys.FLock(handle, global::Interop.Sys.LockOperations.LOCK_UN);
		return global::Interop.Sys.Close(handle) == 0;
	}
}
