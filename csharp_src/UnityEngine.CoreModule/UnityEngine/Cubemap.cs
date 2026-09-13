using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.Bindings;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Internal;
using UnityEngine.Scripting;

namespace UnityEngine;

[NativeHeader("Runtime/Graphics/CubemapTexture.h")]
[ExcludeFromPreset]
public sealed class Cubemap : Texture
{
	public extern TextureFormat format
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeName("GetTextureFormat")]
		get;
	}

	public override extern bool isReadable
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
	}

	public extern bool streamingMipmaps
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
	}

	public extern int streamingMipmapsPriority
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
	}

	public extern int requestedMipmapLevel
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[FreeFunction(Name = "GetTextureStreamingManager().GetRequestedMipmapLevel", HasExplicitThis = true)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[FreeFunction(Name = "GetTextureStreamingManager().SetRequestedMipmapLevel", HasExplicitThis = true)]
		set;
	}

	internal extern bool loadAllMips
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[FreeFunction(Name = "GetTextureStreamingManager().GetLoadAllMips", HasExplicitThis = true)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[FreeFunction(Name = "GetTextureStreamingManager().SetLoadAllMips", HasExplicitThis = true)]
		set;
	}

	public extern int desiredMipmapLevel
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[FreeFunction(Name = "GetTextureStreamingManager().GetDesiredMipmapLevel", HasExplicitThis = true)]
		get;
	}

	public extern int loadingMipmapLevel
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[FreeFunction(Name = "GetTextureStreamingManager().GetLoadingMipmapLevel", HasExplicitThis = true)]
		get;
	}

	public extern int loadedMipmapLevel
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[FreeFunction(Name = "GetTextureStreamingManager().GetLoadedMipmapLevel", HasExplicitThis = true)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("CubemapScripting::Create")]
	private static extern bool Internal_CreateImpl([Writable] Cubemap mono, int ext, int mipCount, GraphicsFormat format, TextureCreationFlags flags, IntPtr nativeTex);

	private static void Internal_Create([Writable] Cubemap mono, int ext, int mipCount, GraphicsFormat format, TextureCreationFlags flags, IntPtr nativeTex)
	{
		if (!Internal_CreateImpl(mono, ext, mipCount, format, flags, nativeTex))
		{
			throw new UnityException("Failed to create texture because of invalid parameters.");
		}
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "CubemapScripting::Apply", HasExplicitThis = true)]
	private extern void ApplyImpl(bool updateMipmaps, bool makeNoLongerReadable);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("CubemapScripting::UpdateExternalTexture", HasExplicitThis = true)]
	public extern void UpdateExternalTexture(IntPtr nativeTexture);

	[NativeName("SetPixel")]
	private void SetPixelImpl(int image, int x, int y, Color color)
	{
		SetPixelImpl_Injected(image, x, y, ref color);
	}

	[NativeName("GetPixel")]
	private Color GetPixelImpl(int image, int x, int y)
	{
		GetPixelImpl_Injected(image, x, y, out var ret);
		return ret;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeName("FixupEdges")]
	public extern void SmoothEdges([DefaultValue("1")] int smoothRegionWidthInPixels);

	public void SmoothEdges()
	{
		SmoothEdges(1);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "CubemapScripting::GetPixels", HasExplicitThis = true, ThrowsException = true)]
	public extern Color[] GetPixels(CubemapFace face, int miplevel);

	public Color[] GetPixels(CubemapFace face)
	{
		return GetPixels(face, 0);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "CubemapScripting::SetPixels", HasExplicitThis = true, ThrowsException = true)]
	public extern void SetPixels(Color[] colors, CubemapFace face, int miplevel);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "CubemapScripting::SetPixelDataArray", HasExplicitThis = true, ThrowsException = true)]
	private extern bool SetPixelDataImplArray(Array data, int mipLevel, int face, int elementSize, int dataArraySize, int sourceDataStartIndex = 0);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "CubemapScripting::SetPixelData", HasExplicitThis = true, ThrowsException = true)]
	private extern bool SetPixelDataImpl(IntPtr data, int mipLevel, int face, int elementSize, int dataArraySize, int sourceDataStartIndex = 0);

	public void SetPixels(Color[] colors, CubemapFace face)
	{
		SetPixels(colors, face, 0);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "GetTextureStreamingManager().ClearRequestedMipmapLevel", HasExplicitThis = true)]
	public extern void ClearRequestedMipmapLevel();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "GetTextureStreamingManager().IsRequestedMipmapLevelLoaded", HasExplicitThis = true)]
	public extern bool IsRequestedMipmapLevelLoaded();

	public Cubemap(int width, DefaultFormat format, TextureCreationFlags flags)
		: this(width, SystemInfo.GetGraphicsFormat(format), flags)
	{
	}

	[RequiredByNativeCode]
	public Cubemap(int width, GraphicsFormat format, TextureCreationFlags flags)
	{
		if (ValidateFormat(format, FormatUsage.Sample))
		{
			Internal_Create(this, width, Texture.GenerateAllMips, format, flags, IntPtr.Zero);
		}
	}

	public Cubemap(int width, TextureFormat format, int mipCount)
		: this(width, format, mipCount, IntPtr.Zero)
	{
	}

	public Cubemap(int width, GraphicsFormat format, TextureCreationFlags flags, int mipCount)
	{
		if (ValidateFormat(format, FormatUsage.Sample))
		{
			Internal_Create(this, width, mipCount, format, flags, IntPtr.Zero);
		}
	}

	internal Cubemap(int width, TextureFormat textureFormat, int mipCount, IntPtr nativeTex)
	{
		if (ValidateFormat(textureFormat))
		{
			GraphicsFormat graphicsFormat = GraphicsFormatUtility.GetGraphicsFormat(textureFormat, isSRGB: false);
			TextureCreationFlags textureCreationFlags = ((mipCount != 1) ? TextureCreationFlags.MipChain : TextureCreationFlags.None);
			if (GraphicsFormatUtility.IsCrunchFormat(textureFormat))
			{
				textureCreationFlags |= TextureCreationFlags.Crunch;
			}
			Internal_Create(this, width, mipCount, graphicsFormat, textureCreationFlags, nativeTex);
		}
	}

	internal Cubemap(int width, TextureFormat textureFormat, bool mipChain, IntPtr nativeTex)
		: this(width, textureFormat, (!mipChain) ? 1 : (-1), nativeTex)
	{
	}

	public Cubemap(int width, TextureFormat textureFormat, bool mipChain)
		: this(width, textureFormat, (!mipChain) ? 1 : (-1), IntPtr.Zero)
	{
	}

	public static Cubemap CreateExternalTexture(int width, TextureFormat format, bool mipmap, IntPtr nativeTex)
	{
		if (nativeTex == IntPtr.Zero)
		{
			throw new ArgumentException("nativeTex can not be null");
		}
		return new Cubemap(width, format, mipmap, nativeTex);
	}

	public void SetPixelData<T>(T[] data, int mipLevel, CubemapFace face, int sourceDataStartIndex = 0)
	{
		if (sourceDataStartIndex < 0)
		{
			throw new UnityException("SetPixelData: sourceDataStartIndex cannot be less than 0.");
		}
		if (!isReadable)
		{
			throw CreateNonReadableException(this);
		}
		if (data == null || data.Length == 0)
		{
			throw new UnityException("No texture data provided to SetPixelData.");
		}
		SetPixelDataImplArray(data, mipLevel, (int)face, Marshal.SizeOf((object)data[0]), data.Length, sourceDataStartIndex);
	}

	public unsafe void SetPixelData<T>(NativeArray<T> data, int mipLevel, CubemapFace face, int sourceDataStartIndex = 0) where T : struct
	{
		if (sourceDataStartIndex < 0)
		{
			throw new UnityException("SetPixelData: sourceDataStartIndex cannot be less than 0.");
		}
		if (!isReadable)
		{
			throw CreateNonReadableException(this);
		}
		if (!data.IsCreated || data.Length == 0)
		{
			throw new UnityException("No texture data provided to SetPixelData.");
		}
		SetPixelDataImpl((IntPtr)data.GetUnsafeReadOnlyPtr(), mipLevel, (int)face, UnsafeUtility.SizeOf<T>(), data.Length, sourceDataStartIndex);
	}

	public void SetPixel(CubemapFace face, int x, int y, Color color)
	{
		if (!isReadable)
		{
			throw CreateNonReadableException(this);
		}
		SetPixelImpl((int)face, x, y, color);
	}

	public Color GetPixel(CubemapFace face, int x, int y)
	{
		if (!isReadable)
		{
			throw CreateNonReadableException(this);
		}
		return GetPixelImpl((int)face, x, y);
	}

	public void Apply([DefaultValue("true")] bool updateMipmaps, [DefaultValue("false")] bool makeNoLongerReadable)
	{
		if (!isReadable)
		{
			throw CreateNonReadableException(this);
		}
		ApplyImpl(updateMipmaps, makeNoLongerReadable);
	}

	public void Apply(bool updateMipmaps)
	{
		Apply(updateMipmaps, makeNoLongerReadable: false);
	}

	public void Apply()
	{
		Apply(updateMipmaps: true, makeNoLongerReadable: false);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern void SetPixelImpl_Injected(int image, int x, int y, ref Color color);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern void GetPixelImpl_Injected(int image, int x, int y, out Color ret);
}
