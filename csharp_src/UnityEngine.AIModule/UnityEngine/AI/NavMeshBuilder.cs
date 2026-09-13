using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine.AI;

[StaticAccessor("NavMeshBuilderBindings", StaticAccessorType.DoubleColon)]
[NativeHeader("Modules/AI/Builder/NavMeshBuilder.bindings.h")]
public static class NavMeshBuilder
{
	public static void CollectSources(Bounds includedWorldBounds, int includedLayerMask, NavMeshCollectGeometry geometry, int defaultArea, List<NavMeshBuildMarkup> markups, List<NavMeshBuildSource> results)
	{
		if (markups == null)
		{
			throw new ArgumentNullException("markups");
		}
		if (results == null)
		{
			throw new ArgumentNullException("results");
		}
		includedWorldBounds.extents = Vector3.Max(includedWorldBounds.extents, 0.001f * Vector3.one);
		NavMeshBuildSource[] collection = CollectSourcesInternal(includedLayerMask, includedWorldBounds, null, useBounds: true, geometry, defaultArea, markups.ToArray());
		results.Clear();
		results.AddRange(collection);
	}

	public static void CollectSources(Transform root, int includedLayerMask, NavMeshCollectGeometry geometry, int defaultArea, List<NavMeshBuildMarkup> markups, List<NavMeshBuildSource> results)
	{
		if (markups == null)
		{
			throw new ArgumentNullException("markups");
		}
		if (results == null)
		{
			throw new ArgumentNullException("results");
		}
		NavMeshBuildSource[] collection = CollectSourcesInternal(includedLayerMask, default(Bounds), root, useBounds: false, geometry, defaultArea, markups.ToArray());
		results.Clear();
		results.AddRange(collection);
	}

	private static NavMeshBuildSource[] CollectSourcesInternal(int includedLayerMask, Bounds includedWorldBounds, Transform root, bool useBounds, NavMeshCollectGeometry geometry, int defaultArea, NavMeshBuildMarkup[] markups)
	{
		return CollectSourcesInternal_Injected(includedLayerMask, ref includedWorldBounds, root, useBounds, geometry, defaultArea, markups);
	}

	public static NavMeshData BuildNavMeshData(NavMeshBuildSettings buildSettings, List<NavMeshBuildSource> sources, Bounds localBounds, Vector3 position, Quaternion rotation)
	{
		if (sources == null)
		{
			throw new ArgumentNullException("sources");
		}
		NavMeshData navMeshData = new NavMeshData(buildSettings.agentTypeID)
		{
			position = position,
			rotation = rotation
		};
		UpdateNavMeshDataListInternal(navMeshData, buildSettings, sources, localBounds);
		return navMeshData;
	}

	public static bool UpdateNavMeshData(NavMeshData data, NavMeshBuildSettings buildSettings, List<NavMeshBuildSource> sources, Bounds localBounds)
	{
		if (data == null)
		{
			throw new ArgumentNullException("data");
		}
		if (sources == null)
		{
			throw new ArgumentNullException("sources");
		}
		return UpdateNavMeshDataListInternal(data, buildSettings, sources, localBounds);
	}

	private static bool UpdateNavMeshDataListInternal(NavMeshData data, NavMeshBuildSettings buildSettings, object sources, Bounds localBounds)
	{
		return UpdateNavMeshDataListInternal_Injected(data, ref buildSettings, sources, ref localBounds);
	}

	public static AsyncOperation UpdateNavMeshDataAsync(NavMeshData data, NavMeshBuildSettings buildSettings, List<NavMeshBuildSource> sources, Bounds localBounds)
	{
		if (data == null)
		{
			throw new ArgumentNullException("data");
		}
		if (sources == null)
		{
			throw new ArgumentNullException("sources");
		}
		return UpdateNavMeshDataAsyncListInternal(data, buildSettings, sources, localBounds);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod("Purge")]
	[NativeHeader("Modules/AI/NavMeshManager.h")]
	[StaticAccessor("GetNavMeshManager().GetNavMeshBuildManager()", StaticAccessorType.Arrow)]
	public static extern void Cancel(NavMeshData data);

	private static AsyncOperation UpdateNavMeshDataAsyncListInternal(NavMeshData data, NavMeshBuildSettings buildSettings, object sources, Bounds localBounds)
	{
		return UpdateNavMeshDataAsyncListInternal_Injected(data, ref buildSettings, sources, ref localBounds);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern NavMeshBuildSource[] CollectSourcesInternal_Injected(int includedLayerMask, ref Bounds includedWorldBounds, Transform root, bool useBounds, NavMeshCollectGeometry geometry, int defaultArea, NavMeshBuildMarkup[] markups);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern bool UpdateNavMeshDataListInternal_Injected(NavMeshData data, ref NavMeshBuildSettings buildSettings, object sources, ref Bounds localBounds);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern AsyncOperation UpdateNavMeshDataAsyncListInternal_Injected(NavMeshData data, ref NavMeshBuildSettings buildSettings, object sources, ref Bounds localBounds);
}
