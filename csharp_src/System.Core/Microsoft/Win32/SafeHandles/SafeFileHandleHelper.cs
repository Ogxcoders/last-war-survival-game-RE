using System;

namespace Microsoft.Win32.SafeHandles;

internal static class SafeFileHandleHelper
{
	private static readonly IntPtr s_invalidHandle = new IntPtr(-1);

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
}
