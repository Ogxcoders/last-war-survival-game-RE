using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine;

[Serializable]
[StructLayout(LayoutKind.Sequential)]
[NativeHeader("Modules/AssetBundle/Public/AssetBundleLoadingCache.h")]
[UsedByNativeCode]
internal static class AssetBundleLoadingCache
{
	internal const int kMinAllowedBlockCount = 2;

	internal const int kMinAllowedMaxBlocksPerFile = 2;

	internal static extern uint maxBlocksPerFile
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	internal static extern uint blockCount
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	internal static extern uint blockSize
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
	}

	internal static uint memoryBudgetKB
	{
		get
		{
			return blockCount * blockSize;
		}
		set
		{
			uint num = Math.Max(value / blockSize, 2u);
			uint num2 = Math.Max(blockCount / 4, 2u);
			if (num != blockCount || num2 != maxBlocksPerFile)
			{
				blockCount = num;
				maxBlocksPerFile = num2;
			}
		}
	}
}
