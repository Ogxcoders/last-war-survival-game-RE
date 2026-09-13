using System;
using System.Buffers;

namespace ZstdNet;

public class Compressor : IDisposable
{
	public readonly CompressionOptions Options;

	private IntPtr cctx;

	public Compressor()
		: this(CompressionOptions.Default)
	{
	}

	public Compressor(CompressionOptions options)
	{
		Options = options;
		cctx = ExternMethods.ZSTD_createCCtx().EnsureZstdSuccess();
		options.ApplyCompressionParams(cctx);
		if (options.Cdict != IntPtr.Zero)
		{
			ExternMethods.ZSTD_CCtx_refCDict(cctx, options.Cdict).EnsureZstdSuccess();
		}
	}

	~Compressor()
	{
		Dispose(disposing: false);
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	private void Dispose(bool disposing)
	{
		if (!(cctx == IntPtr.Zero))
		{
			ExternMethods.ZSTD_freeCCtx(cctx);
			cctx = IntPtr.Zero;
		}
	}

	public byte[] Wrap(byte[] src)
	{
		return Wrap(new ReadOnlySpan<byte>(src));
	}

	public byte[] Wrap(ArraySegment<byte> src)
	{
		return Wrap((ReadOnlySpan<byte>)src);
	}

	public byte[] Wrap(ReadOnlySpan<byte> src)
	{
		ulong num = Math.Min(2147483591uL, GetCompressBoundLong((ulong)src.Length));
		byte[] array = ArrayPool<byte>.Shared.Rent((int)num);
		try
		{
			int num2 = Wrap(src, new Span<byte>(array));
			byte[] array2 = new byte[num2];
			Array.Copy(array, array2, num2);
			return array2;
		}
		finally
		{
			ArrayPool<byte>.Shared.Return(array);
		}
	}

	public static int GetCompressBound(int size)
	{
		return (int)(uint)ExternMethods.ZSTD_compressBound((UIntPtr)(ulong)size);
	}

	public static ulong GetCompressBoundLong(ulong size)
	{
		return (ulong)ExternMethods.ZSTD_compressBound((UIntPtr)size);
	}

	public int Wrap(byte[] src, byte[] dst, int offset)
	{
		return Wrap(new ReadOnlySpan<byte>(src), dst, offset);
	}

	public int Wrap(ArraySegment<byte> src, byte[] dst, int offset)
	{
		return Wrap((ReadOnlySpan<byte>)src, dst, offset);
	}

	public int Wrap(ReadOnlySpan<byte> src, byte[] dst, int offset)
	{
		if (offset < 0 || offset >= dst.Length)
		{
			throw new ArgumentOutOfRangeException("offset");
		}
		return Wrap(src, new Span<byte>(dst, offset, dst.Length - offset));
	}

	public int Wrap(ReadOnlySpan<byte> src, Span<byte> dst)
	{
		return (int)(uint)((Options.AdvancedParams != null) ? ExternMethods.ZSTD_compress2(cctx, dst, (UIntPtr)(ulong)dst.Length, src, (UIntPtr)(ulong)src.Length) : ((Options.Cdict == IntPtr.Zero) ? ExternMethods.ZSTD_compressCCtx(cctx, dst, (UIntPtr)(ulong)dst.Length, src, (UIntPtr)(ulong)src.Length, Options.CompressionLevel) : ExternMethods.ZSTD_compress_usingCDict(cctx, dst, (UIntPtr)(ulong)dst.Length, src, (UIntPtr)(ulong)src.Length, Options.Cdict))).EnsureZstdSuccess();
	}
}
