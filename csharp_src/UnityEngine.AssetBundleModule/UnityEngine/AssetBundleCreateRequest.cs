using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine;

[StructLayout(LayoutKind.Sequential)]
[NativeHeader("Modules/AssetBundle/Public/AssetBundleLoadFromAsyncOperation.h")]
[RequiredByNativeCode]
public class AssetBundleCreateRequest : AsyncOperation
{
	public extern AssetBundle assetBundle
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeMethod("GetAssetBundleBlocking")]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod("SetEnableCompatibilityChecks")]
	private extern void SetEnableCompatibilityChecks(bool set);

	internal void DisableCompatibilityChecks()
	{
		SetEnableCompatibilityChecks(set: false);
	}
}
