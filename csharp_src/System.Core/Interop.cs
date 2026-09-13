using System;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

internal static class Interop
{
	internal enum Error
	{
		SUCCESS = 0,
		E2BIG = 65537,
		EACCES = 65538,
		EADDRINUSE = 65539,
		EADDRNOTAVAIL = 65540,
		EAFNOSUPPORT = 65541,
		EAGAIN = 65542,
		EALREADY = 65543,
		EBADF = 65544,
		EBADMSG = 65545,
		EBUSY = 65546,
		ECANCELED = 65547,
		ECHILD = 65548,
		ECONNABORTED = 65549,
		ECONNREFUSED = 65550,
		ECONNRESET = 65551,
		EDEADLK = 65552,
		EDESTADDRREQ = 65553,
		EDOM = 65554,
		EDQUOT = 65555,
		EEXIST = 65556,
		EFAULT = 65557,
		EFBIG = 65558,
		EHOSTUNREACH = 65559,
		EIDRM = 65560,
		EILSEQ = 65561,
		EINPROGRESS = 65562,
		EINTR = 65563,
		EINVAL = 65564,
		EIO = 65565,
		EISCONN = 65566,
		EISDIR = 65567,
		ELOOP = 65568,
		EMFILE = 65569,
		EMLINK = 65570,
		EMSGSIZE = 65571,
		EMULTIHOP = 65572,
		ENAMETOOLONG = 65573,
		ENETDOWN = 65574,
		ENETRESET = 65575,
		ENETUNREACH = 65576,
		ENFILE = 65577,
		ENOBUFS = 65578,
		ENODEV = 65580,
		ENOENT = 65581,
		ENOEXEC = 65582,
		ENOLCK = 65583,
		ENOLINK = 65584,
		ENOMEM = 65585,
		ENOMSG = 65586,
		ENOPROTOOPT = 65587,
		ENOSPC = 65588,
		ENOSYS = 65591,
		ENOTCONN = 65592,
		ENOTDIR = 65593,
		ENOTEMPTY = 65594,
		ENOTRECOVERABLE = 65595,
		ENOTSOCK = 65596,
		ENOTSUP = 65597,
		ENOTTY = 65598,
		ENXIO = 65599,
		EOVERFLOW = 65600,
		EOWNERDEAD = 65601,
		EPERM = 65602,
		EPIPE = 65603,
		EPROTO = 65604,
		EPROTONOSUPPORT = 65605,
		EPROTOTYPE = 65606,
		ERANGE = 65607,
		EROFS = 65608,
		ESPIPE = 65609,
		ESRCH = 65610,
		ESTALE = 65611,
		ETIMEDOUT = 65613,
		ETXTBSY = 65614,
		EXDEV = 65615,
		ESOCKTNOSUPPORT = 65630,
		EPFNOSUPPORT = 65632,
		ESHUTDOWN = 65644,
		EHOSTDOWN = 65648,
		ENODATA = 65649,
		EOPNOTSUPP = 65597,
		EWOULDBLOCK = 65542
	}

	internal struct ErrorInfo
	{
		private Error _error;

		private int _rawErrno;

		internal Error Error => _error;

		internal int RawErrno
		{
			get
			{
				if (_rawErrno != -1)
				{
					return _rawErrno;
				}
				return _rawErrno = Sys.ConvertErrorPalToPlatform(_error);
			}
		}

		internal ErrorInfo(int errno)
		{
			_error = Sys.ConvertErrorPlatformToPal(errno);
			_rawErrno = errno;
		}

		internal ErrorInfo(Error error)
		{
			_error = error;
			_rawErrno = -1;
		}

		internal string GetErrorMessage()
		{
			return Sys.StrError(RawErrno);
		}

		public override string ToString()
		{
			return $"RawErrno: {RawErrno} Error: {Error} GetErrorMessage: {GetErrorMessage()}";
		}
	}

	internal static class Sys
	{
		internal enum LockOperations
		{
			LOCK_SH = 1,
			LOCK_EX = 2,
			LOCK_NB = 4,
			LOCK_UN = 8
		}

		internal static class Fcntl
		{
			internal static readonly bool CanGetSetPipeSz = FcntlCanGetSetPipeSz() != 0;

			[DllImport("System.Native", EntryPoint = "SystemNative_FcntlGetPipeSz", SetLastError = true)]
			internal static extern int GetPipeSz(SafePipeHandle fd);

