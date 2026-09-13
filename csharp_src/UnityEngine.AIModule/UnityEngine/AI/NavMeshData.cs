using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine.AI;

[NativeHeader("Modules/AI/NavMesh/NavMesh.bindings.h")]
public sealed class NavMeshData : Object
{
	public Bounds sourceBounds
	{
		get
		{
			get_sourceBounds_Injected(out var ret);
			return ret;
		}
	}

	public Vector3 position
	{
		get
		{
			get_position_Injected(out var ret);
			return ret;
		}
		set
		{
			set_position_Injected(ref value);
		}
	}

	public Quaternion rotation
	{
		get
		{
			get_rotation_Injected(out var ret);
			return ret;
		}
		set
		{
			set_rotation_Injected(ref value);
		}
	}

	public NavMeshData()
	{
		Internal_Create(this, 0);
	}

	public NavMeshData(int agentTypeID)
	{
		Internal_Create(this, agentTypeID);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[StaticAccessor("NavMeshDataBindings", StaticAccessorType.DoubleColon)]
	private static extern void Internal_Create([Writable] NavMeshData mono, int agentTypeID);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void get_sourceBounds_Injected(out Bounds ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void get_position_Injected(out Vector3 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void set_position_Injected(ref Vector3 value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void get_rotation_Injected(out Quaternion ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void set_rotation_Injected(ref Quaternion value);
}
