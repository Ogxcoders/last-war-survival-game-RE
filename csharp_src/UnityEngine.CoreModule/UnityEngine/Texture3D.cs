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

[NativeHeader("Runtime/Graphics/Texture3D.h")]
[ExcludeFromPreset]
public sealed class Texture3D : Texture
{
	public extern int depth
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeName("GetTextureLayerCount")]
		get;
	}

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

	[NativeName("SetPixel")]
	private void SetPixelImpl(int image, int x, int y, int z, Color color)
	{
		SetPixelImpl_Injected(image, x, y, z, ref color);
	}

	[NativeName("GetPixel")]
	private Color GetPixelImpl(int image, int x, int y, int z)
	{
		GetPixelImpl_Injected(image, x, y, z, out var ret);
		return ret;
	}

	[NativeName("GetPixelBilinear")]
	private Color GetPixelBilinearImpl(int image, float u, float v, float w)
	{
		GetPixelBilinearImpl_Injected(image, u, v, w, out var ret);
		return ret;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("Texture3DScripting::Create")]
	private static extern bool Internal_CreateImpl([Writable] Texture3D mono, int w, int h, int d, int mipCount, GraphicsFormat format, TextureCreationFlags flags);

	private static void Internal_Create([Writable] Texture3D mono, int w, int h, int d, int mipCount, GraphicsFormat format, TextureCreationFlags flags)
	{
		if (!Internal_CreateImpl(mono, w, h, d, mipCount, format, flags))
		{
			throw new UnityException("Failed to create texture because of invalid parameters.");
		}
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "Texture3DScripting::Apply", HasExplicitThis = true)]
	private extern void ApplyImpl(bool updateMipmaps, bool makeNoLongerReadable);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "Texture3DScripting::GetPixels", HasExplicitThis = true, ThrowsException = true)]
	public extern Color[] GetPixels(int miplevel);

	public Color[] GetPixels()
	{
		return GetPixels(0);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "Texture3DScripting::GetPixels32", HasExplicitThis = true, ThrowsException = true)]
	public extern Color32[] GetPixels32(int miplevel);

	public Color32[] GetPixels32()
	{
		return GetPixels32(0);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "Texture3DScripting::SetPixels", HasExplicitThis = true, ThrowsException = true)]
	public extern void SetPixels(Color[] colors, int miplevel);

	public void SetPixels(Color[] colors)
	{
		SetPixels(colors, 0);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "Texture3DScripting::SetPixels32", HasExplicitThis = true, ThrowsException = true)]
	public extern void SetPixels32(Color32[] colors, int miplevel);

	public void SetPixels32(Color32[] colors)
	{
		SetPixels32(colors, 0);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "Texture3DScripting::SetPixelDataArray", HasExplicitThis = true, ThrowsException = true)]
	private extern bool SetPixelDataImplArray(Array data, int mipLevel, int elementSize, int dataArraySize, int sourceDataStartIndex = 0);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "Texture3DScripting::SetPixelData", HasExplicitThis = true, ThrowsException = true)]
	private extern bool SetPixelDataImpl(IntPtr data, int mipLevel, int elementSize, int dataArraySize, int sourceDataStartIndex = 0);

	public Texture3D(int width, int height, int depth, DefaultFormat format, TextureCreationFlags flags)
		: this(width, height, depth, SystemInfo.GetGraphicsFormat(format), flags)
	{
	}

	[RequiredByNativeCode]
	public Texture3D(int width, int height, int depth, GraphicsFormat format, TextureCreationFlags flags)
		: this(width, height, depth, format, flags, Texture.GenerateAllMips)
	{
	}

	public Texture3D(int width, int height, int depth, GraphicsFormat format, TextureCreationFlags flags, int mipCount)
	{
		if (ValidateFormat(format, FormatUsage.Sample))
		{
			Internal_Create(this, width, height, depth, mipCount, format, flags);
		}
	}

	public Texture3D(int width, int height, int depth, TextureFormat textureFormat, int mipCount)
	{
		if (ValidateFormat(textureFormat))
		{
			GraphicsFormat graphicsFormat = GraphicsFormatUtility.GetGraphicsFormat(textureFormat, isSRGB: false);
			TextureCreationFlags textureCreationFlags = ((mipCount != 1) ? TextureCreationFlags.MipChain : TextureCreationFlags.None);
			if (GraphicsFormatUtility.IsCrunchFormat(textureFormat))
			{
				textureCreationFlags |= TextureCreationFlags.Crunch;
			}
			Internal_Create(this, width, height, depth, mipCount, graphicsFormat, textureCreationFlags);
		}
	}

	public Texture3D(int width, int height, int depth, TextureFormat textureFormat, bool mipChain)
		: this(width, height, depth, textureFormat, (!mipChain) ? 1 : (-1))
	{
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

	public void SetPixel(int x, int y, int z, Color color)
	{
		if (!isReadable)
		{
			throw CreateNonReadableException(this);
		}
		SetPixelImpl(0, x, y, z, color);
	}

	public void SetPixel(int x, int y, int z, Color color, int mipLevel)
	{
		if (!isReadable)
		{
			throw CreateNonReadableException(this);
		}
		SetPixelImpl(mipLevel, x, y, z, color);
	}

	public Color GetPixel(int x, int y, int z)
	{
		if (!isReadable)
		{
			throw CreateNonReadableException(this);
		}
		return GetPixelImpl(0, x, y, z);
	}

	public Color GetPixel(int x, int y, int z, int mipLevel)
	{
		if (!isReadable)
		{
			throw CreateNonReadableException(this);
		}
		return GetPixelImpl(mipLevel, x, y, z);
	}

	public Color GetPixelBilinear(float u, float v, float w)
	{
		if (!isReadable)
		{
			throw CreateNonReadableException(this);
		}
		return GetPixelBilinearImpl(0, u, v, w);
	}

	public Color GetPixelBilinear(float u, float v, float w, int mipLevel)
	{
		if (!isReadable)
		{
			throw CreateNonReadableException(this);
		}
		return GetPixelBilinearImpl(mipLevel, u, v, w);
	}

	public void SetPixelData<T>(T[] data, int mipLevel, int sourceDataStartIndex = 0)
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
		SetPixelDataImplArray(data, mipLevel, Marshal.SizeOf((object)data[0]), data.Length, sourceDataStartIndex);
	}

	public unsafe void SetPixelData<T>(NativeArray<T> data, int mipLevel, int sourceDataStartIndex = 0) where T : struct
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
		SetPixelDataImpl((IntPtr)data.GetUnsafeReadOnlyPtr(), mipLevel, UnsafeUtility.SizeOf<T>(), data.Length, sourceDataStartIndex);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern void SetPixelImpl_Injected(int image, int x, int y, int z, ref Color color);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern void GetPixelImpl_Injected(int image, int x, int y, int z, out Color ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern void GetPixelBilinearImpl_Injected(int image, float u, float v, float w, out Color ret);
}
