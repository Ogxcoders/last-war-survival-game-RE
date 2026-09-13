using System;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.Bindings;
using UnityEngine.Experimental.Rendering;

namespace UnityEngine.Rendering;

[StaticAccessor("AsyncGPUReadbackManager::GetInstance()", StaticAccessorType.Dot)]
public static class AsyncGPUReadback
{
	internal static void ValidateFormat(Texture src, GraphicsFormat dstformat)
	{
		GraphicsFormat format = GraphicsFormatUtility.GetFormat(src);
		if (!SystemInfo.IsFormatSupported(format, FormatUsage.ReadPixels))
		{
			Debug.LogError($"'{format}' doesn't support ReadPixels usage on this platform. Async GPU readback failed.");
		}
	}

	private static void SetUpScriptingRequest(AsyncGPUReadbackRequest request, Action<AsyncGPUReadbackRequest> callback)
	{
		request.SetScriptingCallback(callback);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern void WaitAllRequests();

	public static AsyncGPUReadbackRequest Request(ComputeBuffer src, Action<AsyncGPUReadbackRequest> callback = null)
	{
		AsyncGPUReadbackRequest asyncGPUReadbackRequest = Request_Internal_ComputeBuffer_1(src);
		SetUpScriptingRequest(asyncGPUReadbackRequest, callback);
		return asyncGPUReadbackRequest;
	}

	public static AsyncGPUReadbackRequest Request(ComputeBuffer src, int size, int offset, Action<AsyncGPUReadbackRequest> callback = null)
	{
		AsyncGPUReadbackRequest asyncGPUReadbackRequest = Request_Internal_ComputeBuffer_2(src, size, offset);
		SetUpScriptingRequest(asyncGPUReadbackRequest, callback);
		return asyncGPUReadbackRequest;
	}

	public static AsyncGPUReadbackRequest Request(Texture src, int mipIndex = 0, Action<AsyncGPUReadbackRequest> callback = null)
	{
		AsyncGPUReadbackRequest asyncGPUReadbackRequest = Request_Internal_Texture_1(src, mipIndex);
		SetUpScriptingRequest(asyncGPUReadbackRequest, callback);
		return asyncGPUReadbackRequest;
	}

	public static AsyncGPUReadbackRequest Request(Texture src, int mipIndex, TextureFormat dstFormat, Action<AsyncGPUReadbackRequest> callback = null)
	{
		return Request(src, mipIndex, GraphicsFormatUtility.GetGraphicsFormat(dstFormat, QualitySettings.activeColorSpace == ColorSpace.Linear), callback);
	}

	public static AsyncGPUReadbackRequest Request(Texture src, int mipIndex, GraphicsFormat dstFormat, Action<AsyncGPUReadbackRequest> callback = null)
	{
		ValidateFormat(src, dstFormat);
		AsyncGPUReadbackRequest asyncGPUReadbackRequest = Request_Internal_Texture_2(src, mipIndex, dstFormat);
		SetUpScriptingRequest(asyncGPUReadbackRequest, callback);
		return asyncGPUReadbackRequest;
	}

	public static AsyncGPUReadbackRequest Request(Texture src, int mipIndex, int x, int width, int y, int height, int z, int depth, Action<AsyncGPUReadbackRequest> callback = null)
	{
		AsyncGPUReadbackRequest asyncGPUReadbackRequest = Request_Internal_Texture_3(src, mipIndex, x, width, y, height, z, depth);
		SetUpScriptingRequest(asyncGPUReadbackRequest, callback);
		return asyncGPUReadbackRequest;
	}

	public static AsyncGPUReadbackRequest Request(Texture src, int mipIndex, int x, int width, int y, int height, int z, int depth, TextureFormat dstFormat, Action<AsyncGPUReadbackRequest> callback = null)
	{
		return Request(src, mipIndex, x, width, y, height, z, depth, GraphicsFormatUtility.GetGraphicsFormat(dstFormat, QualitySettings.activeColorSpace == ColorSpace.Linear), callback);
	}

	public static AsyncGPUReadbackRequest Request(Texture src, int mipIndex, int x, int width, int y, int height, int z, int depth, GraphicsFormat dstFormat, Action<AsyncGPUReadbackRequest> callback = null)
	{
		ValidateFormat(src, dstFormat);
		AsyncGPUReadbackRequest asyncGPUReadbackRequest = Request_Internal_Texture_4(src, mipIndex, x, width, y, height, z, depth, dstFormat);
		SetUpScriptingRequest(asyncGPUReadbackRequest, callback);
		return asyncGPUReadbackRequest;
	}

	private unsafe static AsyncRequestNativeArrayData CreateAsyncRequestNativeArrayData<T>(ref NativeArray<T> output) where T : struct
	{
		AsyncRequestNativeArrayData result = default(AsyncRequestNativeArrayData);
		result.nativeArrayBuffer = output.GetUnsafePtr();
		result.lengthInBytes = output.Length * UnsafeUtility.SizeOf<T>();
		return result;
	}

	public unsafe static AsyncGPUReadbackRequest RequestIntoNativeArray<T>(ref NativeArray<T> output, ComputeBuffer src, Action<AsyncGPUReadbackRequest> callback = null) where T : struct
	{
		AsyncRequestNativeArrayData asyncRequestNativeArrayData = CreateAsyncRequestNativeArrayData(ref output);
		AsyncGPUReadbackRequest asyncGPUReadbackRequest = Request_Internal_ComputeBuffer_3(src, &asyncRequestNativeArrayData);
		SetUpScriptingRequest(asyncGPUReadbackRequest, callback);
		return asyncGPUReadbackRequest;
	}

	public unsafe static AsyncGPUReadbackRequest RequestIntoNativeArray<T>(ref NativeArray<T> output, ComputeBuffer src, int size, int offset, Action<AsyncGPUReadbackRequest> callback = null) where T : struct
	{
		AsyncRequestNativeArrayData asyncRequestNativeArrayData = CreateAsyncRequestNativeArrayData(ref output);
		AsyncGPUReadbackRequest asyncGPUReadbackRequest = Request_Internal_ComputeBuffer_4(src, size, offset, &asyncRequestNativeArrayData);
		SetUpScriptingRequest(asyncGPUReadbackRequest, callback);
		return asyncGPUReadbackRequest;
	}

	public unsafe static AsyncGPUReadbackRequest RequestIntoNativeArray<T>(ref NativeArray<T> output, Texture src, int mipIndex = 0, Action<AsyncGPUReadbackRequest> callback = null) where T : struct
	{
		AsyncRequestNativeArrayData asyncRequestNativeArrayData = CreateAsyncRequestNativeArrayData(ref output);
		AsyncGPUReadbackRequest asyncGPUReadbackRequest = Request_Internal_Texture_5(src, mipIndex, &asyncRequestNativeArrayData);
		SetUpScriptingRequest(asyncGPUReadbackRequest, callback);
		return asyncGPUReadbackRequest;
	}

	public static AsyncGPUReadbackRequest RequestIntoNativeArray<T>(ref NativeArray<T> output, Texture src, int mipIndex, TextureFormat dstFormat, Action<AsyncGPUReadbackRequest> callback = null) where T : struct
	{
		return RequestIntoNativeArray(ref output, src, mipIndex, GraphicsFormatUtility.GetGraphicsFormat(dstFormat, QualitySettings.activeColorSpace == ColorSpace.Linear), callback);
	}

	public unsafe static AsyncGPUReadbackRequest RequestIntoNativeArray<T>(ref NativeArray<T> output, Texture src, int mipIndex, GraphicsFormat dstFormat, Action<AsyncGPUReadbackRequest> callback = null) where T : struct
	{
		ValidateFormat(src, dstFormat);
		AsyncRequestNativeArrayData asyncRequestNativeArrayData = CreateAsyncRequestNativeArrayData(ref output);
		AsyncGPUReadbackRequest asyncGPUReadbackRequest = Request_Internal_Texture_6(src, mipIndex, dstFormat, &asyncRequestNativeArrayData);
		SetUpScriptingRequest(asyncGPUReadbackRequest, callback);
		return asyncGPUReadbackRequest;
	}

	public static AsyncGPUReadbackRequest RequestIntoNativeArray<T>(ref NativeArray<T> output, Texture src, int mipIndex, int x, int width, int y, int height, int z, int depth, TextureFormat dstFormat, Action<AsyncGPUReadbackRequest> callback = null) where T : struct
	{
		return RequestIntoNativeArray(ref output, src, mipIndex, x, width, y, height, z, depth, GraphicsFormatUtility.GetGraphicsFormat(dstFormat, QualitySettings.activeColorSpace == ColorSpace.Linear), callback);
	}

	public unsafe static AsyncGPUReadbackRequest RequestIntoNativeArray<T>(ref NativeArray<T> output, Texture src, int mipIndex, int x, int width, int y, int height, int z, int depth, GraphicsFormat dstFormat, Action<AsyncGPUReadbackRequest> callback = null) where T : struct
	{
		ValidateFormat(src, dstFormat);
		AsyncRequestNativeArrayData asyncRequestNativeArrayData = CreateAsyncRequestNativeArrayData(ref output);
		AsyncGPUReadbackRequest asyncGPUReadbackRequest = Request_Internal_Texture_7(src, mipIndex, x, width, y, height, z, depth, dstFormat, &asyncRequestNativeArrayData);
		SetUpScriptingRequest(asyncGPUReadbackRequest, callback);
		return asyncGPUReadbackRequest;
	}

	[NativeMethod("Request")]
	private static AsyncGPUReadbackRequest Request_Internal_ComputeBuffer_1([NotNull] ComputeBuffer buffer)
	{
		Request_Internal_ComputeBuffer_1_Injected(buffer, out var ret);
		return ret;
	}

	[NativeMethod("Request")]
	private static AsyncGPUReadbackRequest Request_Internal_ComputeBuffer_2([NotNull] ComputeBuffer src, int size, int offset)
	{
		Request_Internal_ComputeBuffer_2_Injected(src, size, offset, out var ret);
		return ret;
	}

	[NativeMethod("Request")]
	private unsafe static AsyncGPUReadbackRequest Request_Internal_ComputeBuffer_3([NotNull] ComputeBuffer buffer, AsyncRequestNativeArrayData* data)
	{
		Request_Internal_ComputeBuffer_3_Injected(buffer, data, out var ret);
		return ret;
	}

	[NativeMethod("Request")]
	private unsafe static AsyncGPUReadbackRequest Request_Internal_ComputeBuffer_4([NotNull] ComputeBuffer src, int size, int offset, AsyncRequestNativeArrayData* data)
	{
		Request_Internal_ComputeBuffer_4_Injected(src, size, offset, data, out var ret);
		return ret;
	}

	[NativeMethod("Request")]
	private static AsyncGPUReadbackRequest Request_Internal_Texture_1([NotNull] Texture src, int mipIndex)
	{
		Request_Internal_Texture_1_Injected(src, mipIndex, out var ret);
		return ret;
	}

	[NativeMethod("Request")]
	private static AsyncGPUReadbackRequest Request_Internal_Texture_2([NotNull] Texture src, int mipIndex, GraphicsFormat dstFormat)
	{
		Request_Internal_Texture_2_Injected(src, mipIndex, dstFormat, out var ret);
		return ret;
	}

	[NativeMethod("Request")]
	private static AsyncGPUReadbackRequest Request_Internal_Texture_3([NotNull] Texture src, int mipIndex, int x, int width, int y, int height, int z, int depth)
	{
		Request_Internal_Texture_3_Injected(src, mipIndex, x, width, y, height, z, depth, out var ret);
		return ret;
	}

	[NativeMethod("Request")]
	private static AsyncGPUReadbackRequest Request_Internal_Texture_4([NotNull] Texture src, int mipIndex, int x, int width, int y, int height, int z, int depth, GraphicsFormat dstFormat)
	{
		Request_Internal_Texture_4_Injected(src, mipIndex, x, width, y, height, z, depth, dstFormat, out var ret);
		return ret;
	}

	[NativeMethod("Request")]
	private unsafe static AsyncGPUReadbackRequest Request_Internal_Texture_5([NotNull] Texture src, int mipIndex, AsyncRequestNativeArrayData* data)
	{
		Request_Internal_Texture_5_Injected(src, mipIndex, data, out var ret);
		return ret;
	}

	[NativeMethod("Request")]
	private unsafe static AsyncGPUReadbackRequest Request_Internal_Texture_6([NotNull] Texture src, int mipIndex, GraphicsFormat dstFormat, AsyncRequestNativeArrayData* data)
	{
		Request_Internal_Texture_6_Injected(src, mipIndex, dstFormat, data, out var ret);
		return ret;
	}

	[NativeMethod("Request")]
	private unsafe static AsyncGPUReadbackRequest Request_Internal_Texture_7([NotNull] Texture src, int mipIndex, int x, int width, int y, int height, int z, int depth, GraphicsFormat dstFormat, AsyncRequestNativeArrayData* data)
	{
		Request_Internal_Texture_7_Injected(src, mipIndex, x, width, y, height, z, depth, dstFormat, data, out var ret);
		return ret;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void Request_Internal_ComputeBuffer_1_Injected(ComputeBuffer buffer, out AsyncGPUReadbackRequest ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void Request_Internal_ComputeBuffer_2_Injected(ComputeBuffer src, int size, int offset, out AsyncGPUReadbackRequest ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private unsafe static extern void Request_Internal_ComputeBuffer_3_Injected(ComputeBuffer buffer, AsyncRequestNativeArrayData* data, out AsyncGPUReadbackRequest ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private unsafe static extern void Request_Internal_ComputeBuffer_4_Injected(ComputeBuffer src, int size, int offset, AsyncRequestNativeArrayData* data, out AsyncGPUReadbackRequest ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void Request_Internal_Texture_1_Injected(Texture src, int mipIndex, out AsyncGPUReadbackRequest ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void Request_Internal_Texture_2_Injected(Texture src, int mipIndex, GraphicsFormat dstFormat, out AsyncGPUReadbackRequest ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void Request_Internal_Texture_3_Injected(Texture src, int mipIndex, int x, int width, int y, int height, int z, int depth, out AsyncGPUReadbackRequest ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void Request_Internal_Texture_4_Injected(Texture src, int mipIndex, int x, int width, int y, int height, int z, int depth, GraphicsFormat dstFormat, out AsyncGPUReadbackRequest ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private unsafe static extern void Request_Internal_Texture_5_Injected(Texture src, int mipIndex, AsyncRequestNativeArrayData* data, out AsyncGPUReadbackRequest ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private unsafe static extern void Request_Internal_Texture_6_Injected(Texture src, int mipIndex, GraphicsFormat dstFormat, AsyncRequestNativeArrayData* data, out AsyncGPUReadbackRequest ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private unsafe static extern void Request_Internal_Texture_7_Injected(Texture src, int mipIndex, int x, int width, int y, int height, int z, int depth, GraphicsFormat dstFormat, AsyncRequestNativeArrayData* data, out AsyncGPUReadbackRequest ret);
}
