using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine.Scripting;

namespace UnityEngine;

[StructLayout(LayoutKind.Sequential)]
[RequiredByNativeCode]
public class Collision2D
{
	internal int m_Collider;

	internal int m_OtherCollider;

	internal int m_Rigidbody;

	internal int m_OtherRigidbody;

	internal Vector2 m_RelativeVelocity;

	internal int m_Enabled;

	internal int m_ContactCount;

	internal ContactPoint2D[] m_ReusedContacts;

	internal ContactPoint2D[] m_LegacyContacts;

	public Collider2D collider => Object.FindObjectFromInstanceID(m_Collider) as Collider2D;

	public Collider2D otherCollider => Object.FindObjectFromInstanceID(m_OtherCollider) as Collider2D;

	public Rigidbody2D rigidbody => Object.FindObjectFromInstanceID(m_Rigidbody) as Rigidbody2D;

	public Rigidbody2D otherRigidbody => Object.FindObjectFromInstanceID(m_OtherRigidbody) as Rigidbody2D;

	public Transform transform => (rigidbody != null) ? rigidbody.transform : collider.transform;

	public GameObject gameObject => (rigidbody != null) ? rigidbody.gameObject : collider.gameObject;

	public Vector2 relativeVelocity => m_RelativeVelocity;

	public bool enabled => m_Enabled == 1;

	public ContactPoint2D[] contacts
	{
		get
		{
			if (m_LegacyContacts == null)
			{
				m_LegacyContacts = new ContactPoint2D[m_ContactCount];
				Array.Copy(m_ReusedContacts, m_LegacyContacts, m_ContactCount);
			}
			return m_LegacyContacts;
		}
	}

	public int contactCount => m_ContactCount;

	private ContactPoint2D[] GetContacts_Internal()
	{
		return (m_LegacyContacts == null) ? m_ReusedContacts : m_LegacyContacts;
	}

	public ContactPoint2D GetContact(int index)
	{
		if (index < 0 || index >= m_ContactCount)
		{
			throw new ArgumentOutOfRangeException($"Cannot get contact at index {index}. There are {m_ContactCount} contact(s).");
		}
		return GetContacts_Internal()[index];
	}

	public int GetContacts(ContactPoint2D[] contacts)
	{
		if (contacts == null)
		{
			throw new NullReferenceException("Cannot get contacts as the provided array is NULL.");
		}
		int num = Mathf.Min(m_ContactCount, contacts.Length);
		Array.Copy(GetContacts_Internal(), contacts, num);
		return num;
	}

	public int GetContacts(List<ContactPoint2D> contacts)
	{
		if (contacts == null)
		{
			throw new NullReferenceException("Cannot get contacts as the provided list is NULL.");
		}
		contacts.Clear();
		contacts.AddRange(GetContacts_Internal());
		return contactCount;
	}
}
