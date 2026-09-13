using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.AI;

[NativeHeader("Modules/AI/NavMeshManager.h")]
[MovedFrom("UnityEngine")]
[StaticAccessor("NavMeshBindings", StaticAccessorType.DoubleColon)]
[NativeHeader("Modules/AI/NavMesh/NavMesh.bindings.h")]
public static class NavMesh
{
	public delegate void OnNavMeshPreUpdate();

	public const int AllAreas = -1;

	public static OnNavMeshPreUpdate onPreUpdate;

	[StaticAccessor("GetNavMeshManager()")]
	public static extern float avoidancePredictionTime
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	[StaticAccessor("GetNavMeshManager()")]
	public static extern int pathfindingIterationsPerFrame
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	[RequiredByNativeCode]
	private static void Internal_CallOnNavMeshPreUpdate()
	{
		if (onPreUpdate != null)
		{
			onPreUpdate();
		}
	}

	public static bool Raycast(Vector3 sourcePosition, Vector3 targetPosition, out NavMeshHit hit, int areaMask)
	{
		return Raycast_Injected(ref sourcePosition, ref targetPosition, out hit, areaMask);
	}

	public static bool CalculatePath(Vector3 sourcePosition, Vector3 targetPosition, int areaMask, NavMeshPath path)
	{
		path.ClearCorners();
		return CalculatePathInternal(sourcePosition, targetPosition, areaMask, path);
	}

	private static bool CalculatePathInternal(Vector3 sourcePosition, Vector3 targetPosition, int areaMask, NavMeshPath path)
	{
		return CalculatePathInternal_Injected(ref sourcePosition, ref targetPosition, areaMask, path);
	}

	public static bool FindClosestEdge(Vector3 sourcePosition, out NavMeshHit hit, int areaMask)
	{
		return FindClosestEdge_Injected(ref sourcePosition, out hit, areaMask);
	}

