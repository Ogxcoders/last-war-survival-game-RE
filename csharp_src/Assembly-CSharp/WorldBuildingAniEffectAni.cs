using System;
using System.Collections.Generic;
using UnityEngine;

public class WorldBuildingAniEffectAni : MonoBehaviour
{
	[Serializable]
	public class Data
	{
		public string key;

		public List<int> index = new List<int>();
	}

	public List<SimpleAnimation> effectNodeList = new List<SimpleAnimation>();

	public List<Data> aniData = new List<Data>();

	private Dictionary<string, Data> _aniData = new Dictionary<string, Data>();

	public string loopAniName;

	public SimpleAnimation loopAni;

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
		List<int> list = new List<int>();
		if (_aniData.TryGetValue(name, out var value))
		{
			list = value.index;
		}
		for (int i = 0; i < effectNodeList.Count; i++)
		{
			SimpleAnimation simpleAnimation = effectNodeList[i];
			if (simpleAnimation != null && list.Contains(i))
			{
				effectNodeList[i].gameObject.SetActive(value: true);
				string stateName = "Default";
				SimpleAnimation.State state = simpleAnimation.GetState(stateName);
				if (state != null)
				{
					float length = state.length;
					float normalizedTime = startTime / length;
					simpleAnimation.Stop();
					simpleAnimation.SampleAnimationAtTime(stateName, normalizedTime);
					simpleAnimation.Play(stateName);
				}
			}
			else
			{
				effectNodeList[i].gameObject.SetActive(value: false);
			}
		}
		CheckIsLoopAni(name);
	}

	public void StopAll()
	{
		for (int i = 0; i < effectNodeList.Count; i++)
		{
			SimpleAnimation simpleAnimation = effectNodeList[i];
			if (simpleAnimation != null)
			{
				simpleAnimation.Stop();
			}
		}
	}

	private void CheckIsLoopAni(string ani)
	{
		if (string.Equals(ani, loopAniName))
		{
			loopAni.Stop();
			loopAni.SampleAnimationAtTime("Default", 0f);
			loopAni.Play("Default");
		}
		else
		{
			loopAni.Stop("Default");
		}
	}
}
