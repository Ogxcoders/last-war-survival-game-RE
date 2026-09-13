using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32.SafeHandles;

internal static class Interop
{
	internal static class Sys
	{
		internal enum NodeType
		{
			DT_UNKNOWN = 0,
			DT_FIFO = 1,
			DT_CHR = 2,
			DT_DIR = 4,
			DT_BLK = 6,
			DT_REG = 8,
			DT_LNK = 10,
			DT_SOCK = 12,
			DT_WHT = 14
		}

		internal struct DirectoryEntry
		{
			internal unsafe byte* Name;

			internal int NameLength;

			internal NodeType InodeType;

			internal const int NameBufferSize = 256;

			internal unsafe ReadOnlySpan<char> GetName(Span<char> buffer)
			{
				ReadOnlySpan<byte> bytes = ((NameLength == -1) ? new ReadOnlySpan<byte>(Name, new ReadOnlySpan<byte>(Name, 256).IndexOf<byte>(0)) : new ReadOnlySpan<byte>(Name, NameLength));
				return buffer[..Encoding.UTF8.GetChars(bytes, buffer)];
			}
		}

		[Flags]
		internal enum NotifyEvents
		{
			IN_ACCESS = 1,
			IN_MODIFY = 2,
			IN_ATTRIB = 4,
			IN_MOVED_FROM = 0x40,
			IN_MOVED_TO = 0x80,
			IN_CREATE = 0x100,
			IN_DELETE = 0x200,
			IN_Q_OVERFLOW = 0x4000,
			IN_IGNORED = 0x8000,
			IN_ONLYDIR = 0x1000000,
			IN_DONT_FOLLOW = 0x2000000,
			IN_EXCL_UNLINK = 0x4000000,
			IN_ISDIR = 0x40000000
		}

		internal enum LockOperations
		{
			LOCK_SH = 1,
			LOCK_EX = 2,
			LOCK_NB = 4,
			LOCK_UN = 8
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

		[DllImport("System.Native", EntryPoint = "SystemNative_GetReadDirRBufferSize")]
		internal static extern int GetReadDirRBufferSize();

		[DllImport("System.Native", EntryPoint = "SystemNative_INotifyInit", SetLastError = true)]
		internal static extern SafeFileHandle INotifyInit();

		[DllImport("System.Native", EntryPoint = "SystemNative_INotifyAddWatch", SetLastError = true)]
		internal static extern int INotifyAddWatch(SafeFileHandle fd, string pathName, uint mask);

		[DllImport("System.Native", EntryPoint = "SystemNative_INotifyRemoveWatch", SetLastError = true)]
		private static extern int INotifyRemoveWatch_private(SafeFileHandle fd, int wd);

		internal static int INotifyRemoveWatch(SafeFileHandle fd, int wd)
		{
			int num = INotifyRemoveWatch_private(fd, wd);
			if (num < 0 && GetLastError() == Error.EINVAL)
			{
				num = 0;
			}
			return num;
		}

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

		[DllImport("System.Native", EntryPoint = "SystemNative_Open", SetLastError = true)]
		internal static extern SafeFileHandle Open(string filename, OpenFlags flags, int mode);

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

		[DllImport("System.Native", EntryPoint = "SystemNative_FStat2", SetLastError = true)]
		internal static extern int FStat(SafeFileHandle fd, out FileStatus output);

		[DllImport("System.Native", EntryPoint = "SystemNative_Stat2", SetLastError = true)]
		internal static extern int Stat(string path, out FileStatus output);

		[DllImport("System.Native", EntryPoint = "SystemNative_LStat2", SetLastError = true)]
		internal static extern int LStat(string path, out FileStatus output);

		[DllImport("System.Native", EntryPoint = "SystemNative_OpenDir", SetLastError = true)]
		internal static extern IntPtr OpenDir_native(string path);

		[DllImport("System.Native", EntryPoint = "SystemNative_ReadDirR")]
		internal unsafe static extern int ReadDirR_native(IntPtr dir, byte* buffer, int bufferSize, out DirectoryEntry outputEntry);

		[DllImport("System.Native", EntryPoint = "SystemNative_CloseDir", SetLastError = true)]
		internal static extern int CloseDir_native(IntPtr dir);

		internal static IntPtr OpenDir(string path)
		{
			IntPtr intPtr;
			do
			{
				intPtr = OpenDir_native(path);
			}
			while (intPtr == IntPtr.Zero && Marshal.GetLastWin32Error() == 65563);
			return intPtr;
		}

		internal static int CloseDir(IntPtr dir)
		{
			int num;
			do
			{
				num = CloseDir_native(dir);
			}
			while (num < 0 && Marshal.GetLastWin32Error() == 65563);
			return num;
		}

