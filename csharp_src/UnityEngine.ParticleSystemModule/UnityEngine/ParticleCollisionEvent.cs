using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine;

[RequiredByNativeCode(Optional = true)]
public struct ParticleCollisionEvent
{
	internal Vector3 m_Intersection;

	internal Vector3 m_Normal;

	internal Vector3 m_Velocity;

	internal int m_ColliderInstanceID;

	public Vector3 intersection => m_Intersection;

	public Vector3 normal => m_Normal;

	public Vector3 velocity => m_Velocity;

	public Component colliderComponent => InstanceIDToColliderComponent(m_ColliderInstanceID);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "ParticleSystemScriptBindings::InstanceIDToColliderComponent")]
	private static extern Component InstanceIDToColliderComponent(int instanceID);
}
