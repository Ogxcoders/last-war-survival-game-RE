using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine;

[NativeHeader("Modules/Physics2D/WheelJoint2D.h")]
public sealed class WheelJoint2D : AnchoredJoint2D
{
	public JointSuspension2D suspension
	{
		get
		{
			get_suspension_Injected(out var ret);
			return ret;
		}
		set
		{
			set_suspension_Injected(ref value);
		}
	}

	public extern bool useMotor
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

	public extern float jointTranslation
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
	}

	public extern float jointLinearSpeed
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
	}

	public extern float jointSpeed
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeMethod("GetJointAngularSpeed")]
		get;
	}

	public extern float jointAngle
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	public extern float GetMotorTorque(float timeStep);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void get_suspension_Injected(out JointSuspension2D ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void set_suspension_Injected(ref JointSuspension2D value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void get_motor_Injected(out JointMotor2D ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void set_motor_Injected(ref JointMotor2D value);
}
