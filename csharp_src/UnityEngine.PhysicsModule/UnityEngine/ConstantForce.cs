using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[NativeHeader("Modules/Physics/ConstantForce.h")]
public class ConstantForce : Behaviour
{
	public Vector3 force
	{
		get
		{
			get_force_Injected(out var ret);
			return ret;
		}
		set
		{
			set_force_Injected(ref value);
		}
	}

	public Vector3 relativeForce
	{
		get
		{
			get_relativeForce_Injected(out var ret);
			return ret;
		}
		set
		{
			set_relativeForce_Injected(ref value);
		}
	}

	public Vector3 torque
	{
		get
		{
			get_torque_Injected(out var ret);
			return ret;
		}
		set
		{
			set_torque_Injected(ref value);
		}
	}

	public Vector3 relativeTorque
	{
		get
		{
			get_relativeTorque_Injected(out var ret);
			return ret;
		}
		set
		{
			set_relativeTorque_Injected(ref value);
		}
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void get_force_Injected(out Vector3 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void set_force_Injected(ref Vector3 value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void get_relativeForce_Injected(out Vector3 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void set_relativeForce_Injected(ref Vector3 value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void get_torque_Injected(out Vector3 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void set_torque_Injected(ref Vector3 value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void get_relativeTorque_Injected(out Vector3 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void set_relativeTorque_Injected(ref Vector3 value);
}
