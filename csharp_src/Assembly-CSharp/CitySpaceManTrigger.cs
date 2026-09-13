using System;
using System.Collections.Generic;
using UnityEngine;

public class CitySpaceManTrigger : MonoBehaviour
{
	private Dictionary<Collider, CitySpaceManTrigger> triggerDic = new Dictionary<Collider, CitySpaceManTrigger>();

	public long ObjectId { get; set; }

	public int resType { get; set; }

	public Action<long, int> TriggerEnterAction { get; set; }

	public Action<long> TriggerExitAction { get; set; }

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
		CitySpaceManTrigger componentInParent = other.gameObject.GetComponentInParent<CitySpaceManTrigger>();
		if (componentInParent != null)
		{
			TriggerEnterAction?.Invoke(componentInParent.ObjectId, componentInParent.resType);
		}
	}

	private void OnCollisionExit(Collision other)
	{
		CitySpaceManTrigger componentInParent = other.gameObject.GetComponentInParent<CitySpaceManTrigger>();
		if (componentInParent != null)
		{
			TriggerExitAction?.Invoke(componentInParent.ObjectId);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		CitySpaceManTrigger value = null;
		if (triggerDic.TryGetValue(other, out value))
		{
			if (value != null)
			{
				TriggerEnterAction?.Invoke(value.ObjectId, value.resType);
			}
			return;
		}
		CitySpaceManTrigger componentInParent = other.gameObject.GetComponentInParent<CitySpaceManTrigger>();
		if (componentInParent != null)
		{
			TriggerEnterAction?.Invoke(componentInParent.ObjectId, componentInParent.resType);
			triggerDic.Add(other, componentInParent);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		CitySpaceManTrigger value = null;
		if (triggerDic.TryGetValue(other, out value))
		{
			if (value != null)
			{
				TriggerExitAction?.Invoke(value.ObjectId);
			}
			return;
		}
		CitySpaceManTrigger componentInParent = other.gameObject.GetComponentInParent<CitySpaceManTrigger>();
		if (componentInParent != null)
		{
			TriggerExitAction?.Invoke(value.ObjectId);
			triggerDic.Add(other, componentInParent);
		}
	}

	private void OnDestroy()
	{
		triggerDic.Clear();
	}
}
