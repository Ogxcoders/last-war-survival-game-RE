using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Bindings;
using UnityEngine.Internal;

namespace UnityEngine;

[RequireComponent(typeof(Transform))]
[NativeHeader("Modules/Physics2D/Public/Rigidbody2D.h")]
public sealed class Rigidbody2D : Component
{
	public Vector2 position
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

	public extern float rotation
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public Vector2 velocity
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

	public extern float angularVelocity
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern bool useAutoMass
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

	[NativeMethod("Material")]
	public extern PhysicsMaterial2D sharedMaterial
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public Vector2 centerOfMass
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

	public Vector2 worldCenterOfMass
	{
		get
		{
			get_worldCenterOfMass_Injected(out var ret);
			return ret;
		}
	}

	public extern float inertia
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
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

	public extern float gravityScale
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern RigidbodyType2D bodyType
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeMethod("SetBodyType_Binding")]
		set;
	}

	public extern bool useFullKinematicContacts
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public bool isKinematic
	{
		get
		{
			return bodyType == RigidbodyType2D.Kinematic;
		}
		set
		{
			bodyType = (value ? RigidbodyType2D.Kinematic : RigidbodyType2D.Dynamic);
		}
	}

	[NativeMethod("FreezeRotation")]
	[Obsolete("'fixedAngle' is no longer supported. Use constraints instead.", false)]
	public extern bool fixedAngle
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

	public extern RigidbodyConstraints2D constraints
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern bool simulated
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeMethod("SetSimulated_Binding")]
		set;
	}

	public extern RigidbodyInterpolation2D interpolation
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern RigidbodySleepMode2D sleepMode
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern CollisionDetectionMode2D collisionDetectionMode
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern int attachedColliderCount
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
	}

	public void SetRotation(float angle)
	{
		SetRotation_Angle(angle);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod("SetRotation")]
	private extern void SetRotation_Angle(float angle);

	public void SetRotation(Quaternion rotation)
	{
		SetRotation_Quaternion(rotation);
	}

	[NativeMethod("SetRotation")]
	private void SetRotation_Quaternion(Quaternion rotation)
	{
		SetRotation_Quaternion_Injected(ref rotation);
	}

	public void MovePosition(Vector2 position)
	{
		MovePosition_Injected(ref position);
	}

	public void MoveRotation(float angle)
	{
		MoveRotation_Angle(angle);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod("MoveRotation")]
	private extern void MoveRotation_Angle(float angle);

	public void MoveRotation(Quaternion rotation)
	{
		MoveRotation_Quaternion(rotation);
	}

	[NativeMethod("MoveRotation")]
	private void MoveRotation_Quaternion(Quaternion rotation)
	{
		MoveRotation_Quaternion_Injected(ref rotation);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal extern void SetDragBehaviour(bool dragged);

	[MethodImpl(MethodImplOptions.InternalCall)]
	public extern bool IsSleeping();

	[MethodImpl(MethodImplOptions.InternalCall)]
	public extern bool IsAwake();

	[MethodImpl(MethodImplOptions.InternalCall)]
	public extern void Sleep();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod("Wake")]
	public extern void WakeUp();

	[MethodImpl(MethodImplOptions.InternalCall)]
	public extern bool IsTouching([Writable][NotNull] Collider2D collider);

	public bool IsTouching([Writable] Collider2D collider, ContactFilter2D contactFilter)
	{
		return IsTouching_OtherColliderWithFilter_Internal(collider, contactFilter);
	}

	[NativeMethod("IsTouching")]
	private bool IsTouching_OtherColliderWithFilter_Internal([Writable][NotNull] Collider2D collider, ContactFilter2D contactFilter)
	{
		return IsTouching_OtherColliderWithFilter_Internal_Injected(collider, ref contactFilter);
	}

	public bool IsTouching(ContactFilter2D contactFilter)
	{
		return IsTouching_AnyColliderWithFilter_Internal(contactFilter);
	}

	[NativeMethod("IsTouching")]
	private bool IsTouching_AnyColliderWithFilter_Internal(ContactFilter2D contactFilter)
	{
		return IsTouching_AnyColliderWithFilter_Internal_Injected(ref contactFilter);
	}

	[ExcludeFromDocs]
	public bool IsTouchingLayers()
	{
		return IsTouchingLayers(-1);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	public extern bool IsTouchingLayers([DefaultValue("Physics2D.AllLayers")] int layerMask);

	public bool OverlapPoint(Vector2 point)
	{
		return OverlapPoint_Injected(ref point);
	}

	public ColliderDistance2D Distance([Writable] Collider2D collider)
	{
		if (collider == null)
		{
			throw new ArgumentNullException("Collider cannot be null.");
		}
		if (collider.attachedRigidbody == this)
		{
			throw new ArgumentException("The collider cannot be attached to the Rigidbody2D being searched.");
		}
		return Distance_Internal(collider);
	}

	[NativeMethod("Distance")]
	private ColliderDistance2D Distance_Internal([NotNull][Writable] Collider2D collider)
	{
		Distance_Internal_Injected(collider, out var ret);
		return ret;
	}

	public Vector2 ClosestPoint(Vector2 position)
	{
		return Physics2D.ClosestPoint(position, this);
	}

	[ExcludeFromDocs]
	public void AddForce(Vector2 force)
	{
		AddForce(force, ForceMode2D.Force);
	}

	public void AddForce(Vector2 force, [DefaultValue("ForceMode2D.Force")] ForceMode2D mode)
	{
		AddForce_Injected(ref force, mode);
	}

	[ExcludeFromDocs]
	public void AddRelativeForce(Vector2 relativeForce)
	{
		AddRelativeForce(relativeForce, ForceMode2D.Force);
	}

	public void AddRelativeForce(Vector2 relativeForce, [DefaultValue("ForceMode2D.Force")] ForceMode2D mode)
	{
		AddRelativeForce_Injected(ref relativeForce, mode);
	}

	[ExcludeFromDocs]
	public void AddForceAtPosition(Vector2 force, Vector2 position)
	{
		AddForceAtPosition(force, position, ForceMode2D.Force);
	}

	public void AddForceAtPosition(Vector2 force, Vector2 position, [DefaultValue("ForceMode2D.Force")] ForceMode2D mode)
	{
		AddForceAtPosition_Injected(ref force, ref position, mode);
	}

	[ExcludeFromDocs]
	public void AddTorque(float torque)
	{
		AddTorque(torque, ForceMode2D.Force);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	public extern void AddTorque(float torque, [DefaultValue("ForceMode2D.Force")] ForceMode2D mode);

	public Vector2 GetPoint(Vector2 point)
	{
		GetPoint_Injected(ref point, out var ret);
		return ret;
	}

	public Vector2 GetRelativePoint(Vector2 relativePoint)
	{
		GetRelativePoint_Injected(ref relativePoint, out var ret);
		return ret;
	}

	public Vector2 GetVector(Vector2 vector)
	{
		GetVector_Injected(ref vector, out var ret);
		return ret;
	}

	public Vector2 GetRelativeVector(Vector2 relativeVector)
	{
		GetRelativeVector_Injected(ref relativeVector, out var ret);
		return ret;
	}

	public Vector2 GetPointVelocity(Vector2 point)
	{
		GetPointVelocity_Injected(ref point, out var ret);
		return ret;
	}

	public Vector2 GetRelativePointVelocity(Vector2 relativePoint)
	{
		GetRelativePointVelocity_Injected(ref relativePoint, out var ret);
		return ret;
	}

	public int OverlapCollider(ContactFilter2D contactFilter, [Out] Collider2D[] results)
	{
		return OverlapColliderArray_Internal(contactFilter, results);
	}

	[NativeMethod("OverlapColliderArray_Binding")]
	private int OverlapColliderArray_Internal(ContactFilter2D contactFilter, [NotNull] Collider2D[] results)
	{
		return OverlapColliderArray_Internal_Injected(ref contactFilter, results);
	}

	public int OverlapCollider(ContactFilter2D contactFilter, List<Collider2D> results)
	{
		return OverlapColliderList_Internal(contactFilter, results);
	}

	[NativeMethod("OverlapColliderList_Binding")]
	private int OverlapColliderList_Internal(ContactFilter2D contactFilter, [NotNull] List<Collider2D> results)
	{
		return OverlapColliderList_Internal_Injected(ref contactFilter, results);
	}

	public int GetContacts(ContactPoint2D[] contacts)
	{
		return Physics2D.GetContacts(this, default(ContactFilter2D).NoFilter(), contacts);
	}

	public int GetContacts(List<ContactPoint2D> contacts)
	{
		return Physics2D.GetContacts(this, default(ContactFilter2D).NoFilter(), contacts);
	}

	public int GetContacts(ContactFilter2D contactFilter, ContactPoint2D[] contacts)
	{
		return Physics2D.GetContacts(this, contactFilter, contacts);
	}

	public int GetContacts(ContactFilter2D contactFilter, List<ContactPoint2D> contacts)
	{
		return Physics2D.GetContacts(this, contactFilter, contacts);
	}

	public int GetContacts(Collider2D[] colliders)
	{
		return Physics2D.GetContacts(this, default(ContactFilter2D).NoFilter(), colliders);
	}

	public int GetContacts(List<Collider2D> colliders)
	{
		return Physics2D.GetContacts(this, default(ContactFilter2D).NoFilter(), colliders);
	}

	public int GetContacts(ContactFilter2D contactFilter, Collider2D[] colliders)
	{
		return Physics2D.GetContacts(this, contactFilter, colliders);
	}

	public int GetContacts(ContactFilter2D contactFilter, List<Collider2D> colliders)
	{
		return Physics2D.GetContacts(this, contactFilter, colliders);
	}

	public int GetAttachedColliders([Out] Collider2D[] results)
	{
		return GetAttachedCollidersArray_Internal(results);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod("GetAttachedCollidersArray_Binding")]
	private extern int GetAttachedCollidersArray_Internal([NotNull] Collider2D[] results);

	public int GetAttachedColliders(List<Collider2D> results)
	{
		return GetAttachedCollidersList_Internal(results);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod("GetAttachedCollidersList_Binding")]
	private extern int GetAttachedCollidersList_Internal([NotNull] List<Collider2D> results);

	[ExcludeFromDocs]
	public int Cast(Vector2 direction, RaycastHit2D[] results)
	{
		return CastArray_Internal(direction, float.PositiveInfinity, results);
	}

	public int Cast(Vector2 direction, RaycastHit2D[] results, [DefaultValue("Mathf.Infinity")] float distance)
	{
		return CastArray_Internal(direction, distance, results);
	}

	[NativeMethod("CastArray_Binding")]
	private int CastArray_Internal(Vector2 direction, float distance, [NotNull] RaycastHit2D[] results)
	{
		return CastArray_Internal_Injected(ref direction, distance, results);
	}

	public int Cast(Vector2 direction, List<RaycastHit2D> results, [DefaultValue("Mathf.Infinity")] float distance = float.PositiveInfinity)
	{
		return CastList_Internal(direction, distance, results);
	}

	[NativeMethod("CastList_Binding")]
	private int CastList_Internal(Vector2 direction, float distance, [NotNull] List<RaycastHit2D> results)
	{
		return CastList_Internal_Injected(ref direction, distance, results);
	}

	[ExcludeFromDocs]
	public int Cast(Vector2 direction, ContactFilter2D contactFilter, RaycastHit2D[] results)
	{
		return CastFilteredArray_Internal(direction, float.PositiveInfinity, contactFilter, results);
	}

	public int Cast(Vector2 direction, ContactFilter2D contactFilter, RaycastHit2D[] results, [DefaultValue("Mathf.Infinity")] float distance)
	{
		return CastFilteredArray_Internal(direction, distance, contactFilter, results);
	}

	[NativeMethod("CastFilteredArray_Binding")]
	private int CastFilteredArray_Internal(Vector2 direction, float distance, ContactFilter2D contactFilter, [NotNull] RaycastHit2D[] results)
	{
		return CastFilteredArray_Internal_Injected(ref direction, distance, ref contactFilter, results);
	}

	public int Cast(Vector2 direction, ContactFilter2D contactFilter, List<RaycastHit2D> results, [DefaultValue("Mathf.Infinity")] float distance)
	{
		return CastFilteredList_Internal(direction, distance, contactFilter, results);
	}

	[NativeMethod("CastFilteredList_Binding")]
	private int CastFilteredList_Internal(Vector2 direction, float distance, ContactFilter2D contactFilter, [NotNull] List<RaycastHit2D> results)
	{
		return CastFilteredList_Internal_Injected(ref direction, distance, ref contactFilter, results);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void get_position_Injected(out Vector2 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void set_position_Injected(ref Vector2 value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern void SetRotation_Quaternion_Injected(ref Quaternion rotation);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern void MovePosition_Injected(ref Vector2 position);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern void MoveRotation_Quaternion_Injected(ref Quaternion rotation);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void get_velocity_Injected(out Vector2 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void set_velocity_Injected(ref Vector2 value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void get_centerOfMass_Injected(out Vector2 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void set_centerOfMass_Injected(ref Vector2 value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void get_worldCenterOfMass_Injected(out Vector2 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern bool IsTouching_OtherColliderWithFilter_Internal_Injected([Writable] Collider2D collider, ref ContactFilter2D contactFilter);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern bool IsTouching_AnyColliderWithFilter_Internal_Injected(ref ContactFilter2D contactFilter);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern bool OverlapPoint_Injected(ref Vector2 point);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern void Distance_Internal_Injected([Writable] Collider2D collider, out ColliderDistance2D ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern void AddForce_Injected(ref Vector2 force, [DefaultValue("ForceMode2D.Force")] ForceMode2D mode);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern void AddRelativeForce_Injected(ref Vector2 relativeForce, [DefaultValue("ForceMode2D.Force")] ForceMode2D mode);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern void AddForceAtPosition_Injected(ref Vector2 force, ref Vector2 position, [DefaultValue("ForceMode2D.Force")] ForceMode2D mode);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern void GetPoint_Injected(ref Vector2 point, out Vector2 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern void GetRelativePoint_Injected(ref Vector2 relativePoint, out Vector2 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern void GetVector_Injected(ref Vector2 vector, out Vector2 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern void GetRelativeVector_Injected(ref Vector2 relativeVector, out Vector2 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern void GetPointVelocity_Injected(ref Vector2 point, out Vector2 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern void GetRelativePointVelocity_Injected(ref Vector2 relativePoint, out Vector2 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern int OverlapColliderArray_Internal_Injected(ref ContactFilter2D contactFilter, Collider2D[] results);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern int OverlapColliderList_Internal_Injected(ref ContactFilter2D contactFilter, List<Collider2D> results);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern int CastArray_Internal_Injected(ref Vector2 direction, float distance, RaycastHit2D[] results);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern int CastList_Internal_Injected(ref Vector2 direction, float distance, List<RaycastHit2D> results);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern int CastFilteredArray_Internal_Injected(ref Vector2 direction, float distance, ref ContactFilter2D contactFilter, RaycastHit2D[] results);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern int CastFilteredList_Internal_Injected(ref Vector2 direction, float distance, ref ContactFilter2D contactFilter, List<RaycastHit2D> results);
}
