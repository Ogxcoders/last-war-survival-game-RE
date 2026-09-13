using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine;

[RequireComponent(typeof(Transform))]
[NativeHeader("ParticleSystemScriptingClasses.h")]
[NativeHeader("Modules/ParticleSystem/ParticleSystemRenderer.h")]
[NativeHeader("Modules/ParticleSystem/ScriptBindings/ParticleSystemRendererScriptBindings.h")]
public sealed class ParticleSystemRenderer : Renderer
{
	[NativeName("RenderAlignment")]
	public extern ParticleSystemRenderSpace alignment
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern ParticleSystemRenderMode renderMode
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern ParticleSystemSortMode sortMode
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern float lengthScale
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern float velocityScale
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern float cameraVelocityScale
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern float normalDirection
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern float shadowBias
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern float sortingFudge
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern float minParticleSize
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern float maxParticleSize
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public Vector3 pivot
	{
		get
		{
			get_pivot_Injected(out var ret);
			return ret;
		}
		set
		{
			set_pivot_Injected(ref value);
		}
	}

	public Vector3 flip
	{
		get
		{
			get_flip_Injected(out var ret);
			return ret;
		}
		set
		{
			set_flip_Injected(ref value);
		}
	}

	public extern SpriteMaskInteraction maskInteraction
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern Material trailMaterial
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	internal extern Material oldTrailMaterial
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern bool enableGPUInstancing
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern bool allowRoll
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern Mesh mesh
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[FreeFunction(Name = "ParticleSystemRendererScriptBindings::GetMesh", HasExplicitThis = true)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[FreeFunction(Name = "ParticleSystemRendererScriptBindings::SetMesh", HasExplicitThis = true)]
		set;
	}

