using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.AI;

[MovedFrom("UnityEngine")]
[NativeHeader("Modules/AI/Components/NavMeshAgent.bindings.h")]
[NativeHeader("Modules/AI/NavMesh/NavMesh.bindings.h")]
public sealed class NavMeshAgent : Behaviour
{
	public Vector3 destination
	{
		get
		{
			get_destination_Injected(out var ret);
			return ret;
		}
		set
		{
			set_destination_Injected(ref value);
		}
	}

	public extern float stoppingDistance
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public Vector3 velocity
	{
		get
		{
			get_velocity_Injected(out var ret);
			return ret;
		}
		set
		{
			set_velocity_Injected(ref value);
		}
	}

	[NativeProperty("Position")]
	public Vector3 nextPosition
	{
		get
		{
			get_nextPosition_Injected(out var ret);
			return ret;
		}
		set
		{
			set_nextPosition_Injected(ref value);
		}
	}

	public Vector3 steeringTarget
	{
		get
		{
			get_steeringTarget_Injected(out var ret);
			return ret;
		}
	}

	public Vector3 desiredVelocity
	{
		get
		{
			get_desiredVelocity_Injected(out var ret);
			return ret;
		}
	}

	public extern float remainingDistance
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
	}

	public extern float baseOffset
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern bool isOnOffMeshLink
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeName("IsOnOffMeshLink")]
		get;
	}

	public OffMeshLinkData currentOffMeshLinkData => GetCurrentOffMeshLinkDataInternal();

	public OffMeshLinkData nextOffMeshLinkData => GetNextOffMeshLinkDataInternal();

	public extern bool autoTraverseOffMeshLink
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern bool autoBraking
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern bool autoRepath
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern bool hasPath
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeName("HasPath")]
		get;
	}

	public extern bool pathPending
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeName("PathPending")]
		get;
	}

	public extern bool isPathStale
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeName("IsPathStale")]
		get;
	}

	public extern NavMeshPathStatus pathStatus
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
	}

	[NativeProperty("EndPositionOfCurrentPath")]
	public Vector3 pathEndPosition
	{
		get
		{
			get_pathEndPosition_Injected(out var ret);
			return ret;
		}
	}

	public extern bool isStopped
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[FreeFunction("NavMeshAgentScriptBindings::GetIsStopped", HasExplicitThis = true)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[FreeFunction("NavMeshAgentScriptBindings::SetIsStopped", HasExplicitThis = true)]
		set;
	}

	public NavMeshPath path
	{
		get
		{
			NavMeshPath result = new NavMeshPath();
			CopyPathTo(result);
			return result;
		}
		set
		{
			if (value == null)
			{
				throw new NullReferenceException();
			}
			SetPath(value);
		}
	}

	public Object navMeshOwner => GetOwnerInternal();

	public extern int agentTypeID
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	[Obsolete("Use areaMask instead.")]
	public int walkableMask
	{
		get
		{
			return areaMask;
		}
		set
		{
			areaMask = value;
		}
	}

	public extern int areaMask
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern float speed
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern float angularSpeed
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern float acceleration
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern bool updatePosition
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern bool updateRotation
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern bool updateUpAxis
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern float radius
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern float height
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern ObstacleAvoidanceType obstacleAvoidanceType
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern int avoidancePriority
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern bool isOnNavMesh
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeName("InCrowdSystem")]
		get;
	}

	public bool SetDestination(Vector3 target)
	{
		return SetDestination_Injected(ref target);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	public extern void ActivateCurrentOffMeshLink(bool activated);

	[FreeFunction("NavMeshAgentScriptBindings::GetCurrentOffMeshLinkDataInternal", HasExplicitThis = true)]
	internal OffMeshLinkData GetCurrentOffMeshLinkDataInternal()
	{
		GetCurrentOffMeshLinkDataInternal_Injected(out var ret);
		return ret;
	}

	[FreeFunction("NavMeshAgentScriptBindings::GetNextOffMeshLinkDataInternal", HasExplicitThis = true)]
	internal OffMeshLinkData GetNextOffMeshLinkDataInternal()
	{
		GetNextOffMeshLinkDataInternal_Injected(out var ret);
		return ret;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	public extern void CompleteOffMeshLink();

	public bool Warp(Vector3 newPosition)
	{
		return Warp_Injected(ref newPosition);
	}

	public void Move(Vector3 offset)
	{
		Move_Injected(ref offset);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[Obsolete("Set isStopped to true instead.")]
	public extern void Stop();

	[Obsolete("Set isStopped to true instead.")]
	public void Stop(bool stopUpdates)
	{
		Stop();
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[Obsolete("Set isStopped to false instead.")]
	public extern void Resume();

	[MethodImpl(MethodImplOptions.InternalCall)]
	public extern void ResetPath();

	[MethodImpl(MethodImplOptions.InternalCall)]
	public extern bool SetPath([NotNull] NavMeshPath path);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod("CopyPath")]
	internal extern void CopyPathTo([NotNull] NavMeshPath path);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeName("DistanceToEdge")]
	public extern bool FindClosestEdge(out NavMeshHit hit);

	public bool Raycast(Vector3 targetPosition, out NavMeshHit hit)
	{
		return Raycast_Injected(ref targetPosition, out hit);
	}

	public bool CalculatePath(Vector3 targetPosition, NavMeshPath path)
	{
		path.ClearCorners();
		return CalculatePathInternal(targetPosition, path);
	}

	[FreeFunction("NavMeshAgentScriptBindings::CalculatePathInternal", HasExplicitThis = true)]
	private bool CalculatePathInternal(Vector3 targetPosition, [NotNull] NavMeshPath path)
	{
		return CalculatePathInternal_Injected(ref targetPosition, path);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	public extern bool SamplePathPosition(int areaMask, float maxDistance, out NavMeshHit hit);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[Obsolete("Use SetAreaCost instead.")]
	[NativeMethod("SetAreaCost")]
	public extern void SetLayerCost(int layer, float cost);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[Obsolete("Use GetAreaCost instead.")]
	[NativeMethod("GetAreaCost")]
	public extern float GetLayerCost(int layer);

	[MethodImpl(MethodImplOptions.InternalCall)]
	public extern void SetAreaCost(int areaIndex, float areaCost);

	[MethodImpl(MethodImplOptions.InternalCall)]
	public extern float GetAreaCost(int areaIndex);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeName("GetCurrentPolygonOwner")]
	private extern Object GetOwnerInternal();

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern bool SetDestination_Injected(ref Vector3 target);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void get_destination_Injected(out Vector3 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void set_destination_Injected(ref Vector3 value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void get_velocity_Injected(out Vector3 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void set_velocity_Injected(ref Vector3 value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void get_nextPosition_Injected(out Vector3 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void set_nextPosition_Injected(ref Vector3 value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void get_steeringTarget_Injected(out Vector3 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void get_desiredVelocity_Injected(out Vector3 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern void GetCurrentOffMeshLinkDataInternal_Injected(out OffMeshLinkData ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern void GetNextOffMeshLinkDataInternal_Injected(out OffMeshLinkData ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void get_pathEndPosition_Injected(out Vector3 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern bool Warp_Injected(ref Vector3 newPosition);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern void Move_Injected(ref Vector3 offset);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern bool Raycast_Injected(ref Vector3 targetPosition, out NavMeshHit hit);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern bool CalculatePathInternal_Injected(ref Vector3 targetPosition, NavMeshPath path);
}
