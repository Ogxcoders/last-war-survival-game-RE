using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Internal;
using UnityEngine.Scripting;

namespace UnityEngine;

[NativeHeader("Physics2DScriptingClasses.h")]
[NativeHeader("Modules/Physics2D/PhysicsManager2D.h")]
[NativeHeader("Physics2DScriptingClasses.h")]
[StaticAccessor("GetPhysicsManager2D()", StaticAccessorType.Arrow)]
public class Physics2D
{
	public const int IgnoreRaycastLayer = 4;

	public const int DefaultRaycastLayers = -5;

	public const int AllLayers = -1;

	private static List<Rigidbody2D> m_LastDisabledRigidbody2D = new List<Rigidbody2D>();

	public static PhysicsScene2D defaultPhysicsScene => default(PhysicsScene2D);

	[StaticAccessor("GetPhysics2DSettings()")]
	public static extern int velocityIterations
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	[StaticAccessor("GetPhysics2DSettings()")]
	public static extern int positionIterations
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	[StaticAccessor("GetPhysics2DSettings()")]
	public static Vector2 gravity
	{
		get
		{
			get_gravity_Injected(out var ret);
			return ret;
		}
		set
		{
			set_gravity_Injected(ref value);
		}
	}

	[StaticAccessor("GetPhysics2DSettings()")]
	public static extern bool queriesHitTriggers
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	[StaticAccessor("GetPhysics2DSettings()")]
	public static extern bool queriesStartInColliders
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	[StaticAccessor("GetPhysics2DSettings()")]
	public static extern bool callbacksOnDisable
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	[StaticAccessor("GetPhysics2DSettings()")]
	public static extern bool reuseCollisionCallbacks
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	[StaticAccessor("GetPhysics2DSettings()")]
	public static extern bool autoSyncTransforms
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	[StaticAccessor("GetPhysics2DSettings()")]
	public static extern bool autoSimulation
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	[StaticAccessor("GetPhysics2DSettings()")]
	public static PhysicsJobOptions2D jobOptions
	{
		get
		{
			get_jobOptions_Injected(out var ret);
			return ret;
		}
		set
		{
			set_jobOptions_Injected(ref value);
		}
	}

	[StaticAccessor("GetPhysics2DSettings()")]
	public static extern float velocityThreshold
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	[StaticAccessor("GetPhysics2DSettings()")]
	public static extern float maxLinearCorrection
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	[StaticAccessor("GetPhysics2DSettings()")]
	public static extern float maxAngularCorrection
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	[StaticAccessor("GetPhysics2DSettings()")]
	public static extern float maxTranslationSpeed
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	[StaticAccessor("GetPhysics2DSettings()")]
	public static extern float maxRotationSpeed
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	[StaticAccessor("GetPhysics2DSettings()")]
	public static extern float defaultContactOffset
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	[StaticAccessor("GetPhysics2DSettings()")]
	public static extern float baumgarteScale
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	[StaticAccessor("GetPhysics2DSettings()")]
	public static extern float baumgarteTOIScale
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	[StaticAccessor("GetPhysics2DSettings()")]
	public static extern float timeToSleep
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	[StaticAccessor("GetPhysics2DSettings()")]
	public static extern float linearSleepTolerance
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	[StaticAccessor("GetPhysics2DSettings()")]
	public static extern float angularSleepTolerance
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	[StaticAccessor("GetPhysics2DSettings()")]
	public static extern bool alwaysShowColliders
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	[StaticAccessor("GetPhysics2DSettings()")]
	public static extern bool showColliderSleep
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	[StaticAccessor("GetPhysics2DSettings()")]
	public static extern bool showColliderContacts
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	[StaticAccessor("GetPhysics2DSettings()")]
	public static extern bool showColliderAABB
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	[StaticAccessor("GetPhysics2DSettings()")]
	public static extern float contactArrowScale
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	[StaticAccessor("GetPhysics2DSettings()")]
	public static Color colliderAwakeColor
	{
		get
		{
			get_colliderAwakeColor_Injected(out var ret);
			return ret;
		}
		set
		{
			set_colliderAwakeColor_Injected(ref value);
		}
	}

	[StaticAccessor("GetPhysics2DSettings()")]
	public static Color colliderAsleepColor
	{
		get
		{
			get_colliderAsleepColor_Injected(out var ret);
			return ret;
		}
		set
		{
			set_colliderAsleepColor_Injected(ref value);
		}
	}

	[StaticAccessor("GetPhysics2DSettings()")]
	public static Color colliderContactColor
	{
		get
		{
			get_colliderContactColor_Injected(out var ret);
			return ret;
		}
		set
		{
			set_colliderContactColor_Injected(ref value);
		}
	}

	[StaticAccessor("GetPhysics2DSettings()")]
	public static Color colliderAABBColor
	{
		get
		{
			get_colliderAABBColor_Injected(out var ret);
			return ret;
		}
		set
		{
			set_colliderAABBColor_Injected(ref value);
		}
	}

	public static bool Simulate(float step)
	{
		return Simulate_Internal(defaultPhysicsScene, step);
	}

	[NativeMethod("Simulate_Binding")]
	internal static bool Simulate_Internal(PhysicsScene2D physicsScene, float step)
	{
		return Simulate_Internal_Injected(ref physicsScene, step);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern void SyncTransforms();

	[ExcludeFromDocs]
	public static void IgnoreCollision([Writable] Collider2D collider1, [Writable] Collider2D collider2)
	{
		IgnoreCollision(collider1, collider2, ignore: true);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[StaticAccessor("PhysicsScene2D", StaticAccessorType.DoubleColon)]
	[NativeMethod("IgnoreCollision_Binding")]
	public static extern void IgnoreCollision([Writable][NotNull] Collider2D collider1, [Writable][NotNull] Collider2D collider2, [DefaultValue("true")] bool ignore);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod("GetIgnoreCollision_Binding")]
	[StaticAccessor("PhysicsScene2D", StaticAccessorType.DoubleColon)]
	public static extern bool GetIgnoreCollision([NotNull][Writable] Collider2D collider1, [NotNull][Writable] Collider2D collider2);

	[ExcludeFromDocs]
	public static void IgnoreLayerCollision(int layer1, int layer2)
	{
		IgnoreLayerCollision(layer1, layer2, ignore: true);
	}

	public static void IgnoreLayerCollision(int layer1, int layer2, bool ignore)
	{
		if (layer1 < 0 || layer1 > 31)
		{
			throw new ArgumentOutOfRangeException("layer1 is out of range. Layer numbers must be in the range 0 to 31.");
		}
		if (layer2 < 0 || layer2 > 31)
		{
			throw new ArgumentOutOfRangeException("layer2 is out of range. Layer numbers must be in the range 0 to 31.");
		}
		IgnoreLayerCollision_Internal(layer1, layer2, ignore);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[StaticAccessor("GetPhysics2DSettings()")]
	[NativeMethod("IgnoreLayerCollision")]
	private static extern void IgnoreLayerCollision_Internal(int layer1, int layer2, bool ignore);

	public static bool GetIgnoreLayerCollision(int layer1, int layer2)
	{
		if (layer1 < 0 || layer1 > 31)
		{
			throw new ArgumentOutOfRangeException("layer1 is out of range. Layer numbers must be in the range 0 to 31.");
		}
		if (layer2 < 0 || layer2 > 31)
		{
			throw new ArgumentOutOfRangeException("layer2 is out of range. Layer numbers must be in the range 0 to 31.");
		}
		return GetIgnoreLayerCollision_Internal(layer1, layer2);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[StaticAccessor("GetPhysics2DSettings()")]
	[NativeMethod("GetIgnoreLayerCollision")]
	private static extern bool GetIgnoreLayerCollision_Internal(int layer1, int layer2);

	public static void SetLayerCollisionMask(int layer, int layerMask)
	{
		if (layer < 0 || layer > 31)
		{
			throw new ArgumentOutOfRangeException("layer1 is out of range. Layer numbers must be in the range 0 to 31.");
		}
		SetLayerCollisionMask_Internal(layer, layerMask);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod("SetLayerCollisionMask")]
	[StaticAccessor("GetPhysics2DSettings()")]
	private static extern void SetLayerCollisionMask_Internal(int layer, int layerMask);

	public static int GetLayerCollisionMask(int layer)
	{
		if (layer < 0 || layer > 31)
		{
			throw new ArgumentOutOfRangeException("layer1 is out of range. Layer numbers must be in the range 0 to 31.");
		}
		return GetLayerCollisionMask_Internal(layer);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[StaticAccessor("GetPhysics2DSettings()")]
	[NativeMethod("GetLayerCollisionMask")]
	private static extern int GetLayerCollisionMask_Internal(int layer);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[StaticAccessor("PhysicsQuery2D", StaticAccessorType.DoubleColon)]
	public static extern bool IsTouching([NotNull][Writable] Collider2D collider1, [NotNull][Writable] Collider2D collider2);

	public static bool IsTouching([Writable] Collider2D collider1, [Writable] Collider2D collider2, ContactFilter2D contactFilter)
	{
		return IsTouching_TwoCollidersWithFilter(collider1, collider2, contactFilter);
	}

	[StaticAccessor("PhysicsQuery2D", StaticAccessorType.DoubleColon)]
	[NativeMethod("IsTouching")]
	private static bool IsTouching_TwoCollidersWithFilter([Writable][NotNull] Collider2D collider1, [NotNull][Writable] Collider2D collider2, ContactFilter2D contactFilter)
	{
		return IsTouching_TwoCollidersWithFilter_Injected(collider1, collider2, ref contactFilter);
	}

	public static bool IsTouching([Writable] Collider2D collider, ContactFilter2D contactFilter)
	{
		return IsTouching_SingleColliderWithFilter(collider, contactFilter);
	}

	[NativeMethod("IsTouching")]
	[StaticAccessor("PhysicsQuery2D", StaticAccessorType.DoubleColon)]
	private static bool IsTouching_SingleColliderWithFilter([NotNull][Writable] Collider2D collider, ContactFilter2D contactFilter)
	{
		return IsTouching_SingleColliderWithFilter_Injected(collider, ref contactFilter);
	}

	[ExcludeFromDocs]
	public static bool IsTouchingLayers([Writable] Collider2D collider)
	{
		return IsTouchingLayers(collider, -1);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[StaticAccessor("PhysicsQuery2D", StaticAccessorType.DoubleColon)]
	public static extern bool IsTouchingLayers([NotNull][Writable] Collider2D collider, [DefaultValue("Physics2D.AllLayers")] int layerMask);

	public static ColliderDistance2D Distance([Writable] Collider2D colliderA, [Writable] Collider2D colliderB)
	{
		if (colliderA == null)
		{
			throw new ArgumentNullException("ColliderA cannot be NULL.");
		}
		if (colliderB == null)
		{
			throw new ArgumentNullException("ColliderB cannot be NULL.");
		}
		if (colliderA == colliderB)
		{
			throw new ArgumentException("Cannot calculate the distance between the same collider.");
		}
		return Distance_Internal(colliderA, colliderB);
	}

	[NativeMethod("Distance")]
	[StaticAccessor("PhysicsQuery2D", StaticAccessorType.DoubleColon)]
	private static ColliderDistance2D Distance_Internal([Writable][NotNull] Collider2D colliderA, [NotNull][Writable] Collider2D colliderB)
	{
		Distance_Internal_Injected(colliderA, colliderB, out var ret);
		return ret;
	}

	public static Vector2 ClosestPoint(Vector2 position, Collider2D collider)
	{
		if (collider == null)
		{
			throw new ArgumentNullException("Collider cannot be NULL.");
		}
		return ClosestPoint_Collider(position, collider);
	}

	public static Vector2 ClosestPoint(Vector2 position, Rigidbody2D rigidbody)
	{
		if (rigidbody == null)
		{
			throw new ArgumentNullException("Rigidbody cannot be NULL.");
		}
		return ClosestPoint_Rigidbody(position, rigidbody);
	}

	[NativeMethod("ClosestPoint")]
	[StaticAccessor("PhysicsQuery2D", StaticAccessorType.DoubleColon)]
	private static Vector2 ClosestPoint_Collider(Vector2 position, [NotNull] Collider2D collider)
	{
		ClosestPoint_Collider_Injected(ref position, collider, out var ret);
		return ret;
	}

	[NativeMethod("ClosestPoint")]
	[StaticAccessor("PhysicsQuery2D", StaticAccessorType.DoubleColon)]
	private static Vector2 ClosestPoint_Rigidbody(Vector2 position, [NotNull] Rigidbody2D rigidbody)
	{
		ClosestPoint_Rigidbody_Injected(ref position, rigidbody, out var ret);
		return ret;
	}

	[ExcludeFromDocs]
	public static RaycastHit2D Linecast(Vector2 start, Vector2 end)
	{
		return defaultPhysicsScene.Linecast(start, end);
	}

	[ExcludeFromDocs]
	public static RaycastHit2D Linecast(Vector2 start, Vector2 end, int layerMask)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, float.NegativeInfinity, float.PositiveInfinity);
		return defaultPhysicsScene.Linecast(start, end, contactFilter);
	}

	[ExcludeFromDocs]
	public static RaycastHit2D Linecast(Vector2 start, Vector2 end, int layerMask, float minDepth)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, float.PositiveInfinity);
		return defaultPhysicsScene.Linecast(start, end, contactFilter);
	}

	public static RaycastHit2D Linecast(Vector2 start, Vector2 end, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
		return defaultPhysicsScene.Linecast(start, end, contactFilter);
	}

	public static int Linecast(Vector2 start, Vector2 end, ContactFilter2D contactFilter, RaycastHit2D[] results)
	{
		return defaultPhysicsScene.Linecast(start, end, contactFilter, results);
	}

	public static int Linecast(Vector2 start, Vector2 end, ContactFilter2D contactFilter, List<RaycastHit2D> results)
	{
		return defaultPhysicsScene.Linecast(start, end, contactFilter, results);
	}

	[ExcludeFromDocs]
	public static RaycastHit2D[] LinecastAll(Vector2 start, Vector2 end)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(-5, float.NegativeInfinity, float.PositiveInfinity);
		return LinecastAll_Internal(defaultPhysicsScene, start, end, contactFilter);
	}

