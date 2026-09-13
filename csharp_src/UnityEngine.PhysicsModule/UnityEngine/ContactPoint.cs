using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine;

[UsedByNativeCode]
[NativeHeader("Modules/Physics/MessageParameters.h")]
public struct ContactPoint
{
	internal Vector3 m_Point;

	internal Vector3 m_Normal;

	internal int m_ThisColliderInstanceID;

	internal int m_OtherColliderInstanceID;

	internal float m_Separation;

	public Vector3 point => m_Point;

	public Vector3 normal => m_Normal;

	public Collider thisCollider => GetColliderByInstanceID(m_ThisColliderInstanceID);

	public Collider otherCollider => GetColliderByInstanceID(m_OtherColliderInstanceID);

	public float separation => m_Separation;

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	private static extern Collider GetColliderByInstanceID(int instanceID);
}
