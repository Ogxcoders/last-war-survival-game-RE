using System;
using System.Runtime.InteropServices;

namespace ZstdNet;

internal static class ExternMethods
{
	internal struct ZSTD_bounds
	{
		public UIntPtr error;

		public int lowerBound;

		public int upperBound;
	}

	public enum ZSTD_ResetDirective
	{
		ZSTD_reset_session_only = 1,
		ZSTD_reset_parameters,
		ZSTD_reset_session_and_parameters
	}

	public enum ZSTD_EndDirective
	{
		ZSTD_e_continue,
		ZSTD_e_flush,
		ZSTD_e_end
	}

	internal struct ZSTD_Buffer
	{
		public IntPtr buffer;

		public UIntPtr size;

		public UIntPtr pos;

		public bool IsFullyConsumed => (ulong)size <= (ulong)pos;

		public ZSTD_Buffer(UIntPtr pos, UIntPtr size)
		{
			buffer = IntPtr.Zero;
			this.size = size;
			this.pos = pos;
		}
	}

	private const string DllName = "libzstd";

	public const ulong ZSTD_CONTENTSIZE_UNKNOWN = ulong.MaxValue;

	public const ulong ZSTD_CONTENTSIZE_ERROR = 18446744073709551614uL;

	[DllImport("libzstd", CallingConvention = CallingConvention.Cdecl)]
	public static extern UIntPtr ZDICT_trainFromBuffer(byte[] dictBuffer, UIntPtr dictBufferCapacity, byte[] samplesBuffer, UIntPtr[] samplesSizes, uint nbSamples);

	[DllImport("libzstd", CallingConvention = CallingConvention.Cdecl)]
	public static extern uint ZDICT_isError(UIntPtr code);

	[DllImport("libzstd", CallingConvention = CallingConvention.Cdecl)]
	public static extern IntPtr ZDICT_getErrorName(UIntPtr code);

	[DllImport("libzstd", CallingConvention = CallingConvention.Cdecl)]
	public static extern IntPtr ZSTD_createCCtx();

	[DllImport("libzstd", CallingConvention = CallingConvention.Cdecl)]
	public static extern UIntPtr ZSTD_freeCCtx(IntPtr cctx);

	[DllImport("libzstd", CallingConvention = CallingConvention.Cdecl)]
	public static extern IntPtr ZSTD_createDCtx();

	[DllImport("libzstd", CallingConvention = CallingConvention.Cdecl)]
	public static extern UIntPtr ZSTD_freeDCtx(IntPtr cctx);

	[DllImport("libzstd", CallingConvention = CallingConvention.Cdecl)]
	public static extern UIntPtr ZSTD_compressCCtx(IntPtr ctx, IntPtr dst, UIntPtr dstCapacity, IntPtr src, UIntPtr srcSize, int compressionLevel);

	[DllImport("libzstd", CallingConvention = CallingConvention.Cdecl)]
	public static extern UIntPtr ZSTD_compressCCtx(IntPtr ctx, ref byte dst, UIntPtr dstCapacity, ref byte src, UIntPtr srcSize, int compressionLevel);

	public static UIntPtr ZSTD_compressCCtx(IntPtr ctx, Span<byte> dst, UIntPtr dstCapacity, ReadOnlySpan<byte> src, UIntPtr srcSize, int compressionLevel)
	{
		return ZSTD_compressCCtx(ctx, ref MemoryMarshal.GetReference(dst), dstCapacity, ref MemoryMarshal.GetReference(src), srcSize, compressionLevel);
	}

	[DllImport("libzstd", CallingConvention = CallingConvention.Cdecl)]
	public static extern UIntPtr ZSTD_decompressDCtx(IntPtr ctx, IntPtr dst, UIntPtr dstCapacity, IntPtr src, UIntPtr srcSize);

	[DllImport("libzstd", CallingConvention = CallingConvention.Cdecl)]
	public static extern UIntPtr ZSTD_decompressDCtx(IntPtr ctx, ref byte dst, UIntPtr dstCapacity, ref byte src, UIntPtr srcSize);

	public static UIntPtr ZSTD_decompressDCtx(IntPtr ctx, Span<byte> dst, UIntPtr dstCapacity, ReadOnlySpan<byte> src, UIntPtr srcSize)
	{
		return ZSTD_decompressDCtx(ctx, ref MemoryMarshal.GetReference(dst), dstCapacity, ref MemoryMarshal.GetReference(src), srcSize);
	}

	[DllImport("libzstd", CallingConvention = CallingConvention.Cdecl)]
	public static extern UIntPtr ZSTD_compress2(IntPtr ctx, ref byte dst, UIntPtr dstCapacity, ref byte src, UIntPtr srcSize);

