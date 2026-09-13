#define UNITY_ASSERTIONS
using System;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.Bindings;

namespace UnityEngine.UIElements;

[NativeHeader("Modules/UIElements/TextNative.bindings.h")]
internal static class TextNative
{
	public static Vector2 GetCursorPosition(TextNativeSettings settings, Rect rect, int cursorIndex)
	{
		if (settings.font == null)
		{
			Debug.LogError("Cannot process a null font.");
			return Vector2.zero;
		}
		return DoGetCursorPosition(settings, rect, cursorIndex);
	}

	public static float ComputeTextWidth(TextNativeSettings settings)
	{
		if (settings.font == null)
		{
			Debug.LogError("Cannot process a null font.");
			return 0f;
		}
		if (string.IsNullOrEmpty(settings.text))
		{
			return 0f;
		}
		return DoComputeTextWidth(settings);
	}

	public static float ComputeTextHeight(TextNativeSettings settings)
	{
		if (settings.font == null)
		{
			Debug.LogError("Cannot process a null font.");
			return 0f;
		}
		if (string.IsNullOrEmpty(settings.text))
		{
			return 0f;
		}
		return DoComputeTextHeight(settings);
	}

	public unsafe static NativeArray<TextVertex> GetVertices(TextNativeSettings settings)
	{
		int vertexCount = 0;
		GetVertices(settings, IntPtr.Zero, UnsafeUtility.SizeOf<TextVertex>(), ref vertexCount);
		NativeArray<TextVertex> nativeArray = new NativeArray<TextVertex>(vertexCount, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
		if (vertexCount > 0)
		{
			GetVertices(settings, (IntPtr)nativeArray.GetUnsafePtr(), UnsafeUtility.SizeOf<TextVertex>(), ref vertexCount);
			Debug.Assert(vertexCount == nativeArray.Length);
		}
		return nativeArray;
	}

	public static Vector2 GetOffset(TextNativeSettings settings, Rect screenRect)
	{
		if (settings.font == null)
		{
			Debug.LogError("Cannot process a null font.");
			return new Vector2(0f, 0f);
		}
		settings.text = settings.text ?? "";
		return DoGetOffset(settings, screenRect);
	}

	public static float ComputeTextScaling(Matrix4x4 worldMatrix, float pixelsPerPoint)
	{
		Vector3 vector = new Vector3(worldMatrix.m00, worldMatrix.m10, worldMatrix.m20);
		Vector3 vector2 = new Vector3(worldMatrix.m01, worldMatrix.m11, worldMatrix.m21);
		float num = (vector.magnitude + vector2.magnitude) / 2f;
		return num * pixelsPerPoint;
	}

	[FreeFunction(Name = "TextNative::ComputeTextWidth")]
	private static float DoComputeTextWidth(TextNativeSettings settings)
	{
		return DoComputeTextWidth_Injected(ref settings);
	}

	[FreeFunction(Name = "TextNative::ComputeTextHeight")]
	private static float DoComputeTextHeight(TextNativeSettings settings)
	{
		return DoComputeTextHeight_Injected(ref settings);
	}

	[FreeFunction(Name = "TextNative::GetCursorPosition")]
	private static Vector2 DoGetCursorPosition(TextNativeSettings settings, Rect rect, int cursorPosition)
	{
		DoGetCursorPosition_Injected(ref settings, ref rect, cursorPosition, out var ret);
		return ret;
	}

	[FreeFunction(Name = "TextNative::GetVertices")]
	private static void GetVertices(TextNativeSettings settings, IntPtr buffer, int vertexSize, ref int vertexCount)
	{
		GetVertices_Injected(ref settings, buffer, vertexSize, ref vertexCount);
	}

	[FreeFunction(Name = "TextNative::GetOffset")]
	private static Vector2 DoGetOffset(TextNativeSettings settings, Rect rect)
	{
		DoGetOffset_Injected(ref settings, ref rect, out var ret);
		return ret;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern float DoComputeTextWidth_Injected(ref TextNativeSettings settings);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern float DoComputeTextHeight_Injected(ref TextNativeSettings settings);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void DoGetCursorPosition_Injected(ref TextNativeSettings settings, ref Rect rect, int cursorPosition, out Vector2 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void GetVertices_Injected(ref TextNativeSettings settings, IntPtr buffer, int vertexSize, ref int vertexCount);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void DoGetOffset_Injected(ref TextNativeSettings settings, ref Rect rect, out Vector2 ret);
}
