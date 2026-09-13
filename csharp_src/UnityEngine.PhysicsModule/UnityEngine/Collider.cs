using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine;

[RequiredByNativeCode]
[NativeHeader("Modules/Physics/Collider.h")]
[RequireComponent(typeof(Transform))]
public class Collider : Component
{
	public extern bool enabled
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern Rigidbody attachedRigidbody
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeMethod("GetRigidbody")]
		get;
	}

	public extern bool isTrigger
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern float contactOffset
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public Bounds bounds
	{
		get
		{
			get_bounds_Injected(out var ret);
			return ret;
		}
	}

	[NativeMethod("Material")]
	public extern PhysicMaterial sharedMaterial
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern PhysicMaterial material
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeMethod("GetClonedMaterial")]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeMethod("SetMaterial")]
		set;
	}

	public Vector3 ClosestPoint(Vector3 position)
	{
		ClosestPoint_Injected(ref position, out var ret);
		return ret;
	}

	private RaycastHit Raycast(Ray ray, float maxDistance, ref bool hasHit)
	{
		Raycast_Injected(ref ray, maxDistance, ref hasHit, out var ret);
		return ret;
	}

	public bool Raycast(Ray ray, out RaycastHit hitInfo, float maxDistance)
	{
		bool hasHit = false;
		hitInfo = Raycast(ray, maxDistance, ref hasHit);
		return hasHit;
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

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern void ClosestPoint_Injected(ref Vector3 position, out Vector3 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void get_bounds_Injected(out Bounds ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern void Raycast_Injected(ref Ray ray, float maxDistance, ref bool hasHit, out RaycastHit ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern void Internal_ClosestPointOnBounds_Injected(ref Vector3 point, ref Vector3 outPos, ref float distance);
}
