using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;
using UnityEngine.U2D;

namespace UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
[NativeType(Header = "Modules/Tilemap/Public/TilemapRenderer.h")]
[NativeHeader("Modules/Grid/Public/GridMarshalling.h")]
[NativeHeader("Modules/Tilemap/TilemapRendererJobs.h")]
[NativeHeader("Modules/Tilemap/Public/TilemapMarshalling.h")]
public sealed class TilemapRenderer : Renderer
{
	public enum SortOrder
	{
		BottomLeft,
		BottomRight,
		TopLeft,
		TopRight
	}

	public enum Mode
	{
		Chunk,
		Individual
	}

	public enum DetectChunkCullingBounds
	{
		Auto,
		Manual
	}

	public Vector3Int chunkSize
	{
		get
		{
			get_chunkSize_Injected(out var ret);
			return ret;
		}
		set
		{
			set_chunkSize_Injected(ref value);
		}
	}

	public Vector3 chunkCullingBounds
	{
		[FreeFunction("TilemapRendererBindings::GetChunkCullingBounds", HasExplicitThis = true)]
		get
		{
			get_chunkCullingBounds_Injected(out var ret);
			return ret;
		}
		[FreeFunction("TilemapRendererBindings::SetChunkCullingBounds", HasExplicitThis = true)]
		set
		{
			set_chunkCullingBounds_Injected(ref value);
		}
	}

	public extern int maxChunkCount
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern int maxFrameAge
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern SortOrder sortOrder
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	[NativeProperty("RenderMode")]
	public extern Mode mode
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern DetectChunkCullingBounds detectChunkCullingBounds
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern SpriteMaskInteraction maskInteraction
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	[RequiredByNativeCode]
	internal void RegisterSpriteAtlasRegistered()
	{
		SpriteAtlasManager.atlasRegistered += OnSpriteAtlasRegistered;
	}

	[RequiredByNativeCode]
	internal void UnregisterSpriteAtlasRegistered()
	{
		SpriteAtlasManager.atlasRegistered -= OnSpriteAtlasRegistered;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal extern void OnSpriteAtlasRegistered(SpriteAtlas atlas);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void get_chunkSize_Injected(out Vector3Int ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void set_chunkSize_Injected(ref Vector3Int value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void get_chunkCullingBounds_Injected(out Vector3 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void set_chunkCullingBounds_Injected(ref Vector3 value);
}
