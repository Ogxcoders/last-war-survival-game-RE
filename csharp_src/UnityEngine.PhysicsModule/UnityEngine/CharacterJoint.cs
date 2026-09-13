using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine;

[NativeHeader("Modules/Physics/CharacterJoint.h")]
[NativeClass("Unity::CharacterJoint")]
public class CharacterJoint : Joint
{
	[Obsolete("TargetRotation not in use for Unity 5 and assumed disabled.", true)]
	public Quaternion targetRotation;

	[Obsolete("TargetAngularVelocity not in use for Unity 5 and assumed disabled.", true)]
	public Vector3 targetAngularVelocity;

	[Obsolete("RotationDrive not in use for Unity 5 and assumed disabled.")]
	public JointDrive rotationDrive;

	public Vector3 swingAxis
	{
		get
		{
			get_swingAxis_Injected(out var ret);
			return ret;
		}
		set
		{
			set_swingAxis_Injected(ref value);
		}
	}

	public SoftJointLimitSpring twistLimitSpring
	{
		get
		{
			get_twistLimitSpring_Injected(out var ret);
			return ret;
		}
		set
		{
			set_twistLimitSpring_Injected(ref value);
		}
	}

	public SoftJointLimitSpring swingLimitSpring
	{
		get
		{
			get_swingLimitSpring_Injected(out var ret);
			return ret;
		}
		set
		{
			set_swingLimitSpring_Injected(ref value);
		}
	}

	public SoftJointLimit lowTwistLimit
	{
		get
		{
			get_lowTwistLimit_Injected(out var ret);
			return ret;
		}
		set
		{
			set_lowTwistLimit_Injected(ref value);
		}
	}

	public SoftJointLimit highTwistLimit
	{
		get
		{
			get_highTwistLimit_Injected(out var ret);
			return ret;
		}
		set
		{
			set_highTwistLimit_Injected(ref value);
		}
	}

	public SoftJointLimit swing1Limit
	{
		get
		{
			get_swing1Limit_Injected(out var ret);
			return ret;
		}
		set
		{
			set_swing1Limit_Injected(ref value);
		}
	}

	public SoftJointLimit swing2Limit
	{
		get
		{
			get_swing2Limit_Injected(out var ret);
			return ret;
		}
		set
		{
			set_swing2Limit_Injected(ref value);
		}
	}

	public extern bool enableProjection
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern float projectionDistance
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern float projectionAngle
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void get_swingAxis_Injected(out Vector3 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void set_swingAxis_Injected(ref Vector3 value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void get_twistLimitSpring_Injected(out SoftJointLimitSpring ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void set_twistLimitSpring_Injected(ref SoftJointLimitSpring value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void get_swingLimitSpring_Injected(out SoftJointLimitSpring ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void set_swingLimitSpring_Injected(ref SoftJointLimitSpring value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void get_lowTwistLimit_Injected(out SoftJointLimit ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void set_lowTwistLimit_Injected(ref SoftJointLimit value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void get_highTwistLimit_Injected(out SoftJointLimit ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void set_highTwistLimit_Injected(ref SoftJointLimit value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void get_swing1Limit_Injected(out SoftJointLimit ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void set_swing1Limit_Injected(ref SoftJointLimit value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void get_swing2Limit_Injected(out SoftJointLimit ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void set_swing2Limit_Injected(ref SoftJointLimit value);
}
