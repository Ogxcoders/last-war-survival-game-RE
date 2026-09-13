using System;
using UnityEngine;
using UnityEngine.Events;

public class MonoBehaviourEvent : MonoBehaviour
{
	[Serializable]
	public class VisibleEvent : UnityEvent<bool>
	{
	}

	public VisibleEvent visibleEvent;

	private void OnBecameVisible()
	{
		visibleEvent.Invoke(arg0: true);
	}

	private void OnBecameInvisible()
	{
		visibleEvent.Invoke(arg0: false);
	}
}
