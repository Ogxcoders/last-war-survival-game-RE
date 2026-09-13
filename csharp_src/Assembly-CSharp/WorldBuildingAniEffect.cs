using System;
using System.Collections.Generic;
using UnityEngine;

public class WorldBuildingAniEffect : MonoBehaviour
{
	[Serializable]
	public class Data
	{
		public string key;

		public List<int> index = new List<int>();
	}

	public List<ParticleSystem> effectNodeList = new List<ParticleSystem>();

	public List<Data> aniData = new List<Data>();

	private Dictionary<string, Data> _aniData = new Dictionary<string, Data>();

	public float startTime;

	public new string name = "idle";

	private void Awake()
	{
		for (int i = 0; i < aniData.Count; i++)
		{
			Data data = aniData[i];
			_aniData.Add(data.key, data);
		}
		PlayAnimation("idle", 0f);
	}

	public void PlayAnimation(string name, float startTime)
	{
		if (!_aniData.TryGetValue(name, out var value))
		{
			return;
		}
		List<int> index = value.index;
		for (int i = 0; i < effectNodeList.Count; i++)
		{
			ParticleSystem particleSystem = effectNodeList[i];
			if (particleSystem != null && index.Contains(i))
			{
				effectNodeList[i].gameObject.SetActive(value: false);
				effectNodeList[i].gameObject.SetActive(value: true);
				particleSystem.Simulate(startTime, withChildren: true, restart: true);
				particleSystem.Play();
			}
			else
			{
				effectNodeList[i].gameObject.SetActive(value: false);
			}
		}
	}

	public void StopAll()
	{
		for (int i = 0; i < effectNodeList.Count; i++)
		{
			ParticleSystem particleSystem = effectNodeList[i];
			if (particleSystem != null)
			{
				particleSystem.gameObject.SetActive(value: false);
			}
		}
	}

	public HashSet<UnityEngine.Object> GetManagedEffectObjects(HashSet<UnityEngine.Object> cache = null)
	{
		if (cache == null)
		{
			cache = new HashSet<UnityEngine.Object>();
		}
		cache.Clear();
		for (int i = 0; i < effectNodeList.Count; i++)
		{
			ParticleSystem[] componentsInChildren = effectNodeList[i].GetComponentsInChildren<ParticleSystem>(includeInactive: true);
			cache.UnionWith(componentsInChildren);
		}
		return cache;
	}

	public HashSet<UnityEngine.Object> GetStateEffectObjects(string name, HashSet<UnityEngine.Object> cache = null)
	{
		cache?.Clear();
		Data data = aniData.Find((Data val) => val.key == name);
		if (data != null)
		{
			List<int> index = data.index;
			for (int num = 0; num < effectNodeList.Count; num++)
			{
				ParticleSystem particleSystem = effectNodeList[num];
				if (particleSystem != null && index.Contains(num))
				{
					if (cache == null)
					{
						cache = new HashSet<UnityEngine.Object>();
					}
					ParticleSystem[] componentsInChildren = particleSystem.GetComponentsInChildren<ParticleSystem>(includeInactive: true);
					cache.UnionWith(componentsInChildren);
				}
			}
		}
		return cache;
	}

	private void PlayAnimationForUpdate(string name, float startTime)
	{
		if (!_aniData.TryGetValue(name, out var value))
		{
			return;
		}
		List<int> index = value.index;
		for (int i = 0; i < effectNodeList.Count; i++)
		{
			ParticleSystem particleSystem = effectNodeList[i];
			if (particleSystem != null && index.Contains(i))
			{
				effectNodeList[i].gameObject.SetActive(value: false);
				effectNodeList[i].gameObject.SetActive(value: true);
				particleSystem.Simulate(startTime, withChildren: true, restart: true);
				particleSystem.Play();
			}
			else
			{
				effectNodeList[i].gameObject.SetActive(value: false);
			}
		}
	}

	[ContextMenu("test")]
	public void TestPlay()
	{
		PlayAnimation(name, startTime);
	}
}
