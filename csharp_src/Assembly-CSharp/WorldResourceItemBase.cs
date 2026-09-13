using System.Collections.Generic;
using UnityEngine;

public class WorldResourceItemBase : MonoBehaviour
{
	public List<GPUSkinningAnimator> animatorList;

	public List<SimpleAnimation> simpleAnimList;

	public long uuid { get; set; }

	public virtual void Init(long bUuid, string modelName)
	{
	}

	public virtual void UnInit()
	{
	}

	public virtual void OnUpdateTime()
	{
	}

	protected float GetClipLength(int index, string animName)
	{
		if (animatorList != null && animatorList.Count > 0)
		{
			GPUSkinningAnimator gPUSkinningAnimator = animatorList[index];
			if (gPUSkinningAnimator.gameObject.activeInHierarchy)
			{
				return gPUSkinningAnimator.GetClipLength(animName);
			}
		}
		else if (simpleAnimList != null && simpleAnimList.Count > 0)
		{
			SimpleAnimation simpleAnimation = simpleAnimList[0];
			if (simpleAnimation.gameObject.activeInHierarchy)
			{
				return simpleAnimation.GetClipLength(animName);
			}
		}
		return 0f;
	}

	protected void PlayAnim(string animName)
	{
		if (animatorList != null && animatorList.Count > 0)
		{
			foreach (GPUSkinningAnimator animator in animatorList)
			{
				if (animator.gameObject.activeInHierarchy)
				{
					animator.Play(animName);
				}
			}
			return;
		}
		if (simpleAnimList == null || simpleAnimList.Count <= 0)
		{
			return;
		}
		foreach (SimpleAnimation simpleAnim in simpleAnimList)
		{
			if (simpleAnim.gameObject.activeInHierarchy)
			{
				simpleAnim.Play(animName);
			}
		}
	}

	protected void PlayAnimQueued(string animName)
	{
		if (animatorList != null && animatorList.Count > 0)
		{
			foreach (GPUSkinningAnimator animator in animatorList)
			{
				if (animator.gameObject.activeInHierarchy)
				{
					animator.PlayQueued(animName);
				}
			}
			return;
		}
		if (simpleAnimList == null || simpleAnimList.Count <= 0)
		{
			return;
		}
		foreach (SimpleAnimation simpleAnim in simpleAnimList)
		{
			if (simpleAnim.gameObject.activeInHierarchy)
			{
				simpleAnim.PlayQueued(animName);
			}
		}
	}
}
