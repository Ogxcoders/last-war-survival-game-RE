using System;
using System.Collections.Generic;

namespace ZstdNet;

public class DecompressionOptions : IDisposable
{
	public readonly byte[] Dictionary;

	public readonly IReadOnlyDictionary<ZSTD_dParameter, int> AdvancedParams;

	internal IntPtr Ddict;

	public DecompressionOptions()
		: this(null)
	{
	}

	public DecompressionOptions(byte[] dict)
	{
		Dictionary = dict;
		if (dict != null)
		{
			Ddict = ExternMethods.ZSTD_createDDict(dict, (UIntPtr)(ulong)dict.Length).EnsureZstdSuccess();
		}
		else
		{
			GC.SuppressFinalize(this);
		}
	}

	public DecompressionOptions(byte[] dict, IReadOnlyDictionary<ZSTD_dParameter, int> advancedParams)
		: this(dict)
	{
		if (advancedParams == null)
		{
			return;
		}
		foreach (KeyValuePair<ZSTD_dParameter, int> advancedParam in advancedParams)
		{
			ExternMethods.ZSTD_bounds zSTD_bounds = ExternMethods.ZSTD_dParam_getBounds(advancedParam.Key);
			zSTD_bounds.error.EnsureZstdSuccess();
			if (advancedParam.Value < zSTD_bounds.lowerBound || advancedParam.Value > zSTD_bounds.upperBound)
			{
				throw new ArgumentOutOfRangeException("advancedParams", $"Advanced parameter '{advancedParam.Key}' is out of range [{zSTD_bounds.lowerBound}, {zSTD_bounds.upperBound}]");
			}
		}
		AdvancedParams = advancedParams;
	}

	internal void ApplyDecompressionParams(IntPtr dctx)
	{
		if (AdvancedParams == null)
		{
			return;
		}
		foreach (KeyValuePair<ZSTD_dParameter, int> advancedParam in AdvancedParams)
		{
			ExternMethods.ZSTD_DCtx_setParameter(dctx, advancedParam.Key, advancedParam.Value).EnsureZstdSuccess();
		}
	}

	~DecompressionOptions()
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
		if (!(Ddict == IntPtr.Zero))
		{
			ExternMethods.ZSTD_freeDDict(Ddict);
			Ddict = IntPtr.Zero;
		}
	}
}
