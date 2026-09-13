using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine;

[NativeHeader("Modules/Physics2D/SliderJoint2D.h")]
public sealed class SliderJoint2D : AnchoredJoint2D
{
	public extern bool autoConfigureAngle
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern float angle
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
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

	public JointMotor2D motor
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

	public JointTranslationLimits2D limits
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

	public extern JointLimitState2D limitState
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
	}

	public extern float referenceAngle
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
	}

	public extern float jointTranslation
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
	}

	public extern float jointSpeed
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	public extern float GetMotorForce(float timeStep);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void get_motor_Injected(out JointMotor2D ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void set_motor_Injected(ref JointMotor2D value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void get_limits_Injected(out JointTranslationLimits2D ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void set_limits_Injected(ref JointTranslationLimits2D value);
}
