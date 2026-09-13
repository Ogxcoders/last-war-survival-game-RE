using System.Security;
using Microsoft.Win32.SafeHandles;

namespace System.IO.MemoryMappedFiles;

internal class MemoryMappedView : IDisposable
{
	private SafeMemoryMappedViewHandle m_viewHandle;

	private long m_pointerOffset;

	private long m_size;

	private MemoryMappedFileAccess m_access;

	internal SafeMemoryMappedViewHandle ViewHandle
	{
		[SecurityCritical]
		get
		{
			return m_viewHandle;
		}
	}

	internal long PointerOffset => m_pointerOffset;

	internal long Size => m_size;

	internal MemoryMappedFileAccess Access => m_access;

	internal bool IsClosed
	{
		get
		{
			if (m_viewHandle != null)
			{
				return m_viewHandle.IsClosed;
			}
			return true;
		}
	}

	[SecurityCritical]
	private MemoryMappedView(SafeMemoryMappedViewHandle viewHandle, long pointerOffset, long size, MemoryMappedFileAccess access)
	{
		m_viewHandle = viewHandle;
		m_pointerOffset = pointerOffset;
		m_size = size;
		m_access = access;
	}

	internal static MemoryMappedView Create(IntPtr handle, long offset, long size, MemoryMappedFileAccess access)
	{
		MemoryMapImpl.Map(handle, offset, ref size, access, out var mmap_handle, out var base_address);
		return new MemoryMappedView(new SafeMemoryMappedViewHandle(mmap_handle, base_address, size), 0L, size, access);
	}

	public void Flush(IntPtr capacity)
	{
		m_viewHandle.Flush();
	}

	protected virtual void Dispose(bool disposing)
	{
		if (m_viewHandle != null && !m_viewHandle.IsClosed)
		{
			m_viewHandle.Dispose();
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
}
