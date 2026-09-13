using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Bindings;

namespace Unity.Burst.LowLevel;

[NativeHeader("Runtime/Burst/Burst.h")]
[StaticAccessor("BurstCompilerService::Get()", StaticAccessorType.Arrow)]
[NativeHeader("Runtime/Burst/BurstDelegateCache.h")]
internal static class BurstCompilerService
{
	public delegate bool ExtractCompilerFlags(Type jobType, out string flags);

	public static extern bool IsInitialized
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod("Initialize")]
	private static extern string InitializeInternal(string path, ExtractCompilerFlags extractCompilerFlags);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[ThreadSafe]
	public static extern string GetDisassembly(MethodInfo m, string compilerOptions);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	public static extern int CompileAsyncDelegateMethod(object delegateMethod, string compilerOptions);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	public unsafe static extern void* GetAsyncCompiledAsyncDelegateMethod(int userID);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[ThreadSafe]
	public unsafe static extern void* GetOrCreateSharedMemory(ref Hash128 key, uint size_of, uint alignment);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[ThreadSafe]
	public static extern string GetMethodSignature(MethodInfo method);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[ThreadSafe]
	public static extern void SetCurrentExecutionMode(uint environment);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[ThreadSafe]
	public static extern uint GetCurrentExecutionMode();

	public static void Initialize(string folderRuntime, ExtractCompilerFlags extractCompilerFlags)
	{
		if (folderRuntime == null)
		{
			throw new ArgumentNullException("folderRuntime");
		}
		if (extractCompilerFlags == null)
		{
			throw new ArgumentNullException("extractCompilerFlags");
		}
		if (!Directory.Exists(folderRuntime))
		{
			Debug.LogError("Unable to initialize the burst JIT compiler. The folder `" + folderRuntime + "` does not exist");
			return;
		}
		string text = InitializeInternal(folderRuntime, extractCompilerFlags);
		if (!string.IsNullOrEmpty(text))
		{
			Debug.LogError("Unexpected error while trying to initialize the burst JIT compiler: " + text);
		}
	}
}
