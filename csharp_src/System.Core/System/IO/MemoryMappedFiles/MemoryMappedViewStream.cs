using System.Security;
using System.Security.Permissions;
using Microsoft.Win32.SafeHandles;

namespace System.IO.MemoryMappedFiles;

public sealed class MemoryMappedViewStream : UnmanagedMemoryStream
{
	private MemoryMappedView m_view;

	public SafeMemoryMappedViewHandle SafeMemoryMappedViewHandle
	{
		[SecurityCritical]
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		get
		{
			if (m_view == null)
			{
				return null;
			}
			return m_view.ViewHandle;
		}
	}

	public long PointerOffset
	{
		get
		{
			if (m_view == null)
			{
				throw new InvalidOperationException(SR.GetString("The underlying MemoryMappedView object is null."));
			}
			return m_view.PointerOffset;
		}
	}

	[SecurityCritical]
	internal MemoryMappedViewStream(MemoryMappedView view)
	{
		m_view = view;
		Initialize(m_view.ViewHandle, m_view.PointerOffset, m_view.Size, MemoryMappedFile.GetFileAccess(m_view.Access));
	}

	public override void SetLength(long value)
	{
		throw new NotSupportedException(SR.GetString("MemoryMappedViewStreams are fixed length."));
	}

	[SecuritySafeCritical]
	protected override void Dispose(bool disposing)
	{
		try
		{
			if (disposing && m_view != null && !m_view.IsClosed)
			{
				Flush();
			}
		}
		finally
		{
			try
			{
				if (m_view != null)
				{
					m_view.Dispose();
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}
	}

	[SecurityCritical]
	public override void Flush()
	{
		if (!CanSeek)
		{
			__Error.StreamIsClosed();
		}
		if (m_view != null)
		{
			m_view.Flush((IntPtr)base.Capacity);
		}
	}
}
