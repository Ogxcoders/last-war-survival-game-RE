using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.SceneManagement;

namespace UnityEngine;

public static class PhysicsSceneExtensions2D
{
	public static PhysicsScene2D GetPhysicsScene2D(this Scene scene)
	{
		if (!scene.IsValid())
		{
			throw new ArgumentException("Cannot get physics scene; Unity scene is invalid.", "scene");
		}
		PhysicsScene2D physicsScene_Internal = GetPhysicsScene_Internal(scene);
		if (physicsScene_Internal.IsValid())
		{
			return physicsScene_Internal;
		}
		throw new Exception("The physics scene associated with the Unity scene is invalid.");
	}

	[NativeMethod("GetPhysicsSceneFromUnityScene")]
	[StaticAccessor("GetPhysicsManager2D()", StaticAccessorType.Arrow)]
	private static PhysicsScene2D GetPhysicsScene_Internal(Scene scene)
	{
		GetPhysicsScene_Internal_Injected(ref scene, out var ret);
		return ret;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void GetPhysicsScene_Internal_Injected(ref Scene scene, out PhysicsScene2D ret);
}
