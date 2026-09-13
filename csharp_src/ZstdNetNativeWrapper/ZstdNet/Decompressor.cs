using System;

namespace ZstdNet;

public class Decompressor : IDisposable
{
	public readonly DecompressionOptions Options;

	private IntPtr dctx;

	public Decompressor()
		: this(new DecompressionOptions(null))
	{
	}

	public Decompressor(DecompressionOptions options)
	{
		Options = options;
		dctx = ExternMethods.ZSTD_createDCtx().EnsureZstdSuccess();
		options.ApplyDecompressionParams(dctx);
	}

	~Decompressor()
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
		if (!(dctx == IntPtr.Zero))
		{
			ExternMethods.ZSTD_freeDCtx(dctx);
			dctx = IntPtr.Zero;
		}
	}

	public byte[] Unwrap(byte[] src, int maxDecompressedSize = int.MaxValue)
	{
		return Unwrap(new ArraySegment<byte>(src), maxDecompressedSize);
	}

	public byte[] Unwrap(ArraySegment<byte> src, int maxDecompressedSize = int.MaxValue)
	{
		return Unwrap((ReadOnlySpan<byte>)src, maxDecompressedSize);
	}

	public byte[] Unwrap(ReadOnlySpan<byte> src, int maxDecompressedSize = int.MaxValue)
	{
		ulong decompressedSize = GetDecompressedSize(src);
		if (decompressedSize > (ulong)maxDecompressedSize)
		{
			throw new ZstdException(ZSTD_ErrorCode.ZSTD_error_dstSize_tooSmall, string.Format("Decompressed content size {0} is greater than {1} {2}", decompressedSize, "maxDecompressedSize", maxDecompressedSize));
		}
		if (decompressedSize > 2147483591)
		{
			throw new ZstdException(ZSTD_ErrorCode.ZSTD_error_dstSize_tooSmall, $"Decompressed content size {decompressedSize} is greater than max possible byte array size {2147483591uL}");
		}
		byte[] array = new byte[decompressedSize];
		int num = Unwrap(src, new Span<byte>(array), bufferSizePrecheck: false);
		if (decompressedSize != (ulong)num)
		{
			throw new ZstdException(ZSTD_ErrorCode.ZSTD_error_GENERIC, "Decompressed content size specified in the src data frame is invalid");
		}
		return array;
	}

	public static ulong GetDecompressedSize(byte[] src)
	{
		return GetDecompressedSize(new ReadOnlySpan<byte>(src));
	}

	public static ulong GetDecompressedSize(ArraySegment<byte> src)
	{
		return GetDecompressedSize((ReadOnlySpan<byte>)src);
	}

	public static ulong GetDecompressedSize(ReadOnlySpan<byte> src)
	{
		ulong num = ExternMethods.ZSTD_getFrameContentSize(src, (UIntPtr)(ulong)src.Length);
		return num switch
		{
			ulong.MaxValue => throw new ZstdException(ZSTD_ErrorCode.ZSTD_error_GENERIC, "Decompressed content size is not specified"), 
			18446744073709551614uL => throw new ZstdException(ZSTD_ErrorCode.ZSTD_error_GENERIC, "Decompressed content size cannot be determined (e.g. invalid magic number, srcSize too small)"), 
			_ => num, 
		};
	}

	public int Unwrap(byte[] src, byte[] dst, int offset, bool bufferSizePrecheck = true)
	{
		return Unwrap(new ReadOnlySpan<byte>(src), dst, offset, bufferSizePrecheck);
	}

	public int Unwrap(ArraySegment<byte> src, byte[] dst, int offset, bool bufferSizePrecheck = true)
	{
		return Unwrap((ReadOnlySpan<byte>)src, dst, offset, bufferSizePrecheck);
	}

	public int Unwrap(ReadOnlySpan<byte> src, byte[] dst, int offset, bool bufferSizePrecheck = true)
	{
		if (offset < 0 || offset > dst.Length)
		{
			throw new ArgumentOutOfRangeException("offset");
		}
		return Unwrap(src, new Span<byte>(dst, offset, dst.Length - offset), bufferSizePrecheck);
	}

	public int Unwrap(ReadOnlySpan<byte> src, Span<byte> dst, bool bufferSizePrecheck = true)
	{
		if (bufferSizePrecheck && GetDecompressedSize(src) > (ulong)dst.Length)
		{
			throw new ZstdException(ZSTD_ErrorCode.ZSTD_error_dstSize_tooSmall, "Destination buffer size is less than specified decompressed content size");
		}
		return (int)(uint)((Options.Ddict == IntPtr.Zero) ? ExternMethods.ZSTD_decompressDCtx(dctx, dst, (UIntPtr)(ulong)dst.Length, src, (UIntPtr)(ulong)src.Length) : ExternMethods.ZSTD_decompress_usingDDict(dctx, dst, (UIntPtr)(ulong)dst.Length, src, (UIntPtr)(ulong)src.Length, Options.Ddict)).EnsureZstdSuccess();
	}
}
