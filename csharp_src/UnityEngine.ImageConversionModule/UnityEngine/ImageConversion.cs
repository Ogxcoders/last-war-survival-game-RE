using System;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.Bindings;
using UnityEngine.Experimental.Rendering;

namespace UnityEngine;

[NativeHeader("Modules/ImageConversion/ScriptBindings/ImageConversion.bindings.h")]
public static class ImageConversion
{
	public static bool EnableLegacyPngGammaRuntimeLoadBehavior
	{
		get
		{
			return GetEnableLegacyPngGammaRuntimeLoadBehavior();
		}
		set
		{
			SetEnableLegacyPngGammaRuntimeLoadBehavior(value);
		}
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod(Name = "ImageConversionBindings::GetEnableLegacyPngGammaRuntimeLoadBehavior", IsFreeFunction = true, ThrowsException = false)]
	private static extern bool GetEnableLegacyPngGammaRuntimeLoadBehavior();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod(Name = "ImageConversionBindings::SetEnableLegacyPngGammaRuntimeLoadBehavior", IsFreeFunction = true, ThrowsException = false)]
	private static extern void SetEnableLegacyPngGammaRuntimeLoadBehavior(bool enable);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod(Name = "ImageConversionBindings::EncodeToTGA", IsFreeFunction = true, ThrowsException = true)]
	public static extern byte[] EncodeToTGA(this Texture2D tex);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod(Name = "ImageConversionBindings::EncodeToPNG", IsFreeFunction = true, ThrowsException = true)]
	public static extern byte[] EncodeToPNG(this Texture2D tex);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod(Name = "ImageConversionBindings::EncodeToJPG", IsFreeFunction = true, ThrowsException = true)]
	public static extern byte[] EncodeToJPG(this Texture2D tex, int quality);

	public static byte[] EncodeToJPG(this Texture2D tex)
	{
		return tex.EncodeToJPG(75);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod(Name = "ImageConversionBindings::EncodeToEXR", IsFreeFunction = true, ThrowsException = true)]
	public static extern byte[] EncodeToEXR(this Texture2D tex, Texture2D.EXRFlags flags);

	public static byte[] EncodeToEXR(this Texture2D tex)
	{
		return tex.EncodeToEXR(Texture2D.EXRFlags.None);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod(Name = "ImageConversionBindings::LoadImage", IsFreeFunction = true)]
	public static extern bool LoadImage([NotNull] this Texture2D tex, byte[] data, bool markNonReadable);

	public static bool LoadImage(this Texture2D tex, byte[] data)
	{
		return tex.LoadImage(data, markNonReadable: false);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("ImageConversionBindings::EncodeArrayToTGA", true)]
	public static extern byte[] EncodeArrayToTGA(Array array, GraphicsFormat format, uint width, uint height, uint rowBytes = 0u);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("ImageConversionBindings::EncodeArrayToPNG", true)]
	public static extern byte[] EncodeArrayToPNG(Array array, GraphicsFormat format, uint width, uint height, uint rowBytes = 0u);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("ImageConversionBindings::EncodeArrayToJPG", true)]
	public static extern byte[] EncodeArrayToJPG(Array array, GraphicsFormat format, uint width, uint height, uint rowBytes = 0u, int quality = 75);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("ImageConversionBindings::EncodeArrayToEXR", true)]
	public static extern byte[] EncodeArrayToEXR(Array array, GraphicsFormat format, uint width, uint height, uint rowBytes = 0u, Texture2D.EXRFlags flags = Texture2D.EXRFlags.None);

	public unsafe static NativeArray<byte> EncodeNativeArrayToTGA<T>(NativeArray<T> input, GraphicsFormat format, uint width, uint height, uint rowBytes = 0u) where T : struct
	{
		int sizeInBytes = input.Length * UnsafeUtility.SizeOf<T>();
		void* dataPointer = UnsafeEncodeNativeArrayToTGA(NativeArrayUnsafeUtility.GetUnsafeBufferPointerWithoutChecks(input), ref sizeInBytes, format, width, height, rowBytes);
		return NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<byte>(dataPointer, sizeInBytes, Allocator.Persistent);
	}

	public unsafe static NativeArray<byte> EncodeNativeArrayToPNG<T>(NativeArray<T> input, GraphicsFormat format, uint width, uint height, uint rowBytes = 0u) where T : struct
	{
		int sizeInBytes = input.Length * UnsafeUtility.SizeOf<T>();
		void* dataPointer = UnsafeEncodeNativeArrayToPNG(NativeArrayUnsafeUtility.GetUnsafeBufferPointerWithoutChecks(input), ref sizeInBytes, format, width, height, rowBytes);
		return NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<byte>(dataPointer, sizeInBytes, Allocator.Persistent);
	}

	public unsafe static NativeArray<byte> EncodeNativeArrayToJPG<T>(NativeArray<T> input, GraphicsFormat format, uint width, uint height, uint rowBytes = 0u, int quality = 75) where T : struct
	{
		int sizeInBytes = input.Length * UnsafeUtility.SizeOf<T>();
		void* dataPointer = UnsafeEncodeNativeArrayToJPG(NativeArrayUnsafeUtility.GetUnsafeBufferPointerWithoutChecks(input), ref sizeInBytes, format, width, height, rowBytes, quality);
		return NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<byte>(dataPointer, sizeInBytes, Allocator.Persistent);
	}

	public unsafe static NativeArray<byte> EncodeNativeArrayToEXR<T>(NativeArray<T> input, GraphicsFormat format, uint width, uint height, uint rowBytes = 0u, Texture2D.EXRFlags flags = Texture2D.EXRFlags.None) where T : struct
	{
		int sizeInBytes = input.Length * UnsafeUtility.SizeOf<T>();
		void* dataPointer = UnsafeEncodeNativeArrayToEXR(NativeArrayUnsafeUtility.GetUnsafeBufferPointerWithoutChecks(input), ref sizeInBytes, format, width, height, rowBytes, flags);
		return NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<byte>(dataPointer, sizeInBytes, Allocator.Persistent);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("ImageConversionBindings::UnsafeEncodeNativeArrayToTGA", true)]
	private unsafe static extern void* UnsafeEncodeNativeArrayToTGA(void* array, ref int sizeInBytes, GraphicsFormat format, uint width, uint height, uint rowBytes = 0u);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("ImageConversionBindings::UnsafeEncodeNativeArrayToPNG", true)]
	private unsafe static extern void* UnsafeEncodeNativeArrayToPNG(void* array, ref int sizeInBytes, GraphicsFormat format, uint width, uint height, uint rowBytes = 0u);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("ImageConversionBindings::UnsafeEncodeNativeArrayToJPG", true)]
	private unsafe static extern void* UnsafeEncodeNativeArrayToJPG(void* array, ref int sizeInBytes, GraphicsFormat format, uint width, uint height, uint rowBytes = 0u, int quality = 75);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("ImageConversionBindings::UnsafeEncodeNativeArrayToEXR", true)]
	private unsafe static extern void* UnsafeEncodeNativeArrayToEXR(void* array, ref int sizeInBytes, GraphicsFormat format, uint width, uint height, uint rowBytes = 0u, Texture2D.EXRFlags flags = Texture2D.EXRFlags.None);
}