	public static UIntPtr ZSTD_compress2(IntPtr ctx, Span<byte> dst, UIntPtr dstCapacity, ReadOnlySpan<byte> src, UIntPtr srcSize)
	{
		return ZSTD_compress2(ctx, ref MemoryMarshal.GetReference(dst), dstCapacity, ref MemoryMarshal.GetReference(src), srcSize);
	}

	[DllImport("libzstd", CallingConvention = CallingConvention.Cdecl)]
	public static extern IntPtr ZSTD_createCDict(byte[] dict, UIntPtr dictSize, int compressionLevel);

	[DllImport("libzstd", CallingConvention = CallingConvention.Cdecl)]
	public static extern UIntPtr ZSTD_freeCDict(IntPtr cdict);

	[DllImport("libzstd", CallingConvention = CallingConvention.Cdecl)]
	public static extern UIntPtr ZSTD_compress_usingCDict(IntPtr cctx, IntPtr dst, UIntPtr dstCapacity, IntPtr src, UIntPtr srcSize, IntPtr cdict);

	[DllImport("libzstd", CallingConvention = CallingConvention.Cdecl)]
	public static extern UIntPtr ZSTD_compress_usingCDict(IntPtr cctx, ref byte dst, UIntPtr dstCapacity, ref byte src, UIntPtr srcSize, IntPtr cdict);

	public static UIntPtr ZSTD_compress_usingCDict(IntPtr cctx, Span<byte> dst, UIntPtr dstCapacity, ReadOnlySpan<byte> src, UIntPtr srcSize, IntPtr cdict)
	{
		return ZSTD_compress_usingCDict(cctx, ref MemoryMarshal.GetReference(dst), dstCapacity, ref MemoryMarshal.GetReference(src), srcSize, cdict);
	}

	[DllImport("libzstd", CallingConvention = CallingConvention.Cdecl)]
	public static extern IntPtr ZSTD_createDDict(byte[] dict, UIntPtr dictSize);

	[DllImport("libzstd", CallingConvention = CallingConvention.Cdecl)]
	public static extern UIntPtr ZSTD_freeDDict(IntPtr ddict);

	[DllImport("libzstd", CallingConvention = CallingConvention.Cdecl)]
	public static extern UIntPtr ZSTD_decompress_usingDDict(IntPtr dctx, IntPtr dst, UIntPtr dstCapacity, IntPtr src, UIntPtr srcSize, IntPtr ddict);

	[DllImport("libzstd", CallingConvention = CallingConvention.Cdecl)]
	public static extern UIntPtr ZSTD_decompress_usingDDict(IntPtr dctx, ref byte dst, UIntPtr dstCapacity, ref byte src, UIntPtr srcSize, IntPtr ddict);

	public static UIntPtr ZSTD_decompress_usingDDict(IntPtr dctx, Span<byte> dst, UIntPtr dstCapacity, ReadOnlySpan<byte> src, UIntPtr srcSize, IntPtr ddict)
	{
		return ZSTD_decompress_usingDDict(dctx, ref MemoryMarshal.GetReference(dst), dstCapacity, ref MemoryMarshal.GetReference(src), srcSize, ddict);
	}

	[DllImport("libzstd", CallingConvention = CallingConvention.Cdecl)]
	public static extern ulong ZSTD_getDecompressedSize(IntPtr src, UIntPtr srcSize);

	[DllImport("libzstd", CallingConvention = CallingConvention.Cdecl)]
	public static extern ulong ZSTD_getFrameContentSize(IntPtr src, UIntPtr srcSize);

	[DllImport("libzstd", CallingConvention = CallingConvention.Cdecl)]
	public static extern ulong ZSTD_getFrameContentSize(ref byte src, UIntPtr srcSize);

	public static ulong ZSTD_getFrameContentSize(ReadOnlySpan<byte> src, UIntPtr srcSize)
	{
		return ZSTD_getFrameContentSize(ref MemoryMarshal.GetReference(src), srcSize);
	}

	[DllImport("libzstd", CallingConvention = CallingConvention.Cdecl)]
	public static extern int ZSTD_maxCLevel();

	[DllImport("libzstd", CallingConvention = CallingConvention.Cdecl)]
	public static extern int ZSTD_minCLevel();

	[DllImport("libzstd", CallingConvention = CallingConvention.Cdecl)]
	public static extern UIntPtr ZSTD_compressBound(UIntPtr srcSize);

	[DllImport("libzstd", CallingConvention = CallingConvention.Cdecl)]
	public static extern uint ZSTD_isError(UIntPtr code);

	[DllImport("libzstd", CallingConvention = CallingConvention.Cdecl)]
	public static extern IntPtr ZSTD_getErrorName(UIntPtr code);

