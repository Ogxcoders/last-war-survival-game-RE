using System;
using UnityEngine;

public class ColliderEventHandler : MonoBehaviour
{
	public Action<GameObject> OnCollisionEnterAction { get; set; }

	public Action<GameObject> OnCollisionExitAction { get; set; }

	public Action<GameObject> OnTriggerEnterAction { get; set; }

	public Action<GameObject> OnTriggerExitAction { get; set; }

	private void OnEnable()
	{
		if (SceneManager.World != null)
		{
			SceneManager.World.RegisterPhysics(this);
		}
	}

	private void OnDisable()
	{
		if (SceneManager.World != null)
		{
			SceneManager.World.UnregisterPhysics(this);
		}
	}

	private void OnCollisionEnter(Collision other)
	{
		OnCollisionEnterAction?.Invoke(other.gameObject);
	}

	private void OnCollisionExit(Collision other)
	{
		OnCollisionExitAction?.Invoke(other.gameObject);
	}

	private void OnTriggerEnter(Collider other)
	{
		OnTriggerEnterAction?.Invoke(other.gameObject);
	}

	private void OnTriggerExit(Collider other)
	{
		OnTriggerExitAction?.Invoke(other.gameObject);
	}
}
