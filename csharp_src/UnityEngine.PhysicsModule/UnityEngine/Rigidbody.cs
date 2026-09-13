using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Internal;

namespace UnityEngine;

[NativeHeader("Modules/Physics/Rigidbody.h")]
[RequireComponent(typeof(Transform))]
public class Rigidbody : Component
{
	public Vector3 velocity
	{
		get
		{
			get_velocity_Injected(out var ret);
			return ret;
		}
		set
		{
			set_velocity_Injected(ref value);
		}
	}

	public Vector3 angularVelocity
	{
		get
		{
			get_angularVelocity_Injected(out var ret);
			return ret;
		}
		set
		{
			set_angularVelocity_Injected(ref value);
		}
	}

	public extern float drag
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern float angularDrag
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern float mass
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern bool useGravity
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern float maxDepenetrationVelocity
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern bool isKinematic
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern bool freezeRotation
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern RigidbodyConstraints constraints
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern CollisionDetectionMode collisionDetectionMode
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public Vector3 centerOfMass
	{
		get
		{
			get_centerOfMass_Injected(out var ret);
			return ret;
		}
		set
		{
			set_centerOfMass_Injected(ref value);
		}
	}

	public Vector3 worldCenterOfMass
	{
		get
		{
			get_worldCenterOfMass_Injected(out var ret);
			return ret;
		}
	}

	public Quaternion inertiaTensorRotation
	{
		get
		{
			get_inertiaTensorRotation_Injected(out var ret);
			return ret;
		}
		set
		{
			set_inertiaTensorRotation_Injected(ref value);
		}
	}

	public Vector3 inertiaTensor
	{
		get
		{
			get_inertiaTensor_Injected(out var ret);
			return ret;
		}
		set
		{
			set_inertiaTensor_Injected(ref value);
		}
	}

	public extern bool detectCollisions
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
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

	public extern RigidbodyInterpolation interpolation
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern int solverIterations
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern float sleepThreshold
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern float maxAngularVelocity
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern int solverVelocityIterations
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	[Obsolete("The sleepVelocity is no longer supported. Use sleepThreshold. Note that sleepThreshold is energy but not velocity.")]
	public extern float sleepVelocity
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	[Obsolete("The sleepAngularVelocity is no longer supported. Set Use sleepThreshold to specify energy.")]
	public extern float sleepAngularVelocity
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	[Obsolete("Cone friction is no longer supported.")]
	public bool useConeFriction
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	[Obsolete("Please use Rigidbody.solverIterations instead. (UnityUpgradable) -> solverIterations")]
	public int solverIterationCount
	{
		get
		{
			return solverIterations;
		}
		set
		{
			solverIterations = value;
		}
	}

