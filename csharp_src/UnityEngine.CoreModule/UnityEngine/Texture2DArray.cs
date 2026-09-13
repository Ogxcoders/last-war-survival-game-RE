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

[NativeHeader("Runtime/Graphics/Texture2DArray.h")]
public sealed class Texture2DArray : Texture
{
	public static extern int allSlices
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeName("GetAllTextureLayersIdentifier")]
		get;
	}

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

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("Texture2DArrayScripting::Create")]
	private static extern bool Internal_CreateImpl([Writable] Texture2DArray mono, int w, int h, int d, int mipCount, GraphicsFormat format, TextureCreationFlags flags);

	private static void Internal_Create([Writable] Texture2DArray mono, int w, int h, int d, int mipCount, GraphicsFormat format, TextureCreationFlags flags)
	{
		if (!Internal_CreateImpl(mono, w, h, d, mipCount, format, flags))
		{
			throw new UnityException("Failed to create 2D array texture because of invalid parameters.");
		}
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "Texture2DArrayScripting::Apply", HasExplicitThis = true)]
	private extern void ApplyImpl(bool updateMipmaps, bool makeNoLongerReadable);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "Texture2DArrayScripting::GetPixels", HasExplicitThis = true, ThrowsException = true)]
	public extern Color[] GetPixels(int arrayElement, int miplevel);

	public Color[] GetPixels(int arrayElement)
	{
		return GetPixels(arrayElement, 0);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "Texture2DArrayScripting::SetPixelDataArray", HasExplicitThis = true, ThrowsException = true)]
	private extern bool SetPixelDataImplArray(Array data, int mipLevel, int element, int elementSize, int dataArraySize, int sourceDataStartIndex = 0);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "Texture2DArrayScripting::SetPixelData", HasExplicitThis = true, ThrowsException = true)]
	private extern bool SetPixelDataImpl(IntPtr data, int mipLevel, int element, int elementSize, int dataArraySize, int sourceDataStartIndex = 0);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "Texture2DArrayScripting::GetPixels32", HasExplicitThis = true, ThrowsException = true)]
	public extern Color32[] GetPixels32(int arrayElement, int miplevel);

	public Color32[] GetPixels32(int arrayElement)
	{
		return GetPixels32(arrayElement, 0);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "Texture2DArrayScripting::SetPixels", HasExplicitThis = true, ThrowsException = true)]
	public extern void SetPixels(Color[] colors, int arrayElement, int miplevel);

	public void SetPixels(Color[] colors, int arrayElement)
	{
		SetPixels(colors, arrayElement, 0);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "Texture2DArrayScripting::SetPixels32", HasExplicitThis = true, ThrowsException = true)]
	public extern void SetPixels32(Color32[] colors, int arrayElement, int miplevel);

	public void SetPixels32(Color32[] colors, int arrayElement)
	{
		SetPixels32(colors, arrayElement, 0);
	}

	public Texture2DArray(int width, int height, int depth, DefaultFormat format, TextureCreationFlags flags)
		: this(width, height, depth, SystemInfo.GetGraphicsFormat(format), flags)
	{
	}

	[RequiredByNativeCode]
	public Texture2DArray(int width, int height, int depth, GraphicsFormat format, TextureCreationFlags flags)
		: this(width, height, depth, format, flags, Texture.GenerateAllMips)
	{
	}

	public Texture2DArray(int width, int height, int depth, GraphicsFormat format, TextureCreationFlags flags, int mipCount)
	{
		if (ValidateFormat(format, FormatUsage.Sample))
		{
			Internal_Create(this, width, height, depth, mipCount, format, flags);
		}
	}

	public Texture2DArray(int width, int height, int depth, TextureFormat textureFormat, int mipCount, [DefaultValue("true")] bool linear)
	{
		if (ValidateFormat(textureFormat))
		{
			GraphicsFormat graphicsFormat = GraphicsFormatUtility.GetGraphicsFormat(textureFormat, !linear);
			TextureCreationFlags textureCreationFlags = ((mipCount != 1) ? TextureCreationFlags.MipChain : TextureCreationFlags.None);
			if (GraphicsFormatUtility.IsCrunchFormat(textureFormat))
			{
				textureCreationFlags |= TextureCreationFlags.Crunch;
			}
			Internal_Create(this, width, height, depth, mipCount, graphicsFormat, textureCreationFlags);
		}
	}

	public Texture2DArray(int width, int height, int depth, TextureFormat textureFormat, bool mipChain, [DefaultValue("true")] bool linear)
		: this(width, height, depth, textureFormat, (!mipChain) ? 1 : (-1), linear)
	{
	}

	public Texture2DArray(int width, int height, int depth, TextureFormat textureFormat, bool mipChain)
		: this(width, height, depth, textureFormat, (!mipChain) ? 1 : (-1), linear: false)
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

	public void SetPixelData<T>(T[] data, int mipLevel, int element, int sourceDataStartIndex = 0)
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
		SetPixelDataImplArray(data, mipLevel, element, Marshal.SizeOf((object)data[0]), data.Length, sourceDataStartIndex);
	}

	public unsafe void SetPixelData<T>(NativeArray<T> data, int mipLevel, int element, int sourceDataStartIndex = 0) where T : struct
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
		SetPixelDataImpl((IntPtr)data.GetUnsafeReadOnlyPtr(), mipLevel, element, UnsafeUtility.SizeOf<T>(), data.Length, sourceDataStartIndex);
	}

	public void Apply(bool updateMipmaps)
	{
		Apply(updateMipmaps, makeNoLongerReadable: false);
	}

	public void Apply()
	{
		Apply(updateMipmaps: true, makeNoLongerReadable: false);
	}
}