	public static bool SamplePosition(Vector3 sourcePosition, out NavMeshHit hit, float maxDistance, int areaMask)
	{
		return SamplePosition_Injected(ref sourcePosition, out hit, maxDistance, areaMask);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[Obsolete("Use SetAreaCost instead.")]
	[NativeName("SetAreaCost")]
	[StaticAccessor("GetNavMeshProjectSettings()")]
	public static extern void SetLayerCost(int layer, float cost);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[StaticAccessor("GetNavMeshProjectSettings()")]
	[NativeName("GetAreaCost")]
	[Obsolete("Use GetAreaCost instead.")]
	public static extern float GetLayerCost(int layer);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[Obsolete("Use GetAreaFromName instead.")]
	[StaticAccessor("GetNavMeshProjectSettings()")]
	[NativeName("GetAreaFromName")]
	public static extern int GetNavMeshLayerFromName(string layerName);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[StaticAccessor("GetNavMeshProjectSettings()")]
	[NativeName("SetAreaCost")]
	public static extern void SetAreaCost(int areaIndex, float cost);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[StaticAccessor("GetNavMeshProjectSettings()")]
	[NativeName("GetAreaCost")]
	public static extern float GetAreaCost(int areaIndex);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[StaticAccessor("GetNavMeshProjectSettings()")]
	[NativeName("GetAreaFromName")]
	public static extern int GetAreaFromName(string areaName);

	public static NavMeshTriangulation CalculateTriangulation()
	{
		CalculateTriangulation_Injected(out var ret);
		return ret;
	}

	[Obsolete("use NavMesh.CalculateTriangulation() instead.")]
	public static void Triangulate(out Vector3[] vertices, out int[] indices)
	{
		NavMeshTriangulation navMeshTriangulation = CalculateTriangulation();
		vertices = navMeshTriangulation.vertices;
		indices = navMeshTriangulation.indices;
	}

	[Obsolete("AddOffMeshLinks has no effect and is deprecated.")]
	public static void AddOffMeshLinks()
	{
	}

	[Obsolete("RestoreNavMesh has no effect and is deprecated.")]
	public static void RestoreNavMesh()
	{
	}

	public static NavMeshDataInstance AddNavMeshData(NavMeshData navMeshData)
	{
		if (navMeshData == null)
		{
			throw new ArgumentNullException("navMeshData");
		}
		return new NavMeshDataInstance
		{
			id = AddNavMeshDataInternal(navMeshData)
		};
	}

	public static NavMeshDataInstance AddNavMeshData(NavMeshData navMeshData, Vector3 position, Quaternion rotation)
	{
		if (navMeshData == null)
		{
			throw new ArgumentNullException("navMeshData");
		}
		return new NavMeshDataInstance
		{
			id = AddNavMeshDataTransformedInternal(navMeshData, position, rotation)
		};
	}

	public static void RemoveNavMeshData(NavMeshDataInstance handle)
	{
		RemoveNavMeshDataInternal(handle.id);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeName("IsValidSurfaceID")]
	[StaticAccessor("GetNavMeshManager()")]
	internal static extern bool IsValidNavMeshDataHandle(int handle);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[StaticAccessor("GetNavMeshManager()")]
	internal static extern bool IsValidLinkHandle(int handle);

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern Object InternalGetOwner(int dataID);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[StaticAccessor("GetNavMeshManager()")]
	[NativeName("SetSurfaceUserID")]
	internal static extern bool InternalSetOwner(int dataID, int ownerID);

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern Object InternalGetLinkOwner(int linkID);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[StaticAccessor("GetNavMeshManager()")]
	[NativeName("SetLinkUserID")]
	internal static extern bool InternalSetLinkOwner(int linkID, int ownerID);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[StaticAccessor("GetNavMeshManager()")]
	[NativeName("LoadData")]
	internal static extern int AddNavMeshDataInternal(NavMeshData navMeshData);

	[StaticAccessor("GetNavMeshManager()")]
	[NativeName("LoadData")]
	internal static int AddNavMeshDataTransformedInternal(NavMeshData navMeshData, Vector3 position, Quaternion rotation)
	{
		return AddNavMeshDataTransformedInternal_Injected(navMeshData, ref position, ref rotation);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[StaticAccessor("GetNavMeshManager()")]
	[NativeName("UnloadData")]
	internal static extern void RemoveNavMeshDataInternal(int handle);

	public static NavMeshLinkInstance AddLink(NavMeshLinkData link)
	{
		return new NavMeshLinkInstance
		{
			id = AddLinkInternal(link, Vector3.zero, Quaternion.identity)
		};
	}

	public static NavMeshLinkInstance AddLink(NavMeshLinkData link, Vector3 position, Quaternion rotation)
	{
		return new NavMeshLinkInstance
		{
			id = AddLinkInternal(link, position, rotation)
		};
	}

	public static void RemoveLink(NavMeshLinkInstance handle)
	{
		RemoveLinkInternal(handle.id);
	}

	[StaticAccessor("GetNavMeshManager()")]
	[NativeName("AddLink")]
	internal static int AddLinkInternal(NavMeshLinkData link, Vector3 position, Quaternion rotation)
	{
		return AddLinkInternal_Injected(ref link, ref position, ref rotation);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[StaticAccessor("GetNavMeshManager()")]
	[NativeName("RemoveLink")]
	internal static extern void RemoveLinkInternal(int handle);

	public static bool SamplePosition(Vector3 sourcePosition, out NavMeshHit hit, float maxDistance, NavMeshQueryFilter filter)
	{
		return SamplePositionFilter(sourcePosition, out hit, maxDistance, filter.agentTypeID, filter.areaMask);
	}

	private static bool SamplePositionFilter(Vector3 sourcePosition, out NavMeshHit hit, float maxDistance, int type, int mask)
	{
		return SamplePositionFilter_Injected(ref sourcePosition, out hit, maxDistance, type, mask);
	}

	public static bool FindClosestEdge(Vector3 sourcePosition, out NavMeshHit hit, NavMeshQueryFilter filter)
	{
		return FindClosestEdgeFilter(sourcePosition, out hit, filter.agentTypeID, filter.areaMask);
	}

	private static bool FindClosestEdgeFilter(Vector3 sourcePosition, out NavMeshHit hit, int type, int mask)
	{
		return FindClosestEdgeFilter_Injected(ref sourcePosition, out hit, type, mask);
	}

	public static bool Raycast(Vector3 sourcePosition, Vector3 targetPosition, out NavMeshHit hit, NavMeshQueryFilter filter)
	{
		return RaycastFilter(sourcePosition, targetPosition, out hit, filter.agentTypeID, filter.areaMask);
	}

	private static bool RaycastFilter(Vector3 sourcePosition, Vector3 targetPosition, out NavMeshHit hit, int type, int mask)
	{
		return RaycastFilter_Injected(ref sourcePosition, ref targetPosition, out hit, type, mask);
	}

	public static bool CalculatePath(Vector3 sourcePosition, Vector3 targetPosition, NavMeshQueryFilter filter, NavMeshPath path)
	{
		path.ClearCorners();
		return CalculatePathFilterInternal(sourcePosition, targetPosition, path, filter.agentTypeID, filter.areaMask, filter.costs);
	}

	private static bool CalculatePathFilterInternal(Vector3 sourcePosition, Vector3 targetPosition, NavMeshPath path, int type, int mask, float[] costs)
	{
		return CalculatePathFilterInternal_Injected(ref sourcePosition, ref targetPosition, path, type, mask, costs);
	}

	[StaticAccessor("GetNavMeshProjectSettings()")]
	public static NavMeshBuildSettings CreateSettings()
	{
		CreateSettings_Injected(out var ret);
		return ret;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[StaticAccessor("GetNavMeshProjectSettings()")]
	public static extern void RemoveSettings(int agentTypeID);

	public static NavMeshBuildSettings GetSettingsByID(int agentTypeID)
	{
		GetSettingsByID_Injected(agentTypeID, out var ret);
		return ret;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[StaticAccessor("GetNavMeshProjectSettings()")]
	public static extern int GetSettingsCount();

	public static NavMeshBuildSettings GetSettingsByIndex(int index)
	{
		GetSettingsByIndex_Injected(index, out var ret);
		return ret;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern string GetSettingsNameFromID(int agentTypeID);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[StaticAccessor("GetNavMeshManager()")]
	[NativeName("CleanupAfterCarving")]
	public static extern void RemoveAllNavMeshData();

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern bool Raycast_Injected(ref Vector3 sourcePosition, ref Vector3 targetPosition, out NavMeshHit hit, int areaMask);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern bool CalculatePathInternal_Injected(ref Vector3 sourcePosition, ref Vector3 targetPosition, int areaMask, NavMeshPath path);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern bool FindClosestEdge_Injected(ref Vector3 sourcePosition, out NavMeshHit hit, int areaMask);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern bool SamplePosition_Injected(ref Vector3 sourcePosition, out NavMeshHit hit, float maxDistance, int areaMask);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void CalculateTriangulation_Injected(out NavMeshTriangulation ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern int AddNavMeshDataTransformedInternal_Injected(NavMeshData navMeshData, ref Vector3 position, ref Quaternion rotation);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern int AddLinkInternal_Injected(ref NavMeshLinkData link, ref Vector3 position, ref Quaternion rotation);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern bool SamplePositionFilter_Injected(ref Vector3 sourcePosition, out NavMeshHit hit, float maxDistance, int type, int mask);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern bool FindClosestEdgeFilter_Injected(ref Vector3 sourcePosition, out NavMeshHit hit, int type, int mask);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern bool RaycastFilter_Injected(ref Vector3 sourcePosition, ref Vector3 targetPosition, out NavMeshHit hit, int type, int mask);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern bool CalculatePathFilterInternal_Injected(ref Vector3 sourcePosition, ref Vector3 targetPosition, NavMeshPath path, int type, int mask, float[] costs);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void CreateSettings_Injected(out NavMeshBuildSettings ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void GetSettingsByID_Injected(int agentTypeID, out NavMeshBuildSettings ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void GetSettingsByIndex_Injected(int index, out NavMeshBuildSettings ret);
}