			[DllImport("System.Native", EntryPoint = "SystemNative_FcntlSetPipeSz", SetLastError = true)]
			internal static extern int SetPipeSz(SafePipeHandle fd, int size);

			[DllImport("System.Native", EntryPoint = "SystemNative_FcntlCanGetSetPipeSz")]
			private static extern int FcntlCanGetSetPipeSz();

			[DllImport("System.Native", EntryPoint = "SystemNative_FcntlSetCloseOnExec", SetLastError = true)]
			internal static extern int SetCloseOnExec(SafeHandle fd);
		}

		[Flags]
		internal enum OpenFlags
		{
			O_RDONLY = 0,
			O_WRONLY = 1,
			O_RDWR = 2,
			O_CLOEXEC = 0x10,
			O_CREAT = 0x20,
			O_EXCL = 0x40,
			O_TRUNC = 0x80,
			O_SYNC = 0x100
		}

		[Flags]
		internal enum Permissions
		{
			Mask = 0x1FF,
			S_IRWXU = 0x1C0,
			S_IRUSR = 0x100,
			S_IWUSR = 0x80,
			S_IXUSR = 0x40,
			S_IRWXG = 0x38,
			S_IRGRP = 0x20,
			S_IWGRP = 0x10,
			S_IXGRP = 8,
			S_IRWXO = 7,
			S_IROTH = 4,
			S_IWOTH = 2,
			S_IXOTH = 1
		}

		[Flags]
		internal enum PipeFlags
		{
			O_CLOEXEC = 0x10
		}

		[Flags]
		internal enum PollEvents : short
		{
			POLLNONE = 0,
			POLLIN = 1,
			POLLPRI = 2,
			POLLOUT = 4,
			POLLERR = 8,
			POLLHUP = 0x10,
			POLLNVAL = 0x20
		}

		internal struct PollEvent
		{
			internal int FileDescriptor;

			internal PollEvents Events;

			internal PollEvents TriggeredEvents;
		}

		internal struct FileStatus
		{
			internal FileStatusFlags Flags;

			internal int Mode;

			internal uint Uid;

			internal uint Gid;

			internal long Size;

			internal long ATime;

			internal long ATimeNsec;

			internal long MTime;

			internal long MTimeNsec;

			internal long CTime;

			internal long CTimeNsec;

			internal long BirthTime;

			internal long BirthTimeNsec;

			internal long Dev;

			internal long Ino;

			internal uint UserFlags;
		}

		internal static class FileTypes
		{
			internal const int S_IFMT = 61440;

			internal const int S_IFIFO = 4096;

			internal const int S_IFCHR = 8192;

			internal const int S_IFDIR = 16384;

			internal const int S_IFREG = 32768;

			internal const int S_IFLNK = 40960;

			internal const int S_IFSOCK = 49152;
		}

		[Flags]
		internal enum FileStatusFlags
		{
			None = 0,
			HasBirthTime = 1
		}

		internal const int ReadEndOfPipe = 0;

		internal const int WriteEndOfPipe = 1;

		internal static Error GetLastError()
		{
			return ConvertErrorPlatformToPal(Marshal.GetLastWin32Error());
		}

		internal static ErrorInfo GetLastErrorInfo()
		{
			return new ErrorInfo(Marshal.GetLastWin32Error());
		}

		internal unsafe static string StrError(int platformErrno)
		{
			int num = 1024;
			byte* ptr = stackalloc byte[(int)(uint)num];
			byte* ptr2 = StrErrorR(platformErrno, ptr, num);
			if (ptr2 == null)
			{
				ptr2 = ptr;
			}
			return Marshal.PtrToStringAnsi((IntPtr)ptr2);
		}

		[DllImport("System.Native", EntryPoint = "SystemNative_ConvertErrorPlatformToPal")]
		internal static extern Error ConvertErrorPlatformToPal(int platformErrno);

		[DllImport("System.Native", EntryPoint = "SystemNative_ConvertErrorPalToPlatform")]
		internal static extern int ConvertErrorPalToPlatform(Error error);

		[DllImport("System.Native", EntryPoint = "SystemNative_StrErrorR")]
		private unsafe static extern byte* StrErrorR(int platformErrno, byte* buffer, int bufferSize);

		[DllImport("System.Native", EntryPoint = "SystemNative_Close", SetLastError = true)]
		internal static extern int Close(IntPtr fd);

		[DllImport("System.Native", EntryPoint = "SystemNative_FLock", SetLastError = true)]
		internal static extern int FLock(SafeFileHandle fd, LockOperations operation);

