using Microsoft.Win32.SafeHandles;

namespace System.IO.MemoryMappedFiles;

public class MemoryMappedFile : IDisposable
{
	private FileStream stream;

	private bool keepOpen;

	private SafeMemoryMappedFileHandle handle;

	public SafeMemoryMappedFileHandle SafeMemoryMappedFileHandle => handle;

	public static MemoryMappedFile CreateFromFile(string path)
	{
		return CreateFromFile(path, FileMode.Open, null, 0L, MemoryMappedFileAccess.ReadWrite);
	}

	public static MemoryMappedFile CreateFromFile(string path, FileMode mode)
	{
		long capacity = 0L;
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		if (path.Length == 0)
		{
			throw new ArgumentException("path");
		}
		if (mode == FileMode.Append)
		{
			throw new ArgumentException("mode");
		}
		IntPtr preexistingHandle = MemoryMapImpl.OpenFile(path, mode, null, out capacity, MemoryMappedFileAccess.ReadWrite, MemoryMappedFileOptions.None);
		return new MemoryMappedFile
		{
			handle = new SafeMemoryMappedFileHandle(preexistingHandle, ownsHandle: true)
		};
	}

	public static MemoryMappedFile CreateFromFile(string path, FileMode mode, string mapName)
	{
		return CreateFromFile(path, mode, mapName, 0L, MemoryMappedFileAccess.ReadWrite);
	}

	public static MemoryMappedFile CreateFromFile(string path, FileMode mode, string mapName, long capacity)
	{
		return CreateFromFile(path, mode, mapName, capacity, MemoryMappedFileAccess.ReadWrite);
	}

	public static MemoryMappedFile CreateFromFile(string path, FileMode mode, string mapName, long capacity, MemoryMappedFileAccess access)
	{
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		if (path.Length == 0)
		{
			throw new ArgumentException("path");
		}
		if (mapName != null && mapName.Length == 0)
		{
			throw new ArgumentException("mapName");
		}
		if (mode == FileMode.Append)
		{
			throw new ArgumentException("mode");
		}
		if (capacity < 0)
		{
			throw new ArgumentOutOfRangeException("capacity");
		}
		IntPtr preexistingHandle = MemoryMapImpl.OpenFile(path, mode, mapName, out capacity, access, MemoryMappedFileOptions.None);
		return new MemoryMappedFile
		{
			handle = new SafeMemoryMappedFileHandle(preexistingHandle, ownsHandle: true)
		};
	}

	public static MemoryMappedFile CreateFromFile(FileStream fileStream, string mapName, long capacity, MemoryMappedFileAccess access, HandleInheritability inheritability, bool leaveOpen)
	{
		if (fileStream == null)
		{
			throw new ArgumentNullException("fileStream");
		}
		if (mapName != null && mapName.Length == 0)
		{
			throw new ArgumentException("mapName");
		}
		if ((!MonoUtil.IsUnix && capacity == 0L && fileStream.Length == 0L) || capacity > fileStream.Length)
		{
			throw new ArgumentException("capacity");
		}
		IntPtr preexistingHandle = MemoryMapImpl.OpenHandle(fileStream.SafeFileHandle.DangerousGetHandle(), mapName, out capacity, access, MemoryMappedFileOptions.None);
		MemoryMapImpl.ConfigureHandleInheritability(preexistingHandle, inheritability);
		return new MemoryMappedFile
		{
			handle = new SafeMemoryMappedFileHandle(preexistingHandle, ownsHandle: true),
			stream = fileStream,
			keepOpen = leaveOpen
		};
	}

