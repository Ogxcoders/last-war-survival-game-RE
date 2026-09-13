using System.Collections.Generic;
using GameFramework;
using UnityEngine;

public class DominatorTrainSceneIdleModelAnimHelper : MonoBehaviour
{
	[SerializeField]
	private SimpleAnimation mainAnim;

	[SerializeField]
	private List<SimpleAnimation> subAnims;

	private Dictionary<string, List<SimpleAnimation>> subAnimsDict;

	public void Init()
	{
		if (mainAnim == null)
		{
			Log.Error("empty mainAnim, gameobject:" + base.gameObject.name);
			return;
		}
		subAnimsDict = new Dictionary<string, List<SimpleAnimation>>();
		foreach (SimpleAnimation.State state in mainAnim.GetStates())
		{
			subAnimsDict.Add(state.name, new List<SimpleAnimation>());
			if (subAnims == null || subAnims.Count <= 0)
			{
				continue;
			}
			foreach (SimpleAnimation subAnim in subAnims)
			{
				if (subAnim.GetState(state.name) != null)
				{
					subAnimsDict[state.name].Add(subAnim);
				}
			}
		}
	}

	public void Play(string anim)
	{
		if (mainAnim != null)
		{
			mainAnim.Play(anim);
		}
		if (subAnims == null)
		{
			return;
		}
		foreach (SimpleAnimation subAnim in subAnims)
		{
			if (subAnim == null)
			{
				Log.Error("empty subAnim, gameobject:" + base.gameObject.name);
				continue;
			}
			bool flag = false;
			if (subAnimsDict != null && subAnimsDict.ContainsKey(anim) && subAnimsDict[anim].Contains(subAnim))
			{
				flag = true;
			}
			subAnim.gameObject.SetActive(flag);
			if (flag)
			{
				subAnim.Play(anim);
			}
		}
	}

	public SimpleAnimation GetMainAnim()
	{
		return mainAnim;
	}
}