		[DllImport("System.Native", EntryPoint = "SystemNative_FLock", SetLastError = true)]
		internal static extern int FLock(IntPtr fd, LockOperations operation);

		[DllImport("System.Native", EntryPoint = "SystemNative_GetDomainSocketSizes")]
		internal static extern void GetDomainSocketSizes(out int pathOffset, out int pathSize, out int addressSize);

		[DllImport("System.Native", EntryPoint = "SystemNative_GetEUid")]
		internal static extern uint GetEUid();

		[DllImport("System.Native", EntryPoint = "SystemNative_GetHostName", SetLastError = true)]
		private unsafe static extern int GetHostName(byte* name, int nameLength);

		internal unsafe static string GetHostName()
		{
			byte* num = stackalloc byte[256];
			int hostName = GetHostName(num, 256);
			if (hostName != 0)
			{
				throw new InvalidOperationException($"gethostname returned {hostName}");
			}
			num[255] = 0;
			return Marshal.PtrToStringAnsi((IntPtr)num);
		}

		[DllImport("System.Native", EntryPoint = "SystemNative_GetPeerID", SetLastError = true)]
		internal static extern int GetPeerID(SafeHandle socket, out uint euid);

		[DllImport("System.Native", EntryPoint = "SystemNative_GetPeerUserName", SetLastError = true)]
		internal static extern string GetPeerUserName(SafeHandle socket);

		[DllImport("System.Native", EntryPoint = "SystemNative_MkDir", SetLastError = true)]
		internal static extern int MkDir(string path, int mode);

		[DllImport("System.Native", EntryPoint = "SystemNative_Open", SetLastError = true)]
		internal static extern SafeFileHandle Open(string filename, OpenFlags flags, int mode);

		[DllImport("System.Native", EntryPoint = "SystemNative_Pipe", SetLastError = true)]
		internal unsafe static extern int Pipe(int* pipefd, PipeFlags flags = (PipeFlags)0);

		[DllImport("System.Native", EntryPoint = "SystemNative_Poll")]
		internal unsafe static extern Error Poll(PollEvent* pollEvents, uint eventCount, int timeout, uint* triggered);

		internal unsafe static Error Poll(SafeHandle fd, PollEvents events, int timeout, out PollEvents triggered)
		{
			bool success = false;
			try
			{
				fd.DangerousAddRef(ref success);
				PollEvent pollEvent = new PollEvent
				{
					FileDescriptor = fd.DangerousGetHandle().ToInt32(),
					Events = events
				};
				uint num = default(uint);
				Error result = Poll(&pollEvent, 1u, timeout, &num);
				triggered = pollEvent.TriggeredEvents;
				return result;
			}
			finally
			{
				if (success)
				{
					fd.DangerousRelease();
				}
			}
		}

		[DllImport("System.Native", EntryPoint = "SystemNative_Read", SetLastError = true)]
		internal unsafe static extern int Read(SafePipeHandle fd, byte* buffer, int count);

		[DllImport("System.Native", EntryPoint = "SystemNative_SetEUid")]
		internal static extern int SetEUid(uint euid);

		[DllImport("System.Native", EntryPoint = "SystemNative_FStat2", SetLastError = true)]
		internal static extern int FStat(SafePipeHandle fd, out FileStatus output);

		[DllImport("System.Native", EntryPoint = "SystemNative_FStat2", SetLastError = true)]
		internal static extern int FStat(SafeFileHandle fd, out FileStatus output);

		[DllImport("System.Native", EntryPoint = "SystemNative_Stat2", SetLastError = true)]
		internal static extern int Stat(string path, out FileStatus output);

		[DllImport("System.Native", EntryPoint = "SystemNative_LStat2", SetLastError = true)]
		internal static extern int LStat(string path, out FileStatus output);

		[DllImport("System.Native", EntryPoint = "SystemNative_Unlink", SetLastError = true)]
		internal static extern int Unlink(string pathname);

		[DllImport("System.Native", EntryPoint = "SystemNative_Write", SetLastError = true)]
		internal unsafe static extern int Write(SafePipeHandle fd, byte* buffer, int bufferSize);
	}

	internal static class Libraries
	{
		internal const string SystemNative = "System.Native";

		internal const string HttpNative = "System.Net.Http.Native";

		internal const string NetSecurityNative = "System.Net.Security.Native";

		internal const string CryptoNative = "System.Security.Cryptography.Native.OpenSsl";

