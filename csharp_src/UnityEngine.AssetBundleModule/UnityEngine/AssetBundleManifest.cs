using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine;

[NativeHeader("Modules/AssetBundle/Public/AssetBundleManifest.h")]
public class AssetBundleManifest : Object
{
	private AssetBundleManifest()
	{
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod("GetAllAssetBundles")]
	public extern string[] GetAllAssetBundles();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod("GetAllAssetBundlesWithVariant")]
	public extern string[] GetAllAssetBundlesWithVariant();

	[NativeMethod("GetAssetBundleHash")]
	public Hash128 GetAssetBundleHash(string assetBundleName)
	{
		GetAssetBundleHash_Injected(assetBundleName, out var ret);
		return ret;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod("GetDirectDependencies")]
	public extern string[] GetDirectDependencies(string assetBundleName);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod("GetAllDependencies")]
	public extern string[] GetAllDependencies(string assetBundleName);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern void GetAssetBundleHash_Injected(string assetBundleName, out Hash128 ret);
}
