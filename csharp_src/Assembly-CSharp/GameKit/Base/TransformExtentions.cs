using UnityEngine;

namespace GameKit.Base;

public static class TransformExtentions
{
	public static Transform GetOrAddTransform(this Transform parent, string childName, Vector3 position, Vector3 roll)
	{
		Transform orAddTransform = parent.GetOrAddTransform(childName);
		if (orAddTransform != null)
		{
			orAddTransform.localPosition = position;
			orAddTransform.localRotation = Quaternion.Euler(roll);
		}
		return orAddTransform;
	}

	public static Transform GetOrAddTransform(this Transform parent, string childName, Vector3 position, Quaternion rotation)
	{
		Transform orAddTransform = parent.GetOrAddTransform(childName);
		if (orAddTransform != null)
		{
			orAddTransform.localPosition = position;
			orAddTransform.localRotation = rotation;
		}
		return orAddTransform;
	}

	public static Transform GetOrAddTransform(this Transform parent, string childName)
	{
		Transform transform = parent.Find(childName);
		if (transform == null)
		{
			transform = new GameObject(childName).transform;
			transform.SetParent(parent, worldPositionStays: false);
		}
		return transform;
	}
}