		internal const string CompressionNative = "System.IO.Compression.Native";

		internal const string Libdl = "libdl";
	}

	private static void ThrowExceptionForIoErrno(ErrorInfo errorInfo, string path, bool isDirectory, Func<ErrorInfo, ErrorInfo> errorRewriter)
	{
		if (errorRewriter != null)
		{
			errorInfo = errorRewriter(errorInfo);
		}
		throw GetExceptionForIoErrno(errorInfo, path, isDirectory);
	}

	internal static void CheckIo(Error error, string path = null, bool isDirectory = false, Func<ErrorInfo, ErrorInfo> errorRewriter = null)
	{
		if (error != Error.SUCCESS)
		{
			ThrowExceptionForIoErrno(error.Info(), path, isDirectory, errorRewriter);
		}
	}

	internal static long CheckIo(long result, string path = null, bool isDirectory = false, Func<ErrorInfo, ErrorInfo> errorRewriter = null)
	{
		if (result < 0)
		{
			ThrowExceptionForIoErrno(Sys.GetLastErrorInfo(), path, isDirectory, errorRewriter);
		}
		return result;
	}

	internal static int CheckIo(int result, string path = null, bool isDirectory = false, Func<ErrorInfo, ErrorInfo> errorRewriter = null)
	{
		CheckIo((long)result, path, isDirectory, errorRewriter);
		return result;
	}

	internal static IntPtr CheckIo(IntPtr result, string path = null, bool isDirectory = false, Func<ErrorInfo, ErrorInfo> errorRewriter = null)
	{
		CheckIo((long)result, path, isDirectory, errorRewriter);
		return result;
	}

	internal static TSafeHandle CheckIo<TSafeHandle>(TSafeHandle handle, string path = null, bool isDirectory = false, Func<ErrorInfo, ErrorInfo> errorRewriter = null) where TSafeHandle : SafeHandle
	{
		if (handle.IsInvalid)
		{
			ThrowExceptionForIoErrno(Sys.GetLastErrorInfo(), path, isDirectory, errorRewriter);
		}
		return handle;
	}

	internal static Exception GetExceptionForIoErrno(ErrorInfo errorInfo, string path = null, bool isDirectory = false)
	{
		switch (errorInfo.Error)
		{
		case Error.ENOENT:
			if (isDirectory)
			{
				if (string.IsNullOrEmpty(path))
				{
					return new DirectoryNotFoundException("Could not find a part of the path.");
				}
				return new DirectoryNotFoundException(SR.Format("Could not find a part of the path '{0}'.", path));
			}
			if (string.IsNullOrEmpty(path))
			{
				return new FileNotFoundException("Unable to find the specified file.");
			}
			return new FileNotFoundException(SR.Format("Could not find file '{0}'.", path), path);
		case Error.EACCES:
		case Error.EBADF:
		case Error.EPERM:
		{
			Exception iOException = GetIOException(errorInfo);
			if (string.IsNullOrEmpty(path))
			{
				return new UnauthorizedAccessException("Access to the path is denied.", iOException);
			}
			return new UnauthorizedAccessException(SR.Format("Access to the path '{0}' is denied.", path), iOException);
		}
		case Error.ENAMETOOLONG:
			if (string.IsNullOrEmpty(path))
			{
				return new PathTooLongException("The specified file name or path is too long, or a component of the specified path is too long.");
			}
			return new PathTooLongException(SR.Format("The path '{0}' is too long, or a component of the specified path is too long.", path));
		case Error.EAGAIN:
			if (string.IsNullOrEmpty(path))
			{
				return new IOException("The process cannot access the file because it is being used by another process.", errorInfo.RawErrno);
			}
			return new IOException(SR.Format("The process cannot access the file '{0}' because it is being used by another process.", path), errorInfo.RawErrno);
		case Error.ECANCELED:
			return new OperationCanceledException();
		case Error.EFBIG:
			return new ArgumentOutOfRangeException("value", "Specified file length was too large for the file system.");
		case Error.EEXIST:
			if (!string.IsNullOrEmpty(path))
			{
				return new IOException(SR.Format("The file '{0}' already exists.", path), errorInfo.RawErrno);
			}
			break;
		}
		return GetIOException(errorInfo);
	}

	internal static Exception GetIOException(ErrorInfo errorInfo)
	{
		return new IOException(errorInfo.GetErrorMessage(), errorInfo.RawErrno);
	}
}
