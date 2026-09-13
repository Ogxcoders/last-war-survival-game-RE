using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine;

[StructLayout(LayoutKind.Sequential)]
[RequiredByNativeCode]
[NativeHeader("Modules/AssetBundle/Public/AssetBundleLoadAssetOperation.h")]
public class AssetBundleRequest : AsyncOperation
{
	public extern Object asset
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeMethod("GetLoadedAsset")]
		get;
	}

	public extern Object[] allAssets
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeMethod("GetAllLoadedAssets")]
		get;
	}
}
