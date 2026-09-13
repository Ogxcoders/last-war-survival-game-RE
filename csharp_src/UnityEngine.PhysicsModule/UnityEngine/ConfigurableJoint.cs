using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine;

[NativeHeader("Modules/Physics/ConfigurableJoint.h")]
[NativeClass("Unity::ConfigurableJoint")]
public class ConfigurableJoint : Joint
{
	public Vector3 secondaryAxis
	{
		get
		{
			get_secondaryAxis_Injected(out var ret);
			return ret;
		}
		set
		{
			set_secondaryAxis_Injected(ref value);
		}
	}

	public extern ConfigurableJointMotion xMotion
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern ConfigurableJointMotion yMotion
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern ConfigurableJointMotion zMotion
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern ConfigurableJointMotion angularXMotion
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern ConfigurableJointMotion angularYMotion
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern ConfigurableJointMotion angularZMotion
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public SoftJointLimitSpring linearLimitSpring
	{
		get
		{
			get_linearLimitSpring_Injected(out var ret);
			return ret;
		}
		set
		{
			set_linearLimitSpring_Injected(ref value);
		}
	}

	public SoftJointLimitSpring angularXLimitSpring
	{
		get
		{
			get_angularXLimitSpring_Injected(out var ret);
			return ret;
		}
		set
		{
			set_angularXLimitSpring_Injected(ref value);
		}
	}

	public SoftJointLimitSpring angularYZLimitSpring
	{
		get
		{
			get_angularYZLimitSpring_Injected(out var ret);
			return ret;
		}
		set
		{
			set_angularYZLimitSpring_Injected(ref value);
		}
	}

	public SoftJointLimit linearLimit
	{
		get
		{
			get_linearLimit_Injected(out var ret);
			return ret;
		}
		set
		{
			set_linearLimit_Injected(ref value);
		}
	}

	public SoftJointLimit lowAngularXLimit
	{
		get
		{
			get_lowAngularXLimit_Injected(out var ret);
			return ret;
		}
		set
		{
			set_lowAngularXLimit_Injected(ref value);
		}
	}

	public SoftJointLimit highAngularXLimit
	{
		get
		{
			get_highAngularXLimit_Injected(out var ret);
			return ret;
		}
		set
		{
			set_highAngularXLimit_Injected(ref value);
		}
	}

	public SoftJointLimit angularYLimit
	{
		get
		{
			get_angularYLimit_Injected(out var ret);
			return ret;
		}
		set
		{
			set_angularYLimit_Injected(ref value);
		}
	}

	public SoftJointLimit angularZLimit
	{
		get
		{
			get_angularZLimit_Injected(out var ret);
			return ret;
		}
		set
		{
			set_angularZLimit_Injected(ref value);
		}
	}

	public Vector3 targetPosition
	{
		get
		{
			get_targetPosition_Injected(out var ret);
			return ret;
		}
		set
		{
			set_targetPosition_Injected(ref value);
		}
	}

	public Vector3 targetVelocity
	{
		get
		{
			get_targetVelocity_Injected(out var ret);
			return ret;
		}
		set
		{
			set_targetVelocity_Injected(ref value);
		}
	}

	public JointDrive xDrive
	{
		get
		{
			get_xDrive_Injected(out var ret);
			return ret;
		}
		set
		{
			set_xDrive_Injected(ref value);
		}
	}

	public JointDrive yDrive
	{
		get
		{
			get_yDrive_Injected(out var ret);
			return ret;
		}
		set
		{
			set_yDrive_Injected(ref value);
		}
	}

	public JointDrive zDrive
	{
		get
		{
			get_zDrive_Injected(out var ret);
			return ret;
		}
		set
		{
			set_zDrive_Injected(ref value);
		}
	}

	public Quaternion targetRotation
	{
		get
		{
			get_targetRotation_Injected(out var ret);
			return ret;
		}
		set
		{
			set_targetRotation_Injected(ref value);
		}
	}

	public Vector3 targetAngularVelocity
	{
		get
		{
			get_targetAngularVelocity_Injected(out var ret);
			return ret;
		}
		set
		{
			set_targetAngularVelocity_Injected(ref value);
		}
	}

	public extern RotationDriveMode rotationDriveMode
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public JointDrive angularXDrive
	{
		get
		{
			get_angularXDrive_Injected(out var ret);
			return ret;
		}
		set
		{
			set_angularXDrive_Injected(ref value);
		}
	}

	public JointDrive angularYZDrive
	{
		get
		{
			get_angularYZDrive_Injected(out var ret);
			return ret;
		}
		set
		{
			set_angularYZDrive_Injected(ref value);
		}
	}

	public JointDrive slerpDrive
	{
		get
		{
			get_slerpDrive_Injected(out var ret);
			return ret;
		}
		set
		{
			set_slerpDrive_Injected(ref value);
		}
	}

	public extern JointProjectionMode projectionMode
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

	public extern bool configuredInWorldSpace
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern bool swapBodies
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void get_secondaryAxis_Injected(out Vector3 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void set_secondaryAxis_Injected(ref Vector3 value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void get_linearLimitSpring_Injected(out SoftJointLimitSpring ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void set_linearLimitSpring_Injected(ref SoftJointLimitSpring value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void get_angularXLimitSpring_Injected(out SoftJointLimitSpring ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void set_angularXLimitSpring_Injected(ref SoftJointLimitSpring value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void get_angularYZLimitSpring_Injected(out SoftJointLimitSpring ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void set_angularYZLimitSpring_Injected(ref SoftJointLimitSpring value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void get_linearLimit_Injected(out SoftJointLimit ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void set_linearLimit_Injected(ref SoftJointLimit value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void get_lowAngularXLimit_Injected(out SoftJointLimit ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void set_lowAngularXLimit_Injected(ref SoftJointLimit value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void get_highAngularXLimit_Injected(out SoftJointLimit ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void set_highAngularXLimit_Injected(ref SoftJointLimit value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void get_angularYLimit_Injected(out SoftJointLimit ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void set_angularYLimit_Injected(ref SoftJointLimit value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void get_angularZLimit_Injected(out SoftJointLimit ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void set_angularZLimit_Injected(ref SoftJointLimit value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void get_targetPosition_Injected(out Vector3 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void set_targetPosition_Injected(ref Vector3 value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void get_targetVelocity_Injected(out Vector3 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void set_targetVelocity_Injected(ref Vector3 value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void get_xDrive_Injected(out JointDrive ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void set_xDrive_Injected(ref JointDrive value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void get_yDrive_Injected(out JointDrive ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void set_yDrive_Injected(ref JointDrive value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void get_zDrive_Injected(out JointDrive ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void set_zDrive_Injected(ref JointDrive value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void get_targetRotation_Injected(out Quaternion ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void set_targetRotation_Injected(ref Quaternion value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void get_targetAngularVelocity_Injected(out Vector3 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void set_targetAngularVelocity_Injected(ref Vector3 value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void get_angularXDrive_Injected(out JointDrive ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void set_angularXDrive_Injected(ref JointDrive value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void get_angularYZDrive_Injected(out JointDrive ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void set_angularYZDrive_Injected(ref JointDrive value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void get_slerpDrive_Injected(out JointDrive ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void set_slerpDrive_Injected(ref JointDrive value);
}
