using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine;

[NativeHeader("Modules/Physics/CharacterController.h")]
public class CharacterController : Collider
{
	public Vector3 velocity
	{
		get
		{
			get_velocity_Injected(out var ret);
			return ret;
		}
	}

	public extern bool isGrounded
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeName("IsGrounded")]
		get;
	}

	public extern CollisionFlags collisionFlags
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
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

	public Vector3 center
	{
		get
		{
			get_center_Injected(out var ret);
			return ret;
		}
		set
		{
			set_center_Injected(ref value);
		}
	}

	public extern float slopeLimit
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern float stepOffset
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern float skinWidth
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern float minMoveDistance
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern bool detectCollisions
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern bool enableOverlapRecovery
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public bool SimpleMove(Vector3 speed)
	{
		return SimpleMove_Injected(ref speed);
	}

	public CollisionFlags Move(Vector3 motion)
	{
		return Move_Injected(ref motion);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern bool SimpleMove_Injected(ref Vector3 speed);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern CollisionFlags Move_Injected(ref Vector3 motion);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void get_velocity_Injected(out Vector3 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void get_center_Injected(out Vector3 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void set_center_Injected(ref Vector3 value);
}
