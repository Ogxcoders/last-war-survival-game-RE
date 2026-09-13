using System;
using UnityEngine;

namespace FibMatrix.Rendering;

public static class RefreshCallback
{
	public static Action Callback { get; set; }

	static RefreshCallback()
	{
	}

	public static void Invoke(GameObject go = null)
	{
		if (Callback != null)
		{
			Delegate[] invocationList = Callback.GetInvocationList();
			foreach (Delegate obj in invocationList)
			{
				Debug.Log($"{go?.scene.name} {obj.Target} invoke {obj.Method}");
				obj.Method.Invoke(obj.Target, new object[0]);
			}
		}
	}
}
