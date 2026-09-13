using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine;

[NativeHeader("Modules/Physics2D/Joint2D.h")]
[RequireComponent(typeof(Transform), typeof(Rigidbody2D))]
public class Joint2D : Behaviour
{
	public extern Rigidbody2D attachedRigidbody
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
	}

	public extern Rigidbody2D connectedBody
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

	public Vector2 reactionForce
	{
		[NativeMethod("GetReactionForceFixedTime")]
		get
		{
			get_reactionForce_Injected(out var ret);
			return ret;
		}
	}

	public extern float reactionTorque
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeMethod("GetReactionTorqueFixedTime")]
		get;
	}

	public Vector2 GetReactionForce(float timeStep)
	{
		GetReactionForce_Injected(timeStep, out var ret);
		return ret;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	public extern float GetReactionTorque(float timeStep);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void get_reactionForce_Injected(out Vector2 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern void GetReactionForce_Injected(float timeStep, out Vector2 ret);
}
