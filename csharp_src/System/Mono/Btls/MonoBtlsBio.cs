using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Mono.Btls;

internal class MonoBtlsBio : MonoBtlsObject
{
	protected internal class BoringBioHandle : MonoBtlsHandle
	{
		public BoringBioHandle(IntPtr handle)
			: base(handle, ownsHandle: true)
		{
		}

		protected override bool ReleaseHandle()
		{
			if (handle != IntPtr.Zero)
			{
				mono_btls_bio_free(handle);
				handle = IntPtr.Zero;
			}
			return true;
		}
	}

	protected internal new BoringBioHandle Handle => (BoringBioHandle)base.Handle;

	internal MonoBtlsBio(BoringBioHandle handle)
		: base(handle)
	{
	}

	public static MonoBtlsBio CreateMonoStream(Stream stream)
	{
		return MonoBtlsBioMono.CreateStream(stream, ownsStream: false);
	}

	[DllImport("libmono-btls-shared")]
	private static extern int mono_btls_bio_read(IntPtr bio, IntPtr data, int len);

	[DllImport("libmono-btls-shared")]
	private static extern int mono_btls_bio_write(IntPtr bio, IntPtr data, int len);

	[DllImport("libmono-btls-shared")]
	private static extern int mono_btls_bio_flush(IntPtr bio);

	[DllImport("libmono-btls-shared")]
	private static extern int mono_btls_bio_indent(IntPtr bio, uint indent, uint max_indent);

	[DllImport("libmono-btls-shared")]
	private static extern int mono_btls_bio_hexdump(IntPtr bio, IntPtr data, int len, uint indent);

	[DllImport("libmono-btls-shared")]
	private static extern void mono_btls_bio_print_errors(IntPtr bio);

	[DllImport("libmono-btls-shared")]
	private static extern void mono_btls_bio_free(IntPtr handle);

	public int Read(byte[] buffer, int offset, int size)
	{
		CheckThrow();
		IntPtr intPtr = Marshal.AllocHGlobal(size);
		if (intPtr == IntPtr.Zero)
		{
			throw new OutOfMemoryException();
		}
		bool success = false;
		try
		{
			Handle.DangerousAddRef(ref success);
			int num = mono_btls_bio_read(Handle.DangerousGetHandle(), intPtr, size);
			if (num > 0)
			{
				Marshal.Copy(intPtr, buffer, offset, num);
			}
			return num;
		}
		finally
		{
			if (success)
			{
				Handle.DangerousRelease();
			}
			Marshal.FreeHGlobal(intPtr);
		}
	}

	public int Write(byte[] buffer, int offset, int size)
	{
		CheckThrow();
		IntPtr intPtr = Marshal.AllocHGlobal(size);
		if (intPtr == IntPtr.Zero)
		{
			throw new OutOfMemoryException();
		}
		bool success = false;
		try
		{
			Handle.DangerousAddRef(ref success);
			Marshal.Copy(buffer, offset, intPtr, size);
			return mono_btls_bio_write(Handle.DangerousGetHandle(), intPtr, size);
		}
		finally
		{
			if (success)
			{
				Handle.DangerousRelease();
			}
			Marshal.FreeHGlobal(intPtr);
		}
	}

	public int Flush()
	{
		CheckThrow();
		bool success = false;
		try
		{
			Handle.DangerousAddRef(ref success);
			return mono_btls_bio_flush(Handle.DangerousGetHandle());
		}
		finally
		{
			if (success)
			{
				Handle.DangerousRelease();
			}
		}
	}

	public int Indent(uint indent, uint max_indent)
	{
		CheckThrow();
		bool success = false;
		try
		{
			Handle.DangerousAddRef(ref success);
			return mono_btls_bio_indent(Handle.DangerousGetHandle(), indent, max_indent);
		}
		finally
		{
			if (success)
			{
				Handle.DangerousRelease();
			}
		}
	}

	public int HexDump(byte[] buffer, uint indent)
	{
		CheckThrow();
		IntPtr intPtr = Marshal.AllocHGlobal(buffer.Length);
		if (intPtr == IntPtr.Zero)
		{
			throw new OutOfMemoryException();
		}
		bool success = false;
		try
		{
			Handle.DangerousAddRef(ref success);
			Marshal.Copy(buffer, 0, intPtr, buffer.Length);
			return mono_btls_bio_hexdump(Handle.DangerousGetHandle(), intPtr, buffer.Length, indent);
		}
		finally
		{
			if (success)
			{
				Handle.DangerousRelease();
			}
			Marshal.FreeHGlobal(intPtr);
		}
	}

	public void PrintErrors()
	{
		CheckThrow();
		bool success = false;
		try
		{
			Handle.DangerousAddRef(ref success);
			mono_btls_bio_print_errors(Handle.DangerousGetHandle());
		}
		finally
		{
			if (success)
			{
				Handle.DangerousRelease();
			}
		}
	}
}
