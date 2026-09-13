using System;

namespace UnityEngine;

[Serializable]
public struct LazyLoadReference<T> where T : Object
{
	private const int kInstanceID_None = 0;

	[SerializeField]
	private int m_InstanceID;

	internal int instanceID => m_InstanceID;

	public bool isSet => m_InstanceID != 0;

	public bool isBroken => m_InstanceID != 0 && !Object.DoesObjectWithInstanceIDExist(m_InstanceID);

	public T asset
	{
		get
		{
			if (m_InstanceID == 0)
			{
				return null;
			}
			return (T)Object.ForceLoadFromInstanceID(m_InstanceID);
		}
		set
		{
			if (value != null)
			{
				if (!Object.IsPersistent(value))
				{
					throw new ArgumentException("Object that does not belong to a persisted asset cannot be set as the target of a LazyLoadReference.");
				}
				m_InstanceID = value.GetInstanceID();
			}
			else
			{
				m_InstanceID = 0;
			}
		}
	}
}