	[MonoLimitation("memoryMappedFileSecurity is currently ignored")]
	public static MemoryMappedFile CreateFromFile(FileStream fileStream, string mapName, long capacity, MemoryMappedFileAccess access, MemoryMappedFileSecurity memoryMappedFileSecurity, HandleInheritability inheritability, bool leaveOpen)
	{
		if (fileStream == null)
		{
			throw new ArgumentNullException("fileStream");
		}
		if (mapName != null && mapName.Length == 0)
		{
			throw new ArgumentException("mapName");
		}
		if ((!MonoUtil.IsUnix && capacity == 0L && fileStream.Length == 0L) || capacity > fileStream.Length)
		{
			throw new ArgumentException("capacity");
		}
		IntPtr preexistingHandle = MemoryMapImpl.OpenHandle(fileStream.SafeFileHandle.DangerousGetHandle(), mapName, out capacity, access, MemoryMappedFileOptions.None);
		MemoryMapImpl.ConfigureHandleInheritability(preexistingHandle, inheritability);
		return new MemoryMappedFile
		{
			handle = new SafeMemoryMappedFileHandle(preexistingHandle, ownsHandle: true),
			stream = fileStream,
			keepOpen = leaveOpen
		};
	}

	private static MemoryMappedFile CoreShmCreate(string mapName, long capacity, MemoryMappedFileAccess access, MemoryMappedFileOptions options, MemoryMappedFileSecurity memoryMappedFileSecurity, HandleInheritability inheritability, FileMode mode)
	{
		if (mapName != null && mapName.Length == 0)
		{
			throw new ArgumentException("mapName");
		}
		if (capacity < 0)
		{
			throw new ArgumentOutOfRangeException("capacity");
		}
		IntPtr preexistingHandle = MemoryMapImpl.OpenFile(null, mode, mapName, out capacity, access, options);
		return new MemoryMappedFile
		{
			handle = new SafeMemoryMappedFileHandle(preexistingHandle, ownsHandle: true)
		};
	}

	[MonoLimitation("Named mappings scope is process local")]
	public static MemoryMappedFile CreateNew(string mapName, long capacity)
	{
		return CreateNew(mapName, capacity, MemoryMappedFileAccess.ReadWrite, MemoryMappedFileOptions.None, null, HandleInheritability.None);
	}

	[MonoLimitation("Named mappings scope is process local")]
	public static MemoryMappedFile CreateNew(string mapName, long capacity, MemoryMappedFileAccess access)
	{
		return CreateNew(mapName, capacity, access, MemoryMappedFileOptions.None, null, HandleInheritability.None);
	}

	[MonoLimitation("Named mappings scope is process local; options is ignored")]
	public static MemoryMappedFile CreateNew(string mapName, long capacity, MemoryMappedFileAccess access, MemoryMappedFileOptions options, HandleInheritability inheritability)
	{
		return CreateNew(mapName, capacity, access, options, null, inheritability);
	}

	[MonoLimitation("Named mappings scope is process local; options and memoryMappedFileSecurity are ignored")]
	public static MemoryMappedFile CreateNew(string mapName, long capacity, MemoryMappedFileAccess access, MemoryMappedFileOptions options, MemoryMappedFileSecurity memoryMappedFileSecurity, HandleInheritability inheritability)
	{
		return CoreShmCreate(mapName, capacity, access, options, memoryMappedFileSecurity, inheritability, FileMode.CreateNew);
	}

	[MonoLimitation("Named mappings scope is process local")]
	public static MemoryMappedFile CreateOrOpen(string mapName, long capacity)
	{
		return CreateOrOpen(mapName, capacity, MemoryMappedFileAccess.ReadWrite);
	}

	[MonoLimitation("Named mappings scope is process local")]
	public static MemoryMappedFile CreateOrOpen(string mapName, long capacity, MemoryMappedFileAccess access)
	{
		return CreateOrOpen(mapName, capacity, access, MemoryMappedFileOptions.None, null, HandleInheritability.None);
	}

	[MonoLimitation("Named mappings scope is process local")]
	public static MemoryMappedFile CreateOrOpen(string mapName, long capacity, MemoryMappedFileAccess access, MemoryMappedFileOptions options, HandleInheritability inheritability)
	{
		return CreateOrOpen(mapName, capacity, access, options, null, inheritability);
	}