		internal unsafe static int ReadDirR(IntPtr dir, byte* buffer, int bufferSize, out DirectoryEntry outputEntry)
		{
			int num;
			do
			{
				num = ReadDirR_native(dir, buffer, bufferSize, out outputEntry);
			}
			while (num == 65563);
			return num;
		}

		static Sys()
		{
			mono_pal_init();
		}

		internal unsafe static int Read(SafeFileHandle fd, byte* buffer, int count)
		{
			int num = -1;
			bool success = false;
			try
			{
				fd.DangerousAddRef(ref success);
				do
				{
					num = Read(fd.DangerousGetHandle(), buffer, count);
				}
				while (num < 0 && Marshal.GetLastWin32Error() == 65563);
			}
			finally
			{
				if (success)
				{
					fd.DangerousRelease();
				}
			}
			return num;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal unsafe static extern int Read(IntPtr fd, byte* buffer, int count);
	}

	internal static class Libraries
	{
		internal const string Odbc32 = "libodbc.so.2";

		internal const string SystemNative = "System.Native";

		internal const string HttpNative = "System.Net.Http.Native";

		internal const string NetSecurityNative = "System.Net.Security.Native";

		internal const string CryptoNative = "System.Security.Cryptography.Native.OpenSsl";

		internal const string CompressionNative = "System.IO.Compression.Native";

		internal const string Libdl = "libdl";
	}

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

	internal static class NetSecurityNative
	{
		internal sealed class GssApiException : Exception
		{
			private readonly Status _minorStatus;

			public Status MajorStatus => (Status)base.HResult;

			public Status MinorStatus => _minorStatus;

			public GssApiException(string message)
				: base(message)
			{
			}

			public GssApiException(Status majorStatus, Status minorStatus)
				: base(GetGssApiDisplayStatus(majorStatus, minorStatus, null))
			{
				base.HResult = (int)majorStatus;
				_minorStatus = minorStatus;
			}

			public GssApiException(Status majorStatus, Status minorStatus, string helpText)
				: base(GetGssApiDisplayStatus(majorStatus, minorStatus, helpText))
			{
				base.HResult = (int)majorStatus;
				_minorStatus = minorStatus;
			}

			private static string GetGssApiDisplayStatus(Status majorStatus, Status minorStatus, string helpText)
			{
				string gssApiDisplayStatus = GetGssApiDisplayStatus(majorStatus, isMinor: false);
				string text;
				if (minorStatus != Status.GSS_S_COMPLETE)
				{
					string gssApiDisplayStatus2 = GetGssApiDisplayStatus(minorStatus, isMinor: true);
					text = ((gssApiDisplayStatus != null && gssApiDisplayStatus2 != null) ? global::SR.Format("GSSAPI operation failed with error - {0} ({1}).", gssApiDisplayStatus, gssApiDisplayStatus2) : global::SR.Format("GSSAPI operation failed with status: {0} (Minor status: {1}).", majorStatus.ToString("x"), minorStatus.ToString("x")));
				}
				else
				{
					text = ((gssApiDisplayStatus != null) ? global::SR.Format("GSSAPI operation failed with error - {0}.", gssApiDisplayStatus) : global::SR.Format("SSAPI operation failed with status: {0}.", majorStatus.ToString("x")));
				}
				if (!string.IsNullOrEmpty(helpText))
				{
					return text + " " + helpText;
				}
				return text;
			}

			private static string GetGssApiDisplayStatus(Status status, bool isMinor)
			{
				GssBuffer buffer = default(GssBuffer);
				try
				{
					Status minorStatus;
					return ((isMinor ? DisplayMinorStatus(out minorStatus, status, ref buffer) : DisplayMajorStatus(out minorStatus, status, ref buffer)) != Status.GSS_S_COMPLETE) ? null : Marshal.PtrToStringAnsi(buffer._data);
				}
				finally
				{
					buffer.Dispose();
				}
			}
		}

		internal struct GssBuffer : IDisposable
		{
			internal ulong _length;

			internal IntPtr _data;

			internal int Copy(byte[] destination, int offset)
			{
				if (_data == IntPtr.Zero || _length == 0L)
				{
					return 0;
				}
				int num = Convert.ToInt32(_length);
				int num2 = destination.Length - offset;
				if (num > num2)
				{
					throw new GssApiException(global::SR.Format("Insufficient buffer space. Required: {0} Actual: {1}.", num, num2));
				}
				Marshal.Copy(_data, destination, offset, num);
				return num;
			}

			internal byte[] ToByteArray()
			{
				if (_data == IntPtr.Zero || _length == 0L)
				{
					return Array.Empty<byte>();
				}
				int num = Convert.ToInt32(_length);
				byte[] array = new byte[num];
				Marshal.Copy(_data, array, 0, num);
				return array;
			}

