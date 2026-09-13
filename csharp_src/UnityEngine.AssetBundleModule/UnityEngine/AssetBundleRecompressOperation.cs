using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine;

[StructLayout(LayoutKind.Sequential)]
[NativeHeader("Modules/AssetBundle/Public/AssetBundleRecompressOperation.h")]
[RequiredByNativeCode]
public class AssetBundleRecompressOperation : AsyncOperation
{
	public extern string humanReadableResult
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeMethod("GetResultStr")]
		get;
	}

	public extern string inputPath
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeMethod("GetInputPath")]
		get;
	}

	public extern string outputPath
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeMethod("GetOutputPath")]
		get;
	}

	public extern AssetBundleLoadResult result
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeMethod("GetResult")]
		get;
	}

	public extern bool success
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeMethod("GetSuccess")]
		get;
	}
}
