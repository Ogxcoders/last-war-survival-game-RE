using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Experimental.Rendering;

namespace UnityEngine;

[NativeHeader("Runtime/Graphics/SparseTexture.h")]
public sealed class SparseTexture : Texture
{
	public extern int tileWidth
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
	}

	public extern int tileHeight
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
	}

	public extern bool isCreated
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeName("IsInitialized")]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "SparseTextureScripting::Create", ThrowsException = true)]
	private static extern void Internal_Create([Writable] SparseTexture mono, int width, int height, GraphicsFormat format, int mipCount);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "SparseTextureScripting::UpdateTile", HasExplicitThis = true)]
	public extern void UpdateTile(int tileX, int tileY, int miplevel, Color32[] data);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "SparseTextureScripting::UpdateTileRaw", HasExplicitThis = true)]
	public extern void UpdateTileRaw(int tileX, int tileY, int miplevel, byte[] data);

	public void UnloadTile(int tileX, int tileY, int miplevel)
	{
		UpdateTileRaw(tileX, tileY, miplevel, null);
	}

	internal bool ValidateSize(int width, int height, GraphicsFormat format)
	{
		if (GraphicsFormatUtility.GetBlockSize(format) * (width / GraphicsFormatUtility.GetBlockWidth(format)) * (height / GraphicsFormatUtility.GetBlockHeight(format)) < 65536)
		{
			Debug.LogError($"SparseTexture creation failed. The minimum size in bytes of a SparseTexture is 64KB.", this);
			return false;
		}
		return true;
	}

	public SparseTexture(int width, int height, DefaultFormat format, int mipCount)
		: this(width, height, SystemInfo.GetGraphicsFormat(format), mipCount)
	{
	}

	public SparseTexture(int width, int height, GraphicsFormat format, int mipCount)
	{
		if (ValidateFormat(format, FormatUsage.Sample) && ValidateSize(width, height, format))
		{
			Internal_Create(this, width, height, format, mipCount);
		}
	}

	public SparseTexture(int width, int height, TextureFormat textureFormat, int mipCount)
		: this(width, height, textureFormat, mipCount, linear: false)
	{
	}

	public SparseTexture(int width, int height, TextureFormat textureFormat, int mipCount, bool linear)
	{
		if (ValidateFormat(textureFormat))
		{
			GraphicsFormat format = GraphicsFormatUtility.GetGraphicsFormat(textureFormat, !linear);
			if (ValidateSize(width, height, format))
			{
				Internal_Create(this, width, height, format, mipCount);
			}
		}
	}
}