	[DllImport("libzstd", CallingConvention = CallingConvention.Cdecl)]
	public static extern UIntPtr ZSTD_CCtx_reset(IntPtr cctx, ZSTD_ResetDirective reset);

	[DllImport("libzstd", CallingConvention = CallingConvention.Cdecl)]
	public static extern ZSTD_bounds ZSTD_cParam_getBounds(ZSTD_cParameter cParam);

	[DllImport("libzstd", CallingConvention = CallingConvention.Cdecl)]
	public static extern UIntPtr ZSTD_CCtx_setParameter(IntPtr cctx, ZSTD_cParameter param, int value);

	[DllImport("libzstd", CallingConvention = CallingConvention.Cdecl)]
	public static extern UIntPtr ZSTD_DCtx_reset(IntPtr dctx, ZSTD_ResetDirective reset);

	[DllImport("libzstd", CallingConvention = CallingConvention.Cdecl)]
	public static extern ZSTD_bounds ZSTD_dParam_getBounds(ZSTD_dParameter dParam);

	[DllImport("libzstd", CallingConvention = CallingConvention.Cdecl)]
	public static extern UIntPtr ZSTD_DCtx_setParameter(IntPtr dctx, ZSTD_dParameter param, int value);

	[DllImport("libzstd", CallingConvention = CallingConvention.Cdecl)]
	public static extern IntPtr ZSTD_createCStream();

	[DllImport("libzstd", CallingConvention = CallingConvention.Cdecl)]
	public static extern UIntPtr ZSTD_freeCStream(IntPtr zcs);

	[DllImport("libzstd", CallingConvention = CallingConvention.Cdecl)]
	public static extern UIntPtr ZSTD_initCStream(IntPtr zcs, int compressionLevel);

	[DllImport("libzstd", CallingConvention = CallingConvention.Cdecl)]
	public static extern UIntPtr ZSTD_compressStream(IntPtr zcs, ref ZSTD_Buffer output, ref ZSTD_Buffer input);

	[DllImport("libzstd", CallingConvention = CallingConvention.Cdecl)]
	public static extern UIntPtr ZSTD_flushStream(IntPtr zcs, ref ZSTD_Buffer output);

	[DllImport("libzstd", CallingConvention = CallingConvention.Cdecl)]
	public static extern UIntPtr ZSTD_endStream(IntPtr zcs, ref ZSTD_Buffer output);

	[DllImport("libzstd", CallingConvention = CallingConvention.Cdecl)]
	public static extern UIntPtr ZSTD_CStreamInSize();

	[DllImport("libzstd", CallingConvention = CallingConvention.Cdecl)]
	public static extern UIntPtr ZSTD_CStreamOutSize();

	[DllImport("libzstd", CallingConvention = CallingConvention.Cdecl)]
	public static extern IntPtr ZSTD_createDStream();

	[DllImport("libzstd", CallingConvention = CallingConvention.Cdecl)]
	public static extern UIntPtr ZSTD_freeDStream(IntPtr zds);

	[DllImport("libzstd", CallingConvention = CallingConvention.Cdecl)]
	public static extern UIntPtr ZSTD_initDStream(IntPtr zds);

	[DllImport("libzstd", CallingConvention = CallingConvention.Cdecl)]
	public static extern UIntPtr ZSTD_decompressStream(IntPtr zds, ref ZSTD_Buffer output, ref ZSTD_Buffer input);

	[DllImport("libzstd", CallingConvention = CallingConvention.Cdecl)]
	public static extern UIntPtr ZSTD_DStreamInSize();

	[DllImport("libzstd", CallingConvention = CallingConvention.Cdecl)]
	public static extern UIntPtr ZSTD_DStreamOutSize();

	[DllImport("libzstd", CallingConvention = CallingConvention.Cdecl)]
	public static extern UIntPtr ZSTD_compressStream2(IntPtr zcs, ref ZSTD_Buffer output, ref ZSTD_Buffer input, ZSTD_EndDirective endOp);

	[DllImport("libzstd", CallingConvention = CallingConvention.Cdecl)]
	public static extern UIntPtr ZSTD_initDStream_usingDDict(IntPtr zds, IntPtr dict);

	[DllImport("libzstd", CallingConvention = CallingConvention.Cdecl)]
	public static extern UIntPtr ZSTD_initCStream_usingCDict(IntPtr zds, IntPtr dict);

	[DllImport("libzstd", CallingConvention = CallingConvention.Cdecl)]
	public static extern UIntPtr ZSTD_CCtx_refCDict(IntPtr cctx, IntPtr cdict);

	[DllImport("libzstd", CallingConvention = CallingConvention.Cdecl)]
	public static extern UIntPtr ZSTD_DCtx_refDDict(IntPtr cctx, IntPtr cdict);
}
