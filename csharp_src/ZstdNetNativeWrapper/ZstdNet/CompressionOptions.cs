using System;
using System.Collections.Generic;

namespace ZstdNet;

public class CompressionOptions : IDisposable
{
	public const int DefaultCompressionLevel = 3;

	public readonly int CompressionLevel;

	public readonly byte[] Dictionary;

	public readonly IReadOnlyDictionary<ZSTD_cParameter, int> AdvancedParams;

	internal IntPtr Cdict;

	public static int MinCompressionLevel => ExternMethods.ZSTD_minCLevel();

	public static int MaxCompressionLevel => ExternMethods.ZSTD_maxCLevel();

	public static CompressionOptions Default { get; } = new CompressionOptions(3);

	public CompressionOptions(int compressionLevel)
	{
		if (compressionLevel < MinCompressionLevel || compressionLevel > MaxCompressionLevel)
		{
			throw new ArgumentOutOfRangeException("compressionLevel");
		}
		CompressionLevel = compressionLevel;
	}

	public CompressionOptions(byte[] dict, int compressionLevel = 3)
		: this(compressionLevel)
	{
		Dictionary = dict;
		if (dict != null)
		{
			Cdict = ExternMethods.ZSTD_createCDict(dict, (UIntPtr)(ulong)dict.Length, compressionLevel).EnsureZstdSuccess();
		}
		else
		{
			GC.SuppressFinalize(this);
		}
	}

	public CompressionOptions(byte[] dict, IReadOnlyDictionary<ZSTD_cParameter, int> advancedParams, int compressionLevel = 3)
		: this(dict, compressionLevel)
	{
		if (advancedParams == null)
		{
			return;
		}
		foreach (KeyValuePair<ZSTD_cParameter, int> advancedParam in advancedParams)
		{
			ExternMethods.ZSTD_bounds zSTD_bounds = ExternMethods.ZSTD_cParam_getBounds(advancedParam.Key);
			zSTD_bounds.error.EnsureZstdSuccess();
			if (advancedParam.Value < zSTD_bounds.lowerBound || advancedParam.Value > zSTD_bounds.upperBound)
			{
				throw new ArgumentOutOfRangeException("advancedParams", $"Advanced parameter '{advancedParam.Key}' is out of range [{zSTD_bounds.lowerBound}, {zSTD_bounds.upperBound}]");
			}
		}
		AdvancedParams = advancedParams;
	}

	internal void ApplyCompressionParams(IntPtr cctx)
	{
		if (AdvancedParams == null || !AdvancedParams.ContainsKey(ZSTD_cParameter.ZSTD_c_compressionLevel))
		{
			ExternMethods.ZSTD_CCtx_setParameter(cctx, ZSTD_cParameter.ZSTD_c_compressionLevel, CompressionLevel).EnsureZstdSuccess();
		}
		if (AdvancedParams == null)
		{
			return;
		}
		foreach (KeyValuePair<ZSTD_cParameter, int> advancedParam in AdvancedParams)
		{
			ExternMethods.ZSTD_CCtx_setParameter(cctx, advancedParam.Key, advancedParam.Value).EnsureZstdSuccess();
		}
	}

	~CompressionOptions()
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
		if (!(Cdict == IntPtr.Zero))
		{
			ExternMethods.ZSTD_freeCDict(Cdict);
			Cdict = IntPtr.Zero;
		}
	}
}