	public extern int meshCount
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
	}

	public extern int activeVertexStreamsCount
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
	}

	[Obsolete("EnableVertexStreams is deprecated.Use SetActiveVertexStreams instead.", false)]
	public void EnableVertexStreams(ParticleSystemVertexStreams streams)
	{
		Internal_SetVertexStreams(streams, enabled: true);
	}

	[Obsolete("DisableVertexStreams is deprecated.Use SetActiveVertexStreams instead.", false)]
	public void DisableVertexStreams(ParticleSystemVertexStreams streams)
	{
		Internal_SetVertexStreams(streams, enabled: false);
	}

	[Obsolete("AreVertexStreamsEnabled is deprecated.Use GetActiveVertexStreams instead.", false)]
	public bool AreVertexStreamsEnabled(ParticleSystemVertexStreams streams)
	{
		return Internal_GetEnabledVertexStreams(streams) == streams;
	}

	[Obsolete("GetEnabledVertexStreams is deprecated.Use GetActiveVertexStreams instead.", false)]
	public ParticleSystemVertexStreams GetEnabledVertexStreams(ParticleSystemVertexStreams streams)
	{
		return Internal_GetEnabledVertexStreams(streams);
	}

	[Obsolete("Internal_SetVertexStreams is deprecated.Use SetActiveVertexStreams instead.", false)]
	internal void Internal_SetVertexStreams(ParticleSystemVertexStreams streams, bool enabled)
	{
		List<ParticleSystemVertexStream> list = new List<ParticleSystemVertexStream>(activeVertexStreamsCount);
		GetActiveVertexStreams(list);
		if (enabled)
		{
			if ((streams & ParticleSystemVertexStreams.Position) != ParticleSystemVertexStreams.None && !list.Contains(ParticleSystemVertexStream.Position))
			{
				list.Add(ParticleSystemVertexStream.Position);
			}
			if ((streams & ParticleSystemVertexStreams.Normal) != ParticleSystemVertexStreams.None && !list.Contains(ParticleSystemVertexStream.Normal))
			{
				list.Add(ParticleSystemVertexStream.Normal);
			}
			if ((streams & ParticleSystemVertexStreams.Tangent) != ParticleSystemVertexStreams.None && !list.Contains(ParticleSystemVertexStream.Tangent))
			{
				list.Add(ParticleSystemVertexStream.Tangent);
			}
			if ((streams & ParticleSystemVertexStreams.Color) != ParticleSystemVertexStreams.None && !list.Contains(ParticleSystemVertexStream.Color))
			{
				list.Add(ParticleSystemVertexStream.Color);
			}
			if ((streams & ParticleSystemVertexStreams.UV) != ParticleSystemVertexStreams.None && !list.Contains(ParticleSystemVertexStream.UV))
			{
				list.Add(ParticleSystemVertexStream.UV);
			}
			if ((streams & ParticleSystemVertexStreams.UV2BlendAndFrame) != ParticleSystemVertexStreams.None && !list.Contains(ParticleSystemVertexStream.UV2))
			{
				list.Add(ParticleSystemVertexStream.UV2);
				list.Add(ParticleSystemVertexStream.AnimBlend);
				list.Add(ParticleSystemVertexStream.AnimFrame);
			}
			if ((streams & ParticleSystemVertexStreams.CenterAndVertexID) != ParticleSystemVertexStreams.None && !list.Contains(ParticleSystemVertexStream.Center))
			{
				list.Add(ParticleSystemVertexStream.Center);
				list.Add(ParticleSystemVertexStream.VertexID);
			}
			if ((streams & ParticleSystemVertexStreams.Size) != ParticleSystemVertexStreams.None && !list.Contains(ParticleSystemVertexStream.SizeXYZ))
			{
				list.Add(ParticleSystemVertexStream.SizeXYZ);
			}
			if ((streams & ParticleSystemVertexStreams.Rotation) != ParticleSystemVertexStreams.None && !list.Contains(ParticleSystemVertexStream.Rotation3D))
			{
				list.Add(ParticleSystemVertexStream.Rotation3D);
			}
			if ((streams & ParticleSystemVertexStreams.Velocity) != ParticleSystemVertexStreams.None && !list.Contains(ParticleSystemVertexStream.Velocity))
			{
				list.Add(ParticleSystemVertexStream.Velocity);
			}
			if ((streams & ParticleSystemVertexStreams.Lifetime) != ParticleSystemVertexStreams.None && !list.Contains(ParticleSystemVertexStream.AgePercent))
			{
				list.Add(ParticleSystemVertexStream.AgePercent);
				list.Add(ParticleSystemVertexStream.InvStartLifetime);
			}
			if ((streams & ParticleSystemVertexStreams.Custom1) != ParticleSystemVertexStreams.None && !list.Contains(ParticleSystemVertexStream.Custom1XYZW))
			{
				list.Add(ParticleSystemVertexStream.Custom1XYZW);
			}
			if ((streams & ParticleSystemVertexStreams.Custom2) != ParticleSystemVertexStreams.None && !list.Contains(ParticleSystemVertexStream.Custom2XYZW))
			{
				list.Add(ParticleSystemVertexStream.Custom2XYZW);
			}
			if ((streams & ParticleSystemVertexStreams.Random) != ParticleSystemVertexStreams.None && !list.Contains(ParticleSystemVertexStream.StableRandomXYZ))
			{
				list.Add(ParticleSystemVertexStream.StableRandomXYZ);
				list.Add(ParticleSystemVertexStream.VaryingRandomX);
			}
		}
		else
		{
			if ((streams & ParticleSystemVertexStreams.Position) != ParticleSystemVertexStreams.None)
			{
				list.Remove(ParticleSystemVertexStream.Position);
			}
			if ((streams & ParticleSystemVertexStreams.Normal) != ParticleSystemVertexStreams.None)
			{
				list.Remove(ParticleSystemVertexStream.Normal);
			}
			if ((streams & ParticleSystemVertexStreams.Tangent) != ParticleSystemVertexStreams.None)
			{
				list.Remove(ParticleSystemVertexStream.Tangent);
			}
			if ((streams & ParticleSystemVertexStreams.Color) != ParticleSystemVertexStreams.None)
			{
				list.Remove(ParticleSystemVertexStream.Color);
			}
			if ((streams & ParticleSystemVertexStreams.UV) != ParticleSystemVertexStreams.None)
			{
				list.Remove(ParticleSystemVertexStream.UV);
			}
			if ((streams & ParticleSystemVertexStreams.UV2BlendAndFrame) != ParticleSystemVertexStreams.None)
			{
				list.Remove(ParticleSystemVertexStream.UV2);
				list.Remove(ParticleSystemVertexStream.AnimBlend);
				list.Remove(ParticleSystemVertexStream.AnimFrame);
			}
			if ((streams & ParticleSystemVertexStreams.CenterAndVertexID) != ParticleSystemVertexStreams.None)
			{
				list.Remove(ParticleSystemVertexStream.Center);
				list.Remove(ParticleSystemVertexStream.VertexID);
			}
			if ((streams & ParticleSystemVertexStreams.Size) != ParticleSystemVertexStreams.None)
			{
				list.Remove(ParticleSystemVertexStream.SizeXYZ);
			}
			if ((streams & ParticleSystemVertexStreams.Rotation) != ParticleSystemVertexStreams.None)
			{
				list.Remove(ParticleSystemVertexStream.Rotation3D);
			}
			if ((streams & ParticleSystemVertexStreams.Velocity) != ParticleSystemVertexStreams.None)
			{
				list.Remove(ParticleSystemVertexStream.Velocity);
			}
			if ((streams & ParticleSystemVertexStreams.Lifetime) != ParticleSystemVertexStreams.None)
			{
				list.Remove(ParticleSystemVertexStream.AgePercent);
				list.Remove(ParticleSystemVertexStream.InvStartLifetime);
			}
			if ((streams & ParticleSystemVertexStreams.Custom1) != ParticleSystemVertexStreams.None)
			{
				list.Remove(ParticleSystemVertexStream.Custom1XYZW);
			}
			if ((streams & ParticleSystemVertexStreams.Custom2) != ParticleSystemVertexStreams.None)
			{
				list.Remove(ParticleSystemVertexStream.Custom2XYZW);
			}
			if ((streams & ParticleSystemVertexStreams.Random) != ParticleSystemVertexStreams.None)
			{
				list.Remove(ParticleSystemVertexStream.StableRandomXYZW);
				list.Remove(ParticleSystemVertexStream.VaryingRandomX);
			}
		}
		SetActiveVertexStreams(list);
	}

	[Obsolete("Internal_GetVertexStreams is deprecated.Use GetActiveVertexStreams instead.", false)]
	internal ParticleSystemVertexStreams Internal_GetEnabledVertexStreams(ParticleSystemVertexStreams streams)
	{
		List<ParticleSystemVertexStream> list = new List<ParticleSystemVertexStream>(activeVertexStreamsCount);
		GetActiveVertexStreams(list);
		ParticleSystemVertexStreams particleSystemVertexStreams = ParticleSystemVertexStreams.None;
		if (list.Contains(ParticleSystemVertexStream.Position))
		{
			particleSystemVertexStreams |= ParticleSystemVertexStreams.Position;
		}
		if (list.Contains(ParticleSystemVertexStream.Normal))
		{
			particleSystemVertexStreams |= ParticleSystemVertexStreams.Normal;
		}
		if (list.Contains(ParticleSystemVertexStream.Tangent))
		{
			particleSystemVertexStreams |= ParticleSystemVertexStreams.Tangent;
		}
		if (list.Contains(ParticleSystemVertexStream.Color))
		{
			particleSystemVertexStreams |= ParticleSystemVertexStreams.Color;
		}
		if (list.Contains(ParticleSystemVertexStream.UV))
		{
			particleSystemVertexStreams |= ParticleSystemVertexStreams.UV;
		}
		if (list.Contains(ParticleSystemVertexStream.UV2))
		{
			particleSystemVertexStreams |= ParticleSystemVertexStreams.UV2BlendAndFrame;
		}
		if (list.Contains(ParticleSystemVertexStream.Center))
		{
			particleSystemVertexStreams |= ParticleSystemVertexStreams.CenterAndVertexID;
		}
		if (list.Contains(ParticleSystemVertexStream.SizeXYZ))
		{
			particleSystemVertexStreams |= ParticleSystemVertexStreams.Size;
		}
		if (list.Contains(ParticleSystemVertexStream.Rotation3D))
		{
			particleSystemVertexStreams |= ParticleSystemVertexStreams.Rotation;
		}
		if (list.Contains(ParticleSystemVertexStream.Velocity))
		{
			particleSystemVertexStreams |= ParticleSystemVertexStreams.Velocity;
		}
		if (list.Contains(ParticleSystemVertexStream.AgePercent))
		{
			particleSystemVertexStreams |= ParticleSystemVertexStreams.Lifetime;
		}
		if (list.Contains(ParticleSystemVertexStream.Custom1XYZW))
		{
			particleSystemVertexStreams |= ParticleSystemVertexStreams.Custom1;
		}
		if (list.Contains(ParticleSystemVertexStream.Custom2XYZW))
		{
			particleSystemVertexStreams |= ParticleSystemVertexStreams.Custom2;
		}
		if (list.Contains(ParticleSystemVertexStream.StableRandomXYZ))
		{
			particleSystemVertexStreams |= ParticleSystemVertexStreams.Random;
		}
		return particleSystemVertexStreams & streams;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "ParticleSystemRendererScriptBindings::GetMeshes", HasExplicitThis = true)]
	[RequiredByNativeCode]
	public extern int GetMeshes([Out][NotNull] Mesh[] meshes);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "ParticleSystemRendererScriptBindings::SetMeshes", HasExplicitThis = true)]
	public extern void SetMeshes([NotNull] Mesh[] meshes, int size);

	public void SetMeshes(Mesh[] meshes)
	{
		SetMeshes(meshes, meshes.Length);
	}

	public void BakeMesh(Mesh mesh, bool useTransform = false)
	{
		BakeMesh(mesh, Camera.main, useTransform);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	public extern void BakeMesh([NotNull] Mesh mesh, [NotNull] Camera camera, bool useTransform = false);

	public void BakeTrailsMesh(Mesh mesh, bool useTransform = false)
	{
		BakeTrailsMesh(mesh, Camera.main, useTransform);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	public extern void BakeTrailsMesh([NotNull] Mesh mesh, [NotNull] Camera camera, bool useTransform = false);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "ParticleSystemRendererScriptBindings::SetActiveVertexStreams", HasExplicitThis = true)]
	public extern void SetActiveVertexStreams([NotNull] List<ParticleSystemVertexStream> streams);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "ParticleSystemRendererScriptBindings::GetActiveVertexStreams", HasExplicitThis = true)]
	public extern void GetActiveVertexStreams([NotNull] List<ParticleSystemVertexStream> streams);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void get_pivot_Injected(out Vector3 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void set_pivot_Injected(ref Vector3 value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void get_flip_Injected(out Vector3 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void set_flip_Injected(ref Vector3 value);
}
