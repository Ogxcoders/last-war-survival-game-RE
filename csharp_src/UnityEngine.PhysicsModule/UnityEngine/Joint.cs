using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[NativeHeader("Modules/Physics/Joint.h")]
[NativeClass("Unity::Joint")]
public class Joint : Component
{
	public extern Rigidbody connectedBody
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public Vector3 axis
	{
		get
		{
			get_axis_Injected(out var ret);
			return ret;
		}
		set
		{
			set_axis_Injected(ref value);
		}
	}

	public Vector3 anchor
	{
		get
		{
			get_anchor_Injected(out var ret);
			return ret;
		}
		set
		{
			set_anchor_Injected(ref value);
		}
	}

	public Vector3 connectedAnchor
	{
		get
		{
			get_connectedAnchor_Injected(out var ret);
			return ret;
		}
		set
		{
			set_connectedAnchor_Injected(ref value);
		}
	}

	public extern bool autoConfigureConnectedAnchor
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern float breakForce
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern float breakTorque
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern bool enableCollision
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern bool enablePreprocessing
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern float massScale
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern float connectedMassScale
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public Vector3 currentForce
	{
		get
		{
			Vector3 linearForce = Vector3.zero;
			Vector3 angularForce = Vector3.zero;
			GetCurrentForces(ref linearForce, ref angularForce);
			return linearForce;
		}
	}

	public Vector3 currentTorque
	{
		get
		{
			Vector3 linearForce = Vector3.zero;
			Vector3 angularForce = Vector3.zero;
			GetCurrentForces(ref linearForce, ref angularForce);
			return angularForce;
		}
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern void GetCurrentForces(ref Vector3 linearForce, ref Vector3 angularForce);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void get_axis_Injected(out Vector3 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void set_axis_Injected(ref Vector3 value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void get_anchor_Injected(out Vector3 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void set_anchor_Injected(ref Vector3 value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void get_connectedAnchor_Injected(out Vector3 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void set_connectedAnchor_Injected(ref Vector3 value);
}
