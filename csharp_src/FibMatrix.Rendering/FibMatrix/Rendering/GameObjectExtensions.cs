using System;
using UnityEngine;

namespace FibMatrix.Rendering;

public static class GameObjectExtensions
{
	public static T GetOrAddComponent<T>(this GameObject go) where T : Component
	{
		T val = go.GetComponent<T>();
		if (val == null)
		{
			val = go.AddComponent<T>();
		}
		return val;
	}

	public static void TraverseAllComponents<T>(this GameObject go, bool includeInactive = false, Action<T> action = null) where T : Component
	{
		T[] componentsInChildren = go.GetComponentsInChildren<T>(includeInactive);
		foreach (T obj in componentsInChildren)
		{
			action?.Invoke(obj);
		}
	}

	public static void RemoveAllComponents<T>(this GameObject go, bool includeInactive = false, Action<T> preprocess = null) where T : Component
	{
		go.TraverseAllComponents(includeInactive, delegate(T component)
		{
			preprocess?.Invoke(component);
		});
	}

	public static string GetRelativePath(this GameObject child, GameObject parent)
	{
		if (parent.transform == child.transform)
		{
			return "";
		}
		string text = child.name;
		Transform parent2 = child.transform.parent;
		while ((bool)parent2 && !(parent2.gameObject.transform == parent.transform))
		{
			text = parent2.transform.name + "/" + text;
			parent2 = parent2.parent;
		}
		return text;
	}

	public static bool IsChildOf(this GameObject child, GameObject parent, bool trueIfSame = true)
	{
		if (child == null)
		{
			return false;
		}
		if (parent == null)
		{
			return false;
		}
		if (trueIfSame && child.transform == parent.transform)
		{
			return true;
		}
		return child.transform.IsChildOf(parent.transform);
	}
}
