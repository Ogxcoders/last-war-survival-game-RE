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

[NativeHeader("Runtime/Graphics/CubemapArrayTexture.h")]
public sealed class CubemapArray : Texture
{
	public extern int cubemapCount
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
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

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("CubemapArrayScripting::Create")]
	private static extern bool Internal_CreateImpl([Writable] CubemapArray mono, int ext, int count, int mipCount, GraphicsFormat format, TextureCreationFlags flags);

	private static void Internal_Create([Writable] CubemapArray mono, int ext, int count, int mipCount, GraphicsFormat format, TextureCreationFlags flags)
	{
		if (!Internal_CreateImpl(mono, ext, count, mipCount, format, flags))
		{
			throw new UnityException("Failed to create cubemap array texture because of invalid parameters.");
		}
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "CubemapArrayScripting::Apply", HasExplicitThis = true)]
	private extern void ApplyImpl(bool updateMipmaps, bool makeNoLongerReadable);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "CubemapArrayScripting::GetPixels", HasExplicitThis = true, ThrowsException = true)]
	public extern Color[] GetPixels(CubemapFace face, int arrayElement, int miplevel);

	public Color[] GetPixels(CubemapFace face, int arrayElement)
	{
		return GetPixels(face, arrayElement, 0);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "CubemapArrayScripting::GetPixels32", HasExplicitThis = true, ThrowsException = true)]
	public extern Color32[] GetPixels32(CubemapFace face, int arrayElement, int miplevel);

	public Color32[] GetPixels32(CubemapFace face, int arrayElement)
	{
		return GetPixels32(face, arrayElement, 0);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "CubemapArrayScripting::SetPixels", HasExplicitThis = true, ThrowsException = true)]
	public extern void SetPixels(Color[] colors, CubemapFace face, int arrayElement, int miplevel);

	public void SetPixels(Color[] colors, CubemapFace face, int arrayElement)
	{
		SetPixels(colors, face, arrayElement, 0);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "CubemapArrayScripting::SetPixels32", HasExplicitThis = true, ThrowsException = true)]
	public extern void SetPixels32(Color32[] colors, CubemapFace face, int arrayElement, int miplevel);

	public void SetPixels32(Color32[] colors, CubemapFace face, int arrayElement)
	{
		SetPixels32(colors, face, arrayElement, 0);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "CubemapArrayScripting::SetPixelDataArray", HasExplicitThis = true, ThrowsException = true)]
	private extern bool SetPixelDataImplArray(Array data, int mipLevel, int face, int element, int elementSize, int dataArraySize, int sourceDataStartIndex = 0);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "CubemapArrayScripting::SetPixelData", HasExplicitThis = true, ThrowsException = true)]
	private extern bool SetPixelDataImpl(IntPtr data, int mipLevel, int face, int element, int elementSize, int dataArraySize, int sourceDataStartIndex = 0);

	public CubemapArray(int width, int cubemapCount, DefaultFormat format, TextureCreationFlags flags)
		: this(width, cubemapCount, SystemInfo.GetGraphicsFormat(format), flags)
	{
	}

	[RequiredByNativeCode]
	public CubemapArray(int width, int cubemapCount, GraphicsFormat format, TextureCreationFlags flags)
		: this(width, cubemapCount, format, flags, Texture.GenerateAllMips)
	{
	}

	public CubemapArray(int width, int cubemapCount, GraphicsFormat format, TextureCreationFlags flags, int mipCount)
	{
		if (ValidateFormat(format, FormatUsage.Sample))
		{
			Internal_Create(this, width, cubemapCount, mipCount, format, flags);
		}
	}

	public CubemapArray(int width, int cubemapCount, TextureFormat textureFormat, int mipCount, [DefaultValue("true")] bool linear)
	{
		if (ValidateFormat(textureFormat))
		{
			GraphicsFormat graphicsFormat = GraphicsFormatUtility.GetGraphicsFormat(textureFormat, !linear);
			TextureCreationFlags textureCreationFlags = ((mipCount != 1) ? TextureCreationFlags.MipChain : TextureCreationFlags.None);
			if (GraphicsFormatUtility.IsCrunchFormat(textureFormat))
			{
				textureCreationFlags |= TextureCreationFlags.Crunch;
			}
			Internal_Create(this, width, cubemapCount, mipCount, graphicsFormat, textureCreationFlags);
		}
	}

	public CubemapArray(int width, int cubemapCount, TextureFormat textureFormat, bool mipChain, [DefaultValue("true")] bool linear)
		: this(width, cubemapCount, textureFormat, (!mipChain) ? 1 : (-1), linear)
	{
	}

	public CubemapArray(int width, int cubemapCount, TextureFormat textureFormat, bool mipChain)
		: this(width, cubemapCount, textureFormat, (!mipChain) ? 1 : (-1), linear: false)
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

	public void SetPixelData<T>(T[] data, int mipLevel, CubemapFace face, int element, int sourceDataStartIndex = 0)
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
		SetPixelDataImplArray(data, mipLevel, (int)face, element, Marshal.SizeOf((object)data[0]), data.Length, sourceDataStartIndex);
	}

	public unsafe void SetPixelData<T>(NativeArray<T> data, int mipLevel, CubemapFace face, int element, int sourceDataStartIndex = 0) where T : struct
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
		SetPixelDataImpl((IntPtr)data.GetUnsafeReadOnlyPtr(), mipLevel, (int)face, element, UnsafeUtility.SizeOf<T>(), data.Length, sourceDataStartIndex);
	}
}
