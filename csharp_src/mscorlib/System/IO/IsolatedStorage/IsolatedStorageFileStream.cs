using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.Win32.SafeHandles;

namespace System.IO.IsolatedStorage;

[ComVisible(true)]
public class IsolatedStorageFileStream : FileStream
{
	public override bool CanRead => base.CanRead;

	public override bool CanSeek => base.CanSeek;

	public override bool CanWrite => base.CanWrite;

	public override SafeFileHandle SafeFileHandle
	{
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		get
		{
			throw new IsolatedStorageException(Locale.GetText("Information is restricted"));
		}
	}

	[Obsolete("Use SafeFileHandle - once available")]
	public override IntPtr Handle
	{
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		get
		{
			throw new IsolatedStorageException(Locale.GetText("Information is restricted"));
		}
	}

	public override bool IsAsync => base.IsAsync;

	public override long Length => base.Length;

	public override long Position
	{
		get
		{
			return base.Position;
		}
		set
		{
			base.Position = value;
		}
	}

	[ReflectionPermission(SecurityAction.Assert, TypeInformation = true)]
	private static string CreateIsolatedPath(IsolatedStorageFile isf, string path, FileMode mode)
	{
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		if (!Enum.IsDefined(typeof(FileMode), mode))
		{
			throw new ArgumentException("mode");
		}
		if (isf == null)
		{
			isf = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly, null, null);
		}
		if (isf.IsDisposed)
		{
			throw new ObjectDisposedException("IsolatedStorageFile");
		}
		if (isf.IsClosed)
		{
			throw new InvalidOperationException("Storage needs to be open for this operation.");
		}
		FileInfo fileInfo = new FileInfo(isf.Root);
		if (!fileInfo.Directory.Exists)
		{
			fileInfo.Directory.Create();
		}
		if (Path.IsPathRooted(path))
		{
			string pathRoot = Path.GetPathRoot(path);
			path = path.Remove(0, pathRoot.Length);
		}
		string text = Path.Combine(isf.Root, path);
		Path.GetFullPath(text);
		if (!Path.GetFullPath(text).StartsWith(isf.Root))
		{
			throw new IsolatedStorageException();
		}
		fileInfo = new FileInfo(text);
		if (!fileInfo.Directory.Exists)
		{
			throw new DirectoryNotFoundException(string.Format(Locale.GetText("Could not find a part of the path \"{0}\"."), path));
		}
		return text;
	}

	public IsolatedStorageFileStream(string path, FileMode mode)
		: this(path, mode, (mode == FileMode.Append) ? FileAccess.Write : FileAccess.ReadWrite, FileShare.Read, 4096, null)
	{
	}

	public IsolatedStorageFileStream(string path, FileMode mode, FileAccess access)
		: this(path, mode, access, (access != FileAccess.Write) ? FileShare.Read : FileShare.None, 4096, null)
	{
	}

	public IsolatedStorageFileStream(string path, FileMode mode, FileAccess access, FileShare share)
		: this(path, mode, access, share, 4096, null)
	{
	}

	public IsolatedStorageFileStream(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize)
		: this(path, mode, access, share, bufferSize, null)
	{
	}

	[FileIOPermission(SecurityAction.Assert, Unrestricted = true)]
	public IsolatedStorageFileStream(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize, IsolatedStorageFile isf)
		: base(CreateIsolatedPath(isf, path, mode), mode, access, share, bufferSize, isAsync: false, anonymous: true)
	{
	}

	public IsolatedStorageFileStream(string path, FileMode mode, FileAccess access, FileShare share, IsolatedStorageFile isf)
		: this(path, mode, access, share, 4096, isf)
	{
	}

	public IsolatedStorageFileStream(string path, FileMode mode, FileAccess access, IsolatedStorageFile isf)
		: this(path, mode, access, (access != FileAccess.Write) ? FileShare.Read : FileShare.None, 4096, isf)
	{
	}

	public IsolatedStorageFileStream(string path, FileMode mode, IsolatedStorageFile isf)
		: this(path, mode, (mode == FileMode.Append) ? FileAccess.Write : FileAccess.ReadWrite, FileShare.Read, 4096, isf)
	{
	}

	public override IAsyncResult BeginRead(byte[] buffer, int offset, int numBytes, AsyncCallback userCallback, object stateObject)
	{
		return base.BeginRead(buffer, offset, numBytes, userCallback, stateObject);
	}

	public override IAsyncResult BeginWrite(byte[] buffer, int offset, int numBytes, AsyncCallback userCallback, object stateObject)
	{
		return base.BeginWrite(buffer, offset, numBytes, userCallback, stateObject);
	}

	public override int EndRead(IAsyncResult asyncResult)
	{
		return base.EndRead(asyncResult);
	}

	public override void EndWrite(IAsyncResult asyncResult)
	{
		base.EndWrite(asyncResult);
	}

	public override void Flush()
	{
		base.Flush();
	}

	public override void Flush(bool flushToDisk)
	{
		base.Flush(flushToDisk);
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		return base.Read(buffer, offset, count);
	}

	public override int ReadByte()
	{
		return base.ReadByte();
	}

	public override long Seek(long offset, SeekOrigin origin)
	{
		return base.Seek(offset, origin);
	}

	public override void SetLength(long value)
	{
		base.SetLength(value);
	}

	public override void Write(byte[] buffer, int offset, int count)
	{
		base.Write(buffer, offset, count);
	}

	public override void WriteByte(byte value)
	{
		base.WriteByte(value);
	}

	protected override void Dispose(bool disposing)
	{
		base.Dispose(disposing);
	}
}