			public void Dispose()
			{
				if (_data != IntPtr.Zero)
				{
					ReleaseGssBuffer(_data, _length);
					_data = IntPtr.Zero;
				}
				_length = 0uL;
			}
		}

		internal enum Status : uint
		{
			GSS_S_COMPLETE = 0u,
			GSS_S_CONTINUE_NEEDED = 1u,
			GSS_S_BAD_MECH = 65536u,
			GSS_S_BAD_NAME = 131072u,
			GSS_S_BAD_NAMETYPE = 196608u,
			GSS_S_BAD_BINDINGS = 262144u,
			GSS_S_BAD_STATUS = 327680u,
			GSS_S_BAD_SIG = 393216u,
			GSS_S_NO_CRED = 458752u,
			GSS_S_NO_CONTEXT = 524288u,
			GSS_S_DEFECTIVE_TOKEN = 589824u,
			GSS_S_DEFECTIVE_CREDENTIAL = 655360u,
			GSS_S_CREDENTIALS_EXPIRED = 720896u,
			GSS_S_CONTEXT_EXPIRED = 786432u,
			GSS_S_FAILURE = 851968u,
			GSS_S_BAD_QOP = 917504u,
			GSS_S_UNAUTHORIZED = 983040u,
			GSS_S_UNAVAILABLE = 1048576u,
			GSS_S_DUPLICATE_ELEMENT = 1114112u,
			GSS_S_NAME_NOT_MN = 1179648u
		}

		[Flags]
		internal enum GssFlags : uint
		{
			GSS_C_DELEG_FLAG = 1u,
			GSS_C_MUTUAL_FLAG = 2u,
			GSS_C_REPLAY_FLAG = 4u,
			GSS_C_SEQUENCE_FLAG = 8u,
			GSS_C_CONF_FLAG = 0x10u,
			GSS_C_INTEG_FLAG = 0x20u,
			GSS_C_ANON_FLAG = 0x40u,
			GSS_C_PROT_READY_FLAG = 0x80u,
			GSS_C_TRANS_FLAG = 0x100u,
			GSS_C_DCE_STYLE = 0x1000u,
			GSS_C_IDENTIFY_FLAG = 0x2000u,
			GSS_C_EXTENDED_ERROR_FLAG = 0x4000u,
			GSS_C_DELEG_POLICY_FLAG = 0x8000u
		}

		internal const int GSS_C_ROUTINE_ERROR_OFFSET = 16;

		[DllImport("System.Net.Security.Native", EntryPoint = "NetSecurityNative_ReleaseGssBuffer")]
		internal static extern void ReleaseGssBuffer(IntPtr bufferPtr, ulong length);

		[DllImport("System.Net.Security.Native", EntryPoint = "NetSecurityNative_DisplayMinorStatus")]
		internal static extern Status DisplayMinorStatus(out Status minorStatus, Status statusValue, ref GssBuffer buffer);

		[DllImport("System.Net.Security.Native", EntryPoint = "NetSecurityNative_DisplayMajorStatus")]
		internal static extern Status DisplayMajorStatus(out Status minorStatus, Status statusValue, ref GssBuffer buffer);

		[DllImport("System.Net.Security.Native", EntryPoint = "NetSecurityNative_ImportUserName")]
		internal static extern Status ImportUserName(out Status minorStatus, string inputName, int inputNameByteCount, out SafeGssNameHandle outputName);

		[DllImport("System.Net.Security.Native", EntryPoint = "NetSecurityNative_ImportPrincipalName")]
		internal static extern Status ImportPrincipalName(out Status minorStatus, string inputName, int inputNameByteCount, out SafeGssNameHandle outputName);

		[DllImport("System.Net.Security.Native", EntryPoint = "NetSecurityNative_ReleaseName")]
		internal static extern Status ReleaseName(out Status minorStatus, ref IntPtr inputName);

		[DllImport("System.Net.Security.Native", EntryPoint = "NetSecurityNative_InitiateCredSpNego")]
		internal static extern Status InitiateCredSpNego(out Status minorStatus, SafeGssNameHandle desiredName, out SafeGssCredHandle outputCredHandle);

		[DllImport("System.Net.Security.Native", EntryPoint = "NetSecurityNative_InitiateCredWithPassword")]
		internal static extern Status InitiateCredWithPassword(out Status minorStatus, bool isNtlm, SafeGssNameHandle desiredName, string password, int passwordLen, out SafeGssCredHandle outputCredHandle);

		[DllImport("System.Net.Security.Native", EntryPoint = "NetSecurityNative_ReleaseCred")]
		internal static extern Status ReleaseCred(out Status minorStatus, ref IntPtr credHandle);

		[DllImport("System.Net.Security.Native", EntryPoint = "NetSecurityNative_InitSecContext")]
		internal static extern Status InitSecContext(out Status minorStatus, SafeGssCredHandle initiatorCredHandle, ref SafeGssContextHandle contextHandle, bool isNtlmOnly, SafeGssNameHandle targetName, uint reqFlags, byte[] inputBytes, int inputLength, ref GssBuffer token, out uint retFlags, out int isNtlmUsed);

