using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine;

[RequiredByNativeCode(Optional = false, GenerateProxy = true)]
[NativeClass("ScriptingContactPoint2D", "struct ScriptingContactPoint2D;")]
[NativeHeader("Modules/Physics2D/Public/PhysicsScripting2D.h")]
public struct ContactPoint2D
{
	[NativeName("point")]
	private Vector2 m_Point;

	[NativeName("normal")]
	private Vector2 m_Normal;

	[NativeName("relativeVelocity")]
	private Vector2 m_RelativeVelocity;

	[NativeName("separation")]
	private float m_Separation;

	[NativeName("normalImpulse")]
	private float m_NormalImpulse;

	[NativeName("tangentImpulse")]
	private float m_TangentImpulse;

	[NativeName("collider")]
	private int m_Collider;

	[NativeName("otherCollider")]
	private int m_OtherCollider;

	[NativeName("rigidbody")]
	private int m_Rigidbody;

	[NativeName("otherRigidbody")]
	private int m_OtherRigidbody;

	[NativeName("enabled")]
	private int m_Enabled;

	public Vector2 point => m_Point;

	public Vector2 normal => m_Normal;

	public float separation => m_Separation;

	public float normalImpulse => m_NormalImpulse;

	public float tangentImpulse => m_TangentImpulse;

	public Vector2 relativeVelocity => m_RelativeVelocity;

	public Collider2D collider => Object.FindObjectFromInstanceID(m_Collider) as Collider2D;

	public Collider2D otherCollider => Object.FindObjectFromInstanceID(m_OtherCollider) as Collider2D;

	public Rigidbody2D rigidbody => Object.FindObjectFromInstanceID(m_Rigidbody) as Rigidbody2D;

	public Rigidbody2D otherRigidbody => Object.FindObjectFromInstanceID(m_OtherRigidbody) as Rigidbody2D;

	public bool enabled => m_Enabled == 1;
}
