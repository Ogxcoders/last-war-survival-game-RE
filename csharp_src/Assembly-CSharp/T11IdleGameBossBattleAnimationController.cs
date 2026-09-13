using System;
using UnityEngine;

public class T11IdleGameBossBattleAnimationController : MonoBehaviour
{
	[Serializable]
	public class EditorState
	{
		public string name;

		public GameObject[] effectObjs;
	}

	[SerializeField]
	private SimpleAnimation mainAnim;

	[SerializeField]
	private EditorState[] states;

	[SerializeField]
	private GameObject[] hitTargets;

	public void HideAllEffects()
	{
		if (states == null)
		{
			return;
		}
		for (int i = 0; i < states.Length; i++)
		{
			if (states[i].effectObjs == null)
			{
				continue;
			}
			for (int j = 0; j < states[i].effectObjs.Length; j++)
			{
				if (states[i].effectObjs[j] != null)
				{
					states[i].effectObjs[j].SetActive(value: false);
				}
			}
		}
	}

	public SimpleAnimation GetMainAnim()
	{
		return mainAnim;
	}

	public GameObject GetHitTarget(int index)
	{
		if (hitTargets == null || index < 0 || index >= hitTargets.Length)
		{
			return null;
		}
		return hitTargets[index];
	}

	public void Play(string name)
	{
		if (mainAnim == null)
		{
			return;
		}
		mainAnim.CrossFade(name, 0.2f);
		if (states == null)
		{
			return;
		}
		for (int i = 0; i < states.Length; i++)
		{
			if (!(states[i].name == name) || states[i].effectObjs == null)
			{
				continue;
			}
			for (int j = 0; j < states[i].effectObjs.Length; j++)
			{
				if (states[i].effectObjs[j] != null)
				{
					states[i].effectObjs[j].SetActive(value: false);
					states[i].effectObjs[j].SetActive(value: true);
				}
			}
		}
	}
}