		[DllImport("System.Net.Security.Native", EntryPoint = "NetSecurityNative_InitSecContextEx")]
		internal static extern Status InitSecContext(out Status minorStatus, SafeGssCredHandle initiatorCredHandle, ref SafeGssContextHandle contextHandle, bool isNtlmOnly, IntPtr cbt, int cbtSize, SafeGssNameHandle targetName, uint reqFlags, byte[] inputBytes, int inputLength, ref GssBuffer token, out uint retFlags, out int isNtlmUsed);

		[DllImport("System.Net.Security.Native", EntryPoint = "NetSecurityNative_AcceptSecContext")]
		internal static extern Status AcceptSecContext(out Status minorStatus, ref SafeGssContextHandle acceptContextHandle, byte[] inputBytes, int inputLength, ref GssBuffer token, out uint retFlags);

		[DllImport("System.Net.Security.Native", EntryPoint = "NetSecurityNative_DeleteSecContext")]
		internal static extern Status DeleteSecContext(out Status minorStatus, ref IntPtr contextHandle);

		[DllImport("System.Net.Security.Native", EntryPoint = "NetSecurityNative_GetUser")]
		internal static extern Status GetUser(out Status minorStatus, SafeGssContextHandle acceptContextHandle, ref GssBuffer token);

		[DllImport("System.Net.Security.Native", EntryPoint = "NetSecurityNative_Wrap")]
		private static extern Status Wrap(out Status minorStatus, SafeGssContextHandle contextHandle, bool isEncrypt, byte[] inputBytes, int offset, int count, ref GssBuffer outBuffer);

		[DllImport("System.Net.Security.Native", EntryPoint = "NetSecurityNative_Unwrap")]
		private static extern Status Unwrap(out Status minorStatus, SafeGssContextHandle contextHandle, byte[] inputBytes, int offset, int count, ref GssBuffer outBuffer);

		internal static Status WrapBuffer(out Status minorStatus, SafeGssContextHandle contextHandle, bool isEncrypt, byte[] inputBytes, int offset, int count, ref GssBuffer outBuffer)
		{
			return Wrap(out minorStatus, contextHandle, isEncrypt, inputBytes, offset, count, ref outBuffer);
		}

		internal static Status UnwrapBuffer(out Status minorStatus, SafeGssContextHandle contextHandle, byte[] inputBytes, int offset, int count, ref GssBuffer outBuffer)
		{
			return Unwrap(out minorStatus, contextHandle, inputBytes, offset, count, ref outBuffer);
		}
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
				return new DirectoryNotFoundException(global::SR.Format("Could not find a part of the path '{0}'.", path));
			}
			if (string.IsNullOrEmpty(path))
			{
				return new FileNotFoundException("Unable to find the specified file.");
			}
			return new FileNotFoundException(global::SR.Format("Could not find file '{0}'.", path), path);
		case Error.EACCES:
		case Error.EBADF:
		case Error.EPERM:
		{
			Exception iOException = GetIOException(errorInfo);
			if (string.IsNullOrEmpty(path))
			{
				return new UnauthorizedAccessException("Access to the path is denied.", iOException);
			}
			return new UnauthorizedAccessException(global::SR.Format("Access to the path '{0}' is denied.", path), iOException);
		}
		case Error.ENAMETOOLONG:
			if (string.IsNullOrEmpty(path))
			{
				return new PathTooLongException("The specified file name or path is too long, or a component of the specified path is too long.");
			}
			return new PathTooLongException(global::SR.Format("The path '{0}' is too long, or a component of the specified path is too long.", path));
		case Error.EAGAIN:
			if (string.IsNullOrEmpty(path))
			{
				return new IOException("The process cannot access the file because it is being used by another process.", errorInfo.RawErrno);
			}
			return new IOException(global::SR.Format("The process cannot access the file '{0}' because it is being used by another process.", path), errorInfo.RawErrno);
		case Error.ECANCELED:
			return new OperationCanceledException();
		case Error.EFBIG:
			return new ArgumentOutOfRangeException("value", "Specified file length was too large for the file system.");
		case Error.EEXIST:
			if (!string.IsNullOrEmpty(path))
			{
				return new IOException(global::SR.Format("The file '{0}' already exists.", path), errorInfo.RawErrno);
			}
			break;
		}
		return GetIOException(errorInfo);
	}

	internal static Exception GetIOException(ErrorInfo errorInfo)
	{
		return new IOException(errorInfo.GetErrorMessage(), errorInfo.RawErrno);
	}

	[DllImport("System.Native")]
	internal static extern void mono_pal_init();
}
