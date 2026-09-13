using Cinemachine.Utility;
using UnityEngine;

namespace Cinemachine;

[DocumentationSorting(DocumentationSortingAttribute.Level.UserRef)]
[SaveDuringPlay]
public class CinemachineCollisionImpulseSource : CinemachineImpulseSource
{
	[Header("Trigger Object Filter")]
	[Tooltip("Only collisions with objects on these layers will generate Impulse events")]
	public LayerMask m_LayerMask = 1;

	[TagField]
	[Tooltip("No Impulse evemts will be generated for collisions with objects having these tags")]
	public string m_IgnoreTag = string.Empty;

	[Header("How To Generate The Impulse")]
	[Tooltip("If checked, signal direction will be affected by the direction of impact")]
	public bool m_UseImpactDirection;

	[Tooltip("If checked, signal amplitude will be multiplied by the mass of the impacting object")]
	public bool m_ScaleImpactWithMass;

	[Tooltip("If checked, signal amplitude will be multiplied by the speed of the impacting object")]
	public bool m_ScaleImpactWithSpeed;

	private Rigidbody mRigidBody;

	private Rigidbody2D mRigidBody2D;

	private void Start()
	{
		mRigidBody = GetComponent<Rigidbody>();
		mRigidBody2D = GetComponent<Rigidbody2D>();
	}

	private void OnEnable()
	{
	}

	private void OnCollisionEnter(Collision c)
	{
		GenerateImpactEvent(c.collider, null, c.relativeVelocity);
	}

	private void OnTriggerEnter(Collider c)
	{
		GenerateImpactEvent(c, null, Vector3.zero);
	}

	private void OnCollisionEnter2D(Collision2D c)
	{
		GenerateImpactEvent(null, c.collider, c.relativeVelocity);
	}

	private void OnTriggerEnter2D(Collider2D c)
	{
		GenerateImpactEvent(null, c, Vector3.zero);
	}

	private float GetMassAndVelocity(Collider other, Collider2D other2d, ref Vector3 vel)
	{
		bool flag = vel == Vector3.zero;
		float num = 1f;
		if (m_ScaleImpactWithMass || m_ScaleImpactWithSpeed || m_UseImpactDirection)
		{
			if (mRigidBody != null)
			{
				if (m_ScaleImpactWithMass)
				{
					num *= mRigidBody.mass;
				}
				if (flag)
				{
					vel = -mRigidBody.velocity;
				}
			}
			else if (mRigidBody2D != null)
			{
				if (m_ScaleImpactWithMass)
				{
					num *= mRigidBody2D.mass;
				}
				if (flag)
				{
					vel = -mRigidBody2D.velocity;
				}
			}
			Rigidbody rigidbody = ((other != null) ? other.attachedRigidbody : null);
			if (rigidbody != null)
			{
				if (m_ScaleImpactWithMass)
				{
					num *= rigidbody.mass;
				}
				if (flag)
				{
					vel += rigidbody.velocity;
				}
			}
			Rigidbody2D rigidbody2D = ((other2d != null) ? other2d.attachedRigidbody : null);
			if (rigidbody2D != null)
			{
				if (m_ScaleImpactWithMass)
				{
					num *= rigidbody2D.mass;
				}
				if (flag)
				{
					Vector3 vector = rigidbody2D.velocity;
					vel += vector;
				}
			}
		}
		return num;
	}

	private void GenerateImpactEvent(Collider other, Collider2D other2d, Vector3 vel)
	{
		if (!base.enabled)
		{
			return;
		}
		if (other != null)
		{
			int layer = other.gameObject.layer;
			if (((1 << layer) & (int)m_LayerMask) == 0 || (m_IgnoreTag.Length != 0 && other.CompareTag(m_IgnoreTag)))
			{
				return;
			}
		}
		else if (other2d != null)
		{
			int layer2 = other2d.gameObject.layer;
			if (((1 << layer2) & (int)m_LayerMask) == 0 || (m_IgnoreTag.Length != 0 && other2d.CompareTag(m_IgnoreTag)))
			{
				return;
			}
		}
		float num = GetMassAndVelocity(other, other2d, ref vel);
		if (m_ScaleImpactWithSpeed)
		{
			num *= vel.magnitude;
		}
		Vector3 vector = Vector3.down;
		if (m_UseImpactDirection && !vel.AlmostZero())
		{
			vector = -vel.normalized;
		}
		GenerateImpulse(vector * num);
	}
}