	[ExcludeFromDocs]
	public static RaycastHit2D[] LinecastAll(Vector2 start, Vector2 end, int layerMask)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, float.NegativeInfinity, float.PositiveInfinity);
		return LinecastAll_Internal(defaultPhysicsScene, start, end, contactFilter);
	}

	[ExcludeFromDocs]
	public static RaycastHit2D[] LinecastAll(Vector2 start, Vector2 end, int layerMask, float minDepth)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, float.PositiveInfinity);
		return LinecastAll_Internal(defaultPhysicsScene, start, end, contactFilter);
	}

	public static RaycastHit2D[] LinecastAll(Vector2 start, Vector2 end, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
		return LinecastAll_Internal(defaultPhysicsScene, start, end, contactFilter);
	}

	[NativeMethod("LinecastAll_Binding")]
	[StaticAccessor("PhysicsQuery2D", StaticAccessorType.DoubleColon)]
	private static RaycastHit2D[] LinecastAll_Internal(PhysicsScene2D physicsScene, Vector2 start, Vector2 end, ContactFilter2D contactFilter)
	{
		return LinecastAll_Internal_Injected(ref physicsScene, ref start, ref end, ref contactFilter);
	}

	[ExcludeFromDocs]
	public static int LinecastNonAlloc(Vector2 start, Vector2 end, RaycastHit2D[] results)
	{
		return defaultPhysicsScene.Linecast(start, end, results);
	}

	[ExcludeFromDocs]
	public static int LinecastNonAlloc(Vector2 start, Vector2 end, RaycastHit2D[] results, int layerMask)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, float.NegativeInfinity, float.PositiveInfinity);
		return defaultPhysicsScene.Linecast(start, end, contactFilter, results);
	}

	[ExcludeFromDocs]
	public static int LinecastNonAlloc(Vector2 start, Vector2 end, RaycastHit2D[] results, int layerMask, float minDepth)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, float.PositiveInfinity);
		return defaultPhysicsScene.Linecast(start, end, contactFilter, results);
	}

	public static int LinecastNonAlloc(Vector2 start, Vector2 end, RaycastHit2D[] results, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
		return defaultPhysicsScene.Linecast(start, end, contactFilter, results);
	}

	[ExcludeFromDocs]
	public static RaycastHit2D Raycast(Vector2 origin, Vector2 direction)
	{
		return defaultPhysicsScene.Raycast(origin, direction, float.PositiveInfinity);
	}

	[ExcludeFromDocs]
	public static RaycastHit2D Raycast(Vector2 origin, Vector2 direction, float distance)
	{
		return defaultPhysicsScene.Raycast(origin, direction, distance);
	}

	[ExcludeFromDocs]
	[RequiredByNativeCode]
	public static RaycastHit2D Raycast(Vector2 origin, Vector2 direction, float distance, int layerMask)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, float.NegativeInfinity, float.PositiveInfinity);
		return defaultPhysicsScene.Raycast(origin, direction, distance, contactFilter);
	}

	[ExcludeFromDocs]
	public static RaycastHit2D Raycast(Vector2 origin, Vector2 direction, float distance, int layerMask, float minDepth)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, float.PositiveInfinity);
		return defaultPhysicsScene.Raycast(origin, direction, distance, contactFilter);
	}

	public static RaycastHit2D Raycast(Vector2 origin, Vector2 direction, [DefaultValue("Mathf.Infinity")] float distance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
		return defaultPhysicsScene.Raycast(origin, direction, distance, contactFilter);
	}

	[ExcludeFromDocs]
	public static int Raycast(Vector2 origin, Vector2 direction, ContactFilter2D contactFilter, RaycastHit2D[] results)
	{
		return defaultPhysicsScene.Raycast(origin, direction, float.PositiveInfinity, contactFilter, results);
	}

	public static int Raycast(Vector2 origin, Vector2 direction, ContactFilter2D contactFilter, RaycastHit2D[] results, [DefaultValue("Mathf.Infinity")] float distance)
	{
		return defaultPhysicsScene.Raycast(origin, direction, distance, contactFilter, results);
	}

	public static int Raycast(Vector2 origin, Vector2 direction, ContactFilter2D contactFilter, List<RaycastHit2D> results, [DefaultValue("Mathf.Infinity")] float distance = float.PositiveInfinity)
	{
		return defaultPhysicsScene.Raycast(origin, direction, distance, contactFilter, results);
	}

	[ExcludeFromDocs]
	public static int RaycastNonAlloc(Vector2 origin, Vector2 direction, RaycastHit2D[] results)
	{
		return defaultPhysicsScene.Raycast(origin, direction, float.PositiveInfinity, results);
	}

	[ExcludeFromDocs]
	public static int RaycastNonAlloc(Vector2 origin, Vector2 direction, RaycastHit2D[] results, float distance)
	{
		return defaultPhysicsScene.Raycast(origin, direction, distance, results);
	}

	[ExcludeFromDocs]
	public static int RaycastNonAlloc(Vector2 origin, Vector2 direction, RaycastHit2D[] results, float distance, int layerMask)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, float.NegativeInfinity, float.PositiveInfinity);
		return defaultPhysicsScene.Raycast(origin, direction, distance, contactFilter, results);
	}

	[ExcludeFromDocs]
	public static int RaycastNonAlloc(Vector2 origin, Vector2 direction, RaycastHit2D[] results, float distance, int layerMask, float minDepth)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, float.PositiveInfinity);
		return defaultPhysicsScene.Raycast(origin, direction, distance, contactFilter, results);
	}

	public static int RaycastNonAlloc(Vector2 origin, Vector2 direction, RaycastHit2D[] results, [DefaultValue("Mathf.Infinity")] float distance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
		return defaultPhysicsScene.Raycast(origin, direction, distance, contactFilter, results);
	}

	[ExcludeFromDocs]
	public static RaycastHit2D[] RaycastAll(Vector2 origin, Vector2 direction)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(-5, float.NegativeInfinity, float.PositiveInfinity);
		return RaycastAll_Internal(defaultPhysicsScene, origin, direction, float.PositiveInfinity, contactFilter);
	}

	[ExcludeFromDocs]
	public static RaycastHit2D[] RaycastAll(Vector2 origin, Vector2 direction, float distance)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(-5, float.NegativeInfinity, float.PositiveInfinity);
		return RaycastAll_Internal(defaultPhysicsScene, origin, direction, distance, contactFilter);
	}

	[ExcludeFromDocs]
	public static RaycastHit2D[] RaycastAll(Vector2 origin, Vector2 direction, float distance, int layerMask)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, float.NegativeInfinity, float.PositiveInfinity);
		return RaycastAll_Internal(defaultPhysicsScene, origin, direction, distance, contactFilter);
	}

	[ExcludeFromDocs]
	public static RaycastHit2D[] RaycastAll(Vector2 origin, Vector2 direction, float distance, int layerMask, float minDepth)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, float.PositiveInfinity);
		return RaycastAll_Internal(defaultPhysicsScene, origin, direction, distance, contactFilter);
	}

	public static RaycastHit2D[] RaycastAll(Vector2 origin, Vector2 direction, [DefaultValue("Mathf.Infinity")] float distance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
		return RaycastAll_Internal(defaultPhysicsScene, origin, direction, distance, contactFilter);
	}

	[StaticAccessor("PhysicsQuery2D", StaticAccessorType.DoubleColon)]
	[NativeMethod("RaycastAll_Binding")]
	private static RaycastHit2D[] RaycastAll_Internal(PhysicsScene2D physicsScene, Vector2 origin, Vector2 direction, float distance, ContactFilter2D contactFilter)
	{
		return RaycastAll_Internal_Injected(ref physicsScene, ref origin, ref direction, distance, ref contactFilter);
	}

	[ExcludeFromDocs]
	public static RaycastHit2D CircleCast(Vector2 origin, float radius, Vector2 direction)
	{
		return defaultPhysicsScene.CircleCast(origin, radius, direction, float.PositiveInfinity);
	}

	[ExcludeFromDocs]
	public static RaycastHit2D CircleCast(Vector2 origin, float radius, Vector2 direction, float distance)
	{
		return defaultPhysicsScene.CircleCast(origin, radius, direction, distance);
	}

	[ExcludeFromDocs]
	public static RaycastHit2D CircleCast(Vector2 origin, float radius, Vector2 direction, float distance, int layerMask)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, float.NegativeInfinity, float.PositiveInfinity);
		return defaultPhysicsScene.CircleCast(origin, radius, direction, distance, contactFilter);
	}

	[ExcludeFromDocs]
	public static RaycastHit2D CircleCast(Vector2 origin, float radius, Vector2 direction, float distance, int layerMask, float minDepth)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, float.PositiveInfinity);
		return defaultPhysicsScene.CircleCast(origin, radius, direction, distance, contactFilter);
	}

	public static RaycastHit2D CircleCast(Vector2 origin, float radius, Vector2 direction, [DefaultValue("Mathf.Infinity")] float distance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
		return defaultPhysicsScene.CircleCast(origin, radius, direction, distance, contactFilter);
	}

	[ExcludeFromDocs]
	public static int CircleCast(Vector2 origin, float radius, Vector2 direction, ContactFilter2D contactFilter, RaycastHit2D[] results)
	{
		return defaultPhysicsScene.CircleCast(origin, radius, direction, float.PositiveInfinity, contactFilter, results);
	}

	public static int CircleCast(Vector2 origin, float radius, Vector2 direction, ContactFilter2D contactFilter, RaycastHit2D[] results, [DefaultValue("Mathf.Infinity")] float distance)
	{
		return defaultPhysicsScene.CircleCast(origin, radius, direction, distance, contactFilter, results);
	}

	public static int CircleCast(Vector2 origin, float radius, Vector2 direction, ContactFilter2D contactFilter, List<RaycastHit2D> results, [DefaultValue("Mathf.Infinity")] float distance = float.PositiveInfinity)
	{
		return defaultPhysicsScene.CircleCast(origin, radius, direction, distance, contactFilter, results);
	}

	[ExcludeFromDocs]
	public static RaycastHit2D[] CircleCastAll(Vector2 origin, float radius, Vector2 direction)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(-5, float.NegativeInfinity, float.PositiveInfinity);
		return CircleCastAll_Internal(defaultPhysicsScene, origin, radius, direction, float.PositiveInfinity, contactFilter);
	}

	[ExcludeFromDocs]
	public static RaycastHit2D[] CircleCastAll(Vector2 origin, float radius, Vector2 direction, float distance)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(-5, float.NegativeInfinity, float.PositiveInfinity);
		return CircleCastAll_Internal(defaultPhysicsScene, origin, radius, direction, distance, contactFilter);
	}

	[ExcludeFromDocs]
	public static RaycastHit2D[] CircleCastAll(Vector2 origin, float radius, Vector2 direction, float distance, int layerMask)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, float.NegativeInfinity, float.PositiveInfinity);
		return CircleCastAll_Internal(defaultPhysicsScene, origin, radius, direction, distance, contactFilter);
	}

	[ExcludeFromDocs]
	public static RaycastHit2D[] CircleCastAll(Vector2 origin, float radius, Vector2 direction, float distance, int layerMask, float minDepth)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, float.PositiveInfinity);
		return CircleCastAll_Internal(defaultPhysicsScene, origin, radius, direction, distance, contactFilter);
	}

	public static RaycastHit2D[] CircleCastAll(Vector2 origin, float radius, Vector2 direction, [DefaultValue("Mathf.Infinity")] float distance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
		return CircleCastAll_Internal(defaultPhysicsScene, origin, radius, direction, distance, contactFilter);
	}

	[NativeMethod("CircleCastAll_Binding")]
	[StaticAccessor("PhysicsQuery2D", StaticAccessorType.DoubleColon)]
	private static RaycastHit2D[] CircleCastAll_Internal(PhysicsScene2D physicsScene, Vector2 origin, float radius, Vector2 direction, float distance, ContactFilter2D contactFilter)
	{
		return CircleCastAll_Internal_Injected(ref physicsScene, ref origin, radius, ref direction, distance, ref contactFilter);
	}

	[ExcludeFromDocs]
	public static int CircleCastNonAlloc(Vector2 origin, float radius, Vector2 direction, RaycastHit2D[] results)
	{
		return defaultPhysicsScene.CircleCast(origin, radius, direction, float.PositiveInfinity, results);
	}

	[ExcludeFromDocs]
	public static int CircleCastNonAlloc(Vector2 origin, float radius, Vector2 direction, RaycastHit2D[] results, float distance)
	{
		return defaultPhysicsScene.CircleCast(origin, radius, direction, distance, results);
	}

	[ExcludeFromDocs]
	public static int CircleCastNonAlloc(Vector2 origin, float radius, Vector2 direction, RaycastHit2D[] results, float distance, int layerMask)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, float.NegativeInfinity, float.PositiveInfinity);
		return defaultPhysicsScene.CircleCast(origin, radius, direction, distance, contactFilter, results);
	}

	[ExcludeFromDocs]
	public static int CircleCastNonAlloc(Vector2 origin, float radius, Vector2 direction, RaycastHit2D[] results, float distance, int layerMask, float minDepth)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, float.PositiveInfinity);
		return defaultPhysicsScene.CircleCast(origin, radius, direction, distance, contactFilter, results);
	}

	public static int CircleCastNonAlloc(Vector2 origin, float radius, Vector2 direction, RaycastHit2D[] results, [DefaultValue("Mathf.Infinity")] float distance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
		return defaultPhysicsScene.CircleCast(origin, radius, direction, distance, contactFilter, results);
	}

	[ExcludeFromDocs]
	public static RaycastHit2D BoxCast(Vector2 origin, Vector2 size, float angle, Vector2 direction)
	{
		return defaultPhysicsScene.BoxCast(origin, size, angle, direction, float.PositiveInfinity);
	}

	[ExcludeFromDocs]
	public static RaycastHit2D BoxCast(Vector2 origin, Vector2 size, float angle, Vector2 direction, float distance)
	{
		return defaultPhysicsScene.BoxCast(origin, size, angle, direction, distance);
	}

	[ExcludeFromDocs]
	public static RaycastHit2D BoxCast(Vector2 origin, Vector2 size, float angle, Vector2 direction, float distance, int layerMask)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, float.NegativeInfinity, float.PositiveInfinity);
		return defaultPhysicsScene.BoxCast(origin, size, angle, direction, distance, contactFilter);
	}

	[ExcludeFromDocs]
	public static RaycastHit2D BoxCast(Vector2 origin, Vector2 size, float angle, Vector2 direction, float distance, int layerMask, float minDepth)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, float.PositiveInfinity);
		return defaultPhysicsScene.BoxCast(origin, size, angle, direction, distance, contactFilter);
	}

	public static RaycastHit2D BoxCast(Vector2 origin, Vector2 size, float angle, Vector2 direction, [DefaultValue("Mathf.Infinity")] float distance, [DefaultValue("Physics2D.AllLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
		return defaultPhysicsScene.BoxCast(origin, size, angle, direction, distance, contactFilter);
	}

	[ExcludeFromDocs]
	public static int BoxCast(Vector2 origin, Vector2 size, float angle, Vector2 direction, ContactFilter2D contactFilter, RaycastHit2D[] results)
	{
		return defaultPhysicsScene.BoxCast(origin, size, angle, direction, float.PositiveInfinity, contactFilter, results);
	}

	public static int BoxCast(Vector2 origin, Vector2 size, float angle, Vector2 direction, ContactFilter2D contactFilter, RaycastHit2D[] results, [DefaultValue("Mathf.Infinity")] float distance)
	{
		return defaultPhysicsScene.BoxCast(origin, size, angle, direction, distance, contactFilter, results);
	}

	public static int BoxCast(Vector2 origin, Vector2 size, float angle, Vector2 direction, ContactFilter2D contactFilter, List<RaycastHit2D> results, [DefaultValue("Mathf.Infinity")] float distance = float.PositiveInfinity)
	{
		return defaultPhysicsScene.BoxCast(origin, size, angle, direction, distance, contactFilter, results);
	}

	[ExcludeFromDocs]
	public static RaycastHit2D[] BoxCastAll(Vector2 origin, Vector2 size, float angle, Vector2 direction)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(-5, float.NegativeInfinity, float.PositiveInfinity);
		return BoxCastAll_Internal(defaultPhysicsScene, origin, size, angle, direction, float.PositiveInfinity, contactFilter);
	}

	[ExcludeFromDocs]
	public static RaycastHit2D[] BoxCastAll(Vector2 origin, Vector2 size, float angle, Vector2 direction, float distance)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(-5, float.NegativeInfinity, float.PositiveInfinity);
		return BoxCastAll_Internal(defaultPhysicsScene, origin, size, angle, direction, distance, contactFilter);
	}

	[ExcludeFromDocs]
	public static RaycastHit2D[] BoxCastAll(Vector2 origin, Vector2 size, float angle, Vector2 direction, float distance, int layerMask)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, float.NegativeInfinity, float.PositiveInfinity);
		return BoxCastAll_Internal(defaultPhysicsScene, origin, size, angle, direction, distance, contactFilter);
	}

	[ExcludeFromDocs]
	public static RaycastHit2D[] BoxCastAll(Vector2 origin, Vector2 size, float angle, Vector2 direction, float distance, int layerMask, float minDepth)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, float.PositiveInfinity);
		return BoxCastAll_Internal(defaultPhysicsScene, origin, size, angle, direction, distance, contactFilter);
	}

	public static RaycastHit2D[] BoxCastAll(Vector2 origin, Vector2 size, float angle, Vector2 direction, [DefaultValue("Mathf.Infinity")] float distance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
		return BoxCastAll_Internal(defaultPhysicsScene, origin, size, angle, direction, distance, contactFilter);
	}

	[NativeMethod("BoxCastAll_Binding")]
	[StaticAccessor("PhysicsQuery2D", StaticAccessorType.DoubleColon)]
	private static RaycastHit2D[] BoxCastAll_Internal(PhysicsScene2D physicsScene, Vector2 origin, Vector2 size, float angle, Vector2 direction, float distance, ContactFilter2D contactFilter)
	{
		return BoxCastAll_Internal_Injected(ref physicsScene, ref origin, ref size, angle, ref direction, distance, ref contactFilter);
	}

	[ExcludeFromDocs]
	public static int BoxCastNonAlloc(Vector2 origin, Vector2 size, float angle, Vector2 direction, RaycastHit2D[] results)
	{
		return defaultPhysicsScene.BoxCast(origin, size, angle, direction, float.PositiveInfinity, results);
	}

	[ExcludeFromDocs]
	public static int BoxCastNonAlloc(Vector2 origin, Vector2 size, float angle, Vector2 direction, RaycastHit2D[] results, float distance)
	{
		return defaultPhysicsScene.BoxCast(origin, size, angle, direction, distance, results);
	}

	[ExcludeFromDocs]
	public static int BoxCastNonAlloc(Vector2 origin, Vector2 size, float angle, Vector2 direction, RaycastHit2D[] results, float distance, int layerMask)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, float.NegativeInfinity, float.PositiveInfinity);
		return defaultPhysicsScene.BoxCast(origin, size, angle, direction, distance, contactFilter, results);
	}

	[ExcludeFromDocs]
	public static int BoxCastNonAlloc(Vector2 origin, Vector2 size, float angle, Vector2 direction, RaycastHit2D[] results, float distance, int layerMask, float minDepth)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, float.PositiveInfinity);
		return defaultPhysicsScene.BoxCast(origin, size, angle, direction, distance, contactFilter, results);
	}

	public static int BoxCastNonAlloc(Vector2 origin, Vector2 size, float angle, Vector2 direction, RaycastHit2D[] results, [DefaultValue("Mathf.Infinity")] float distance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
		return defaultPhysicsScene.BoxCast(origin, size, angle, direction, distance, contactFilter, results);
	}

	[ExcludeFromDocs]
	public static RaycastHit2D CapsuleCast(Vector2 origin, Vector2 size, CapsuleDirection2D capsuleDirection, float angle, Vector2 direction)
	{
		return defaultPhysicsScene.CapsuleCast(origin, size, capsuleDirection, angle, direction, float.PositiveInfinity);
	}

	[ExcludeFromDocs]
	public static RaycastHit2D CapsuleCast(Vector2 origin, Vector2 size, CapsuleDirection2D capsuleDirection, float angle, Vector2 direction, float distance)
	{
		return defaultPhysicsScene.CapsuleCast(origin, size, capsuleDirection, angle, direction, distance);
	}

	[ExcludeFromDocs]
	public static RaycastHit2D CapsuleCast(Vector2 origin, Vector2 size, CapsuleDirection2D capsuleDirection, float angle, Vector2 direction, float distance, int layerMask)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, float.NegativeInfinity, float.PositiveInfinity);
		return defaultPhysicsScene.CapsuleCast(origin, size, capsuleDirection, angle, direction, distance, contactFilter);
	}

	[ExcludeFromDocs]
	public static RaycastHit2D CapsuleCast(Vector2 origin, Vector2 size, CapsuleDirection2D capsuleDirection, float angle, Vector2 direction, float distance, int layerMask, float minDepth)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, float.PositiveInfinity);
		return defaultPhysicsScene.CapsuleCast(origin, size, capsuleDirection, angle, direction, distance, contactFilter);
	}

	public static RaycastHit2D CapsuleCast(Vector2 origin, Vector2 size, CapsuleDirection2D capsuleDirection, float angle, Vector2 direction, [DefaultValue("Mathf.Infinity")] float distance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
		return defaultPhysicsScene.CapsuleCast(origin, size, capsuleDirection, angle, direction, distance, contactFilter);
	}

	[ExcludeFromDocs]
	public static int CapsuleCast(Vector2 origin, Vector2 size, CapsuleDirection2D capsuleDirection, float angle, Vector2 direction, ContactFilter2D contactFilter, RaycastHit2D[] results)
	{
		return defaultPhysicsScene.CapsuleCast(origin, size, capsuleDirection, angle, direction, float.PositiveInfinity, contactFilter, results);
	}

	public static int CapsuleCast(Vector2 origin, Vector2 size, CapsuleDirection2D capsuleDirection, float angle, Vector2 direction, ContactFilter2D contactFilter, RaycastHit2D[] results, [DefaultValue("Mathf.Infinity")] float distance)
	{
		return defaultPhysicsScene.CapsuleCast(origin, size, capsuleDirection, angle, direction, distance, contactFilter, results);
	}

	public static int CapsuleCast(Vector2 origin, Vector2 size, CapsuleDirection2D capsuleDirection, float angle, Vector2 direction, ContactFilter2D contactFilter, List<RaycastHit2D> results, [DefaultValue("Mathf.Infinity")] float distance = float.PositiveInfinity)
	{
		return defaultPhysicsScene.CapsuleCast(origin, size, capsuleDirection, angle, direction, distance, contactFilter, results);
	}

	[ExcludeFromDocs]
	public static RaycastHit2D[] CapsuleCastAll(Vector2 origin, Vector2 size, CapsuleDirection2D capsuleDirection, float angle, Vector2 direction)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(-5, float.NegativeInfinity, float.PositiveInfinity);
		return CapsuleCastAll_Internal(defaultPhysicsScene, origin, size, capsuleDirection, angle, direction, float.PositiveInfinity, contactFilter);
	}

	[ExcludeFromDocs]
	public static RaycastHit2D[] CapsuleCastAll(Vector2 origin, Vector2 size, CapsuleDirection2D capsuleDirection, float angle, Vector2 direction, float distance)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(-5, float.NegativeInfinity, float.PositiveInfinity);
		return CapsuleCastAll_Internal(defaultPhysicsScene, origin, size, capsuleDirection, angle, direction, distance, contactFilter);
	}

	[StaticAccessor("PhysicsQuery2D", StaticAccessorType.DoubleColon)]
	[NativeMethod("CapsuleCastAll_Binding")]
	private static RaycastHit2D[] CapsuleCastAll_Internal(PhysicsScene2D physicsScene, Vector2 origin, Vector2 size, CapsuleDirection2D capsuleDirection, float angle, Vector2 direction, float distance, ContactFilter2D contactFilter)
	{
		return CapsuleCastAll_Internal_Injected(ref physicsScene, ref origin, ref size, capsuleDirection, angle, ref direction, distance, ref contactFilter);
	}

	[ExcludeFromDocs]
	public static RaycastHit2D[] CapsuleCastAll(Vector2 origin, Vector2 size, CapsuleDirection2D capsuleDirection, float angle, Vector2 direction, float distance, int layerMask)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, float.NegativeInfinity, float.PositiveInfinity);
		return CapsuleCastAll_Internal(defaultPhysicsScene, origin, size, capsuleDirection, angle, direction, distance, contactFilter);
	}

	[ExcludeFromDocs]
	public static RaycastHit2D[] CapsuleCastAll(Vector2 origin, Vector2 size, CapsuleDirection2D capsuleDirection, float angle, Vector2 direction, float distance, int layerMask, float minDepth)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, float.PositiveInfinity);
		return CapsuleCastAll_Internal(defaultPhysicsScene, origin, size, capsuleDirection, angle, direction, distance, contactFilter);
	}

	public static RaycastHit2D[] CapsuleCastAll(Vector2 origin, Vector2 size, CapsuleDirection2D capsuleDirection, float angle, Vector2 direction, [DefaultValue("Mathf.Infinity")] float distance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
		return CapsuleCastAll_Internal(defaultPhysicsScene, origin, size, capsuleDirection, angle, direction, distance, contactFilter);
	}

	[ExcludeFromDocs]
	public static int CapsuleCastNonAlloc(Vector2 origin, Vector2 size, CapsuleDirection2D capsuleDirection, float angle, Vector2 direction, RaycastHit2D[] results)
	{
		return defaultPhysicsScene.CapsuleCast(origin, size, capsuleDirection, angle, direction, float.PositiveInfinity, results);
	}

	[ExcludeFromDocs]
	public static int CapsuleCastNonAlloc(Vector2 origin, Vector2 size, CapsuleDirection2D capsuleDirection, float angle, Vector2 direction, RaycastHit2D[] results, float distance)
	{
		return defaultPhysicsScene.CapsuleCast(origin, size, capsuleDirection, angle, direction, distance, results);
	}

	[ExcludeFromDocs]
	public static int CapsuleCastNonAlloc(Vector2 origin, Vector2 size, CapsuleDirection2D capsuleDirection, float angle, Vector2 direction, RaycastHit2D[] results, float distance, int layerMask)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, float.NegativeInfinity, float.PositiveInfinity);
		return defaultPhysicsScene.CapsuleCast(origin, size, capsuleDirection, angle, direction, distance, contactFilter, results);
	}

	[ExcludeFromDocs]
	public static int CapsuleCastNonAlloc(Vector2 origin, Vector2 size, CapsuleDirection2D capsuleDirection, float angle, Vector2 direction, RaycastHit2D[] results, float distance, int layerMask, float minDepth)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, float.PositiveInfinity);
		return defaultPhysicsScene.CapsuleCast(origin, size, capsuleDirection, angle, direction, distance, contactFilter, results);
	}

	public static int CapsuleCastNonAlloc(Vector2 origin, Vector2 size, CapsuleDirection2D capsuleDirection, float angle, Vector2 direction, RaycastHit2D[] results, [DefaultValue("Mathf.Infinity")] float distance, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
		return defaultPhysicsScene.CapsuleCast(origin, size, capsuleDirection, angle, direction, distance, contactFilter, results);
	}

	[ExcludeFromDocs]
	public static RaycastHit2D GetRayIntersection(Ray ray)
	{
		return defaultPhysicsScene.GetRayIntersection(ray, float.PositiveInfinity);
	}

	[ExcludeFromDocs]
	public static RaycastHit2D GetRayIntersection(Ray ray, float distance)
	{
		return defaultPhysicsScene.GetRayIntersection(ray, distance);
	}

	public static RaycastHit2D GetRayIntersection(Ray ray, [DefaultValue("Mathf.Infinity")] float distance, [DefaultValue("DefaultRaycastLayers")] int layerMask)
	{
		return defaultPhysicsScene.GetRayIntersection(ray, distance, layerMask);
	}

	[ExcludeFromDocs]
	public static RaycastHit2D[] GetRayIntersectionAll(Ray ray)
	{
		return GetRayIntersectionAll_Internal(defaultPhysicsScene, ray.origin, ray.direction, float.PositiveInfinity, -5);
	}

	[ExcludeFromDocs]
	public static RaycastHit2D[] GetRayIntersectionAll(Ray ray, float distance)
	{
		return GetRayIntersectionAll_Internal(defaultPhysicsScene, ray.origin, ray.direction, distance, -5);
	}

	[RequiredByNativeCode]
	public static RaycastHit2D[] GetRayIntersectionAll(Ray ray, [DefaultValue("Mathf.Infinity")] float distance, [DefaultValue("DefaultRaycastLayers")] int layerMask)
	{
		return GetRayIntersectionAll_Internal(defaultPhysicsScene, ray.origin, ray.direction, distance, layerMask);
	}

	[StaticAccessor("PhysicsQuery2D", StaticAccessorType.DoubleColon)]
	[NativeMethod("GetRayIntersectionAll_Binding")]
	private static RaycastHit2D[] GetRayIntersectionAll_Internal(PhysicsScene2D physicsScene, Vector3 origin, Vector3 direction, float distance, int layerMask)
	{
		return GetRayIntersectionAll_Internal_Injected(ref physicsScene, ref origin, ref direction, distance, layerMask);
	}

	[ExcludeFromDocs]
	public static int GetRayIntersectionNonAlloc(Ray ray, RaycastHit2D[] results)
	{
		return defaultPhysicsScene.GetRayIntersection(ray, float.PositiveInfinity, results);
	}

	[ExcludeFromDocs]
	public static int GetRayIntersectionNonAlloc(Ray ray, RaycastHit2D[] results, float distance)
	{
		return defaultPhysicsScene.GetRayIntersection(ray, distance, results);
	}

	[RequiredByNativeCode]
	public static int GetRayIntersectionNonAlloc(Ray ray, RaycastHit2D[] results, [DefaultValue("Mathf.Infinity")] float distance, [DefaultValue("DefaultRaycastLayers")] int layerMask)
	{
		return defaultPhysicsScene.GetRayIntersection(ray, distance, results, layerMask);
	}

	[ExcludeFromDocs]
	public static Collider2D OverlapPoint(Vector2 point)
	{
		return defaultPhysicsScene.OverlapPoint(point);
	}

	[ExcludeFromDocs]
	public static Collider2D OverlapPoint(Vector2 point, int layerMask)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, float.NegativeInfinity, float.PositiveInfinity);
		return defaultPhysicsScene.OverlapPoint(point, contactFilter);
	}

	[ExcludeFromDocs]
	public static Collider2D OverlapPoint(Vector2 point, int layerMask, float minDepth)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, float.PositiveInfinity);
		return defaultPhysicsScene.OverlapPoint(point, contactFilter);
	}

	public static Collider2D OverlapPoint(Vector2 point, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
		return defaultPhysicsScene.OverlapPoint(point, contactFilter);
	}

	public static int OverlapPoint(Vector2 point, ContactFilter2D contactFilter, Collider2D[] results)
	{
		return defaultPhysicsScene.OverlapPoint(point, contactFilter, results);
	}

	public static int OverlapPoint(Vector2 point, ContactFilter2D contactFilter, List<Collider2D> results)
	{
		return defaultPhysicsScene.OverlapPoint(point, contactFilter, results);
	}

	[ExcludeFromDocs]
	public static Collider2D[] OverlapPointAll(Vector2 point)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(-5, float.NegativeInfinity, float.PositiveInfinity);
		return OverlapPointAll_Internal(defaultPhysicsScene, point, contactFilter);
	}

	[ExcludeFromDocs]
	public static Collider2D[] OverlapPointAll(Vector2 point, int layerMask)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, float.NegativeInfinity, float.PositiveInfinity);
		return OverlapPointAll_Internal(defaultPhysicsScene, point, contactFilter);
	}

	[ExcludeFromDocs]
	public static Collider2D[] OverlapPointAll(Vector2 point, int layerMask, float minDepth)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, float.PositiveInfinity);
		return OverlapPointAll_Internal(defaultPhysicsScene, point, contactFilter);
	}

	public static Collider2D[] OverlapPointAll(Vector2 point, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
		return OverlapPointAll_Internal(defaultPhysicsScene, point, contactFilter);
	}

	[StaticAccessor("PhysicsQuery2D", StaticAccessorType.DoubleColon)]
	[NativeMethod("OverlapPointAll_Binding")]
	private static Collider2D[] OverlapPointAll_Internal(PhysicsScene2D physicsScene, Vector2 point, ContactFilter2D contactFilter)
	{
		return OverlapPointAll_Internal_Injected(ref physicsScene, ref point, ref contactFilter);
	}

	[ExcludeFromDocs]
	public static int OverlapPointNonAlloc(Vector2 point, Collider2D[] results)
	{
		return defaultPhysicsScene.OverlapPoint(point, results);
	}

	[ExcludeFromDocs]
	public static int OverlapPointNonAlloc(Vector2 point, Collider2D[] results, int layerMask)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, float.NegativeInfinity, float.PositiveInfinity);
		return defaultPhysicsScene.OverlapPoint(point, contactFilter, results);
	}

	[ExcludeFromDocs]
	public static int OverlapPointNonAlloc(Vector2 point, Collider2D[] results, int layerMask, float minDepth)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, float.PositiveInfinity);
		return defaultPhysicsScene.OverlapPoint(point, contactFilter, results);
	}

	public static int OverlapPointNonAlloc(Vector2 point, Collider2D[] results, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
		return defaultPhysicsScene.OverlapPoint(point, contactFilter, results);
	}

	[ExcludeFromDocs]
	public static Collider2D OverlapCircle(Vector2 point, float radius)
	{
		return defaultPhysicsScene.OverlapCircle(point, radius);
	}

	[ExcludeFromDocs]
	public static Collider2D OverlapCircle(Vector2 point, float radius, int layerMask)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, float.NegativeInfinity, float.PositiveInfinity);
		return defaultPhysicsScene.OverlapCircle(point, radius, contactFilter);
	}

	[ExcludeFromDocs]
	public static Collider2D OverlapCircle(Vector2 point, float radius, int layerMask, float minDepth)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, float.PositiveInfinity);
		return defaultPhysicsScene.OverlapCircle(point, radius, contactFilter);
	}

	public static Collider2D OverlapCircle(Vector2 point, float radius, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
		return defaultPhysicsScene.OverlapCircle(point, radius, contactFilter);
	}

	public static int OverlapCircle(Vector2 point, float radius, ContactFilter2D contactFilter, Collider2D[] results)
	{
		return defaultPhysicsScene.OverlapCircle(point, radius, contactFilter, results);
	}

	public static int OverlapCircle(Vector2 point, float radius, ContactFilter2D contactFilter, List<Collider2D> results)
	{
		return defaultPhysicsScene.OverlapCircle(point, radius, contactFilter, results);
	}

	[ExcludeFromDocs]
	public static Collider2D[] OverlapCircleAll(Vector2 point, float radius)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(-5, float.NegativeInfinity, float.PositiveInfinity);
		return OverlapCircleAll_Internal(defaultPhysicsScene, point, radius, contactFilter);
	}

	[ExcludeFromDocs]
	public static Collider2D[] OverlapCircleAll(Vector2 point, float radius, int layerMask)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, float.NegativeInfinity, float.PositiveInfinity);
		return OverlapCircleAll_Internal(defaultPhysicsScene, point, radius, contactFilter);
	}

	[ExcludeFromDocs]
	public static Collider2D[] OverlapCircleAll(Vector2 point, float radius, int layerMask, float minDepth)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, float.PositiveInfinity);
		return OverlapCircleAll_Internal(defaultPhysicsScene, point, radius, contactFilter);
	}

	public static Collider2D[] OverlapCircleAll(Vector2 point, float radius, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
		return OverlapCircleAll_Internal(defaultPhysicsScene, point, radius, contactFilter);
	}

	[StaticAccessor("PhysicsQuery2D", StaticAccessorType.DoubleColon)]
	[NativeMethod("OverlapCircleAll_Binding")]
	private static Collider2D[] OverlapCircleAll_Internal(PhysicsScene2D physicsScene, Vector2 point, float radius, ContactFilter2D contactFilter)
	{
		return OverlapCircleAll_Internal_Injected(ref physicsScene, ref point, radius, ref contactFilter);
	}

	[ExcludeFromDocs]
	public static int OverlapCircleNonAlloc(Vector2 point, float radius, Collider2D[] results)
	{
		return defaultPhysicsScene.OverlapCircle(point, radius, results);
	}

	[ExcludeFromDocs]
	public static int OverlapCircleNonAlloc(Vector2 point, float radius, Collider2D[] results, int layerMask)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, float.NegativeInfinity, float.PositiveInfinity);
		return defaultPhysicsScene.OverlapCircle(point, radius, contactFilter, results);
	}

	[ExcludeFromDocs]
	public static int OverlapCircleNonAlloc(Vector2 point, float radius, Collider2D[] results, int layerMask, float minDepth)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, float.PositiveInfinity);
		return defaultPhysicsScene.OverlapCircle(point, radius, contactFilter, results);
	}

	public static int OverlapCircleNonAlloc(Vector2 point, float radius, Collider2D[] results, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
		return defaultPhysicsScene.OverlapCircle(point, radius, contactFilter, results);
	}

	[ExcludeFromDocs]
	public static Collider2D OverlapBox(Vector2 point, Vector2 size, float angle)
	{
		return defaultPhysicsScene.OverlapBox(point, size, angle);
	}

	[ExcludeFromDocs]
	public static Collider2D OverlapBox(Vector2 point, Vector2 size, float angle, int layerMask)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, float.NegativeInfinity, float.PositiveInfinity);
		return defaultPhysicsScene.OverlapBox(point, size, angle, contactFilter);
	}

	[ExcludeFromDocs]
	public static Collider2D OverlapBox(Vector2 point, Vector2 size, float angle, int layerMask, float minDepth)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, float.PositiveInfinity);
		return defaultPhysicsScene.OverlapBox(point, size, angle, contactFilter);
	}

	public static Collider2D OverlapBox(Vector2 point, Vector2 size, float angle, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
		return defaultPhysicsScene.OverlapBox(point, size, angle, contactFilter);
	}

	public static int OverlapBox(Vector2 point, Vector2 size, float angle, ContactFilter2D contactFilter, Collider2D[] results)
	{
		return defaultPhysicsScene.OverlapBox(point, size, angle, contactFilter, results);
	}

	public static int OverlapBox(Vector2 point, Vector2 size, float angle, ContactFilter2D contactFilter, List<Collider2D> results)
	{
		return defaultPhysicsScene.OverlapBox(point, size, angle, contactFilter, results);
	}

	[ExcludeFromDocs]
	public static Collider2D[] OverlapBoxAll(Vector2 point, Vector2 size, float angle)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(-5, float.NegativeInfinity, float.PositiveInfinity);
		return OverlapBoxAll_Internal(defaultPhysicsScene, point, size, angle, contactFilter);
	}

	[ExcludeFromDocs]
	public static Collider2D[] OverlapBoxAll(Vector2 point, Vector2 size, float angle, int layerMask)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, float.NegativeInfinity, float.PositiveInfinity);
		return OverlapBoxAll_Internal(defaultPhysicsScene, point, size, angle, contactFilter);
	}

	[ExcludeFromDocs]
	public static Collider2D[] OverlapBoxAll(Vector2 point, Vector2 size, float angle, int layerMask, float minDepth)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, float.PositiveInfinity);
		return OverlapBoxAll_Internal(defaultPhysicsScene, point, size, angle, contactFilter);
	}

	public static Collider2D[] OverlapBoxAll(Vector2 point, Vector2 size, float angle, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
		return OverlapBoxAll_Internal(defaultPhysicsScene, point, size, angle, contactFilter);
	}

	[StaticAccessor("PhysicsQuery2D", StaticAccessorType.DoubleColon)]
	[NativeMethod("OverlapBoxAll_Binding")]
	private static Collider2D[] OverlapBoxAll_Internal(PhysicsScene2D physicsScene, Vector2 point, Vector2 size, float angle, ContactFilter2D contactFilter)
	{
		return OverlapBoxAll_Internal_Injected(ref physicsScene, ref point, ref size, angle, ref contactFilter);
	}

	[ExcludeFromDocs]
	public static int OverlapBoxNonAlloc(Vector2 point, Vector2 size, float angle, Collider2D[] results)
	{
		return defaultPhysicsScene.OverlapBox(point, size, angle, results);
	}

	[ExcludeFromDocs]
	public static int OverlapBoxNonAlloc(Vector2 point, Vector2 size, float angle, Collider2D[] results, int layerMask)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, float.NegativeInfinity, float.PositiveInfinity);
		return defaultPhysicsScene.OverlapBox(point, size, angle, contactFilter, results);
	}

	[ExcludeFromDocs]
	public static int OverlapBoxNonAlloc(Vector2 point, Vector2 size, float angle, Collider2D[] results, int layerMask, float minDepth)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, float.PositiveInfinity);
		return defaultPhysicsScene.OverlapBox(point, size, angle, contactFilter, results);
	}

	public static int OverlapBoxNonAlloc(Vector2 point, Vector2 size, float angle, Collider2D[] results, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
		return defaultPhysicsScene.OverlapBox(point, size, angle, contactFilter, results);
	}

	[ExcludeFromDocs]
	public static Collider2D OverlapArea(Vector2 pointA, Vector2 pointB)
	{
		return defaultPhysicsScene.OverlapArea(pointA, pointB);
	}

	[ExcludeFromDocs]
	public static Collider2D OverlapArea(Vector2 pointA, Vector2 pointB, int layerMask)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, float.NegativeInfinity, float.PositiveInfinity);
		return defaultPhysicsScene.OverlapArea(pointA, pointB, contactFilter);
	}

	[ExcludeFromDocs]
	public static Collider2D OverlapArea(Vector2 pointA, Vector2 pointB, int layerMask, float minDepth)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, float.PositiveInfinity);
		return defaultPhysicsScene.OverlapArea(pointA, pointB, contactFilter);
	}

	public static Collider2D OverlapArea(Vector2 pointA, Vector2 pointB, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
		return defaultPhysicsScene.OverlapArea(pointA, pointB, contactFilter);
	}

	public static int OverlapArea(Vector2 pointA, Vector2 pointB, ContactFilter2D contactFilter, Collider2D[] results)
	{
		return defaultPhysicsScene.OverlapArea(pointA, pointB, contactFilter, results);
	}

	public static int OverlapArea(Vector2 pointA, Vector2 pointB, ContactFilter2D contactFilter, List<Collider2D> results)
	{
		return defaultPhysicsScene.OverlapArea(pointA, pointB, contactFilter, results);
	}

	[ExcludeFromDocs]
	public static Collider2D[] OverlapAreaAll(Vector2 pointA, Vector2 pointB)
	{
		return OverlapAreaAllToBox_Internal(pointA, pointB, -5, float.NegativeInfinity, float.PositiveInfinity);
	}

	[ExcludeFromDocs]
	public static Collider2D[] OverlapAreaAll(Vector2 pointA, Vector2 pointB, int layerMask)
	{
		return OverlapAreaAllToBox_Internal(pointA, pointB, layerMask, float.NegativeInfinity, float.PositiveInfinity);
	}

	[ExcludeFromDocs]
	public static Collider2D[] OverlapAreaAll(Vector2 pointA, Vector2 pointB, int layerMask, float minDepth)
	{
		return OverlapAreaAllToBox_Internal(pointA, pointB, layerMask, minDepth, float.PositiveInfinity);
	}

	public static Collider2D[] OverlapAreaAll(Vector2 pointA, Vector2 pointB, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
	{
		return OverlapAreaAllToBox_Internal(pointA, pointB, layerMask, minDepth, maxDepth);
	}

	private static Collider2D[] OverlapAreaAllToBox_Internal(Vector2 pointA, Vector2 pointB, int layerMask, float minDepth, float maxDepth)
	{
		Vector2 point = (pointA + pointB) * 0.5f;
		Vector2 size = new Vector2(Mathf.Abs(pointA.x - pointB.x), Math.Abs(pointA.y - pointB.y));
		return OverlapBoxAll(point, size, 0f, layerMask, minDepth, maxDepth);
	}

	[ExcludeFromDocs]
	public static int OverlapAreaNonAlloc(Vector2 pointA, Vector2 pointB, Collider2D[] results)
	{
		return defaultPhysicsScene.OverlapArea(pointA, pointB, results);
	}

	[ExcludeFromDocs]
	public static int OverlapAreaNonAlloc(Vector2 pointA, Vector2 pointB, Collider2D[] results, int layerMask)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, float.NegativeInfinity, float.PositiveInfinity);
		return defaultPhysicsScene.OverlapArea(pointA, pointB, contactFilter, results);
	}

	[ExcludeFromDocs]
	public static int OverlapAreaNonAlloc(Vector2 pointA, Vector2 pointB, Collider2D[] results, int layerMask, float minDepth)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, float.PositiveInfinity);
		return defaultPhysicsScene.OverlapArea(pointA, pointB, contactFilter, results);
	}

	public static int OverlapAreaNonAlloc(Vector2 pointA, Vector2 pointB, Collider2D[] results, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
		return defaultPhysicsScene.OverlapArea(pointA, pointB, contactFilter, results);
	}

	[ExcludeFromDocs]
	public static Collider2D OverlapCapsule(Vector2 point, Vector2 size, CapsuleDirection2D direction, float angle)
	{
		return defaultPhysicsScene.OverlapCapsule(point, size, direction, angle);
	}

	[ExcludeFromDocs]
	public static Collider2D OverlapCapsule(Vector2 point, Vector2 size, CapsuleDirection2D direction, float angle, int layerMask)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, float.NegativeInfinity, float.PositiveInfinity);
		return defaultPhysicsScene.OverlapCapsule(point, size, direction, angle, contactFilter);
	}

	[ExcludeFromDocs]
	public static Collider2D OverlapCapsule(Vector2 point, Vector2 size, CapsuleDirection2D direction, float angle, int layerMask, float minDepth)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, float.PositiveInfinity);
		return defaultPhysicsScene.OverlapCapsule(point, size, direction, angle, contactFilter);
	}

	public static Collider2D OverlapCapsule(Vector2 point, Vector2 size, CapsuleDirection2D direction, float angle, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
		return defaultPhysicsScene.OverlapCapsule(point, size, direction, angle, contactFilter);
	}

	public static int OverlapCapsule(Vector2 point, Vector2 size, CapsuleDirection2D direction, float angle, ContactFilter2D contactFilter, Collider2D[] results)
	{
		return defaultPhysicsScene.OverlapCapsule(point, size, direction, angle, contactFilter, results);
	}

	public static int OverlapCapsule(Vector2 point, Vector2 size, CapsuleDirection2D direction, float angle, ContactFilter2D contactFilter, List<Collider2D> results)
	{
		return defaultPhysicsScene.OverlapCapsule(point, size, direction, angle, contactFilter, results);
	}

	[ExcludeFromDocs]
	public static Collider2D[] OverlapCapsuleAll(Vector2 point, Vector2 size, CapsuleDirection2D direction, float angle)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(-5, float.NegativeInfinity, float.PositiveInfinity);
		return OverlapCapsuleAll_Internal(defaultPhysicsScene, point, size, direction, angle, contactFilter);
	}

	[ExcludeFromDocs]
	public static Collider2D[] OverlapCapsuleAll(Vector2 point, Vector2 size, CapsuleDirection2D direction, float angle, int layerMask)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, float.NegativeInfinity, float.PositiveInfinity);
		return OverlapCapsuleAll_Internal(defaultPhysicsScene, point, size, direction, angle, contactFilter);
	}

	[ExcludeFromDocs]
	public static Collider2D[] OverlapCapsuleAll(Vector2 point, Vector2 size, CapsuleDirection2D direction, float angle, int layerMask, float minDepth)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, float.PositiveInfinity);
		return OverlapCapsuleAll_Internal(defaultPhysicsScene, point, size, direction, angle, contactFilter);
	}

	public static Collider2D[] OverlapCapsuleAll(Vector2 point, Vector2 size, CapsuleDirection2D direction, float angle, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
		return OverlapCapsuleAll_Internal(defaultPhysicsScene, point, size, direction, angle, contactFilter);
	}

	[NativeMethod("OverlapCapsuleAll_Binding")]
	[StaticAccessor("PhysicsQuery2D", StaticAccessorType.DoubleColon)]
	private static Collider2D[] OverlapCapsuleAll_Internal(PhysicsScene2D physicsScene, Vector2 point, Vector2 size, CapsuleDirection2D direction, float angle, ContactFilter2D contactFilter)
	{
		return OverlapCapsuleAll_Internal_Injected(ref physicsScene, ref point, ref size, direction, angle, ref contactFilter);
	}

	[ExcludeFromDocs]
	public static int OverlapCapsuleNonAlloc(Vector2 point, Vector2 size, CapsuleDirection2D direction, float angle, Collider2D[] results)
	{
		return defaultPhysicsScene.OverlapCapsule(point, size, direction, angle, results);
	}

	[ExcludeFromDocs]
	public static int OverlapCapsuleNonAlloc(Vector2 point, Vector2 size, CapsuleDirection2D direction, float angle, Collider2D[] results, int layerMask)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, float.NegativeInfinity, float.PositiveInfinity);
		return defaultPhysicsScene.OverlapCapsule(point, size, direction, angle, contactFilter, results);
	}

	[ExcludeFromDocs]
	public static int OverlapCapsuleNonAlloc(Vector2 point, Vector2 size, CapsuleDirection2D direction, float angle, Collider2D[] results, int layerMask, float minDepth)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, float.PositiveInfinity);
		return defaultPhysicsScene.OverlapCapsule(point, size, direction, angle, contactFilter, results);
	}

	public static int OverlapCapsuleNonAlloc(Vector2 point, Vector2 size, CapsuleDirection2D direction, float angle, Collider2D[] results, [DefaultValue("DefaultRaycastLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
	{
		ContactFilter2D contactFilter = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
		return defaultPhysicsScene.OverlapCapsule(point, size, direction, angle, contactFilter, results);
	}

	public static int OverlapCollider(Collider2D collider, ContactFilter2D contactFilter, Collider2D[] results)
	{
		return PhysicsScene2D.OverlapCollider(collider, contactFilter, results);
	}

	public static int OverlapCollider(Collider2D collider, ContactFilter2D contactFilter, List<Collider2D> results)
	{
		return PhysicsScene2D.OverlapCollider(collider, contactFilter, results);
	}

	public static int GetContacts(Collider2D collider1, Collider2D collider2, ContactFilter2D contactFilter, ContactPoint2D[] contacts)
	{
		return GetColliderColliderContactsArray(collider1, collider2, contactFilter, contacts);
	}

	public static int GetContacts(Collider2D collider, ContactPoint2D[] contacts)
	{
		return GetColliderContactsArray(collider, default(ContactFilter2D).NoFilter(), contacts);
	}

	public static int GetContacts(Collider2D collider, ContactFilter2D contactFilter, ContactPoint2D[] contacts)
	{
		return GetColliderContactsArray(collider, contactFilter, contacts);
	}

	public static int GetContacts(Collider2D collider, Collider2D[] colliders)
	{
		return GetColliderContactsCollidersOnlyArray(collider, default(ContactFilter2D).NoFilter(), colliders);
	}

	public static int GetContacts(Collider2D collider, ContactFilter2D contactFilter, Collider2D[] colliders)
	{
		return GetColliderContactsCollidersOnlyArray(collider, contactFilter, colliders);
	}

	public static int GetContacts(Rigidbody2D rigidbody, ContactPoint2D[] contacts)
	{
		return GetRigidbodyContactsArray(rigidbody, default(ContactFilter2D).NoFilter(), contacts);
	}

	public static int GetContacts(Rigidbody2D rigidbody, ContactFilter2D contactFilter, ContactPoint2D[] contacts)
	{
		return GetRigidbodyContactsArray(rigidbody, contactFilter, contacts);
	}

	public static int GetContacts(Rigidbody2D rigidbody, Collider2D[] colliders)
	{
		return GetRigidbodyContactsCollidersOnlyArray(rigidbody, default(ContactFilter2D).NoFilter(), colliders);
	}

	public static int GetContacts(Rigidbody2D rigidbody, ContactFilter2D contactFilter, Collider2D[] colliders)
	{
		return GetRigidbodyContactsCollidersOnlyArray(rigidbody, contactFilter, colliders);
	}

	[StaticAccessor("PhysicsQuery2D", StaticAccessorType.DoubleColon)]
	[NativeMethod("GetColliderContactsArray_Binding")]
	private static int GetColliderContactsArray([NotNull] Collider2D collider, ContactFilter2D contactFilter, [NotNull] ContactPoint2D[] results)
	{
		return GetColliderContactsArray_Injected(collider, ref contactFilter, results);
	}

	[StaticAccessor("PhysicsQuery2D", StaticAccessorType.DoubleColon)]
	[NativeMethod("GetColliderColliderContactsArray_Binding")]
	private static int GetColliderColliderContactsArray([NotNull] Collider2D collider1, [NotNull] Collider2D collider2, ContactFilter2D contactFilter, [NotNull] ContactPoint2D[] results)
	{
		return GetColliderColliderContactsArray_Injected(collider1, collider2, ref contactFilter, results);
	}

	[NativeMethod("GetRigidbodyContactsArray_Binding")]
	[StaticAccessor("PhysicsQuery2D", StaticAccessorType.DoubleColon)]
	private static int GetRigidbodyContactsArray([NotNull] Rigidbody2D rigidbody, ContactFilter2D contactFilter, [NotNull] ContactPoint2D[] results)
	{
		return GetRigidbodyContactsArray_Injected(rigidbody, ref contactFilter, results);
	}

	[NativeMethod("GetColliderContactsCollidersOnlyArray_Binding")]
	[StaticAccessor("PhysicsQuery2D", StaticAccessorType.DoubleColon)]
	private static int GetColliderContactsCollidersOnlyArray([NotNull] Collider2D collider, ContactFilter2D contactFilter, [NotNull] Collider2D[] results)
	{
		return GetColliderContactsCollidersOnlyArray_Injected(collider, ref contactFilter, results);
	}

	[StaticAccessor("PhysicsQuery2D", StaticAccessorType.DoubleColon)]
	[NativeMethod("GetRigidbodyContactsCollidersOnlyArray_Binding")]
	private static int GetRigidbodyContactsCollidersOnlyArray([NotNull] Rigidbody2D rigidbody, ContactFilter2D contactFilter, [NotNull] Collider2D[] results)
	{
		return GetRigidbodyContactsCollidersOnlyArray_Injected(rigidbody, ref contactFilter, results);
	}

	public static int GetContacts(Collider2D collider1, Collider2D collider2, ContactFilter2D contactFilter, List<ContactPoint2D> contacts)
	{
		return GetColliderColliderContactsList(collider1, collider2, contactFilter, contacts);
	}

	public static int GetContacts(Collider2D collider, List<ContactPoint2D> contacts)
	{
		return GetColliderContactsList(collider, default(ContactFilter2D).NoFilter(), contacts);
	}

	public static int GetContacts(Collider2D collider, ContactFilter2D contactFilter, List<ContactPoint2D> contacts)
	{
		return GetColliderContactsList(collider, contactFilter, contacts);
	}

	public static int GetContacts(Collider2D collider, List<Collider2D> colliders)
	{
		return GetColliderContactsCollidersOnlyList(collider, default(ContactFilter2D).NoFilter(), colliders);
	}

	public static int GetContacts(Collider2D collider, ContactFilter2D contactFilter, List<Collider2D> colliders)
	{
		return GetColliderContactsCollidersOnlyList(collider, contactFilter, colliders);
	}

	public static int GetContacts(Rigidbody2D rigidbody, List<ContactPoint2D> contacts)
	{
		return GetRigidbodyContactsList(rigidbody, default(ContactFilter2D).NoFilter(), contacts);
	}

	public static int GetContacts(Rigidbody2D rigidbody, ContactFilter2D contactFilter, List<ContactPoint2D> contacts)
	{
		return GetRigidbodyContactsList(rigidbody, contactFilter, contacts);
	}

	public static int GetContacts(Rigidbody2D rigidbody, List<Collider2D> colliders)
	{
		return GetRigidbodyContactsCollidersOnlyList(rigidbody, default(ContactFilter2D).NoFilter(), colliders);
	}

	public static int GetContacts(Rigidbody2D rigidbody, ContactFilter2D contactFilter, List<Collider2D> colliders)
	{
		return GetRigidbodyContactsCollidersOnlyList(rigidbody, contactFilter, colliders);
	}

	[NativeMethod("GetColliderContactsList_Binding")]
	[StaticAccessor("PhysicsQuery2D", StaticAccessorType.DoubleColon)]
	private static int GetColliderContactsList([NotNull] Collider2D collider, ContactFilter2D contactFilter, [NotNull] List<ContactPoint2D> results)
	{
		return GetColliderContactsList_Injected(collider, ref contactFilter, results);
	}

	[NativeMethod("GetColliderColliderContactsList_Binding")]
	[StaticAccessor("PhysicsQuery2D", StaticAccessorType.DoubleColon)]
	private static int GetColliderColliderContactsList([NotNull] Collider2D collider1, [NotNull] Collider2D collider2, ContactFilter2D contactFilter, [NotNull] List<ContactPoint2D> results)
	{
		return GetColliderColliderContactsList_Injected(collider1, collider2, ref contactFilter, results);
	}

	[NativeMethod("GetRigidbodyContactsList_Binding")]
	[StaticAccessor("PhysicsQuery2D", StaticAccessorType.DoubleColon)]
	private static int GetRigidbodyContactsList([NotNull] Rigidbody2D rigidbody, ContactFilter2D contactFilter, [NotNull] List<ContactPoint2D> results)
	{
		return GetRigidbodyContactsList_Injected(rigidbody, ref contactFilter, results);
	}

	[NativeMethod("GetColliderContactsCollidersOnlyList_Binding")]
	[StaticAccessor("PhysicsQuery2D", StaticAccessorType.DoubleColon)]
	private static int GetColliderContactsCollidersOnlyList([NotNull] Collider2D collider, ContactFilter2D contactFilter, [NotNull] List<Collider2D> results)
	{
		return GetColliderContactsCollidersOnlyList_Injected(collider, ref contactFilter, results);
	}

	[StaticAccessor("PhysicsQuery2D", StaticAccessorType.DoubleColon)]
	[NativeMethod("GetRigidbodyContactsCollidersOnlyList_Binding")]
	private static int GetRigidbodyContactsCollidersOnlyList([NotNull] Rigidbody2D rigidbody, ContactFilter2D contactFilter, [NotNull] List<Collider2D> results)
	{
		return GetRigidbodyContactsCollidersOnlyList_Injected(rigidbody, ref contactFilter, results);
	}

	internal static void SetEditorDragMovement(bool dragging, GameObject[] objs)
	{
		foreach (Rigidbody2D item in m_LastDisabledRigidbody2D)
		{
			if (item != null)
			{
				item.SetDragBehaviour(dragged: false);
			}
		}
		m_LastDisabledRigidbody2D.Clear();
		if (!dragging)
		{
			return;
		}
		foreach (GameObject gameObject in objs)
		{
			Rigidbody2D[] componentsInChildren = gameObject.GetComponentsInChildren<Rigidbody2D>(includeInactive: false);
			Rigidbody2D[] array = componentsInChildren;
			foreach (Rigidbody2D rigidbody2D in array)
			{
				m_LastDisabledRigidbody2D.Add(rigidbody2D);
				rigidbody2D.SetDragBehaviour(dragged: true);
			}
		}
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private static extern void get_gravity_Injected(out Vector2 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private static extern void set_gravity_Injected(ref Vector2 value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private static extern void get_jobOptions_Injected(out PhysicsJobOptions2D ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private static extern void set_jobOptions_Injected(ref PhysicsJobOptions2D value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private static extern void get_colliderAwakeColor_Injected(out Color ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private static extern void set_colliderAwakeColor_Injected(ref Color value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private static extern void get_colliderAsleepColor_Injected(out Color ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private static extern void set_colliderAsleepColor_Injected(ref Color value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private static extern void get_colliderContactColor_Injected(out Color ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private static extern void set_colliderContactColor_Injected(ref Color value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private static extern void get_colliderAABBColor_Injected(out Color ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private static extern void set_colliderAABBColor_Injected(ref Color value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern bool Simulate_Internal_Injected(ref PhysicsScene2D physicsScene, float step);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern bool IsTouching_TwoCollidersWithFilter_Injected([Writable] Collider2D collider1, [Writable] Collider2D collider2, ref ContactFilter2D contactFilter);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern bool IsTouching_SingleColliderWithFilter_Injected([Writable] Collider2D collider, ref ContactFilter2D contactFilter);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void Distance_Internal_Injected([Writable] Collider2D colliderA, [Writable] Collider2D colliderB, out ColliderDistance2D ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void ClosestPoint_Collider_Injected(ref Vector2 position, Collider2D collider, out Vector2 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void ClosestPoint_Rigidbody_Injected(ref Vector2 position, Rigidbody2D rigidbody, out Vector2 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern RaycastHit2D[] LinecastAll_Internal_Injected(ref PhysicsScene2D physicsScene, ref Vector2 start, ref Vector2 end, ref ContactFilter2D contactFilter);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern RaycastHit2D[] RaycastAll_Internal_Injected(ref PhysicsScene2D physicsScene, ref Vector2 origin, ref Vector2 direction, float distance, ref ContactFilter2D contactFilter);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern RaycastHit2D[] CircleCastAll_Internal_Injected(ref PhysicsScene2D physicsScene, ref Vector2 origin, float radius, ref Vector2 direction, float distance, ref ContactFilter2D contactFilter);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern RaycastHit2D[] BoxCastAll_Internal_Injected(ref PhysicsScene2D physicsScene, ref Vector2 origin, ref Vector2 size, float angle, ref Vector2 direction, float distance, ref ContactFilter2D contactFilter);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern RaycastHit2D[] CapsuleCastAll_Internal_Injected(ref PhysicsScene2D physicsScene, ref Vector2 origin, ref Vector2 size, CapsuleDirection2D capsuleDirection, float angle, ref Vector2 direction, float distance, ref ContactFilter2D contactFilter);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern RaycastHit2D[] GetRayIntersectionAll_Internal_Injected(ref PhysicsScene2D physicsScene, ref Vector3 origin, ref Vector3 direction, float distance, int layerMask);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern Collider2D[] OverlapPointAll_Internal_Injected(ref PhysicsScene2D physicsScene, ref Vector2 point, ref ContactFilter2D contactFilter);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern Collider2D[] OverlapCircleAll_Internal_Injected(ref PhysicsScene2D physicsScene, ref Vector2 point, float radius, ref ContactFilter2D contactFilter);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern Collider2D[] OverlapBoxAll_Internal_Injected(ref PhysicsScene2D physicsScene, ref Vector2 point, ref Vector2 size, float angle, ref ContactFilter2D contactFilter);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern Collider2D[] OverlapCapsuleAll_Internal_Injected(ref PhysicsScene2D physicsScene, ref Vector2 point, ref Vector2 size, CapsuleDirection2D direction, float angle, ref ContactFilter2D contactFilter);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern int GetColliderContactsArray_Injected(Collider2D collider, ref ContactFilter2D contactFilter, ContactPoint2D[] results);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern int GetColliderColliderContactsArray_Injected(Collider2D collider1, Collider2D collider2, ref ContactFilter2D contactFilter, ContactPoint2D[] results);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern int GetRigidbodyContactsArray_Injected(Rigidbody2D rigidbody, ref ContactFilter2D contactFilter, ContactPoint2D[] results);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern int GetColliderContactsCollidersOnlyArray_Injected(Collider2D collider, ref ContactFilter2D contactFilter, Collider2D[] results);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern int GetRigidbodyContactsCollidersOnlyArray_Injected(Rigidbody2D rigidbody, ref ContactFilter2D contactFilter, Collider2D[] results);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern int GetColliderContactsList_Injected(Collider2D collider, ref ContactFilter2D contactFilter, List<ContactPoint2D> results);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern int GetColliderColliderContactsList_Injected(Collider2D collider1, Collider2D collider2, ref ContactFilter2D contactFilter, List<ContactPoint2D> results);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern int GetRigidbodyContactsList_Injected(Rigidbody2D rigidbody, ref ContactFilter2D contactFilter, List<ContactPoint2D> results);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern int GetColliderContactsCollidersOnlyList_Injected(Collider2D collider, ref ContactFilter2D contactFilter, List<Collider2D> results);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern int GetRigidbodyContactsCollidersOnlyList_Injected(Rigidbody2D rigidbody, ref ContactFilter2D contactFilter, List<Collider2D> results);
}
