using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine.Scripting;

namespace UnityEngine;

[StructLayout(LayoutKind.Sequential)]
[RequiredByNativeCode]
public class Collision
{
	internal Vector3 m_Impulse;

	internal Vector3 m_RelativeVelocity;

	internal Rigidbody m_Rigidbody;

	internal Collider m_Collider;

	internal int m_ContactCount;

	internal ContactPoint[] m_ReusedContacts;

	internal ContactPoint[] m_LegacyContacts;

	public Vector3 relativeVelocity => m_RelativeVelocity;

	public Rigidbody rigidbody => m_Rigidbody;

	public Collider collider => m_Collider;

	public Transform transform => (rigidbody != null) ? rigidbody.transform : collider.transform;

	public GameObject gameObject => (m_Rigidbody != null) ? m_Rigidbody.gameObject : m_Collider.gameObject;

	public int contactCount => m_ContactCount;

	public ContactPoint[] contacts
	{
		get
		{
			if (m_LegacyContacts == null)
			{
				m_LegacyContacts = new ContactPoint[m_ContactCount];
				Array.Copy(m_ReusedContacts, m_LegacyContacts, m_ContactCount);
			}
			return m_LegacyContacts;
		}
	}

	public Vector3 impulse => m_Impulse;

	[Obsolete("Use Collision.relativeVelocity instead.", false)]
	public Vector3 impactForceSum => relativeVelocity;

	[Obsolete("Will always return zero.", false)]
	public Vector3 frictionForceSum => Vector3.zero;

	[Obsolete("Please use Collision.rigidbody, Collision.transform or Collision.collider instead", false)]
	public Component other => (m_Rigidbody != null) ? ((Component)m_Rigidbody) : ((Component)m_Collider);

	private ContactPoint[] GetContacts_Internal()
	{
		return (m_LegacyContacts == null) ? m_ReusedContacts : m_LegacyContacts;
	}

	public ContactPoint GetContact(int index)
	{
		if (index < 0 || index >= m_ContactCount)
		{
			throw new ArgumentOutOfRangeException($"Cannot get contact at index {index}. There are {m_ContactCount} contact(s).");
		}
		return GetContacts_Internal()[index];
	}

	public int GetContacts(ContactPoint[] contacts)
	{
		if (contacts == null)
		{
			throw new NullReferenceException("Cannot get contacts as the provided array is NULL.");
		}
		int num = Mathf.Min(m_ContactCount, contacts.Length);
		Array.Copy(GetContacts_Internal(), contacts, num);
		return num;
	}

	public int GetContacts(List<ContactPoint> contacts)
	{
		if (contacts == null)
		{
			throw new NullReferenceException("Cannot get contacts as the provided list is NULL.");
		}
		contacts.Clear();
		contacts.AddRange(GetContacts_Internal());
		return contactCount;
	}

	[Obsolete("Do not use Collision.GetEnumerator(), enumerate using non-allocating array returned by Collision.GetContacts() or enumerate using Collision.GetContact(index) instead.", false)]
	public virtual IEnumerator GetEnumerator()
	{
		return contacts.GetEnumerator();
	}
}
