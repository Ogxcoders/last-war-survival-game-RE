using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine;

[NativeHeader("Modules/Physics/HingeJoint.h")]
[NativeClass("Unity::HingeJoint")]
public class HingeJoint : Joint
{
	public JointMotor motor
	{
		get
		{
			get_motor_Injected(out var ret);
			return ret;
		}
		set
		{
			set_motor_Injected(ref value);
		}
	}

	public JointLimits limits
	{
		get
		{
			get_limits_Injected(out var ret);
			return ret;
		}
		set
		{
			set_limits_Injected(ref value);
		}
	}

	public JointSpring spring
	{
		get
		{
			get_spring_Injected(out var ret);
			return ret;
		}
		set
		{
			set_spring_Injected(ref value);
		}
	}

	public extern bool useMotor
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern bool useLimits
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern bool useSpring
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern float velocity
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
	}

	public extern float angle
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void get_motor_Injected(out JointMotor ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void set_motor_Injected(ref JointMotor value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void get_limits_Injected(out JointLimits ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void set_limits_Injected(ref JointLimits value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void get_spring_Injected(out JointSpring ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void set_spring_Injected(ref JointSpring value);
}