	[MonoLimitation("Named mappings scope is process local")]
	public static MemoryMappedFile CreateOrOpen(string mapName, long capacity, MemoryMappedFileAccess access, MemoryMappedFileOptions options, MemoryMappedFileSecurity memoryMappedFileSecurity, HandleInheritability inheritability)
	{
		return CoreShmCreate(mapName, capacity, access, options, memoryMappedFileSecurity, inheritability, FileMode.OpenOrCreate);
	}

	[MonoLimitation("Named mappings scope is process local")]
	public static MemoryMappedFile OpenExisting(string mapName)
	{
		return OpenExisting(mapName, MemoryMappedFileRights.ReadWrite);
	}

	[MonoLimitation("Named mappings scope is process local")]
	public static MemoryMappedFile OpenExisting(string mapName, MemoryMappedFileRights desiredAccessRights)
	{
		return OpenExisting(mapName, desiredAccessRights, HandleInheritability.None);
	}

	[MonoLimitation("Named mappings scope is process local")]
	public static MemoryMappedFile OpenExisting(string mapName, MemoryMappedFileRights desiredAccessRights, HandleInheritability inheritability)
	{
		return CoreShmCreate(mapName, 0L, MemoryMappedFileAccess.ReadWrite, MemoryMappedFileOptions.None, null, inheritability, FileMode.Open);
	}

	public MemoryMappedViewStream CreateViewStream()
	{
		return CreateViewStream(0L, 0L);
	}

	public MemoryMappedViewStream CreateViewStream(long offset, long size)
	{
		return CreateViewStream(offset, size, MemoryMappedFileAccess.ReadWrite);
	}

	public MemoryMappedViewStream CreateViewStream(long offset, long size, MemoryMappedFileAccess access)
	{
		return new MemoryMappedViewStream(MemoryMappedView.Create(handle.DangerousGetHandle(), offset, size, access));
	}

	public MemoryMappedViewAccessor CreateViewAccessor()
	{
		return CreateViewAccessor(0L, 0L);
	}

	public MemoryMappedViewAccessor CreateViewAccessor(long offset, long size)
	{
		return CreateViewAccessor(offset, size, MemoryMappedFileAccess.ReadWrite);
	}

	public MemoryMappedViewAccessor CreateViewAccessor(long offset, long size, MemoryMappedFileAccess access)
	{
		return new MemoryMappedViewAccessor(MemoryMappedView.Create(handle.DangerousGetHandle(), offset, size, access));
	}

	private MemoryMappedFile()
	{
	}

	public void Dispose()
	{
		Dispose(disposing: true);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (disposing && stream != null)
		{
			if (!keepOpen)
			{
				stream.Close();
			}
			stream = null;
		}
		if (handle != null)
		{
			handle.Dispose();
			handle = null;
		}
	}

	[MonoTODO]
	public MemoryMappedFileSecurity GetAccessControl()
	{
		throw new NotImplementedException();
	}

	[MonoTODO]
	public void SetAccessControl(MemoryMappedFileSecurity memoryMappedFileSecurity)
	{
		throw new NotImplementedException();
	}

	internal static FileAccess GetFileAccess(MemoryMappedFileAccess access)
	{
		return access switch
		{
			MemoryMappedFileAccess.Read => FileAccess.Read, 
			MemoryMappedFileAccess.Write => FileAccess.Write, 
			MemoryMappedFileAccess.ReadWrite => FileAccess.ReadWrite, 
			MemoryMappedFileAccess.CopyOnWrite => FileAccess.ReadWrite, 
			MemoryMappedFileAccess.ReadExecute => FileAccess.Read, 
			MemoryMappedFileAccess.ReadWriteExecute => FileAccess.ReadWrite, 
			_ => throw new ArgumentOutOfRangeException("access"), 
		};
	}
}