	[Obsolete("Please use Rigidbody.solverVelocityIterations instead. (UnityUpgradable) -> solverVelocityIterations")]
	public int solverVelocityIterationCount
	{
		get
		{
			return solverVelocityIterations;
		}
		set
		{
			solverVelocityIterations = value;
		}
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	public extern void SetDensity(float density);

	public void MovePosition(Vector3 position)
	{
		MovePosition_Injected(ref position);
	}

	public void MoveRotation(Quaternion rot)
	{
		MoveRotation_Injected(ref rot);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	public extern void Sleep();

	[MethodImpl(MethodImplOptions.InternalCall)]
	public extern bool IsSleeping();

	[MethodImpl(MethodImplOptions.InternalCall)]
	public extern void WakeUp();

	[MethodImpl(MethodImplOptions.InternalCall)]
	public extern void ResetCenterOfMass();

	[MethodImpl(MethodImplOptions.InternalCall)]
	public extern void ResetInertiaTensor();

	public Vector3 GetRelativePointVelocity(Vector3 relativePoint)
	{
		GetRelativePointVelocity_Injected(ref relativePoint, out var ret);
		return ret;
	}

	public Vector3 GetPointVelocity(Vector3 worldPoint)
	{
		GetPointVelocity_Injected(ref worldPoint, out var ret);
		return ret;
	}

	[Obsolete("Use Rigidbody.maxAngularVelocity instead.")]
	public void SetMaxAngularVelocity(float a)
	{
		maxAngularVelocity = a;
	}

	public void AddForce(Vector3 force, [DefaultValue("ForceMode.Force")] ForceMode mode)
	{
		AddForce_Injected(ref force, mode);
	}

	[ExcludeFromDocs]
	public void AddForce(Vector3 force)
	{
		AddForce(force, ForceMode.Force);
	}

	public void AddForce(float x, float y, float z, [DefaultValue("ForceMode.Force")] ForceMode mode)
	{
		AddForce(new Vector3(x, y, z), mode);
	}

	[ExcludeFromDocs]
	public void AddForce(float x, float y, float z)
	{
		AddForce(new Vector3(x, y, z), ForceMode.Force);
	}

	public void AddRelativeForce(Vector3 force, [DefaultValue("ForceMode.Force")] ForceMode mode)
	{
		AddRelativeForce_Injected(ref force, mode);
	}

	[ExcludeFromDocs]
	public void AddRelativeForce(Vector3 force)
	{
		AddRelativeForce(force, ForceMode.Force);
	}

	public void AddRelativeForce(float x, float y, float z, [DefaultValue("ForceMode.Force")] ForceMode mode)
	{
		AddRelativeForce(new Vector3(x, y, z), mode);
	}

	[ExcludeFromDocs]
	public void AddRelativeForce(float x, float y, float z)
	{
		AddRelativeForce(new Vector3(x, y, z), ForceMode.Force);
	}

	public void AddTorque(Vector3 torque, [DefaultValue("ForceMode.Force")] ForceMode mode)
	{
		AddTorque_Injected(ref torque, mode);
	}

	[ExcludeFromDocs]
	public void AddTorque(Vector3 torque)
	{
		AddTorque(torque, ForceMode.Force);
	}

	public void AddTorque(float x, float y, float z, [DefaultValue("ForceMode.Force")] ForceMode mode)
	{
		AddTorque(new Vector3(x, y, z), mode);
	}

	[ExcludeFromDocs]
	public void AddTorque(float x, float y, float z)
	{
		AddTorque(new Vector3(x, y, z), ForceMode.Force);
	}

	public void AddRelativeTorque(Vector3 torque, [DefaultValue("ForceMode.Force")] ForceMode mode)
	{
		AddRelativeTorque_Injected(ref torque, mode);
	}

	[ExcludeFromDocs]
	public void AddRelativeTorque(Vector3 torque)
	{
		AddRelativeTorque(torque, ForceMode.Force);
	}

	public void AddRelativeTorque(float x, float y, float z, [DefaultValue("ForceMode.Force")] ForceMode mode)
	{
		AddRelativeTorque(new Vector3(x, y, z), mode);
	}

	[ExcludeFromDocs]
	public void AddRelativeTorque(float x, float y, float z)
	{
		AddRelativeTorque(x, y, z, ForceMode.Force);
	}

	public void AddForceAtPosition(Vector3 force, Vector3 position, [DefaultValue("ForceMode.Force")] ForceMode mode)
	{
		AddForceAtPosition_Injected(ref force, ref position, mode);
	}

	[ExcludeFromDocs]
	public void AddForceAtPosition(Vector3 force, Vector3 position)
	{
		AddForceAtPosition(force, position, ForceMode.Force);
	}

	public void AddExplosionForce(float explosionForce, Vector3 explosionPosition, float explosionRadius, [DefaultValue("0.0f")] float upwardsModifier, [DefaultValue("ForceMode.Force)")] ForceMode mode)
	{
		AddExplosionForce_Injected(explosionForce, ref explosionPosition, explosionRadius, upwardsModifier, mode);
	}

	[ExcludeFromDocs]
	public void AddExplosionForce(float explosionForce, Vector3 explosionPosition, float explosionRadius, float upwardsModifier)
	{
		AddExplosionForce(explosionForce, explosionPosition, explosionRadius, upwardsModifier, ForceMode.Force);
	}

	[ExcludeFromDocs]
	public void AddExplosionForce(float explosionForce, Vector3 explosionPosition, float explosionRadius)
	{
		AddExplosionForce(explosionForce, explosionPosition, explosionRadius, 0f, ForceMode.Force);
	}

	[NativeName("ClosestPointOnBounds")]
	private void Internal_ClosestPointOnBounds(Vector3 point, ref Vector3 outPos, ref float distance)
	{
		Internal_ClosestPointOnBounds_Injected(ref point, ref outPos, ref distance);
	}

	public Vector3 ClosestPointOnBounds(Vector3 position)
	{
		float distance = 0f;
		Vector3 outPos = Vector3.zero;
		Internal_ClosestPointOnBounds(position, ref outPos, ref distance);
		return outPos;
	}

	private RaycastHit SweepTest(Vector3 direction, float maxDistance, QueryTriggerInteraction queryTriggerInteraction, ref bool hasHit)
	{
		SweepTest_Injected(ref direction, maxDistance, queryTriggerInteraction, ref hasHit, out var ret);
		return ret;
	}

	public bool SweepTest(Vector3 direction, out RaycastHit hitInfo, [DefaultValue("Mathf.Infinity")] float maxDistance, [DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
	{
		float magnitude = direction.magnitude;
		if (magnitude > float.Epsilon)
		{
			Vector3 direction2 = direction / magnitude;
			bool hasHit = false;
			hitInfo = SweepTest(direction2, maxDistance, queryTriggerInteraction, ref hasHit);
			return hasHit;
		}
		hitInfo = default(RaycastHit);
		return false;
	}

	[ExcludeFromDocs]
	public bool SweepTest(Vector3 direction, out RaycastHit hitInfo, float maxDistance)
	{
		return SweepTest(direction, out hitInfo, maxDistance, QueryTriggerInteraction.UseGlobal);
	}

	[ExcludeFromDocs]
	public bool SweepTest(Vector3 direction, out RaycastHit hitInfo)
	{
		return SweepTest(direction, out hitInfo, float.PositiveInfinity, QueryTriggerInteraction.UseGlobal);
	}

	[NativeName("SweepTestAll")]
	private RaycastHit[] Internal_SweepTestAll(Vector3 direction, float maxDistance, QueryTriggerInteraction queryTriggerInteraction)
	{
		return Internal_SweepTestAll_Injected(ref direction, maxDistance, queryTriggerInteraction);
	}

	public RaycastHit[] SweepTestAll(Vector3 direction, [DefaultValue("Mathf.Infinity")] float maxDistance, [DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
	{
		float magnitude = direction.magnitude;
		if (magnitude > float.Epsilon)
		{
			Vector3 direction2 = direction / magnitude;
			return Internal_SweepTestAll(direction2, maxDistance, queryTriggerInteraction);
		}
		return new RaycastHit[0];
	}

	[ExcludeFromDocs]
	public RaycastHit[] SweepTestAll(Vector3 direction, float maxDistance)
	{
		return SweepTestAll(direction, maxDistance, QueryTriggerInteraction.UseGlobal);
	}

	[ExcludeFromDocs]
	public RaycastHit[] SweepTestAll(Vector3 direction)
	{
		return SweepTestAll(direction, float.PositiveInfinity, QueryTriggerInteraction.UseGlobal);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void get_velocity_Injected(out Vector3 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void set_velocity_Injected(ref Vector3 value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void get_angularVelocity_Injected(out Vector3 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void set_angularVelocity_Injected(ref Vector3 value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void get_centerOfMass_Injected(out Vector3 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void set_centerOfMass_Injected(ref Vector3 value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void get_worldCenterOfMass_Injected(out Vector3 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void get_inertiaTensorRotation_Injected(out Quaternion ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void set_inertiaTensorRotation_Injected(ref Quaternion value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void get_inertiaTensor_Injected(out Vector3 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void set_inertiaTensor_Injected(ref Vector3 value);

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

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern void MovePosition_Injected(ref Vector3 position);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern void MoveRotation_Injected(ref Quaternion rot);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern void GetRelativePointVelocity_Injected(ref Vector3 relativePoint, out Vector3 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern void GetPointVelocity_Injected(ref Vector3 worldPoint, out Vector3 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern void AddForce_Injected(ref Vector3 force, [DefaultValue("ForceMode.Force")] ForceMode mode);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern void AddRelativeForce_Injected(ref Vector3 force, [DefaultValue("ForceMode.Force")] ForceMode mode);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern void AddTorque_Injected(ref Vector3 torque, [DefaultValue("ForceMode.Force")] ForceMode mode);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern void AddRelativeTorque_Injected(ref Vector3 torque, [DefaultValue("ForceMode.Force")] ForceMode mode);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern void AddForceAtPosition_Injected(ref Vector3 force, ref Vector3 position, [DefaultValue("ForceMode.Force")] ForceMode mode);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern void AddExplosionForce_Injected(float explosionForce, ref Vector3 explosionPosition, float explosionRadius, [DefaultValue("0.0f")] float upwardsModifier, [DefaultValue("ForceMode.Force)")] ForceMode mode);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern void Internal_ClosestPointOnBounds_Injected(ref Vector3 point, ref Vector3 outPos, ref float distance);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern void SweepTest_Injected(ref Vector3 direction, float maxDistance, QueryTriggerInteraction queryTriggerInteraction, ref bool hasHit, out RaycastHit ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern RaycastHit[] Internal_SweepTestAll_Injected(ref Vector3 direction, float maxDistance, QueryTriggerInteraction queryTriggerInteraction);
}
