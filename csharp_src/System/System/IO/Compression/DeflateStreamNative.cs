using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace System.IO.Compression;

internal class DeflateStreamNative
{
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	private delegate int UnmanagedReadOrWrite(IntPtr buffer, int length, IntPtr data);

	private sealed class SafeDeflateStreamHandle : SafeHandle
	{
		public override bool IsInvalid => handle == IntPtr.Zero;

		private SafeDeflateStreamHandle()
			: base(IntPtr.Zero, ownsHandle: true)
		{
		}

		internal SafeDeflateStreamHandle(IntPtr handle)
			: base(handle, ownsHandle: true)
		{
		}

		protected override bool ReleaseHandle()
		{
			try
			{
				CloseZStream(handle);
			}
			catch
			{
			}
			return true;
		}
	}

	private const int BufferSize = 4096;

	private UnmanagedReadOrWrite feeder;

	private Stream base_stream;

	private SafeDeflateStreamHandle z_stream;

	private GCHandle data;

	private bool disposed;

	private byte[] io_buffer;

	private Exception last_error;

	private DeflateStreamNative()
	{
	}

	public static DeflateStreamNative Create(Stream compressedStream, CompressionMode mode, bool gzip)
	{
		DeflateStreamNative deflateStreamNative = new DeflateStreamNative();
		deflateStreamNative.data = GCHandle.Alloc(deflateStreamNative);
		deflateStreamNative.feeder = ((mode == CompressionMode.Compress) ? new UnmanagedReadOrWrite(UnmanagedWrite) : new UnmanagedReadOrWrite(UnmanagedRead));
		deflateStreamNative.z_stream = CreateZStream(mode, gzip, deflateStreamNative.feeder, GCHandle.ToIntPtr(deflateStreamNative.data));
		if (deflateStreamNative.z_stream.IsInvalid)
		{
			deflateStreamNative.Dispose(disposing: true);
			return null;
		}
		deflateStreamNative.base_stream = compressedStream;
		return deflateStreamNative;
	}

	~DeflateStreamNative()
	{
		Dispose(disposing: false);
	}

	public void Dispose(bool disposing)
	{
		if (disposing && !disposed)
		{
			disposed = true;
			GC.SuppressFinalize(this);
		}
		else
		{
			base_stream = Stream.Null;
		}
		io_buffer = null;
		if (z_stream != null && !z_stream.IsInvalid)
		{
			z_stream.Dispose();
		}
		_ = data;
		if (data.IsAllocated)
		{
			data.Free();
		}
	}

	public void Flush()
	{
		int result = Flush(z_stream);
		CheckResult(result, "Flush");
	}

	public int ReadZStream(IntPtr buffer, int length)
	{
		int result = ReadZStream(z_stream, buffer, length);
		CheckResult(result, "ReadInternal");
		return result;
	}

	public void WriteZStream(IntPtr buffer, int length)
	{
		int result = WriteZStream(z_stream, buffer, length);
		CheckResult(result, "WriteInternal");
	}

	private static int UnmanagedRead(IntPtr buffer, int length, IntPtr data)
	{
		if (!(GCHandle.FromIntPtr(data).Target is DeflateStreamNative deflateStreamNative))
		{
			return -1;
		}
		return deflateStreamNative.UnmanagedRead(buffer, length);
	}

	private int UnmanagedRead(IntPtr buffer, int length)
	{
		if (io_buffer == null)
		{
			io_buffer = new byte[4096];
		}
		int count = Math.Min(length, io_buffer.Length);
		int num;
		try
		{
			num = base_stream.Read(io_buffer, 0, count);
		}
		catch (Exception ex)
		{
			last_error = ex;
			return -12;
		}
		if (num > 0)
		{
			Marshal.Copy(io_buffer, 0, buffer, num);
		}
		return num;
	}

	private static int UnmanagedWrite(IntPtr buffer, int length, IntPtr data)
	{
		if (!(GCHandle.FromIntPtr(data).Target is DeflateStreamNative deflateStreamNative))
		{
			return -1;
		}
		return deflateStreamNative.UnmanagedWrite(buffer, length);
	}

	private unsafe int UnmanagedWrite(IntPtr buffer, int length)
	{
		int num = 0;
		while (length > 0)
		{
			if (io_buffer == null)
			{
				io_buffer = new byte[4096];
			}
			int num2 = Math.Min(length, io_buffer.Length);
			Marshal.Copy(buffer, io_buffer, 0, num2);
			try
			{
				base_stream.Write(io_buffer, 0, num2);
			}
			catch (Exception ex)
			{
				last_error = ex;
				return -12;
			}
			buffer = new IntPtr((byte*)buffer.ToPointer() + num2);
			length -= num2;
			num += num2;
		}
		return num;
	}

	private void CheckResult(int result, string where)
	{
		if (result >= 0)
		{
			return;
		}
		Exception ex = Interlocked.Exchange(ref last_error, null);
		if (ex != null)
		{
			throw ex;
		}
		throw new IOException(result switch
		{
			-1 => "Unknown error", 
			-2 => "Internal error", 
			-3 => "Corrupted data", 
			-4 => "Not enough memory", 
			-5 => "Internal error (no progress possible)", 
			-6 => "Invalid version", 
			-10 => "Invalid argument(s)", 
			-11 => "IO error", 
			_ => "Unknown error", 
		} + " " + where);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern IntPtr CreateZStream(int compress, bool gzip, IntPtr feeder, IntPtr data);

	private static SafeDeflateStreamHandle CreateZStream(CompressionMode compress, bool gzip, UnmanagedReadOrWrite feeder, IntPtr data)
	{
		SafeDeflateStreamHandle result;
		try
		{
		}
		finally
		{
			result = new SafeDeflateStreamHandle(CreateZStream((int)compress, gzip, Marshal.GetFunctionPointerForDelegate(feeder), data));
		}
		return result;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern int CloseZStream(IntPtr stream);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern int Flush(IntPtr stream);

	private static int Flush(SafeDeflateStreamHandle stream)
	{
		bool success = false;
		try
		{
			stream.DangerousAddRef(ref success);
			return Flush(stream.DangerousGetHandle());
		}
		finally
		{
			if (success)
			{
				stream.DangerousRelease();
			}
		}
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern int ReadZStream(IntPtr stream, IntPtr buffer, int length);

	private static int ReadZStream(SafeDeflateStreamHandle stream, IntPtr buffer, int length)
	{
		bool success = false;
		try
		{
			stream.DangerousAddRef(ref success);
			return ReadZStream(stream.DangerousGetHandle(), buffer, length);
		}
		finally
		{
			if (success)
			{
				stream.DangerousRelease();
			}
		}
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern int WriteZStream(IntPtr stream, IntPtr buffer, int length);

	private static int WriteZStream(SafeDeflateStreamHandle stream, IntPtr buffer, int length)
	{
		bool success = false;
		try
		{
			stream.DangerousAddRef(ref success);
			return WriteZStream(stream.DangerousGetHandle(), buffer, length);
		}
		finally
		{
			if (success)
			{
				stream.DangerousRelease();
			}
		}
	}
}
